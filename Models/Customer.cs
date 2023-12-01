using System;
using System.Collections.Generic;

namespace Hakuna.Models;

public partial class Customer
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string ? LastName { get; set; } 

    public int Age { get; set; }
    // has at most membership
    public Membership? Membershiptype { get; set; }
    public Guid? MembershipId { get; set; }
    // can rent movies
    public virtual ICollection<Movie> Movies {get;set;} = new List<Movie>();

    // public virtual Membership? MembershiptypeNavigation { get; set; }
}
