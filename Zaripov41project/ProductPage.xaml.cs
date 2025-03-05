using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Zaripov41project
{
	/// <summary>
	/// Логика взаимодействия для ProductPage.xaml
	/// </summary>
	public partial class ProductPage : Page
	{


        List<Product> selectedProduct = new List<Product>();
        private User client = null;
		public ProductPage()
		{
			InitializeComponent();
			var currentProduct = Zaripov41Entities.GetContext().Product.ToList();
			ProductListView.ItemsSource = currentProduct;
			Combotype.SelectedIndex = 0;
			UpdateProducts();
		}
		public ProductPage(User user)
		{
            InitializeComponent();
            FIOClient.Text = "Вы авторизованы как " + user.UserName + " " + user.UserSurname + " " + user.UserPatronymic;
            

            Role.Text = user.UserRole == 1 ? "Роль: Клиент" : user.UserRole == 2 ? "Роль: Менеджер" : "Роль: Администратор";

            client = user;
            var currentProduct = Zaripov41Entities.GetContext().Product.ToList();
            ProductListView.ItemsSource = currentProduct;
            Combotype.SelectedIndex = 0;
            UpdateProducts();
        }
		private void UpdateProducts()
		{
			var currentProducts = Zaripov41Entities.GetContext().Product.ToList();
			switch(Combotype.SelectedIndex)
			{
				case 1:
                    currentProducts = currentProducts.Where(p => (Convert.ToInt32(p.ProductDiscountAmount) >= 0 && Convert.ToInt32(p.ProductDiscountAmount) < 10)).ToList(); break;
                case 2:
                    currentProducts = currentProducts.Where(p => (Convert.ToInt32(p.ProductDiscountAmount) >= 10 && Convert.ToInt32(p.ProductDiscountAmount) < 15)).ToList(); break;
                case 3:
                    currentProducts = currentProducts.Where(p => (Convert.ToInt32(p.ProductDiscountAmount) >= 15 && Convert.ToInt32(p.ProductDiscountAmount) <= 100)).ToList(); break;
				default:
					break;
            }
			currentProducts = currentProducts.Where(p => p.ProductName.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
			ProductListView.ItemsSource = currentProducts;
			if(RDown.IsChecked.Value)
                ProductListView.ItemsSource = currentProducts.OrderBy(p => p.ProductCost).ToList();
			if(RUp.IsChecked.Value)
                ProductListView.ItemsSource = currentProducts.OrderByDescending(p => p.ProductCost).ToList();
			TextOne.Text = "кол-во " + currentProducts.Count.ToString() + " из " + Zaripov41Entities.GetContext().Product.ToList().Count.ToString();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
		{
			Manager.MainFrame.Navigate(new AddEditPage());
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProducts();

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            UpdateProducts();

        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            UpdateProducts();

        }

        private void Combotype_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			UpdateProducts();
        }

        private void ProductListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var currentChoiceProduct = ProductListView.SelectedItems.Cast<Product>().ToList();
            for (int i = 0;i < currentChoiceProduct.Count;i++)
            {
                selectedProduct.Add(currentChoiceProduct[i]);
            }
            BasketButton.Visibility = Visibility.Visible;

        }

        private void BasketButton_Click(object sender, RoutedEventArgs e)
        {


            OrderWindow orderWindow = new OrderWindow(client, selectedProduct);

            orderWindow.Show();
            selectedProduct = new List<Product>();
            BasketButton.Visibility = Visibility.Hidden;

        }
    }
}
