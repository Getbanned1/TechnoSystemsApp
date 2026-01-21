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
using TechnoSystemsApp.Models;

namespace TechnoSystemsApp
{
    /// <summary>
    /// Логика взаимодействия для TariffPage.xaml
    /// </summary>
    public partial class TariffPage : Page
    {
        static User _user;
        public TariffPage(User user)
        {
            InitializeComponent();
            _user = user;
            SearchMenu.Visibility = user.Role == null ? SearchMenu.Visibility = Visibility.Collapsed : SearchMenu.Visibility = Visibility.Visible;
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

        private void TariffCard_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TariffCard.SelectedItem is Tariff selectedTariff)
            {
                // Формируем сообщение для подтверждения
                string message = $"Вы хотите оформить заявку на тариф:\n\n" +
                                 $"Название: {selectedTariff.Name}\n" +
                                 $"Сервис: {selectedTariff.Service.Name}\n" +
                                 $"Дата начала: {selectedTariff.StartDate:dd.MM.yyyy}\n" +
                                 $"Срок подписки: {selectedTariff.SubscriptionDuration} дней\n" +
                                 $"Доступные лицензии: {selectedTariff.AvalibleLicenses}\n" +
                                 $"Стоимость: {selectedTariff.Price} руб.";

                // Отображаем MessageBox с подтверждением
                MessageBoxResult result = MessageBox.Show(message, "Подтверждение заявки",
                                                          MessageBoxButton.OKCancel, MessageBoxImage.Information);

                if (result == MessageBoxResult.OK)
                {
                    try
                    {
                        // Создаем заявку (количество лицензий берем равным доступным лицензиям)
                        Request newRequest = Request.CreateRequest(
                            tariff: selectedTariff,
                            user: _user,
                            licenses: selectedTariff.AvalibleLicenses,
                            comment: null // комментарий необязателен
                        );

                        // Здесь сохраняем заявку в базу (пример, зависит от вашего DbContext)
                        using (var db = new TechnoSystemsContext())
                        {
                            db.Requests.Add(newRequest);
                            db.SaveChanges();
                        }

                        MessageBox.Show($"Заявка на тариф '{selectedTariff.Name}' успешно оформлена!",
                                        "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при создании заявки: {ex.Message}",
                                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                // Сброс выбора, чтобы можно было кликнуть снова
                TariffCard.SelectedItem = null;
            }
        }
    }
}
