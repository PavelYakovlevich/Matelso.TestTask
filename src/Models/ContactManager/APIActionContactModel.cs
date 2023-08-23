namespace Models.ContactManager;

public class APIActionContactModel
{
    public string Salutation { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string? DisplayName { get; set; }

    public DateTime? Birthday { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }
}