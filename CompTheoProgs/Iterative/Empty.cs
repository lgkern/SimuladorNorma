using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompTheoProgs.Monolithic.SimpleInstructions;
using Pretty;

namespace CompTheoProgs.Iterative
{

    /*  Class for the empty iterative program, 
     * which basically does nothing.
     * 
     *  Uses the default constructor.
     */
    public class Empty : Program
    {

        /*  Executes one step for (✓; R), by doing nothing and
         * and generating R as the next program to be ran.
         */
        internal override IList<Program> EvalAndGetProgramsToPrepend(IMachine mach)
        { return new List<Program>(); }

        /* Returns the number of instructions required for this program: 0
         */
        internal override int InstructionCount
        { get { return 0; } }

        /*  Generates an (empty) enumeration for the instructions
         * necessary for running this program: none at all.
         */
        internal override IEnumerable<Monolithic.SimpleInstructions.Instruction> makeInstructions(int currentLabel, string endLabel)
        {
            return new List<Instruction>();
        }

        /* Returns a string representation for this program.
         */
        public override string ToString()
        { return emptyStr; }

        /* Returns a Doc for pretty-printing
         * this program
         */
        public override Doc ToDoc()
        { return Doc.text(emptyStr); }

    }
}
