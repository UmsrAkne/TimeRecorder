using System;
using System.ComponentModel.DataAnnotations;

namespace TimeRecorder.Models
{
    public class TimeStamp
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public int GroupId { get; set; }

        [Required]
        public DateTime DateTime { get; set; } = DateTime.Now;

        [Required]
        public string Comment { get; set; } = string.Empty;
    }
}