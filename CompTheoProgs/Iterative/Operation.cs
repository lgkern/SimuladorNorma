using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompTheoProgs.Iterative
{

    public class Operation : Program
    {
        private string opID;

        public Operation(string ID)
        {
            opID = ID;
        }

        public override string ToString()
        {
            return opID;
        }

        internal override IList<Program> EvalAndGetProgramsToPrepend(IMachine mach)
        {
            mach.executeOperation(operationID);
            return new List<Program>();
        }

        public string operationID
        {
            get { return opID; }
        }

    }
}
