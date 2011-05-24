using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompTheoProgs.Monolithic.SimpleInstructions;

namespace CompTheoProgs.Iterative
{
    /*  An abstract class for an iterative program, which
     * is actually defined polymorphically and inductively
     * on the following subclasses:
     *  - Empty
     *  - Operation
     *  - Test
     *  - Compostion
     *  - Until
     *  - While
     *  
     *   May be directly converted to a monolithic, 
     *  simple instructions program.
     */
    public abstract class Program : IProgram
    {
        // Creates a computation for the current program
        public CompTheoProgs.Computation NewComputation(IMachine mach, string input) 
        {
            mach.PutValue(input);
            return new Iterative.Computation(this, mach);
        }

        // Returns an equivalent monolithic program
        public Monolithic.SimpleInstructions.Program toSimpleInstructions()
        {
            IEnumerable<Instruction> instructs = makeInstructions(1, "0");
            string initial;

            // If the program is empty, already begins on the ending label
            if (instructs.Count() == 0)
                initial = "0";
            else
                initial = "1";

            return new Monolithic.SimpleInstructions.Program(instructs, initial);
        }

        // Returns the number of instructions for the equivalent monolithic program
        internal abstract int InstructionCount { get; }

        /*  Creates and returns a collection of instructions for an
         * equivalent monolithic program. Labels each instruction with
         * a number, starting on 'currentLabel' and incrementing by one
         * for each instruction.
         *  What would be the "stop" node on a flowchart is represented
         * by the 'endLabel'.
         */
        internal abstract IEnumerable<Monolithic.SimpleInstructions.Instruction> makeInstructions(int currentLabel, string endLabel);

        /*  Executes the current program P, as defined formally for the composition
         * (P; R), returning the P' which will be prepended to the composition list
         * to generate the next state.
         */
        internal abstract IList<Program> EvalAndGetProgramsToPrepend(IMachine mach);
    }
}