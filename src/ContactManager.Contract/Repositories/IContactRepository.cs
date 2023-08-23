using ContactManager.Domain.Models;

namespace ContactManager.Contract.Repositories;

public interface IContactRepository
{
    Task<Guid> CreateAsync(ContactModel contact);

    Task UpdateAsync(Guid id);

    Task DeleteAsync(Guid id);

    Task<ContactModel> SelectByIdAsync(Guid id);

    IAsyncEnumerable<ContactModel> SelectAsync(int skip, int count);
}