using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hakuna.Models;

public partial class Movie
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public int? ReleaseDate { get; set; }
    
    public virtual ICollection<Customer> Customers {get;set;} = new List<Customer>();
    
    [Display(Name = "Poster Picture for Movie")]
    [NotMapped]
    public IFormFile? PosterPicture { get; set; } 
    
    [ForeignKey("Genre")]
    public Guid? GenreID {get;set;}
    public Genre? Genre {get;set;}
}
