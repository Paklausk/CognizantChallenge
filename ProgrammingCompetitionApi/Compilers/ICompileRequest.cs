using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCompetitionApi.Compilers
{
    public interface ICompileRequest
    {
        ProgrammingLanguages ProgrammingLanguage { get; }
        public string Script { get; }
        public string Input { get; }
    }
}
