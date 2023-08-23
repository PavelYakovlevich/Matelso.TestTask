using ContactManager.Contract.Services;
using ContactManager.Domain.Models;

namespace ContactManager.Core.Services;

public class ContactService : IContactService
{
    public Task<Guid> CreateAsync(ContactModel contact)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ContactModel> ReadByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<ContactModel> ReadAsync(int skip, int count)
    {
        throw new NotImplementedException();
    }
}