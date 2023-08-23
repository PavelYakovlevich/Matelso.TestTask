using AutoMapper;
using ContactManager.Contract.Services;
using ContactManager.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Models.ContactManager;

namespace ContactManager.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ContactsController : ControllerBase
{
    private readonly IContactService _service;
    private readonly IMapper _mapper;

    public ContactsController(IContactService service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Create(APIActionContactModel apiModel)
    {
        var contact = _mapper.Map<ContactModel>(apiModel);

        var id = await _service.CreateAsync(contact);

        return CreatedAtAction(nameof(Create), id);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var contact = await _service.ReadByIdAsync(id);

        return Ok(contact);
    }
    
    [HttpGet]
    public async IAsyncEnumerable<APIContactModel> Get([FromQuery] APIContactsFilters filters)
    {
        await foreach (var contact in _service.ReadAsync(filters.Skip, filters.Count))
        {
            yield return _mapper.Map<APIContactModel>(contact);
        }
    }
}