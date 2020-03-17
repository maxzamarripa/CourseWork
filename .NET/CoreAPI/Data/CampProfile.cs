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
            CreateMap<Camp, CampModel>();
            CreateMap<Talk, TalkModel>();
            CreateMap<Speaker, SpeakerModel>();
            CreateMap<Location, LocationModel>();

            //For post operation
            CreateMap<CampModel, Camp>();
            CreateMap<TalkModel, Talk>();
            CreateMap<SpeakerModel, Speaker>();
            CreateMap<LocationModel, Location>();
        }
    }
}
