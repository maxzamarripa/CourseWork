﻿using AutoMapper;
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
                .ForMember(c => c.Venue, o => o.MapFrom(m => m.Location.VenueName));

            CreateMap<Talk, TalkModel>();

            CreateMap<Speaker, SpeakerModel>();
        }
    }
}