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

namespace TechnoSystemsApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    static string userRole;
    public MainWindow(string role)
    {
        InitializeComponent();
        MainFrame.Navigate(new TariffPage(role));
        userRole = role;
        CheckUserRole();
    }
    public void CheckUserRole()
    {
        if (userRole == "Гость")
        {
            NavigationPanel.Visibility = Visibility.Collapsed;
        }

    }

    private void Tariff_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new TariffPage(userRole));
    }

    private void Requests_Click(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new RequestsPage());

    }
}