using ContactManager.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Data.Context;

public class ContactsDbContext : DbContext
{
    public ContactsDbContext(DbContextOptions<ContactsDbContext> context) :
        base(context)
    {
    }

    public DbSet<Contact> Contacts { get; set; }
}