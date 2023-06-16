using System;
using System.ComponentModel.DataAnnotations;

namespace TimeRecorder.Models
{
    public class TimeStampGroup
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateTime { get; set; }
    }
}