using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Data.Entities;

[Index(nameof(Email), IsUnique = true)]
public class Contact
{
    [Key]
    public Guid Id { get; set; }
    
    public string Salutation { get; set; }

    [MaxLength(50)]
    public string FirstName { get; set; }

    [MaxLength(50)]
    public string LastName { get; set; }

    [MaxLength(50)]
    public string? DisplayName { get; set; }

    public DateTime Birthday { get; set; }

    public DateTime CreationTimestamp { get; set; }
    
    public DateTime LastChangeTimestamp  { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }
}