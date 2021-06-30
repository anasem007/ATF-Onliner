using System;
using System.IO;
using System.Reflection;
using ATF_Onliner.Models;
using ATF_Onliner.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ATF_Onliner.Context
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected UserContext()
        {
            Database.EnsureCreated();
        }
       
        protected UserContext(DbContextOptions<UserContext> options)
            : base(options)
        {
        }

        protected UserContext(DbContextOptions options)
            : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var config = builder.Build();
           // optionsBuilder.UseSqlServer(Configurator.DataBaseConnectionStrings);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Type.GetType("Configuration.EntityFramework.UserContext, Configuration.EntityFramework").GetTypeInfo().Assembly);
        }
    }
}