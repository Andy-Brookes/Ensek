using AutoMapper;
using Ensek.Test.Application.Models;
using Ensek.Test.Domain.Entities;

namespace Ensek.Test.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<MeterReadingModel, MeterReading>();
        }
    }
}
