using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersStore.Dal.Entities;

namespace UsersStore.Dal.EF
{
    public class UsersStoreContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UsersStoreContext(DbContextOptions<UsersStoreContext> options)
            : base(options)
        {
        }

        //protected internal virtual void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //}
    }
}
