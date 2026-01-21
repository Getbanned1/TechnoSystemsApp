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
    /// Логика взаимодействия для RequestsPage.xaml
    /// </summary>
    public partial class RequestsPage : Page
    {
        public RequestsPage()
        {
            InitializeComponent();
            LoadRequests();
            DataSortBox.ItemsSource = new List<string> { "Все время", "По возрастанию", "По убыванию" };
        }
        public void LoadRequests()
        {
            using (var context = new TechnoSystemsContext())
            {
                RequestsView.ItemsSource = context.Requests
                    .Include(t => t.Status)
                    .Include(t => t.Tariff)
                    .Include(t => t.User)

                    .ToList();
                var statuses = context.RequestStatuses.Select(s => s.Name).ToList();
                statuses.Insert(0, "Все");
                SortBox.ItemsSource = statuses;

            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateRequests();
        }

        private void UpdateRequests()
        {
            using (var context = new TechnoSystemsContext())
            {
                var query = context.Requests
                    .Include(t => t.Status)
                    .Include(t => t.Tariff)
                    .Include(t => t.User).AsQueryable();
                if (!string.IsNullOrEmpty(SearchBar.Text))
                {
                    string searchtext = SearchBar.Text.ToLower();
                    query = query.Where(r => r.User.FullName.ToLower().Contains(searchtext) ||
                    r.Id.ToString().Contains(searchtext));
                }
                if (SortBox.SelectedItem != null && SortBox.SelectedItem.ToString() != "Все")
                {
                    var item = SortBox.SelectedItem.ToString();
                    query = query.Where(r => r.Status.Name == item);
                }
                if (DataSortBox.SelectedItem != null && DataSortBox.SelectedItem.ToString() != "Все время")
                {
                    if (DataSortBox.SelectedItem.ToString() == "По возрастанию")
                        query = query.OrderBy(r => r.Date);
                    else
                    {
                        query = query.OrderByDescending(r => r.Date);

                    }
                }

                RequestsView.ItemsSource = query.ToList();
            }

        }

        private void SortBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateRequests();
        }

        private void CreateRequest_Click(object sender, RoutedEventArgs e)
        {
            var createWindow = new RequestsChangeWindow();
            createWindow.ShowDialog();
            UpdateRequests();
        }

        private void RequestsView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (RequestsView.SelectedItem is Models.Request selectedrequest)
            {
                var editwindow = new RequestsChangeWindow(selectedrequest);
                editwindow.ShowDialog();

                UpdateRequests();
            }
        }
    }
}
