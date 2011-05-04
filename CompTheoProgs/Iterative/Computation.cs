using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompTheoProgs.Iterative
{
    class Computation : CompTheoProgs.Computation
    {
        private Composition currentProgram;

        public string CurrentState
        {
            get { return machine.CurrentState; }
        }

        public Computation(Program prog, IMachine mach)
        {
            currentProgram = new Composition( prog, new Empty() );
            machine = mach;
            AddStep(currentProgram.ToString(), mach.CurrentState);
        }

        public override void RunStep()
        {
            if (finished)
                throw new EndOfComputationException("Cannot run steps after the computation ended");

            currentProgram.RunComputationStep(machine);
            AddStep(currentProgram.ToString(), machine.CurrentState);

            if (currentProgram.Subprograms[0].GetType() == typeof(Empty)) 
            {
                result = machine.GetValue();
                NotifyObserversOfResult(result);
                finished = true;
            }
        }
    }
}
