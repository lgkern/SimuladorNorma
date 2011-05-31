using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompTheoProgs.Monolithic.SimpleInstructions;
using Pretty;

namespace CompTheoProgs.Iterative
{

    /* Class for an iterative Test, which
     * executes different subprograms according
     * to the given test's result
     */
    public class Test : Program
    {
        private string testID;
        private Program thenProg, elseProg;

        /* Constructs a test from the machine test's ID,
         * the program to be ran when the result is true
         * and the program to be ran when it is false.
         */
        public Test(string ID, Program thenCase, Program elseCase)
        {
            testID = ID;
            thenProg = thenCase;
            elseProg = elseCase;
        }

        /* Creates a Doc for pretty-printing
         * the current program.
         */
        public override Doc ToDoc()
        {
            Doc ifPart = Doc.text(ifStr + " " + testID);
            Doc thenPart = Doc.text(thenStr) + (Doc.line + thenProg.ToDoc().Indent(thenStr.Count() - 2)).Indent(2).Group();
            Doc elsePart = Doc.text(elseStr) + (Doc.line + elseProg.ToDoc().Indent(elseStr.Count() - 2)).Indent(2).Group();

            return PrettyPrinter.bracket("(", ifPart + Doc.line + thenPart + Doc.line + elsePart, ")", 2);
        }

        /* Returns the number of instruction required for
         * executing the program: those needed for each
         * subprogram + 1
         */
        internal override int InstructionCount
        {
            get { return 1 + thenProg.InstructionCount + elseProg.InstructionCount; }
        }

        /*  Generates an enumeration of all instructions for executing the test.
         */
        internal override IEnumerable<Monolithic.SimpleInstructions.Instruction> makeInstructions(int currentLabel, string endLabel)
        {
            IEnumerable<Instruction> composition;
            IList<Instruction> single = new List<Instruction>();
            IEnumerable<Instruction> partial;
            Instruction test;

            int thenLabel = currentLabel + 1;
            int elseLabel = currentLabel + 1 + thenProg.InstructionCount;

            // Generates the test instruction
            test = new Monolithic.SimpleInstructions.Test(currentLabel.ToString(), testID, thenLabel.ToString(), elseLabel.ToString());
            single.Add(test);

            // Appends the instructions from the thenProg
            partial = thenProg.makeInstructions(thenLabel, endLabel);
            composition = single.Concat(partial);

            // Appends the instructions from the elseProg
            partial = elseProg.makeInstructions(elseLabel, endLabel);
            composition = composition.Concat(partial);

            return composition;
        }

        /* Executes one step for ((se T então P1 senão P2); R), by executing T
         * and generating (P1;R) as the next program to be ran if T returned true,
         * or (P2;R) if it returned false.
         */
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
