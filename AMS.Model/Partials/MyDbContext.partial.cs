using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Model.Models
{
    public partial class QonosSchemaContext
    {

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<AmsNeo4JNodeLabel>()
                .HasOne(x => x.ParentLabel)
                .WithMany(x => x.ChildLabels)
                .HasForeignKey(x=>x.ParentLabelId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AmsNeo4JNode>()
                .HasOne(x => x.Label)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);*/
        }

    }
}
