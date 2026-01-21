using System;
using System.Collections.Generic;

namespace TechnoSystemsApp.Models;
public partial class Tariff
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int ServiceId { get; set; }

    public int SubscriptionDuration { get; set; }

    public DateOnly StartDate { get; set; }

    public int Price { get; set; }

    public int UserLimit { get; set; }

    public int AvalibleLicenses { get; set; }

    public string FileName { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual Service Service { get; set; } = null!;

    public string ImagePath => string.IsNullOrEmpty(FileName) ? "/Images/no-image.png" : $"/Images/{FileName}";

    public bool HasBigDiscount
    {
        get
        {
            if (Service?.Tariffs == null || Service.Tariffs.Count == 0)
                return false;

            double avgPrice = Service.Tariffs.Average(t => t.Price);
            return Price < avgPrice * 0.85;
        }
    }
    // Мало лицензий (<10%)
    public bool LowLicenses =>
        UserLimit > 0 &&
        AvalibleLicenses < UserLimit * 0.1;

    // Начало менее чем через 7 дней
    public bool StartsSoon
    {
        get
        {
            var today = new DateOnly(2024, 5, 28);
            var days = StartDate.DayNumber - today.DayNumber;
            return days >= 0 && days <= 7;
        }
    }
}
