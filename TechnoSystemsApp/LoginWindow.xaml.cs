using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Shapes;
using TechnoSystemsApp.Data;

namespace TechnoSystemsApp
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var role = Auth();
            MainWindow mainWindow = new MainWindow(role);
            mainWindow.Show();
            this.Close();
        }

        private void GuestButton_Click(object sender, RoutedEventArgs e)
        {
            Role role = new Role();
            role.Name = "Гость";
            MainWindow mainWindow = new MainWindow(role);
            mainWindow.Show();
            this.Close();
        }

        private Role Auth()
        {
            using (var context = new TechnoSystemsContext())
            {
                try
                {
                    var user = context.Users.FirstOrDefault(u => (LoginTextBox.Text == u.Email && PasswordTextBox.Password == u.Password));
                    if (user == null)
                    {
                        MessageBox.Show($"Пользователь не найден" ,"Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    return context.Roles.FirstOrDefault(r => r.Id == user.RoleId);

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка авторизации {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }

            }
        }
    }
}
