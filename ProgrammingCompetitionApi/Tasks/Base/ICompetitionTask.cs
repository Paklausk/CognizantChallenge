using ProgrammingCompetitionApi.Compilers;
using System.Collections.Generic;

namespace ProgrammingCompetitionApi.Tasks.Base
{
    public interface ICompetitionTask
    {
        long Id { get; set; }
        string Name { get; }
        ProgrammingLanguages ProgrammingLanguage { get; }
        string FunctionHeader { get; }
        string FunctionFooter { get; }
        string Description { get; }
        string CodeHeader { get; }
        string CodeFooter { get; }
        List<CompetitionTaskTest> Tests { get; }

        string FormatCode(string rawCode);
    }
}