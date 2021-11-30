using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCompetitionApi.Compilers
{
    public class CompileRequest : ICompileRequest
    {
        public ProgrammingLanguages ProgrammingLanguage { get; }
        public string Script { get; }
        public string Input { get; }

        public CompileRequest(ProgrammingLanguages language, string script, string input)
        {
            ProgrammingLanguage = language;
            Script = script;
            Input = input;
        }
    }
}
