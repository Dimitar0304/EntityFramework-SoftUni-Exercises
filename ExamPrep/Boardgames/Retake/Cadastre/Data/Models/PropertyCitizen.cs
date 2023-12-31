﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cadastre.Data.Models
{
    public class PropertyCitizen
    {
        public PropertyCitizen()
        {
            
        }
        [Required]
        [ForeignKey(nameof(Property))]
        public int PropertyId { get; set; }
        public Property Property { get; set; }
        [Required]
        [ForeignKey(nameof(Citizen))]
        public int CitizenId { get; set; }
        public Citizen Citizen { get; set; }
    }
}