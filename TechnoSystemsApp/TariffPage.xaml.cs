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
            DateSortBox.ItemsSource = new List<string> { "Все", "По возрастанию", "По убыванию" };
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
        private void UpdateTariffs()
        {
            using (var context = new TechnoSystemsContext())
            {
                var query = context.Tariffs
                    .Include(t => t.Requests)
                    .Include(t => t.Service).AsQueryable();
                if (!string.IsNullOrEmpty(SearchBar.Text))
                {
                    string searchtext = SearchBar.Text.ToLower();
                    query = query.Where(r => r.Name.ToLower().Contains(searchtext) ||
                    r.Id.ToString().Contains(searchtext));
                }

                if (DateSortBox.SelectedItem != null && DateSortBox.SelectedItem.ToString() != "Все время")
                {
                    if (DateSortBox.SelectedItem.ToString() == "По возрастанию")
                        query = query.OrderBy(r => r.StartDate);
                    else
                    {
                        query = query.OrderByDescending(r => r.StartDate);

                    }
                }

                TariffCard.ItemsSource = query.ToList();
            }

        }
        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTariffs();
        }

        private void DateSortBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTariffs();
        }
    }
}
