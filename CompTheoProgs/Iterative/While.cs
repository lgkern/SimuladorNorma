using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompTheoProgs.Monolithic.SimpleInstructions;
using Pretty;

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

        /* A While is never empty
         */
        public override bool IsEmpty
        { get { return false; } }

        /* Creates a Doc for pretty-printing
         * the current program
         */
        public override Doc ToDoc()
        {
            Doc whilePart = Doc.text(whileStr + " " + testID);
            Doc doPart = Doc.text(doStr + " ") + subprogram.ToDoc().Indent(doStr.Count() + 1);

            return PrettyPrinter.bracket("(", whilePart + Doc.line + doPart, ")", 2);
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
            Instruction test = new Monolithic.SimpleInstructions.Test(currentLabel.ToString(), testID, subLabel.ToString(), endLabel);
            single.Add(test);

            // Returns all instructions
            return single.Concat(subInstructions);
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
