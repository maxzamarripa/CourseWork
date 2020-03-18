using AutoMapper;
using CoreAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAPI.Data
{
    public class CampProfile : Profile
    {
        public CampProfile()
        {
            CreateMap<Camp, CampModel>()
                .ReverseMap();

            CreateMap<Talk, TalkModel>()
                .ReverseMap()
                //this needs to be done after ReverseMap()
                .ForMember(t => t.Camp, opt => opt.Ignore())
                .ForMember(t => t.Speaker, opt => opt.Ignore());

            CreateMap<Speaker, SpeakerModel>()
                .ReverseMap();

            CreateMap<Location, LocationModel>()
                .ReverseMap();
        }
    }
}
