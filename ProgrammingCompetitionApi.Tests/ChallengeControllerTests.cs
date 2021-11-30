using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProgrammingCompetitionApi.Controllers;
using ProgrammingCompetitionApi.Tasks.Base;
using System.Collections.Generic;
using System.Linq;

namespace ProgrammingCompetitionApi.Tests
{
    [TestClass]
    public class ChallengeControllerTests
    {
        [TestMethod]
        public void GetTasksList_ShouldReturnTaskList()
        {
            var tasks = GenerateTasks();
            var controller = new ChallengeController(tasks, null, null);
            var receivedTasks = controller.GetTasksList();

            Assert.IsNotNull(receivedTasks, "Received task list from controller equals null");
            Assert.AreEqual(tasks.Count(), receivedTasks.Count(), "Input and output tasks count differs");
        }
        [TestMethod]
        public void GetTasksList_ShouldReturnValidTasks()
        {
            var tasks = GenerateTasks();
            var controller = new ChallengeController(tasks, null, null);
            var receivedTasks = controller.GetTasksList();

            foreach (var task in receivedTasks) Assert.IsNotNull(task, "Received task from controller equals null");

            foreach (var task in tasks)
            {
                var receivedTask = receivedTasks.Where(t => task.Id.Equals(t.Id)).FirstOrDefault();
                Assert.IsNotNull(receivedTask, "Required task from controller was not found");
                Assert.IsTrue(receivedTask.Name.Equals(task.Name)
                    && receivedTask.Description.Equals(task.Description)
                    && receivedTask.FunctionHeader.Equals(task.FunctionHeader)
                    && receivedTask.FunctionFooter.Equals(task.FunctionFooter),
                    "Tasks with same id has different property values"
                );
            }
        }
        IEnumerable<CompetitionTask> GenerateTasks()
        {
            var tasks = new[] {
                new CompetitionTask("Task1", "My description", Compilers.ProgrammingLanguages.CSharp, "function alpha() {", "} ", "using System; Program {", "} ", null) { Id = 111234 },
                new CompetitionTask("Task2", "My description2", Compilers.ProgrammingLanguages.CSharp, "function beta() {", "} ", "using System; ProgramA {", "} ", null) { Id = 111235 },
            };
            return tasks;
        }
    }
}
