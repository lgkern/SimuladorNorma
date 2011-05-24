using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompTheoProgs.Monolithic.SimpleInstructions;

namespace CompTheoProgs.Iterative
{

    /* Class for an iterative While program.
     */
    public class While : Program
    {
        private string testID;
        private Program subprogram;

        /* Constructs an instance from a machine test ID and
         * the program to be repeatedly ran.
         */
        public While(string ID, Program prg)
        {
            testID = ID;
            subprogram = prg;
        }

        /* Creates a string representation for the
         * current program
         */
        public override string ToString()
        {
            return "( enquanto " + testID + " faça " + subprogram.ToString() + " )";
        }

        /* Returns the number of instruction required for executing 
         * the program: those needed for the subprogram + 1
         */
        internal override int InstructionCount
        {
            get { return 1 + subprogram.InstructionCount; }
        }

        /*  Generates an enumeration of all instructions for executing the while program.
         */
        internal override IEnumerable<Monolithic.SimpleInstructions.Instruction> makeInstructions(int currentLabel, string endLabel)
        {
            IList<Instruction> single = new List<Instruction>();
            IEnumerable<Instruction> partial;
            Instruction test;

            int subprogLabel = currentLabel + 1;

            // Generates the test instruction
            test = new Monolithic.SimpleInstructions.Test(currentLabel.ToString(), testID, subprogLabel.ToString(), endLabel);
            single.Add(test);

            // Appends the instructions from the thenProg
            partial = subprogram.makeInstructions(subprogLabel, currentLabel.ToString());
            return single.Concat(partial); ;
        }

        /* Executes one step for ((enquanto T faça P); R), by executing T and 
         * generating (P;(enquanto T faça P);R) as the next program to be ran 
         * if T returned true, or R if it returned false.
         */
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
