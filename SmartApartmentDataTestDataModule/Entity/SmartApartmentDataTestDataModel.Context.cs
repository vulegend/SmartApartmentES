﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmartApartmentDataTestDataModule.Entity
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SmartApartmentEntities : DbContext
    {
        public SmartApartmentEntities()
            : base("name=SmartApartmentEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<@class> classes { get; set; }
        public virtual DbSet<faction> factions { get; set; }
        public virtual DbSet<guild> guilds { get; set; }
        public virtual DbSet<player> players { get; set; }
        public virtual DbSet<race> races { get; set; }
        public virtual DbSet<guild_members> guild_members { get; set; }
        public virtual DbSet<user> users { get; set; }
    }
}
