using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN231_Assignment2_eBookStoreAPI.UnitOfWork;
using PRN231_FinalProject.DTO;
using PRN231_FinalProject.Models;

namespace PRN231_FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsLabelController : ControllerBase
    {
        private UnitOfWork _unitOfWork = new UnitOfWork(new PRN231_FinalProjectContext());
        private IMapper _mapper;

        public ContactsLabelController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public IActionResult GetIndex(int id)
        {
            var obj = _unitOfWork.ContactsLabelRepository.GetById(id);
            if (obj == null)
            {
                return NotFound("No Contact Label  were found!");
            }
            return Ok(obj);

        }

        [HttpGet]
        public IActionResult Get()
        {
            var obj = _unitOfWork.ContactsLabelRepository.GetAll();
            if (!obj.Any())
            {
                return NotFound("No Contact Label were found!");
            }
            return Ok(obj);
        }
        [HttpPost]
        public IActionResult Add([FromBody] ContactsLabel input)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ContactsLabelRepository.Add(input);
                _unitOfWork.Complete();
                return Ok(input);
            }

            return BadRequest("loi");

        }
        [HttpPost("AddRange")]
        public IActionResult AddRange([FromBody] List<ContactsLabel> input)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ContactsLabelRepository.AddRange(input);
                _unitOfWork.Complete();
                return Ok(input);
            }

            return BadRequest("loi");

        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var obj = _unitOfWork.ContactsLabelRepository.GetById(id);
            if (obj == null)
            {
                return NotFound("No  Contact Label were found!");
            }
            _unitOfWork.ContactsLabelRepository.Delete(obj);
            _unitOfWork.Complete();
            return Ok("Deleted");
        }

        [HttpPut]
        public IActionResult Update([FromBody] ContactsLabel input)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.ContactsLabelRepository.Update(input);
                _unitOfWork.Complete();
                return Ok(input);
            }

            return BadRequest("loi");

        }
    }
}
