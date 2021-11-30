using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProgrammingCompetitionApi.Compilers;
using ProgrammingCompetitionApi.Database;
using ProgrammingCompetitionApi.Database.Models;
using ProgrammingCompetitionApi.Tasks.Base;
using ProgrammingCompetitionApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingCompetitionApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ChallengeController : ControllerBase
    {
        readonly IEnumerable<ICompetitionTask> _competitionTasks;
        readonly ICompiler _compiler;
        readonly Db _database;

        public ChallengeController(IEnumerable<ICompetitionTask> competitionTasks, ICompiler compiler, Db database)
        {
            _competitionTasks = competitionTasks;
            _compiler = compiler;
            _database = database;
        }

        [HttpGet]
        public IEnumerable<CompetitionTaskView> GetTasksList()
        {
            foreach (var competitionTask in _competitionTasks)
            {
                yield return new CompetitionTaskView(competitionTask);
            }
        }
        [HttpPost]
        public async Task<ObjectResult> SubmitTask(TaskSubmission submission)
        {
            var task = _competitionTasks.Where(task => task.Id == submission.TaskId).FirstOrDefault();
            if (task == null)
                return BadRequest("Selected task was not found");

            var fullCode = new StringBuilder();
            fullCode.AppendLine(task.CodeHeader);
            fullCode.AppendLine(task.FunctionHeader);
            fullCode.AppendLine(task.FormatCode(submission.Code));
            fullCode.AppendLine(task.FunctionFooter);
            fullCode.AppendLine(task.CodeFooter);

            var log = new CodeSubmissionLog() {
                ProgramCode = fullCode.ToString(),
                ProgrammersName = submission.DevelopersName,
                TaskId = task.Id,
                TaskName = task.Name,
                TaskDescription = task.Description
            };

            foreach (var test in task.Tests)
            {
                var compileRequest = new CompileRequest(task.ProgrammingLanguage, fullCode.ToString(), test.Input);
                var compileResult = await _compiler.CompileAsync(compileRequest);
                if (compileResult.Status == CompileResultStatus.Fail)
                {
                    log.Error = compileResult.Error;
                    break;
                }
                else if (compileResult.Output != test.ExpectedResult)
                {
                    log.Error = "Invalid application output result";
                    break;
                }
            }

            log.Success = log.Error == null;
            _database.CodeSubmissionLogs.Add(log);
            await _database.SaveChangesAsync();

            var result = new TaskSubmissionResultView()
            {
                Success = log.Success,
                Error = log.Error
            };

            return Ok(result);
        }
    }
}
