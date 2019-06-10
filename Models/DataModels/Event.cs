using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace weddingplanner.Models
{
  public class Event
  {
    [Key]
    public int EventID {get;set;}
    public string Name1 {get;set;}
    public string Name2 {get;set;}
    public DateTime Date {get;set;}
    public string Address {get;set;}
    public DateTime CreatedAt {get;set;}
    public DateTime UpdatedAt {get;set;}
    public int UserID {get;set;}
    public User Host {get;set;}
    public List<Attendance> Guests {get;set;}
  }
}