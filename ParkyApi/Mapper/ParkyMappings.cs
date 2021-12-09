using AutoMapper;
using ParkyApi.Models;
using ParkyApi.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Mapper
{
    public class ParkyMappings : Profile
    {

        public ParkyMappings()
        {
            CreateMap<NationalPark, NationalParkDTO>().ReverseMap();
            CreateMap<Trail, TrailDTO>().ReverseMap();
            CreateMap<Trail, TrailUpdateDTO>().ReverseMap();
            CreateMap<Trail, TrailInsertDTO>().ReverseMap();
        }
    }
}
