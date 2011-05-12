using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompTheoProgs.Monolithic.SimpleInstructions
{
    class Computation : CompTheoProgs.Computation
    {
        private Instruction currentInstruction;
        private SimpleInstructions.Program program;

        public Computation(SimpleInstructions.Program prog, IMachine mach)
        {
            program = prog;
            currentInstruction = prog.InitialInstruction;
            finished = false;
            result = null;
        }

        public override void RunStep()
        {
            if (finished)
                throw new EndOfComputationException("Cannot execute computation step after it has finished.");
          
            currentInstruction = program.Instructions[ currentInstruction.ExecuteToNextInstruction(machine) ];

            if (currentInstruction == null)
            {
                finished = true;
                result = machine.GetValue();
            }
        }
    }
}
