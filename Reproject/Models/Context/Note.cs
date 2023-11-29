using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Reproject.Models.Context
{
    [Table("notes")]
    public class Note
    {
            [Key]
            [Column("id")]
            public Guid Id { get; set; }

            [Required]
            [Column("title")]
            public string Title { get; set; }

            [Column("description")]
            public string? Description { get; set; }

            [Required]
            [Column("created_at")]
            public DateTime CreatedAt { get; set; }

            [Column("type")]
            public string? Type { get; set; }

            [Column("execute_at")]
            public DateTime? Execute_at { get; set; }

            [Required]
        [Column("is_done")]
        public bool IsDone { get; set; }
    }
}
