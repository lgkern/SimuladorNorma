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
        /* Empty is, naturally, always empty
         */
        public override bool IsEmpty
        { get { return true; } }

        /*  Executes one step for (✓; R), by doing nothing and
         * and generating R as the next program to be ran.
         */
        internal override IList<Program> EvalAndGetProgramsToPrepend(IMachine mach)
        { return new List<Program>(); }

        /* Returns the number of instructions required for this program: 0
         */
        internal override int InstructionCount
        { get { return 0; } }

        /* Cannot generate instructions for this program.
         */
        internal override IEnumerable<Monolithic.SimpleInstructions.Instruction> makeInstructions(int currentLabel, string endLabel)
        { throw new System.InvalidOperationException("Can't generate instructions from an empty program."); }

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
