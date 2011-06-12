using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pretty;

namespace CompTheoProgs.Monolithic.SimpleInstructions
{
    public class Operation : SimpleInstructions.Instruction
    {
        private string operationID;
        private string nextLabel;

        public override bool IsTest
        {
            get { return false; }
        }

        public override string Command
        {
            get { return operationID; }
        }

        public override string ThenCase
        {
            get { return nextLabel; }
        }

        public override string ElseCase
        {
            get { return nextLabel; }
        }

        public Operation(string label, string operation, string next) : base(label)
        {
            operationID = operation;
            nextLabel = next;
        }

        public override string ExecuteToNextInstruction(IMachine mach)
        {
            mach.executeOperation(operationID);
            return nextLabel;
        }

        public override Doc  makeSpecificDoc()
        {
            Doc doPart = Doc.text(doStr + " " + operationID);
            Doc gotoPart = Doc.text(gotoStr + " " + nextLabel);

            return (doPart + Doc.line + gotoPart).Group();
        }
    }
}
