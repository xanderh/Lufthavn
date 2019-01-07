using API.DTO;
using AutoMapper;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Flight, FlightDTO>();
            CreateMap<FlightDTO, Flight>();
            CreateMap<Flight, FlightDetailedDTO>();
            CreateMap<FlightDetailedDTO, Flight>();
        }
    }
}
