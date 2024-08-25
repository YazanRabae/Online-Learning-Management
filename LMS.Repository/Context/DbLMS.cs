﻿using LMS.Domain.Entities.Courses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Repository.Context
{
    public class DbLMS : IdentityDbContext
    {
        public DbLMS(DbContextOptions<DbLMS> option) : base(option) { }
        public DbSet<Course> Courses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Course");
            });
        }
    }
}
