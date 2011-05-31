using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompTheoProgs.Monolithic.SimpleInstructions;
using Pretty;

namespace CompTheoProgs.Iterative
{

    /*  Class for a composition, represents the
     * execution, in order, of a list of programs.
     * 
     *  Besides the required behaviour for a program,
     * it also implements the execution of a computation
     * step, as the formal definition does it in terms
     * of compositions.
     */
    public class Composition : Program
    {
        private IList<Program> subprograms;

        /* The list of subprograms to be executed in order
         */
        public IList<Program> Subprograms
        {
            get { return subprograms; }
        }

        /* Creates the composition of two programs
         */
        public Composition(Program a, Program b)
        {
            subprograms = new List<Program>();
            subprograms.Add(a);
            subprograms.Add(b);
        }

        /* Creates the composition of a list of programs
         */
        public Composition(IList<Program> programs)
        {
            subprograms = programs;
        }

        /* Creates a Doc from the composition
         */
        public override Doc ToDoc()
        {
            return PrettyPrinter.fromDocList("(", subprograms, ";", ")");
        }

        /*  The number of instructions required for a composition is
         * the sum of the instructions required for each subprogram;
         */
        internal override int InstructionCount
        {
            get
            {
                int count = 0;

                foreach (Program p in subprograms)
                    count += p.InstructionCount;

                return count;
            }
        }

        /*  Generates an enumeration of all instructions for executing the composition.
         */
        internal override IEnumerable<Monolithic.SimpleInstructions.Instruction> makeInstructions(int currentLabel, string endLabel)
        {
            IEnumerable<Instruction> composition = new List<Instruction>();
            IEnumerable<Instruction> partial;
            int nextLabel = currentLabel;

            /*  Appends all enumerations of instructions from all subprograms, except the last,
             * each ending by leading to the next subprogram's first instruction.
             */
            foreach (Program p in subprograms.Take(subprograms.Count-1))
            {
                nextLabel = currentLabel + p.InstructionCount;
                partial = p.makeInstructions( currentLabel, nextLabel.ToString() );
                composition = composition.Concat(partial);
                currentLabel = nextLabel;
            }

            // Appends the last subpogram's instructions, leading to endLabel
            partial = subprograms.Last().makeInstructions(currentLabel, endLabel);
            composition = composition.Concat(partial);

            return composition;
        }

        /*  Executes one step for ((p1;...;pn); R), by doing nothing
         * and generating (p1;...;pn;R) as the next program to be ran.
         */
        internal override IList<Program> EvalAndGetProgramsToPrepend(IMachine mach)
        {
            return this.subprograms;
        }

        /*  Runs a computation step on this composition by 
         * delegating the execution to the first composed
         * program, then prepending the required programs
         * to its composition list.
         */
        public void RunComputationStep(IMachine mach)
        {
            Program first;
            IList<Program> toPrepend;

            // Takes the first program on the composition
            first = this.subprograms.First();
            this.subprograms.RemoveAt(0);

            // Evaluates the first program and prepends any required programs
            toPrepend = first.EvalAndGetProgramsToPrepend(mach);
            subprograms = toPrepend.Concat(subprograms).ToList();
        }
    }
}
