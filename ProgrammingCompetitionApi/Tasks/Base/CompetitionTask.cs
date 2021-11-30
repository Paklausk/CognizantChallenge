using ProgrammingCompetitionApi.Compilers;
using ProgrammingCompetitionApi.Tasks.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ProgrammingCompetitionApi.Tasks.Base
{
    public class CompetitionTask : ICompetitionTask
    {
        public long Id { get; set; }
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public ProgrammingLanguages ProgrammingLanguage { get; protected set; }
        public string FunctionHeader { get; protected set; }
        public string FunctionFooter { get; protected set; }
        public string CodeHeader { get; protected set; }
        public string CodeFooter { get; protected set; }
        public List<CompetitionTaskTest> Tests { get; protected set; } = new List<CompetitionTaskTest>();

        public CompetitionTask(string name, string description, ProgrammingLanguages programmingLanguage, string functionHeader, string functionFooter, string codeHeader, string codeFooter, CompetitionTaskTest[] tests)
        {
            Name = name;
            Description = description;
            ProgrammingLanguage = programmingLanguage;
            FunctionHeader = functionHeader;
            FunctionFooter = functionFooter;
            CodeHeader = codeHeader;
            CodeFooter = codeFooter;
            if (tests != null)
                Tests.AddRange(tests);
        }

        public virtual string FormatCode(string rawCode)
        {
            return rawCode;
        }
    }
}
