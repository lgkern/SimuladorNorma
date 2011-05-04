using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompTheoProgs.Iterative
{

    public abstract class Program : IProgram
    {
        public CompTheoProgs.Computation NewComputation(IMachine mach, string input) 
        {
            mach.PutValue(input);
            return new Iterative.Computation(this, mach);
        }

        internal abstract IList<Program> EvalAndGetProgramsToPrepend(IMachine mach);
    }

}