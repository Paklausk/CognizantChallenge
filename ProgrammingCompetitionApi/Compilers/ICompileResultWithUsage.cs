using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCompetitionApi.Compilers
{
    public interface ICompileResultWithUsage : ICompileResult
    {
        long MemoryUsage { get; }
        long CpuTimeUsage { get; }
    }
}
