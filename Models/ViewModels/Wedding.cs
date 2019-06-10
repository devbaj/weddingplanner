using System;
using System.Collections.Generic;

namespace weddingplanner.Models
{
  public class Wedding
  {
    public int WeddingID {get;set;}
    public string Couple {get;set;}
    public DateTime Date {get;set;}
    public string Address {get;set;}
    public List<User> Guests {get;set;}
    public bool IsHosting {get;set;}
    public bool IsAttending {get;set;}
  }
}