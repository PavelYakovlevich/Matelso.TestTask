using ContactManager.Domain.Models;

namespace ContactManager.Contract.Services;

public interface IContactService
{
    Task<Guid> CreateAsync(ContactModel contact);

    Task UpdateAsync(Guid id, UpdateContactModel contact);

    Task DeleteAsync(Guid id);

    Task<ContactModel> ReadByIdAsync(Guid id);

    IAsyncEnumerable<ContactModel> ReadAsync(int skip, int count);
}