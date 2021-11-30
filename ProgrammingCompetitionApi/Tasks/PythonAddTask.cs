using ProgrammingCompetitionApi.Tasks.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingCompetitionApi.Tasks
{
    public class PythonAddTask : CompetitionTask
    {
        public PythonAddTask() : base(
            "Python add",
            "Write in Python function body, which takes its parameters, adds them and returns a result",
            Compilers.ProgrammingLanguages.Python,
            "def fnc(val1, val2):",
            "    return result",
            "import sys",
            @"string_inputs = input().split()
int_inputs = [int(string_input) for string_input in string_inputs]
result = fnc(int_inputs[0], int_inputs[1])
print(result)
",
            new[] { new CompetitionTaskTest("10 5", "15") }
            )
        {

        }

        public override string FormatCode(string rawCode)
        {
            string[] codeLines = rawCode.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var sb = new StringBuilder();
            foreach (var codeLine in codeLines)
            {
                codeLine.Trim();
                sb.AppendLine($"    {codeLine}");
            }
            return sb.ToString();
        }
    }
}
