using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingCompetitionApi.Compilers
{
    public class CSharpCoreCompiler : ICompiler
    {
        MetadataReference[] _references;
        CSharpCompilationOptions _compilationOptions;

        public CSharpCoreCompiler()
        {
            string baseRefPath = Path.GetDirectoryName(typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location);
            var refPaths = GetDllsToReference();/* new[] {
                typeof(System.Object).GetTypeInfo().Assembly.Location,
                typeof(Console).GetTypeInfo().Assembly.Location,
                Path.Combine(baseRefPath, "System.Runtime.dll"),
                Path.Combine(baseRefPath, "mscorlib.dll"),
                Path.Combine(baseRefPath, "System.Core.dll"),
                Path.Combine(baseRefPath, "System.Linq.dll"),
                Path.Combine(baseRefPath, "System.Collections.dll"),
                Path.Combine(baseRefPath, "system.private.corelib.dll")
            };*/
            _references = refPaths.Select(r => MetadataReference.CreateFromFile(r)).ToArray();
            _compilationOptions = new CSharpCompilationOptions(OutputKind.ConsoleApplication);
        }
        public static string From(string shortDllName)
        {
            string dllString = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES").ToString();
            var dlls = dllString.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string dll = dlls.Single(d => d.Contains(shortDllName, StringComparison.OrdinalIgnoreCase));
            return dll;
        }
        public IEnumerable<string> GetDllsToReference()
        {
            // add a big pile of .net dlls, all using AppContext.GetData( "TRUSTED_PLATFORM_ASSEMBLIES" )
            List<string> dllsToReturn = new List<string>()
     {
        "mscorlib.dll",
        "system.runtime.dll",
        "system.private.corelib.dll",
        "netstandard.dll",
        "System.Text.RegularExpressions.dll",
        "system.linq.dll",
        "system.net.dll",
        "system.data.dll",
        "System.Collections.dll",
        "System.IO.dll",
        "System.IO.FileSystem.dll",
        "System.Runtime.Extensions.dll",
        "System.CodeDom.dll",
        "System.Private.Uri.dll",
        "System.Diagnostics.Process.dll",
        "System.Drawing.Primitives.dll",
        "System.Data.Common.dll",
        "System.ComponentModel.Primitives.dll",
        "Microsoft.VisualBasic.Core.dll",
        "System.ComponentModel.TypeConverter.dll",
        "System.Linq.Expressions.dll",
        "System.ObjectModel.dll",
        "System.Diagnostics.Tools.dll",
        "System.Net.Http.dll",
        "System.ComponentModel.dll",
        "Microsoft.Win32.Primitives.dll",
        "System.Collections.Specialized.dll",
        "System.Collections.NonGeneric.dll",
        "System.Diagnostics.FileVersionInfo.dll",
        "System.Resources.ResourceManager.dll",
        "System.Net.Requests.dll",
        "System.Net.Mail.dll",
        "System.Linq.Parallel.dll",
        "System.Net.Primitives.dll",
        "System.Resources.Writer.dll",
        "System.Collections.Concurrent.dll",
        "System.Collections.Immutable.dll",
        "System.Security.Claims.dll",
        "System.Console.dll",
        "System.Web.HttpUtility.dll",
        "System.Runtime.InteropServices.dll",
        "System.Threading.Tasks.Parallel.dll",
        "WindowsBase.dll",
        "Microsoft.Win32.Registry.dll",
        "System.Security.Principal.Windows.dll",
     };

            foreach (var dll in dllsToReturn)
            {
                yield return From(dll);
            }
        }
        public Task<ICompileResult> CompileAsync(ICompileRequest request)
        {
            SyntaxTree codeTree = CSharpSyntaxTree.ParseText(request.Script);
            string assemblyName = Path.GetRandomFileName();

            CSharpCompilation compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { codeTree },
                references: _references,
                options: _compilationOptions
            );

            using var ms = new MemoryStream();
            EmitResult result = compilation.Emit(ms);

            if (!result.Success)
            {
                var errors = result.Diagnostics.Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error);

                var sb = new StringBuilder();
                errors.ToList().ForEach(error => sb.AppendLine(error.GetMessage()));
                ICompileResult compileResult = CSharpCompileResult.Fail(sb.ToString());
                return Task.FromResult(compileResult);
            }

            ms.Seek(0, SeekOrigin.Begin);
            string tmpFilePath = Path.GetTempPath() + Guid.NewGuid().ToString() + ".exe";
            using (FileStream file = new FileStream(tmpFilePath, FileMode.Create, FileAccess.Write))
            {
                ms.CopyTo(file);
            }

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = tmpFilePath;
            startInfo.Arguments = request.Input;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            var process = Process.Start(startInfo);
            if (!process.WaitForExit(5000))
            {
                process.Kill();
                throw new PublicException("Compiled application failed to exit in time");
            }
            var output = process.StandardOutput.ReadToEnd();

            ICompileResult successResult = CSharpCompileResult.Success(output);
            return Task.FromResult(successResult);

            /*var type = assembly.GetTypes().FirstOrDefault();
            if (type == null)
                throw new PublicException("No compiled assembly types found");
            var ms2 = type.GetMethods();
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
            }*/
        }
    }
}
