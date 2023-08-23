using ContactManager.Contract.Repositories;
using ContactManager.Domain.Models;

namespace ContactManager.Data.Repositories;

public class ContactRepository : IContactRepository
{
    public Task CreateAsync(ContactModel contact)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Guid id, UpdateContactModel model)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ContactModel?> SelectByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<ContactModel?> SelectByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<ContactModel> SelectAsync(int skip, int count)
    {
        throw new NotImplementedException();
    }
}