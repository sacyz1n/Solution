using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log
{
    public static class DEV
    {

        [Conditional("DEBUG")]
        [Conditional("RELEASE")]
        public static void CHECK(bool condition, string message = "", int stackTraceSkipDepth = 2)
        {
            if (condition)
                return;

        }
    }
}
