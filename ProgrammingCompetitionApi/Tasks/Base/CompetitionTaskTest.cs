using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCompetitionApi.Tasks.Base
{
    public class CompetitionTaskTest
    {
        public string Input { get; }
        public string ExpectedResult { get; }

        public CompetitionTaskTest(string input, string expectedResult)
        {
            Input = input;
            ExpectedResult = expectedResult;
        }
    }
}
