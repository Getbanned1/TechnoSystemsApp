using System;
using System.Collections.Generic;
using TechnoSystemsApp.Models;

namespace TechnoSystemsApp.Models;
public partial class PaymentType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
