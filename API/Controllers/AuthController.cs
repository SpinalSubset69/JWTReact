using API.Dtos;
using API.Helpers;
using API.Models;
using AutoMapper;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly JWTService _jwtService;

        public AuthController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration config, JWTService jwtService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _config = config;
            _jwtService = jwtService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterDto registerDto)
        {
            try
            {
                var user = _mapper.Map<RegisterDto, User>(registerDto);
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                var userSaved = await _unitOfWork.User.SaveAndGetUser(user);
                var userToSend = _mapper.Map<User, UserDto>(userSaved);
                return Created("success", userToSend);
            }
            catch(Exception ex)
            {
                return Ok(new { message = "Error", error = ex.Message });
            }            
        }

        [HttpGet]
        public async Task<IActionResult> GetUseryByToken()
        {
            try
            {
                //First wee need to get the cookie
                var jwt = Request.Cookies["jwt"];
                var token = _jwtService.VerifyJWT(jwt, _config.GetSection("Cryptography").GetSection("SecurityKey").Value);
                var userFromDb = await _unitOfWork.User.FindAsync(x => x.Id == Convert.ToInt32(token.Issuer));

                var userToSend = _mapper.Map<User, UserDto>(userFromDb);
                return Ok(new
                {
                    mesage = "Success",
                    data = userToSend
                });
            }
            catch(Exception ex)
            {
                return Unauthorized();
            }
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

            //GeneratesJWT
            var jwt = _jwtService.GenerateJWT(user.Id, _config.GetSection("Cryptography").GetSection("SecurityKey").Value);

            //Append Cooki to the response, httponly = can acces to the cookie
            Response.Cookies.Append("jwt", jwt, new Microsoft.AspNetCore.Http.CookieOptions { HttpOnly = true});

            return Ok(new
            {
                message = "Success",                               
            });
        }

        [HttpPost("logout")]
        public IActionResult LogOut()
        {
            Response.Cookies.Delete("jwt");

            return Ok(new
            {
                message = "Success"
            });
        }
    }
}
