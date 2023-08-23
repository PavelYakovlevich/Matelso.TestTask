namespace Models.ContactManager;

public class APIContactModel
{
    public Guid Id { get; set; }
    
    public string Salutation { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string? DisplayName { get; set; }

    public DateTime? Birthday { get; set; }

    public DateTime CreationTimestamp { get; set; }
    
    public DateTime LastChangeTimestamp  { get; set; }

    public bool NotifyHasBirthdaySoon { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }
}