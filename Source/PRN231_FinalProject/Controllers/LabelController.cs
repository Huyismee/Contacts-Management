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
    public class LabelController : ControllerBase
    {
        private UnitOfWork _unitOfWork = new UnitOfWork(new PRN231_FinalProjectContext());
        private IMapper _mapper;

        public LabelController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public IActionResult GetIndex(int id)
        {
            var obj = _unitOfWork.LabelRepository.GetById(id);
            if (obj == null)
            {
                return NotFound("No  Label  were found!");
            }
            return Ok(obj);

        }

        [HttpGet("User/{id}")]
        public IActionResult GetLabelsByUser(int id)
        {
            var obj = _unitOfWork.LabelRepository.GetLabelsByUserId(id);
            if (obj == null)
            {
                return NotFound("No  Label  were found!");
            }
            return Ok(obj);

        }

        [HttpGet]
        public IActionResult Get()
        {
            var obj = _unitOfWork.LabelRepository.GetAll();
            if (!obj.Any())
            {
                return NotFound("No Label were found!");
            }
            var LabelDtos = _mapper.Map<List<LabelDTO>>(obj);
            return Ok(LabelDtos);
        }
        [HttpPost]
        public IActionResult Add([FromBody] LabelDTO input)
        {
            if (ModelState.IsValid)
            {
                var label = _mapper.Map<Label>(input);
                _unitOfWork.LabelRepository.Add(label);
                _unitOfWork.Complete();
                return Ok(label);
            }

            return BadRequest("loi");

        }

        [HttpPost("AddRange")]
        public IActionResult AddRange([FromBody] List<LabelDTO> input)
        {
            if (ModelState.IsValid)
            {
                List<Label> Labels = new List<Label>();
                foreach (var item in input)
                {
                    var label = _mapper.Map<Label>(item);
                    Labels.Add(label);
                }

                _unitOfWork.LabelRepository.AddRange(Labels);
                _unitOfWork.Complete();
                return Ok(input);
            }

            return BadRequest("loi");

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var obj = _unitOfWork.LabelRepository.GetById(id);
            if (obj == null)
            {
                return NotFound("No  Label  were found!");
            }
            _unitOfWork.LabelRepository.Delete(obj);
            _unitOfWork.Complete();
            return Ok("Deleted");
        }

        [HttpPut]
        public IActionResult Update([FromBody] LabelDTO input)
        {
            if (ModelState.IsValid)
            {
                var label = _mapper.Map<Label>(input);
                _unitOfWork.LabelRepository.Update(label);
                _unitOfWork.Complete();
                return Ok(input);
            }

            return BadRequest("loi");

        }
    }
}
