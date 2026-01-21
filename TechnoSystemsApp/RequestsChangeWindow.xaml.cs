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
            TariffComboBox.SelectedItem = _request.Tariff;
            StatusComboBox.SelectedItem = _request.Status;
            UserComboBox.SelectedItem = _request.User;

            DateBox.SelectedDate = _request.Date.ToDateTime(TimeOnly.MinValue);
            LicensesTextBox.Text = _request.Licenses?.ToString();
            CommentTextBox.Text = _request.Comment;
        }

        //public void ChangeRequest()
        //{
        //    using (var context = new TechnoSystemsContext())
        //    {
        //        var request = context.Requests.FirstOrDefault(r => r.Id == _request.Id);

        //        if (request == null)
        //        {
        //            MessageBox.Show("Заявка не найдена");
        //            return;
        //        }

        //        if (UserComboBox.SelectedItem is User user)
        //            request.UserId = user.Id;

        //        if (TariffComboBox.SelectedItem is Tariff tariff)
        //            request.TariffId = tariff.Id;

        //        if (StatusComboBox.SelectedItem is RequestStatus status)
        //            request.StatusId = status.Id;

        //        if (DateBox.SelectedDate.HasValue)
        //            request.Date = DateOnly.FromDateTime(DateBox.SelectedDate.Value);
        //        _request.Licenses = string.IsNullOrWhiteSpace(LicensesTextBox.Text)? null: int.Parse(LicensesTextBox.Text);

        //        _request.Comment = CommentTextBox.Text;
        //        if (_request.Id == 0)
        //            context.Requests.Add(_request);
        //        else
        //            context.Requests.Update(_request);
        //        context.SaveChanges();
        //        DialogResult = true;
        //    }
        //}
        private void UpdateRequestFromControls(Request request)
        {
            if (UserComboBox.SelectedItem is User user) request.UserId = user.Id;
            if (TariffComboBox.SelectedItem is Tariff tariff) request.TariffId = tariff.Id;
            if (StatusComboBox.SelectedItem is RequestStatus status) request.StatusId = status.Id;
            if (DateBox.SelectedDate.HasValue) request.Date = DateOnly.FromDateTime(DateBox.SelectedDate.Value);

            request.Licenses = string.IsNullOrWhiteSpace(LicensesTextBox.Text) ? null : int.Parse(LicensesTextBox.Text);
            request.Comment = CommentTextBox.Text;
        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new TechnoSystemsContext())
                {
                    if (_request.Id == 0)
                    {
                        // Новый объект — обновляем и добавляем
                        UpdateRequestFromControls(_request);
                        context.Requests.Add(_request);
                    }
                    else
                    {
                        // Редактируем существующий объект из базы
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
