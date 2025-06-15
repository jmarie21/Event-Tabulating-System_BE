using System.ComponentModel.DataAnnotations;


namespace ETS.Domain.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? PasswordHash { get; set; }
        public string? UserRole { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }


        public ICollection<Event> Events { get; set; } = new List<Event>();
    }
}
