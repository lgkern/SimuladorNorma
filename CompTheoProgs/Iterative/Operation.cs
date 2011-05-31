using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompTheoProgs.Monolithic.SimpleInstructions;
using Pretty;

namespace CompTheoProgs.Iterative
{

    /* Class for an iterative operation, which
     * just executes the machine's operation.
     */
    public class Operation : Program
    {
        private string opID;

        /* Constructs an operation for a given
         * machine operation ID.
         */
        public Operation(string ID)
        {
            opID = ID;
        }

        /* An operation is never empty
         */
        public override bool IsEmpty
        { get { return false; } }

        /* Generates a string representation
         * for the current program
         */
        public override string ToString()
        {
            return opID;
        }

        /* Creates a Doc for pretty-printing
         * the corrent program
         */
        public override Doc ToDoc()
        {
            return Doc.text(opID);
        }

        /* Returns the number of instructions required for this program: 1
         */
        internal override int InstructionCount
        {
            get { return 1; }
        }
        
        /* Generates an enumeration for the one instruction
         * necessary for running this program.
         */
        internal override IEnumerable<Monolithic.SimpleInstructions.Instruction> makeInstructions(int currentLabel, string endLabel)
        {
            IList<Instruction> instructs = new List<Instruction>();
            Instruction operation;
            
            // Generates the enumeration with a single operation
            operation = new Monolithic.SimpleInstructions.Operation(currentLabel.ToString(), opID, endLabel);
            instructs.Add(operation);

            return instructs;
        }

        /* Executes one step for (OP; R), by executing OP
         * and generating R as the next program to be ran.
         */
        internal override IList<Program> EvalAndGetProgramsToPrepend(IMachine mach)
        {
            mach.executeOperation(operationID);
            return new List<Program>();
        }

        /* The ID of the machine operaton 
         * executed by this program.
         */
        public string operationID
        {
            get { return opID; }
        }

    }
}
