using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjectOneMVC.Core.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace ProjectOneMVC.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }
        public DbSet<Class> Classes { get; set; }
        public DbSet<UserClass> UserClasses { get; set; }
        public DbSet<SchoolUser> SchoolUsers { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<UserClass>()
                .HasKey(x => new { x.ClassId, x.SchoolUserId });
            builder.Entity<Class>()
                .Property(c => c.Price).HasColumnType("Money");

            base.OnModelCreating(builder);
        }

    }

}
