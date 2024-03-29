﻿using System.Reflection.Metadata.Ecma335;
using ContactManager.Contract.Repositories;
using ContactManager.Contract.Services;
using ContactManager.Domain.Models;
using Exceptions;
using Serilog;

namespace ContactManager.Core.Services;

public class ContactService : IContactService
{
    private readonly IContactRepository _repository;

    public ContactService(IContactRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<Guid> CreateAsync(ContactModel contact)
    {
        contact = PrepareForCreate(contact);
        
        if (await _repository.SelectByEmailAsync(contact.Email) is not null)
        {
            throw new AlreadyExistsException($"Contact with email '{contact.Email}' exists");
        }

        await _repository.CreateAsync(contact);
        
        Log.Information("Contact with id '{id}' was saved. Contact: {@contact}", contact.Id, contact);

        return contact.Id;
    }

    public async Task UpdateAsync(Guid id, ContactModel contact)
    {
        contact = PrepareForUpdate(contact);

        var targetContact = await _repository.SelectByEmailAsync(contact.Email);
        if (targetContact is not null && targetContact.Id != id)
        {
            throw new AlreadyExistsException($"Contact with email '{contact.Email}' exists");
        }
        
        if (!await _repository.UpdateAsync(id, contact))
        {
            throw new NotFoundException($"Contact with id '{id}' was not found");
        }
        
        Log.Information("Contact with id '{id}' was updated. Contact: {@contact}", id, contact);
    }

    public async Task DeleteAsync(Guid id)
    {
        if (!await _repository.DeleteAsync(id))
        {
            throw new NotFoundException($"Contact with id '{id}' was not found");
        }
        
        Log.Information("Contact with id '{id}' was deleted.", id);
    }

    public async Task<ContactModel> ReadByIdAsync(Guid id)
    {
        var contact = await _repository.SelectByIdAsync(id) ??
                      throw new NotFoundException($"Contact with id '{id}' was not found");
        
        Log.Information("Contact with id '{id}' was found. Contact: {@contact}", id, contact);

        return contact;
    }

    public IAsyncEnumerable<ContactModel> ReadAsync(int skip, int count)
    {
        return _repository.SelectAsync(skip, count);
    }

    private static ContactModel PrepareForCreate(ContactModel contact)
    {
        contact.Id = Guid.NewGuid();
        contact.CreationTimestamp = DateTime.UtcNow;
        contact.LastChangeTimestamp = contact.CreationTimestamp;

        return PrepareForUpdate(contact);
    }
    
    private static ContactModel PrepareForUpdate(ContactModel contact)
    {
        contact.Email = contact.Email.Trim().ToLower();
        contact.LastChangeTimestamp = DateTime.UtcNow;
        
        if (string.IsNullOrEmpty(contact.DisplayName) || string.IsNullOrWhiteSpace(contact.DisplayName))
        {
            contact.DisplayName = $"{contact.Salutation} {contact.FirstName} {contact.LastName}";
        }

        return contact;
    }
}