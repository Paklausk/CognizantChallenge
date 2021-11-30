using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammingCompetitionApi
{
    public class PublicException : Exception
    {
        public PublicException(string message) : base(message) { }
    }
}
