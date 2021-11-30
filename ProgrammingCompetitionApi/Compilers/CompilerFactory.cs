using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ProgrammingCompetitionApi.Configure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCompetitionApi.Compilers
{
    public class CompilerFactory
    {
        JDoodleSettings _settings;

        public string CompilerType { get; set; }

        public CompilerFactory(IConfiguration configuration, IOptions<JDoodleSettings> settings)
        {
            CompilerType = configuration.GetValue<string>("CompilerType");
            _settings = settings.Value;
        }
        public ICompiler Create()
        {
            ICompiler compiler = null;
            switch (CompilerType)
            {
                case "core":
                    //compiler = new CSharpCoreCompiler();
                    throw new Exception("Executing using reflection works, but that's not what we want and standalone launch doesn't, because of inaccurate list of references");
                case "framework":
                    //compiler = new CSharpFrameworkCompiler();
                    throw new Exception("Won't work in .NET Core project");
                case "jdoodle":
                    compiler = new JDoodleCompiler(_settings);
                    break;
            }
            return compiler;
        }
    }
}
