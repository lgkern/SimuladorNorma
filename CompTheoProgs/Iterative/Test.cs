using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompTheoProgs.Iterative
{
    public class Test : Program
    {
        private string testID;
        private Program thenProg, elseProg;

        public Test(string ID, Program thenCase, Program elseCase)
        {
            testID = ID;
            thenProg = thenCase;
            elseProg = elseCase;
        }

        public override string ToString()
        {
            return "( se " + testID
                    + " então " + thenProg.ToString()
                    + " senão " + elseProg.ToString() + " )";
        }

        internal override IList<Program> EvalAndGetProgramsToPrepend(IMachine mach)
        {
            IList<Program> toPrepend = new List<Program>();

            if (mach.executeTest(testID))
                toPrepend.Add(thenProg);
            else
                toPrepend.Add(elseProg);

            return toPrepend;
        }
    }
}
