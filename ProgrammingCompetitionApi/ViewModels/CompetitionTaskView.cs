using ProgrammingCompetitionApi.Tasks.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCompetitionApi.ViewModels
{
    public class CompetitionTaskView
    {
        public long Id { get; set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string FunctionHeader { get; protected set; }
        public string FunctionFooter { get; protected set; }

        public CompetitionTaskView(ICompetitionTask competitionTask)
        {
            Id = competitionTask.Id;
            Name = competitionTask.Name;
            Description = competitionTask.Description;
            FunctionHeader = competitionTask.FunctionHeader;
            FunctionFooter = competitionTask.FunctionFooter;
        }
    }
}
