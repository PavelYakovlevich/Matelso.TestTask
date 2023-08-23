using ContactManager.Contract.Repositories;
using ContactManager.Domain.Models;

namespace ContactManager.Data.Repositories;

public class ContactRepository : IContactRepository
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

    public Task<ContactModel> SelectByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<ContactModel> SelectAsync(int skip, int count)
    {
        throw new NotImplementedException();
    }
}