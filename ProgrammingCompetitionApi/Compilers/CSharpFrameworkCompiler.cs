using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingCompetitionApi.Compilers
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
    public class CSharpFrameworkCompiler : IDisposable, ICompiler
    {
        CSharpCodeProvider _codeProvider = new CSharpCodeProvider();
        CompilerParameters _compilerParameters;

        public CSharpFrameworkCompiler()
        {
            _compilerParameters = new CompilerParameters()
            {
                GenerateInMemory = true,
                GenerateExecutable = false,
                TreatWarningsAsErrors = false,
                IncludeDebugInformation = false
            };

            string[] references = { "mscorlib.dll", "System.Core.dll", "System.dll", "System.Collections.dll", "System.Console.dll", "System.Linq.dll" };
            _compilerParameters.ReferencedAssemblies.AddRange(references);
        }

        public Task<ICompileResult> CompileAsync(ICompileRequest request)
        {
            CompilerResults compile = _codeProvider.CompileAssemblyFromSource(_compilerParameters, request.Script);
            if (compile.Errors.HasErrors)
            {
                var sb = new StringBuilder();
                compile.Errors.Cast<CompilerError>().ToList().ForEach(error => sb.AppendLine(error.ErrorText));
                ICompileResult compileResult = CSharpCompileResult.Fail(sb.ToString());
                return Task.FromResult(compileResult);
            }

            var module = compile.CompiledAssembly.GetModules().FirstOrDefault();
            if (module == null)
                throw new PublicException("No compiled assembly modules found");
            var type = module.GetTypes().FirstOrDefault();
            if (type == null)
                throw new PublicException("No compiled assembly types found");
            var method = type.GetMethods(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public).FirstOrDefault();
            if (method == null)
                throw new PublicException("No compiled assembly static methods found in first type");

            try
            {
                var parameters = !string.IsNullOrEmpty(request.Input) ? request.Input.Split(' ') : null;
                var methodOutput = method.Invoke(null, parameters);
                string output = methodOutput != null ? methodOutput.ToString() : null;

                ICompileResult compileResult = CSharpCompileResult.Success(output);
                return Task.FromResult(compileResult);
            }
            catch (Exception e)
            {
                ICompileResult compileResult = CSharpCompileResult.Fail($"Error during code execution: {e.Message}");
                return Task.FromResult(compileResult);
            }
        }
        public void Dispose()
        {
            _codeProvider.Dispose();
        }
    }
}
