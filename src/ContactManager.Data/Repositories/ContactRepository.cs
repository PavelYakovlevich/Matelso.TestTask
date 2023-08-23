using AutoMapper;
using ContactManager.Contract.Repositories;
using ContactManager.Data.Context;
using ContactManager.Data.Entities;
using ContactManager.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactManager.Data.Repositories;

public class ContactRepository : IContactRepository
{
    private readonly ContactsDbContext _context;
    private readonly IMapper _mapper;

    public ContactRepository(ContactsDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task CreateAsync(ContactModel contact)
    {
        var entity = _mapper.Map<Contact>(contact);

        await _context.Contacts.AddAsync(entity);
        
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(Guid id, ContactModel model)
    {
        model.Id = id;
        var entity = await _context.Contacts.FirstOrDefaultAsync(contact => contact.Id == id);
        if (entity is null)
        {
            return false;
        }
        
        _mapper.Map(model, entity);

        return await _context.SaveChangesAsync() == 1;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.Contacts.FirstOrDefaultAsync(contact => contact.Id == id);
        if (entity is null)
        {
            return false;
        }
        
        _context.Contacts.Remove(entity);

        return await _context.SaveChangesAsync() != 0;
    }

    public async Task<ContactModel?> SelectByIdAsync(Guid id)
    {
        var entity = await _context.Contacts.AsNoTracking()
            .FirstOrDefaultAsync(contact => contact.Id == id);

        return _mapper.Map<ContactModel?>(entity);
    }

    public async Task<ContactModel?> SelectByEmailAsync(string email)
    {
        var entity = await _context.Contacts.AsNoTracking()
            .FirstOrDefaultAsync(contact => contact.Email == email);

        return _mapper.Map<ContactModel?>(entity);
    }

    public IAsyncEnumerable<ContactModel> SelectAsync(int skip, int count)
    {
        return _context.Contacts.AsNoTracking()
            .OrderByDescending(contact => contact.CreationTimestamp)
            .Skip(skip)
            .Take(count)
            .Select(contact => _mapper.Map<ContactModel>(contact))
            .AsAsyncEnumerable();
    }
}