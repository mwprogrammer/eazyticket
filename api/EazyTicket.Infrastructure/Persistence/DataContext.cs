using EazyTicket.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EazyTicket.Infrastructure.Persistence;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public virtual DbSet<Ticket> Tickets { get; set; }
}
