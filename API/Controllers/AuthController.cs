using API.Dtos;
using API.Helpers;
using API.Models;
using AutoMapper;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("{controller}")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public AuthController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterDto registerDto)
        {
            var user = _mapper.Map<RegisterDto, User>(registerDto);
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            var userSaved = await _unitOfWork.User.SaveAndGetUser(user);
            return Created("success", userSaved); 
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int? id)
        {
            var user = await _unitOfWork.User.FindByIdAsync(x => x.Id == id);
            var securityKey = _config.GetSection("Cryptography").GetSection("SecurityKey").Value;
            //var decrypted = user.Password = BCrypt.Net.BCrypt.Verify(user.Password, "Resumiendo69%");
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> SignIng([FromBody] LoginDto loginDto)
        {
            var user = await _unitOfWork.User.GetUserByEmailAsync(loginDto.Email);

            if(user == null)
            {
                return BadRequest(new { message = "Invalid Credentials" });
            }

            if(!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
            {
                return BadRequest(new { message = "Invalid Credentials" });
            }

            var userToSend = _mapper.Map<User, UserDto>(user);

            return Ok(userToSend);
        }
    }
}
