using AutoMapper;
using Api.Entities.Models;
using Api.Entities.DTOs;
using API.Extentions;

namespace Api.Entities.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
                    //this is to bring the photo url from user's photo list when isMain= ture
                    // and store that in memberDto.PhotoUrl
                    .ForMember(dest => dest.PhotoUrl,
                    opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p=>p.IsMain).Url))
                    //also in same case of Age because age in membeDto is int & the dateofbirth is datetime ,
                    //we need to tell automapper how to get the age value using static function 
                    //and store it in Age
                    .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalcuateAge()));
            CreateMap<Photo, PhotoDto>();
        }
    }
}