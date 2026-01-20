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
    public MainWindow(Role role)
    {
        InitializeComponent();
        userRole = role.Name;
    }
    public void CheckUserRole()
    {
        //if (userRole.Name ==;
    }
}