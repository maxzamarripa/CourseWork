﻿using CoreAPI.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAPI.Models
{
    public class CampModel
    {
        /// <summary>
        /// CAMP ENTITY
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        public string Moniker { get; set; }
        public DateTime EventDate { get; set; } = DateTime.MinValue;
        [Range(1,100)]
        public int Length { get; set; } = 1;
        /// <summary>
        /// LOCATION ENTITY
        /// </summary>
        public LocationModel Location { get; set; }
        /// <summary>
        /// TALKS ENTITY
        /// </summary>
        public ICollection<TalkModel> Talks { get; set; }
    }
}
