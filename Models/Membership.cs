using System;
using System.Collections.Generic;

namespace Hakuna.Models;

public partial class Membership
{
    public Guid Id { get; set; }

    public string? Title { get; set; }
    public int? DurationInMonth { get; set; }

    public double? SignUpFee { get; set; }

    public double? DiscountRate { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
