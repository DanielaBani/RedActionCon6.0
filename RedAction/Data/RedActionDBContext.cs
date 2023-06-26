using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RedAction.Models;

    public class RedActionDBContext : IdentityDbContext
{
        public RedActionDBContext (DbContextOptions<RedActionDBContext> options)
            : base(options)
        {
        }

        public DbSet<RedAction.Models.Articulo> Articulo { get; set; } = default!;

        public DbSet<RedAction.Models.Usuario>? Usuario { get; set; }
    }
