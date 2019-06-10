using System;
using System.ComponentModel.DataAnnotations;

namespace weddingplanner.Models
{
  public class EventForm
  {
    [Required]
    [Display(Name = "Name of First Party")]
    public string Name1 {get;set;}

    [Required]
    [Display(Name = "Name of Second Party")]
    public string Name2 {get;set;}

    [Required]
    [DataType(DataType.Date)]
    public DateTime Date {get;set;}

    [Required]
    public string Address {get;set;}
  }
}