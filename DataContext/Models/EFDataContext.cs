using DataContext.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DataContext.DataContext
{
    public class EFDataContext : DbContext
    {
        public DbSet<m_PMKS> EF_PMKS { get; set; }

        //public DbSet<Designation> Designations { get; set; }

        //public DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"data source=xxx; initial catalog=EFCrudDemo;persist security info=True;user id=sa;password=ssss;");
        }

    }
}