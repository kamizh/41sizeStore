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
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }
        private bool isExit = false;
        
        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            string login = Login.Text;
            string password = Password.Text;

            if(login == "" && password == "")
            {
                MessageBox.Show("Все поля должны быть заполнены!");
                return ;
            }
            User user = Zaripov41Entities.GetContext().User.ToList().Find(p => p.UserLogin == login && p.UserPassword == password);
            
            if (user == null)
            {
                
                MessageBox.Show("Пользователя с такими данными не существует!");
                Login.Text = "";
                Password.Text = "";
                captchaSet();
                if (isExit)
                {
                    string cap = captcha1.Text + captcha2.Text + captcha3.Text + captcha4.Text;
                    if (cap == CaptchaBox.Text)
                    {
                        Manager.MainFrame.Navigate(new ProductPage(user));
                        Login.Text = "";
                        Password.Text = "";
                        hideData();
                    }
                    else
                    {
                        LoginBtn.IsEnabled = false;
                        await Task.Delay(10000);
                        LoginBtn.IsEnabled = true;

                    }

                }
            }
            else
            {
                if (isExit)
                {
                    string cap = captcha1.Text + captcha2.Text + captcha3.Text + captcha4.Text;
                    if (cap == CaptchaBox.Text)
                    {
                        Manager.MainFrame.Navigate(new ProductPage(user));
                        Login.Text = "";
                        Password.Text = "";
                    }
                    else
                    {
                        LoginBtn.IsEnabled = false;
                        await Task.Delay(10000);
                        LoginBtn.IsEnabled = true;
                    }

                }
                Manager.MainFrame.Navigate(new ProductPage(user));
                hideData();
            }
            isExit = true;
           
        }

        private void ExitGuestBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ProductPage());
            hideData();
        }
        void captchaSet()
        {
            CaptchaBox.Visibility = Visibility.Visible;
            CaptchaText.Visibility = Visibility.Visible;

            Random random = new Random();
            captcha1.Visibility = Visibility.Visible;
            captcha2.Visibility = Visibility.Visible;
            captcha3.Visibility = Visibility.Visible;
            captcha4.Visibility = Visibility.Visible;
            captcha1.Text = random.Next(0, 9).ToString();
            captcha2.Text = random.Next(0, 9).ToString();
            captcha3.Text = random.Next(0, 9).ToString();
            captcha4.Text = random.Next(0, 9).ToString();
            Password.Text = "";
            Login.Text = "";
            CaptchaBox.Text = "";
        }
        void hideData()
        {
            Login.Text = "";
            Password.Text = "";
            isExit = false;
            CaptchaBox.Visibility = Visibility.Hidden;
            CaptchaText.Visibility= Visibility.Hidden;
            captcha1.Visibility = Visibility.Hidden;
            captcha2.Visibility = Visibility.Hidden;
            captcha3.Visibility = Visibility.Hidden;
            captcha4.Visibility = Visibility.Hidden;
        }
    }
}

