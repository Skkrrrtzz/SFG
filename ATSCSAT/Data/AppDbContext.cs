﻿using Microsoft.EntityFrameworkCore;
using PIMES_DMS.Models;

namespace PIMES_DMS.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {

        }

        public DbSet<IssueModel> IssueDb { get; set; }

        public DbSet<AccountsModel> AccountsDb { get; set; }

        public DbSet<ERModel> ERDb { get; set; }

        public DbSet<ActionModel> ActionDb { get; set; }

        public DbSet<Vermodel> VerDb { get; set; }

        public DbSet<SmtpEmailsModel> SEDb { get; set; }

        public DbSet<ART_8DModel> ART_8D { get; set; }

        public DbSet<TESModel> TESDb { get; set; }

        public DbSet<NotifModel> NotifDb { get; set; }

        public DbSet<RMAModel> RMADb { get; set; }

        public DbSet<TargetDateModel> TDDb { get; set; }

        public DbSet<_8DModel> _8DDb { get; set; }

        public DbSet<DefinitionsModel> DefDb { get; set; } 

        public DbSet<AnnouncementModel> AnnDb { get; set; }
    }
}
