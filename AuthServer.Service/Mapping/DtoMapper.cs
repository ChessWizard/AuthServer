using AuthServer.Core.Dtos;
using AuthServer.Core.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Mapping
{
    public class DtoMapper : Profile
    {
        public DtoMapper()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<UserAppDto, UserApp>().ReverseMap();
            CreateMap<AddressDto, Address>().ReverseMap();
            CreateMap<CityDto, City>().ReverseMap();
            CreateMap<TownDto, Town>().ReverseMap();
            CreateMap<NeighborhoodDto, Neighborhood>().ReverseMap();
        }
    }
}
