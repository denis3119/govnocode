using System;
using System.ComponentModel.DataAnnotations;

namespace govnocode.Models
{
    public class RateComments
    {
        [Key]
        public Int32 Id { get; set; }
        public Int32 IdComment { get; set; }
        public String IdUser { get; set; }
        public Int32 Rate { get; set; }

    }
}