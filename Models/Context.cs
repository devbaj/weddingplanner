using Microsoft.EntityFrameworkCore;

namespace weddingplanner.Models
{
  public class Context : DbContext
  {
    public Context(DbContextOptions options) : base(options) {}
    public DbSet<User> Users {get;set;}
    public DbSet<Event> Events {get;set;}
    public DbSet<Attendance> Attendances {get;set;}
  }
}