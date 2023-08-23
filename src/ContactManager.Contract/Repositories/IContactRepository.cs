using ContactManager.Domain.Models;

namespace ContactManager.Contract.Repositories;

public interface IContactRepository
{
    Task CreateAsync(ContactModel contact);

    Task<bool> UpdateAsync(Guid id, ContactModel model);

    Task<bool> DeleteAsync(Guid id);

    Task<ContactModel?> SelectByIdAsync(Guid id);
    
    Task<ContactModel?> SelectByEmailAsync(string email);

    IAsyncEnumerable<ContactModel> SelectAsync(int skip, int count);
}