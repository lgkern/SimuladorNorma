using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompTheoProgs.Monolithic.SimpleInstructions
{
    class Test : SimpleInstructions.Instruction
    {
        private string testID;
        private string thenCase;
        private string elseCase;

        public override bool IsTest
        {
            get { return true; }
        }

        public override string Command
        {
            get { return testID; }
        }

        public override string ThenCase
        {
            get { return thenCase; }
        }

        public override string ElseCase
        {
            get { return elseCase; }
        }

        public Test(string label, string test, string thenLabel, string elseLabel) : base(label)
        {
            testID = test;
            thenCase = thenLabel;
            elseCase = elseLabel;
        }

        public override string ExecuteToNextInstruction(IMachine mach)
        {
            if (mach.executeTest(testID))
                return thenCase;
            else
                return elseCase;
        }
    }
}
