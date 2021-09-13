using Common;
using Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using net_core_backend.Models;
using System;

namespace net_core_backend.Database
{
    public class ApplicationContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public virtual DbSet<Operation> Operations { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<OperationSource> OperationSource { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Operation>()
            .HasQueryFilter(operation => operation.PersonId.ToString() == _httpContextAccessor.HttpContext.User.GetUserId());
            modelBuilder.Entity<Person>(PersonConfigure);
            modelBuilder.Entity<Operation>(OperationConfigure);
            modelBuilder.Entity<Person>().HasData(
                new Person[]
                {
                new Person { Id = Guid.NewGuid(), Login = "admin@gmail.com", Password = "12345", Role = "admin" },
                new Person { Id = Guid.NewGuid(), Login = "qwerty@gmail.com", Password = "55555", Role = "user" },
                });
        }

        public void PersonConfigure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("Person").HasKey(p => p.Id);
            builder.Property(p => p.Login).IsRequired().HasMaxLength(30);
            builder.Property(p => p.Password).IsRequired().HasMaxLength(30);
            builder.Property(p => p.Role).IsRequired().HasMaxLength(30);
        }

        public void OperationConfigure(EntityTypeBuilder<Operation> builder)
        {
            builder.ToTable("Operations").HasKey(p => p.Id);
            builder.Property(p => p.SourceId).IsRequired().HasMaxLength(30);
            builder.Property(p => p.Amount).IsRequired().HasMaxLength(30);
            builder.Property(p => p.PersonId).IsRequired().HasMaxLength(30);
        }

        public void OperationSourceConfigure(EntityTypeBuilder<OperationSource> builder)
        {
            builder.ToTable("OperationSource").HasKey(p => p.Id);
            builder.Property(p => p.Type).IsRequired().HasMaxLength(1);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(30);
        }
    }
}
