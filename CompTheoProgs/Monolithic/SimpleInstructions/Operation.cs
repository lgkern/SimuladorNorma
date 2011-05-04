using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompTheoProgs.Monolithic.SimpleInstructions
{
    /*
    class Operation : SimpleInstructions.Instruction
    {
        private string operationID;
        private string nextLabel;

        public string ThenCase
        {
            get { return nextLabel; }
        }

        public string ElseCase
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
    }
     */
}
