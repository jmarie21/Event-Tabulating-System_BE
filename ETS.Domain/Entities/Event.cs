

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ETS.Domain.Entities
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [Required]
        public string? EventName { get; set; }
        [Required]
        public string? Description { get; set; }
        [Required]
        public DateTime EventDate { get; set; }
        [Required]
        public string? EventType { get; set; }
        [Required]
        public string? Location { get; set; }
        public string? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }


        public User User { get; set; } = null!;
    }
}
