using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCompetitionApi.Database.Models
{
    [Index(nameof(TaskId), IsUnique = false)]
    public class CodeSubmissionLog
    {
        [Key]
        public long Id { get; set; }

        public long? TaskId { get; set; }
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }

        public string ProgrammersName { get; set; }
        public string ProgramCode { get; set; }

        public bool Success { get; set; } = false;
        public string Error { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
    }
}
