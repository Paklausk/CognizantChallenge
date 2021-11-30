using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCompetitionApi.Compilers
{
    public interface ICompileResult
    {
        CompileResultStatus Status { get; }
        string Output { get; }
        string Error { get; }
    }
}
