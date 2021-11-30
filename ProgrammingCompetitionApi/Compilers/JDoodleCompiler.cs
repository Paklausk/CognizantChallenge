using ProgrammingCompetitionApi.Configure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProgrammingCompetitionApi.Compilers
{
    public class JDoodleCompiler : IDisposable, ICompiler
    {
        protected class CompileContent
        {
            public string ClientId { get; set; }
            public string ClientSecret { get; set; }
            public string Script { get; set; }
            public string Stdin { get; set; }
            public string Language { get; set; }
            public string VersionIndex { get; set; }
        }
        protected class CompileLanguage
        {
            public string Language { get; set; }
            public string VersionIndex { get; set; }
            public CompileLanguage(string language, string versionIndex)
            {
                Language = language;
                VersionIndex = versionIndex;
            }
        }

        const string URL = "https://api.jdoodle.com";

        HttpClient _client = new HttpClient() { BaseAddress = new Uri(URL) };
        JDoodleSettings _subscription;

        public JDoodleCompiler(JDoodleSettings subscription)
        {
            _subscription = subscription;
        }
        public async Task<long?> CreditsSpent()
        {
            using var httpContent = ToHttpContent(new { ClientId = _subscription.ClientId, ClientSecret = _subscription.Secret });
            using var response = await _client.PostAsync("/v1/credit-spent", httpContent).ConfigureAwait(false);
            var json = await response.Content.ReadAsStringAsync();
            var jsonElement = JsonDocument.Parse(json).RootElement;
            if (jsonElement.TryGetProperty("used", out var usedElement))
                return usedElement.GetInt64();
            return null;
        }
        public async Task<ICompileResult> CompileAsync(ICompileRequest request)
        {
            var compileContent = CompileRequestToCompileContent(request, _subscription);
            using var httpContent = ToHttpContent(compileContent);
            using var response = await _client.PostAsync("/v1/execute", httpContent).ConfigureAwait(false);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                throw new PublicException("Compilation failed at remote endpoint");
            if (!response.Content.Headers.ContentType.MediaType.Equals("application/json", StringComparison.InvariantCultureIgnoreCase))
                throw new PublicException("Compiler returned incomprehensible result");
            if (response.Content.Headers.ContentLength < 1)
                throw new PublicException("Compiler failed to return result");

            var json = await response.Content.ReadAsStringAsync();
            var compileResult = JsonSerializer.Deserialize<JDoodleCompileResult>(json, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            compileResult.Output = compileResult.Output.TrimEnd();

            return compileResult;
        }
        public void Dispose()
        {
            _client.Dispose();
        }

        protected CompileLanguage GetCompileLanguage(ICompileRequest request)
        {
            CompileLanguage language = null;
            switch (request.ProgrammingLanguage)
            {
                case ProgrammingLanguages.CSharp:
                    language = new CompileLanguage("csharp", "3");
                    break;
                case ProgrammingLanguages.Java:
                    language = new CompileLanguage("java", "3");
                    break;
                case ProgrammingLanguages.Javascript:
                    language = new CompileLanguage("nodejs", "3");
                    break;
                case ProgrammingLanguages.PHP:
                    language = new CompileLanguage("php", "3");
                    break;
                case ProgrammingLanguages.Python:
                    language = new CompileLanguage("python3", "3");
                    break;
                default:
                    throw new PublicException("Programming language in compilation request was not set");
            }
            return language;
        }
        protected CompileContent CompileRequestToCompileContent(ICompileRequest request, JDoodleSettings subscription)
        {
            var language = GetCompileLanguage(request);
            var content = new CompileContent()
            {
                ClientId = subscription.ClientId,
                ClientSecret = subscription.Secret,
                Script = request.Script,
                Stdin = request.Input,
                Language = language.Language,
                VersionIndex = language.VersionIndex
            };
            return content;
        }
        protected HttpContent ToHttpContent(object obj)
        {
            var json = JsonSerializer.Serialize(obj, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            return httpContent;
        }
    }
}
