using System;
using System.Collections.Generic;

namespace TechnoSystemsApp;

public partial class Service
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? ServiceTypeId { get; set; }

    public string? Description { get; set; }

    public virtual ServiceType? ServiceType { get; set; }

    public virtual ICollection<Tariff> Tariffs { get; set; } = new List<Tariff>();
}
