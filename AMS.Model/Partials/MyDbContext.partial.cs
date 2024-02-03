using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AMS.Model.Models
{
    public partial class MyDbContext : IDisposable
    {
        public override void Dispose()
        {
	        Console.WriteLine(" DISPOSED ! " + new string('#', 100));
            //base.Dispose();
		}

		public override ValueTask DisposeAsync()
        {
            //var caller = new StackTrace().GetFrames();
            // foreach (var frame in caller)
            // {
            //     string callerMethodName = frame.ToString();
            //     Console.WriteLine("--------------------> " + callerMethodName);
            //
            //     var methodBase = frame.GetMethod();
            //     Console.WriteLine("The caller method is: " + methodBase.Name);
            // }

            Console.WriteLine(" DISPOSED Async ! "  +new string('#', 100));
            //return base.DisposeAsync();
            return ValueTask.CompletedTask;
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AmsNeo4JNodeLabel>()
                .HasOne(x => x.InheriteFromLabel)
                .WithMany()
                .HasForeignKey(x=>x.InheriteFromLabelId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);
/*
            modelBuilder.Entity<AmsNeo4JNode>()
                .HasOne(x => x.Label)
                .WithOne()
                .OnDelete(DeleteBehavior.NoAction);*/
        }

    }
}
