using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCompetitionApi.Compilers
{
    public class JDoodleCompileResult : ICompileResultWithUsage
    {
        public long StatusCode { get; set; }
        public string Output { get; set; }
        public string Error { get; set; }
        public string Memory { get; set; }
        public string CpuTime { get; set; }

        public CompileResultStatus Status { get { 
                return StatusCode.Equals(200) ? CompileResultStatus.Success : CompileResultStatus.Fail; 
            } }
        public long MemoryUsage { get { return long.Parse(Memory); } }
        public long CpuTimeUsage { get { return long.Parse(CpuTime); } }
    }
}
