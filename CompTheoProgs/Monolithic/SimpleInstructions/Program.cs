using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pretty;

namespace CompTheoProgs.Monolithic.SimpleInstructions
{
    /*  Program class for simple instructions.
     * 
     *  Contains a set of labeled instructions, without
     * any particular order, and a single instruction
     * from it on which the computation starts.
     */
    public class Program : IProgram, IPrettyfiable
    {
        private Instruction initial;
        private IDictionary<string, Instruction> instructions;

        public Instruction InitialInstruction { get { return initial; } }
        public IDictionary<string, Instruction> Instructions { get { return instructions; } }

        /*  Creates a program from a collection of instructions and the
         * label for the first one. The label MUST identify one of the
         * given instructions, or else an exception will be raised.
         */
        public Program(IEnumerable<Instruction> instructs, string initialLabel)
        {
            instructions = new Dictionary<string, Instruction>();

            // Creates the dictionary from the labels to the whole instructions
            foreach (Instruction i in instructs)
            {
                instructions[i.Label] = i;
            }

            // Selects the first instruction from the label
            initial = instructions[initialLabel];
        }

        /* Creates a new computation for the monolithic program.
         * Initializes the machine with the given input value
         */
        public CompTheoProgs.Computation NewComputation(IMachine mach)
        {
            return new SimpleInstructions.Computation(this, mach);
        }

        // Creates a string representation of the 
        public override string ToString()
        {
            const int linesize = 80;
            return PrettyPrinter.prettify(this, linesize);
        }

        // Creates a Document for pretty-printing
        public Doc ToDoc()
        {
            Doc result = instructions.Values.First().ToDoc();

            foreach (IPrettyfiable i in instructions.Values.Skip(1))
            {
                result += Doc.line + i;
            }

            return result;
        }
    }
}
