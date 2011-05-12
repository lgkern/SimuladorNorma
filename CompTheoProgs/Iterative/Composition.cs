using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompTheoProgs.Iterative
{

    public class Composition : Program
    {
        private IList<Program> subprograms;

        public IList<Program> Subprograms
        {
            get { return subprograms; }
        }

        public Composition()
        {
            subprograms = new List<Program>();
        }

        public Composition(Program a, Program b)
        {
            subprograms = new List<Program>();
            subprograms.Add(a);
            subprograms.Add(b);
        }

        public Composition(IList<Program> prgs)
        {
            subprograms = prgs;
        }

        public override string ToString()
        {
            string result;
            result = "( ";

            foreach (Program p in subprograms)
            {
                result += p.ToString();
                result += "; ";
            }

            return result.Substring(0, result.Length-2) + " )";
        }

        internal override IList<Program> EvalAndGetProgramsToPrepend(IMachine mach)
        {
            return this.subprograms;
        }

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
