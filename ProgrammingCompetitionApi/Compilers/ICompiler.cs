using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCompetitionApi.Compilers
{
    public interface ICompiler
    {
        Task<ICompileResult> CompileAsync(ICompileRequest request);
    }
}
