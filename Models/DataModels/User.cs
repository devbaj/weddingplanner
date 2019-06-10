using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace weddingplanner.Models
{
  public class User
  {
    [Key]
    public int UserID {get;set;}
    public string FirstName {get;set;}
    public string LastName {get;set;}
    public string Email {get;set;}
    public string HashedPassword {get;set;}
    public DateTime CreatedAt {get;set;}
    public DateTime UpdatedAt {get;set;}
    public List<Event> HostingList {get;set;}
    public List<Attendance> Invitations {get;set;}
  }
}