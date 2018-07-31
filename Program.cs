using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;

namespace psqlquery
{
    class Todos {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get;set; }
    }

    class mosheaContext : DbContext{
        public mosheaContext()
        {
        }
        public mosheaContext(DbContextOptions<mosheaContext> options)
            : base(options)
        {}

        protected override void OnConfiguring(DbContextOptionsBuilder builder) {
            if (!builder.IsConfigured)
                builder.UseNpgsql("Host=localhost;Database=moshea;Username=moshea");
        }

        protected override void OnModelCreating(ModelBuilder model) {
            model.Entity<Todos>(entity =>
            {
                entity.ToTable("todos");
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.IsComplete).HasColumnName("iscomplete");
            });
        }

        public DbSet<Todos> Todos { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new mosheaContext())
            {
                db.Todos.ToList().ForEach(t=>{
                    Console.WriteLine($"{t.Id} {t.Description}");
                });
            }
        }
    }
}
