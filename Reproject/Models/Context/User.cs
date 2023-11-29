using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reproject.Models.Context
{
    [Table("users")]
    public class User
    {
        //public User(string login, string password, string name)
        //{
        //    UserLogin = login;
        //    UserPassword = password;
        //    UserName = name;
        //}
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Required]
        [Column("login")]
        public string UserLogin { get; set; }
        [Required]
        [Column("username")]
        public string UserName { get; set; }
        [Required]
        [Column("password")]
        public string UserPassword { get; set; }

    }
}
