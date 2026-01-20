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
using System.Windows.Shapes;
using TechnoSystemsApp.Data;
using TechnoSystemsApp.Models;

namespace TechnoSystemsApp
{
    /// <summary>
    /// Логика взаимодействия для RequestsChangeWindow.xaml
    /// </summary>
    public partial class RequestsChangeWindow : Window
    {
        private Request _request;

        private List<RequestStatus> _status;
        private List<Tariff> _tariff;
        private List<User> _user;

        public RequestsChangeWindow(Request request)
        {
            InitializeComponent();
            _request = request;
            FillData();
        }
        public void FillData()
        {
            using (var context = new TechnoSystemsContext())
            {
                _user = context.Users.ToList();
                _tariff = context.Tariffs.ToList();
                _status = context.RequestStatuses.ToList();
            }
            UserComboBox.ItemsSource = _user;
            TariffComboBox.ItemsSource = _tariff;
            StatusComboBox.ItemsSource = _status;
            DateBox.DataContext = _request.Date;
        }

        public void ChangeRequest()
        {
            using (var context = new TechnoSystemsContext())
            {
                var request = context.Requests.FirstOrDefault(r => r.Id == _request.Id);

                if (request == null)
                {
                    MessageBox.Show("Заявка не найдена");
                    return;
                }

                if (UserComboBox.SelectedItem is User user)
                    request.UserId = user.Id;

                if (TariffComboBox.SelectedItem is Tariff tariff)
                    request.TariffId = tariff.Id;

                if (StatusComboBox.SelectedItem is RequestStatus status)
                    request.StatusId = status.Id;

                if (DateBox.SelectedDate.HasValue)
                    request.Date = DateOnly.FromDateTime(DateBox.SelectedDate.Value);

                context.SaveChanges();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Заявка обновлена","Изменение", MessageBoxButton.OK);
            ChangeRequest();
            this.Close();
        }
    }
}
