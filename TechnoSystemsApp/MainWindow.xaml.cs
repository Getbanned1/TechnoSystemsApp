using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TechnoSystemsApp.Models;

namespace TechnoSystemsApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    static User _user;
    public MainWindow(User user)
    {
        InitializeComponent();
        MainFrame.Navigate(new TariffPage(user));
        _user = user;
        CheckUserRole();
    }
    public void CheckUserRole()
    {
        if (_user.Role == null)
        {
            NavigationPanel.Visibility = Visibility.Collapsed;
        }

    }

    private void Tariff_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new TariffPage(_user));
    }

    private void Requests_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new RequestsPage());

    }
}