using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace RDLCDesign
{
    public partial class Model : DbContext
    {
        public Model()
            : base("name=FFB_Dbase")
        {
        }

        public virtual DbSet<t_Incentive> t_Incentive { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<t_Incentive>()
                .Property(e => e.Periode)
                .IsUnicode(false);

            modelBuilder.Entity<t_Incentive>()
                .Property(e => e.PMKSID)
                .IsUnicode(false);

            modelBuilder.Entity<t_Incentive>()
                .Property(e => e.SupplierID)
                .IsUnicode(false);

            modelBuilder.Entity<t_Incentive>()
                .Property(e => e.Remarks)
                .IsUnicode(false);
        }
    }
}
