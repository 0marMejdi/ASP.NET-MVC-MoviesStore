using System;
using System.Collections.Generic;

namespace Hakuna.Models;

public partial class Genre
{
    public Guid Id { get; set; }

    public string? Name { get; set; } 
    
    public ICollection<Movie> Movies {get;set;} = new List<Movie>();
}
