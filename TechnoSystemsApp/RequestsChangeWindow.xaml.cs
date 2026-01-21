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



        public RequestsChangeWindow(Request? request = null)
        {
            InitializeComponent();
            _request = request ?? new Request();
            InitControls();

            this.Title = _request.Id == 0 ? "Новая заявка" : $"Редактирование заявки #{_request.Id}";

            if (_request.Id != 0)
                FillFields();
        }
        private void InitControls()
        {
            using (var context = new TechnoSystemsContext())
            {
                TariffComboBox.ItemsSource = context.Tariffs.ToList();
                StatusComboBox.ItemsSource = context.RequestStatuses.ToList();
                UserComboBox.ItemsSource = context.Users.ToList();
            }
        }
        private void FillFields()
        {
            TariffComboBox.SelectedValue = _request.TariffId;
            StatusComboBox.SelectedValue = _request.StatusId;
            UserComboBox.SelectedValue = _request.UserId;

            DateBox.SelectedDate = _request.Date.ToDateTime(TimeOnly.MinValue);
            LicensesTextBox.Text = _request.Licenses?.ToString();
            CommentTextBox.Text = _request.Comment;
        }

        private void UpdateRequestFromControls(Request request)
        {
            if (UserComboBox.SelectedItem is User user) request.UserId = user.Id;
            if (TariffComboBox.SelectedItem is Tariff tariff) request.TariffId = tariff.Id;
            if (StatusComboBox.SelectedItem is RequestStatus status) request.StatusId = status.Id;
            if (DateBox.SelectedDate.HasValue) request.Date = DateOnly.FromDateTime(DateBox.SelectedDate.Value);

            request.Licenses = string.IsNullOrWhiteSpace(LicensesTextBox.Text) ? null : int.Parse(LicensesTextBox.Text);
            request.Comment = CommentTextBox.Text;
        }



        private bool ValidateLicenses()
        {
            if (StatusComboBox.SelectedItem is not RequestStatus status)
                return true;

            // Статус "Подтверждена" (при необходимости замените имя)
            if (status.Name != "Подтверждена")
                return true;

            if (TariffComboBox.SelectedItem is not Tariff tariff)
                return true;

            if (!int.TryParse(LicensesTextBox.Text, out int requestedLicenses))
            {
                MessageBox.Show("Введите корректное количество лицензий",
                    "Ошибка валидации",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return false;
            }

            if (requestedLicenses > tariff.AvalibleLicenses)
            {
                MessageBox.Show(
                    $"Недостаточно свободных лицензий.\n" +
                    $"Доступно: {tariff.AvalibleLicenses}, запрошено: {requestedLicenses}",
                    "Недостаточно лицензий",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return false;
            }

            return true;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateLicenses())
                    return;

                using (var context = new TechnoSystemsContext())
                {
                    if (_request.Id == 0)
                    {
                        UpdateRequestFromControls(_request);
                        context.Requests.Add(_request);
                    }
                    else
                    {
                        var requestInDb = context.Requests.FirstOrDefault(r => r.Id == _request.Id);
                        if (requestInDb == null)
                        {
                            MessageBox.Show("Заявка не найдена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        UpdateRequestFromControls(requestInDb);
                        context.Requests.Update(requestInDb);
                    }

                    context.SaveChanges();
                }

                MessageBox.Show("Заявка успешно сохранена", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении заявки:\n{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
