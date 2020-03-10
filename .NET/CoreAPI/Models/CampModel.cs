﻿using CoreAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAPI.Models
{
    public class CampModel
    {
        /// <summary>
        /// CAMP ENTITY
        /// </summary>
        public string Name { get; set; }
        public string Moniker { get; set; }
        public DateTime EventDate { get; set; } = DateTime.MinValue;
        public int Length { get; set; } = 1;
        /// <summary>
        /// LOCATION ENTITY
        /// </summary>
        public string Venue { get; set; }
        public string LocationAddress1 { get; set; }
        public string LocationAddress2 { get; set; }
        public string LocationAddress3 { get; set; }
        public string LocationCityTown { get; set; }
        public string LocationStateProvince { get; set; }
        public string LocationPostalCode { get; set; }
        public string LocationCountry { get; set; }
        /// <summary>
        /// TALKS ENTITY
        /// </summary>
        public ICollection<TalkModel> Talks { get; set; }
    }
}
