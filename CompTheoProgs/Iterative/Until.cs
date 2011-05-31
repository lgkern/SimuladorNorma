using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompTheoProgs.Monolithic.SimpleInstructions;
using Pretty;

namespace CompTheoProgs.Iterative
{

    /* Class for an iterative Until program.
     */
    public class Until : Program
    {
        private string testID;
        private Program subprogram;

        /* Constructs an instance from a machine test ID and
         * the program to be repeatedly ran.
         */
        public Until(string testID, Program prg)
        {
            this.testID = testID;
            subprogram = prg;
        }

        /* An Until is never empty
         */
        public override bool IsEmpty
        { get { return false; } }

        /* Creates a Doc for pretty-printing
         * the current program
         */
        public override Doc ToDoc()
        {
            Doc untilPart = Doc.text(untilStr + " " + testID);
            Doc doPart = Doc.text(doStr + " ") + subprogram.ToDoc().Indent(doStr.Count() + 1);

            return PrettyPrinter.bracket("(", untilPart + Doc.line + doPart, ")", 2);
        }


        /* Returns the number of instruction required for executing 
         * the program: those needed for the subprogram + 1
         */
        internal override int InstructionCount
        {
            get { return 1 + subprogram.InstructionCount; }
        }

        /*  Generates an enumeration of all instructions for executing the until program.
         */
        internal override IEnumerable<Monolithic.SimpleInstructions.Instruction> makeInstructions(int currentLabel, string endLabel)
        {
            // Generates the repeated instructions
            IEnumerable<Instruction> subInstructions;
            int subLabel;

            if (subprogram.IsEmpty)
            {
                // Just goes back to the test instruction
                subLabel = currentLabel;
                subInstructions = new List<Instruction>(1);
            }
            else
            {
                // Generates the subprogram leading back to the test
                subLabel = currentLabel + 1;
                subInstructions = subprogram.makeInstructions(subLabel, currentLabel.ToString());
            }

            // Generates the test instruction
            List<Instruction> single = new List<Instruction>(1);
            Instruction test = new Monolithic.SimpleInstructions.Test(currentLabel.ToString(), testID, endLabel, subLabel.ToString());
            single.Add(test);

            // Returns all instructions
            return single.Concat(subInstructions);
        }

        /* Executes one step for ((até T faça P); R), by executing T and 
         * generating R as the next program to be ran if T returned true, 
         * or (P;(até T faça P);R) if it returned false.
         */
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
