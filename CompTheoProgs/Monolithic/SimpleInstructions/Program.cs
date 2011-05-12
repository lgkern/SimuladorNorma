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

        public Program(string initialLabel, ICollection<Instruction> instrs)
        {
            instructions = new Dictionary<string, Instruction>();


            foreach (Instruction i in instrs)
            {
                instructions.Add(i.Label, i);
            }

            initial = instructions[initialLabel];
        }

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

        public IDictionary<string, SimpleInstructions.Instruction> Instructions
        {
            get { return instructions; }
        }

        public override string ToString()
        {
            String result = "";
            List<SimpleInstructions.Instruction> insts = instructions.Values.ToList();
            insts.Sort();

            foreach (SimpleInstructions.Instruction i in insts)
            {
                result += i.ToString() + "\n";
            }

            return result;
        }
    }
}
