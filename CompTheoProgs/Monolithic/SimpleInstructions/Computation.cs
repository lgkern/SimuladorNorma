using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompTheoProgs.Monolithic.SimpleInstructions
{
    /*  A computation for monolithic programs.
     * 
     *  Runs one instruction on each step. 
     *  Uses the labels to identify each program state
     */
    class Computation : CompTheoProgs.Computation
    {
        private Instruction currentInstruction;
        private SimpleInstructions.Program program;

        /*  Sets the current instruction to the first one, adds 
         * a step for the initial state of execution.
         */
        public Computation(SimpleInstructions.Program prog, IMachine mach) : base(mach)
        {
            program = prog;
            currentInstruction = prog.InitialInstruction;

            AddStep(currentInstruction.Label, mach.CurrentState);
        }

        // Runs a single instruction
        protected override bool ExecuteSingleStep()
        {
            string nextLabel;

            nextLabel = currentInstruction.ExecuteToNextInstruction(machine);
            AddStep(nextLabel, machine.CurrentState);

            try
            {
                currentInstruction = program.Instructions[nextLabel];
            }
            catch (KeyNotFoundException)
            {
                return true;
            }

            return false;
        }
    }
}
