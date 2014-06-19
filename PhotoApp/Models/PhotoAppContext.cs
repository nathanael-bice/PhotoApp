using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PhotoApp.Models
{
    public class PhotoAppContext : DbContext
    {
        public virtual DbSet<User> users { get; set; }
    }
}