using Microsoft.EntityFrameworkCore;
using MeuProjetoApiJwt.Models;

namespace MeuProjetoApiJwt.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<User> Users { get; set; }
}