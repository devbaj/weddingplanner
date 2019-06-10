using System.ComponentModel.DataAnnotations;

namespace weddingplanner.Models
{
  public class Attendance
  {
    [Key]
    public int AttendanceID {get;set;}
    public int UserID {get;set;}
    public User Guest {get;set;}
    public int EventID {get;set;}
    public Event Event {get;set;}
  }
}