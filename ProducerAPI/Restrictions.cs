using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Producer_API_
{
    public static class Restrictions
    {

        public static int MessageLength = 30;

        public static char[] SpecificSymbols = {'/','%'};

        public static int StepTime;

        public static bool IsTimeStepOver = true;
    }
}
