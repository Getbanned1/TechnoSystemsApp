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
}
