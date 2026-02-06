using AutoMapper;
using CamelRegistry.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CamelRegistry.Logic.Helpers
{
    public class CamelMappingProfile : Profile
    {
        public CamelMappingProfile()
        {
            CreateMap<CamelCreateModel, Camel>();
            CreateMap<CamelUpdateModel, Camel>();
        }
    }
}
