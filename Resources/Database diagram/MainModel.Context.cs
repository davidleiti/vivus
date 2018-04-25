﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBModel
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MainModelContainer : DbContext
    {
        public MainModelContainer()
            : base("name=MainModelContainer")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<County> Counties { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<RH> RHs { get; set; }
        public virtual DbSet<BloodType> BloodTypes { get; set; }
        public virtual DbSet<PersonStatus> PersonStatuses { get; set; }
        public virtual DbSet<DonationStatus> DonationStatuses { get; set; }
    }
}