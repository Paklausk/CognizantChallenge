using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCompetitionApi.Compilers
{
    class CSharpCompileResult : ICompileResult
    {
        public static CSharpCompileResult Success(string output)
        {
            return new CSharpCompileResult() { Status = CompileResultStatus.Success, Output = output };
        }
        public static CSharpCompileResult Fail(string errorMessage)
        {
            return new CSharpCompileResult() { Status = CompileResultStatus.Fail, Error = errorMessage };
        }

        public CompileResultStatus Status { get; private set; }
        public string Output { get; private set; }
        public string Error { get; private set; }

        private CSharpCompileResult() { }
    }
}
