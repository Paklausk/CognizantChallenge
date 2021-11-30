using Microsoft.EntityFrameworkCore;
using ProgrammingCompetitionApi.Database.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCompetitionApi.Database
{
    public class Db : DbContext
    {
        public Db(DbContextOptions options) : base(options) { }
        public DbSet<CodeSubmissionLog> CodeSubmissionLogs { get; set; }
    }
}
