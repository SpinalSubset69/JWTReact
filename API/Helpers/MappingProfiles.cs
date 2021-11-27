using API.Dtos;
using API.Models;
using AutoMapper;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, RegisterDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();    
        }
    }
}
