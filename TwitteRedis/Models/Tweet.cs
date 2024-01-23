using System.ComponentModel.DataAnnotations;

namespace TwitteRedis.Models
{
    public class Tweet
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [StringLength(280, MinimumLength = 1)]
        public string Description { get; set; }

        public DateTime CreatedOnDate { get; set; }

        public Tweet()
        {
            Id = Guid.NewGuid();
        }

        public Tweet(string description)
        {
            Id = Guid.NewGuid();
            Description = description;
            CreatedOnDate = DateTime.Now;
        }

        public Tweet(string description, DateTime createdOnDate)
        {
            Id = Guid.NewGuid();
            Description = description;
            CreatedOnDate = createdOnDate;
        }
    }
}
