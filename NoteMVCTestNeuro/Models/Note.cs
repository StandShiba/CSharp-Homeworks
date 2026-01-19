using System;
using System.ComponentModel.DataAnnotations;

namespace NoteMVCTestNeuro.Models
{
    public class Note
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; } = "";

        [StringLength(4000)]
        public string? Content { get; set; }

        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;
    }
}