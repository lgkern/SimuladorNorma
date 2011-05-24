using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompTheoProgs.Monolithic.SimpleInstructions
{
    /*  A class representing a simple instruction
     * 
     *  Contains four properties:
     *   - The label
     *   - Whether or not it is a test (as opposed to an operation)
     *   - The next label when the test returns true
     *   - The next label when the test returns false
     *   
     *  The last two must be the same for an operation, meaning
     *  the next label on any case.
     *  
     *  Also defines an abstract method for executing the instruction,
     *  returning the next label to be executed, which is done
     *  differently for each subclass.
     */
    public abstract class Instruction : IComparable<Instruction>
    {
        // The label, common to all instructions
        protected string label;
        public string Label { get { return label; } }

        /* Abstract properties, implemented differently
         * for operations and tests.
         */
        public abstract bool IsTest { get; }
        public abstract string Command { get; }
        public abstract string ThenCase { get; }
        public abstract string ElseCase { get; }

        // Constructor that initializes the label
        public Instruction(string labl)
        {
            label = labl;
        }

        // Execution is done differently for tests and ops
        public abstract string ExecuteToNextInstruction(IMachine mach);

        // Comparison between operations is the comparison of its labels
        public int CompareTo(Instruction other) { return this.label.CompareTo(other.label); }
    }
}
