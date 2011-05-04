using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompTheoProgs.Iterative
{
    public class Until : Program
    {
        private string testID;
        private Program subprogram;

        public Until(string ID, Program prg)
        {
            testID = ID;
            subprogram = prg;
        }

        public override string ToString()
        {
            return "( até " + testID + " faça " + subprogram.ToString() + " )";
        }

        internal override IList<Program> EvalAndGetProgramsToPrepend(IMachine mach)
        {
            IList<Program> toPrepend = new List<Program>();

            if (!mach.executeTest(testID))
            {
                toPrepend.Add(subprogram);
                toPrepend.Add(this);
            }

            return toPrepend;
        }
    }
}
