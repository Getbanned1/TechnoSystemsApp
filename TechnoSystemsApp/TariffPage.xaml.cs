using Microsoft.EntityFrameworkCore;
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
using TechnoSystemsApp.Data;

namespace TechnoSystemsApp
{
    /// <summary>
    /// Логика взаимодействия для TariffPage.xaml
    /// </summary>
    public partial class TariffPage : Page
    {
        public TariffPage(string role)
        {
            InitializeComponent();
            SearchMenu.Visibility = role == "Гость" ? SearchMenu.Visibility = Visibility.Collapsed : SearchMenu.Visibility = Visibility.Visible;
            LoadTariffs();
            
        }
        public void LoadTariffs()
        {
            TariffCard.Items.Clear(); // <- очищаем дизайн-тайм элементы
            using (var context = new TechnoSystemsContext())
            {
                TariffCard.ItemsSource = context.Tariffs
                    .Include(t => t.Requests)
                    .Include(t => t.Service)

                    .ToList();
            }
        }
    }
}
