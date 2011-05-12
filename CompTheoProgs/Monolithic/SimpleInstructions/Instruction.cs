using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompTheoProgs.Monolithic.SimpleInstructions
{
    public abstract class Instruction
    {
        protected string label;

        public string Label
        {
            get { return label; }
        }

        public abstract bool IsTest
        { get; }

        public abstract string Command
        { get; }

        public abstract string ThenCase
        { get; }

        public abstract string ElseCase
        { get; }

        public Instruction(string labl)
        {
            label = labl;
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public abstract string ExecuteToNextInstruction(IMachine mach);
    }
}
