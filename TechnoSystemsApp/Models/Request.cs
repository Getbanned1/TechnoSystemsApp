using System;
using System.Collections.Generic;
using TechnoSystemsApp.Models;

namespace TechnoSystemsApp.Models;
public partial class Request
{
    public int Id { get; set; }

    public int TariffId { get; set; }

    public int UserId { get; set; }

    public DateOnly Date { get; set; }

    public int StatusId { get; set; }

    public int? Licenses { get; set; }

    public string? Comment { get; set; }

    public virtual RequestStatus Status { get; set; } = null!;

    public virtual Tariff Tariff { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public static Request CreateRequest(Tariff tariff, User user, int licenses, string? comment = null)
    {
        if (tariff == null) throw new ArgumentNullException(nameof(tariff));
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (licenses <= 0) throw new ArgumentException("Количество лицензий должно быть больше нуля.", nameof(licenses));

        return new Request
        {
            // Id не указываем!
            TariffId = tariff.Id,
            UserId = user.Id,
            Licenses = 1,
            Comment = comment,
            Date = DateOnly.FromDateTime(DateTime.Now),
            StatusId = 2 // например, "Новая" заявка
        };
    }
}
