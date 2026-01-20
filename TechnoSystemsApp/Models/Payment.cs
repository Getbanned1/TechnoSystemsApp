using System;
using System.Collections.Generic;

namespace TechnoSystemsApp.Models;

public partial class Payment
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public DateOnly? Date { get; set; }

    public int? TotalPrice { get; set; }

    public int? PaymentStatusId { get; set; }

    public int? PaymentTypeId { get; set; }

    public virtual PaymentStatus? PaymentStatus { get; set; }

    public virtual PaymentType? PaymentType { get; set; }
}
