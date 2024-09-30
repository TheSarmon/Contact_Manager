using AutoMapper;
using DataService.Entities;
using DataService.Domain;

namespace DataService.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ContactEntity, Contact>().ReverseMap();
        }
    }
}
