using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompTheoProgs.Iterative
{
    /*  A computation for iterative programs.
     * 
     *  Executes "recursively" on the structure of
     * the program, as defined formally.
     * 
     *  Uses whole programs (on a single line)
     * to identify each computation step.
     */ 
    class Computation : CompTheoProgs.Computation
    {
        private Composition currentProgram;

        /*  Composes the program to be executed with the empty program,
         * as required in the formal definition, and records the inital
         * computation step.
         */
        public Computation(Program prog, IMachine mach) : base(mach)
        {
            currentProgram = new Composition( prog, new Empty() );

            AddStep(currentProgram.ToString(), mach.CurrentState);
        }

        /*  Executes the computation "recursively" on its structure, as defined formally.
         * Such execution is delegated from here to the Composition class, and there
         * to each program class.
         */
        protected override bool ExecuteSingleStep()
        {
            // Delegates the execution to the composition class
            currentProgram.RunComputationStep(machine);
            AddStep(currentProgram.ToString(), machine.CurrentState);

            /* When there's only one program on the composition, it is
             * certainly the empty one and thus the computation must end.
             */
            if (currentProgram.Subprograms.Count == 1)
            {
                return true;
            }

            return false;
        }
    }
}
