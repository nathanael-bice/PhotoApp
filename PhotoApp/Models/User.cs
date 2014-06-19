using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PhotoApp.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        public string username { get; set; }

        public byte[] password { get; set; }
    }
}