using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompTheoProgs.Iterative
{

    public class While : Program
    {
        private string testID;
        private Program subprogram;

        public While(string ID, Program prg)
        {
            testID = ID;
            subprogram = prg;
        }

        public override string ToString()
        {
            return "( enquanto " + testID + " faça " + subprogram.ToString() + " )";
        }

        internal override IList<Program> EvalAndGetProgramsToPrepend(IMachine mach)
        {
            IList<Program> toPrepend = new List<Program>();

            if (mach.executeTest(testID))
            {
                toPrepend.Add(subprogram);
                toPrepend.Add(this);
            }

            return toPrepend;
        }
    }
}
