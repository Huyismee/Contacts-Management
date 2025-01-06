using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRN231_Assignment2_eBookStoreAPI.DTO;
using PRN231_Assignment2_eBookStoreAPI.UnitOfWork;
using PRN231_FinalProject.Models;

namespace PRN231_FinalProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UnitOfWork _unitOfWork = new UnitOfWork(new PRN231_FinalProjectContext());
        private IMapper _mapper;
        public UserController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _unitOfWork.UserRepository.GetById(id);
            if (user == null)
            {
                return NotFound("No user were found!");
            }
            return Ok(user);
        }

        [HttpPut]
        public IActionResult Update([FromBody] User obj)
        {
            _unitOfWork.UserRepository.Update(obj);
            _unitOfWork.Complete();
            return Ok("Updated successful");
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDto user)
        {
            User authen = _unitOfWork.UserRepository.FindUserByEmail(user.EmailAddress);
            if (authen == null)
            {
                return NotFound("Incorrect email");
            }

            if (!user.Password.Equals(authen.Password))
            {
                return BadRequest("Incorrect password");
            }

            return Ok(authen);
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterDto user)
        {
            if (!user.Password.Equals(user.ReEnterPassword))
            {
                return BadRequest("Please re-check your password");
            }

            var u = _unitOfWork.UserRepository.FindUserByEmail(user.EmailAddress);
            if (u != null)
            {
                return BadRequest("This email already registered!");
            }

            User signUpUser = new User()
            {
                FullName = user.FullName,
                Email = user.EmailAddress,
                Password = user.Password,
            };

            _unitOfWork.UserRepository.Add(signUpUser);
            _unitOfWork.Complete();
            return Ok(signUpUser);
        }
    }
}
