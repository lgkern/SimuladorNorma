using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompTheoProgs.Monolithic.SimpleInstructions
{

    public class Program : IProgram
    {
        private Instruction initial;
        private IDictionary<string, Instruction> instructions;

        public CompTheoProgs.Computation NewComputation(IMachine mach, string input)
        {
            CompTheoProgs.Computation comp;

            mach.PutValue(input);
            comp = new SimpleInstructions.Computation(this, mach);

            return comp;
        }

        public Instruction InitialInstruction
        {
            get { return initial; }
        }

        public IDictionary<string, Instruction> Instructions
        {
            get { return instructions; }
        }

        public Instruction FirstInstruction { get; set; }
    }
}
