using Microsoft.EntityFrameworkCore;
using SipayApi.Models;
using System.Collections.Generic;

namespace SipayApi.Data
{
    public class StudentDbContext : DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
 
    }
}
