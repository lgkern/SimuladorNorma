using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompTheoProgs.Monolithic.SimpleInstructions
{
    /*  Program class for simple instructions.
     * 
     *  Contains a set of labeled instructions, without
     * any particular order, and a single instruction
     * from it on which the computation starts.
     */
    public class Program : IProgram
    {
        private Instruction initial;
        private IDictionary<string, Instruction> instructions;

        public Instruction InitialInstruction { get { return initial; } }
        public IDictionary<string, Instruction> Instructions { get { return instructions; } }

        /*  Creates a program from a collection of instructions and the
         * label for the first one. The label MUST identify one of the
         * given instructions, or else an exception will be raised.
         */
        public Program(ICollection<Instruction> instructs, string firstLabel)
        {
            instructions = new Dictionary<string, Instruction>();

            // Creates the dictionary from the labels to the whole instructions
            foreach (Instruction i in instructs)
            {
                instructions[i.Label] = i;
            }

            // Selects the first instruction from the label
            initial = instructions[firstLabel];
        }

        /* Creates a new computation for the monolithic program.
         * Initializes the machine with the given input value
         */
        public CompTheoProgs.Computation NewComputation(IMachine mach, string input)
        {
            mach.PutValue(input);
            return new SimpleInstructions.Computation(this, mach);
        }
    }
}
