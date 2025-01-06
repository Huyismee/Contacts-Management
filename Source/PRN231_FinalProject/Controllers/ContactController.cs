using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN231_Assignment2_eBookStoreAPI.Interface;
using PRN231_Assignment2_eBookStoreAPI.UnitOfWork;
using PRN231_FinalProject.DTO;
using PRN231_FinalProject.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace PRN231_FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public ContactController(IMapper mapper, IUnitOfWork unitOfWork )
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id}")]
        public IActionResult GetIndex(int id)
        {
            Contact contact = _unitOfWork.ContactRepository.GetContactById(id);
            if (contact == null)
            {
                return NotFound("No contact were found!");
            }
            return Ok(contact);

        }

        [HttpGet("ContactWOLabel/{id}")]
        public IActionResult GetIndexWithoutLabel(int id)
        {
            Contact contact = _unitOfWork.ContactRepository.GetContactByIdWOLabel(id);
            if (contact == null)
            {
                return NotFound("No contact were found!");
            }
            return Ok(contact);

        }

        [HttpGet("ListContact/{UserId}")]
        public IActionResult Get(int UserId)
        {
            _unitOfWork.ContactRepository.DeleteTrashContact();
            var contacts = _unitOfWork.ContactRepository.GetContactsByUserId(UserId);
            if (!contacts.Any())
            {
                return NotFound("No contact were found!");
            }
            return Ok(contacts);
        }
        [HttpPost]
        public IActionResult Add([FromBody] ContactDTO contactDto)
        {
            if (ModelState.IsValid)
            {
                var contact = _mapper.Map<Contact>(contactDto);
                _unitOfWork.ContactRepository.Add(contact);
                _unitOfWork.Complete();
                return Ok(contact);
            }

            return BadRequest("loi");

        }

        [HttpPost("AddRange")]
        public IActionResult AddRange([FromBody] List<ContactDTO> contactDto)
        {
            if (ModelState.IsValid)
            {
                var contact = _mapper.Map<List<Contact>>(contactDto);
                _unitOfWork.ContactRepository.AddRange(contact);
                _unitOfWork.Complete();
                return Ok(contact);
            }

            return BadRequest("loi");

        }

        [HttpDelete("{ContactId}")]
        public IActionResult Delete(int ContactId)
        {
            var contact = _unitOfWork.ContactRepository.GetById(ContactId);
            if (contact == null)
            {
                return NotFound("No contact were found!");
            }
            _unitOfWork.ContactRepository.Delete(contact);
            _unitOfWork.Complete();
            return Ok(contact);
        }

        [HttpDelete("deleteRange")]
        public IActionResult Delete([FromBody] List<int> contactIds)
        {
            var contact = _unitOfWork.ContactRepository.GetAll().ToList();
            var deleteContact = contact.Where(e => contactIds.Contains(e.ContactId)).ToList();
            if (contact.Count < 0)
            {
                return NotFound("No contact were found!");
            }
            _unitOfWork.ContactRepository.DeleteRange(deleteContact);
            _unitOfWork.Complete();
            return Ok(contact);
        }

        [HttpPut]
        public IActionResult Update([FromBody] ContactDTO contactDto)
        {
            Console.WriteLine("hello");
            if (ModelState.IsValid)
            {
                var contact = _mapper.Map<Contact>(contactDto);
                _unitOfWork.ContactRepository.ModifyContact(contact);
                _unitOfWork.Complete();
                return Ok(contact);
            }

            return BadRequest("loi");

        }

        [HttpPut("UpdateRange")]
        public IActionResult UpdateRange([FromBody] List<ContactDTO> contactDto)
        {
            Console.WriteLine("hello");
            if (ModelState.IsValid)
            {
                var contact = _mapper.Map<List<Contact>>(contactDto);
                _unitOfWork.ContactRepository.ModifyContactRange(contact);
                _unitOfWork.Complete();
                return Ok(contact);
            }

            return BadRequest("loi");

        }

    }
}
