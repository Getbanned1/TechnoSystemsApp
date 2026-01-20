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
}
