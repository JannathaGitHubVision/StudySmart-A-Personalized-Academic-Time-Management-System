﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SampleProgPoe
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class UsersInfoEntities : DbContext
    {
        public UsersInfoEntities()
            : base("name=UsersInfoEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<CaptureModule> CaptureModules { get; set; }
        public virtual DbSet<ModifiedHoursTable> ModifiedHoursTables { get; set; }
        public virtual DbSet<user> users { get; set; }
    }
}