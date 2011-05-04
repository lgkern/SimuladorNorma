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

            
            //firstType = first.GetType();

            //if (firstType == typeof(Empty))
            //{
            //    // Empty op will just be skipped
            //    this.subprograms.RemoveAt(0);
            //}

            //else if (firstType == typeof(Operation))
            //{
            //    // An operation will be executed then taken from the list
            //    Operation prog = (Operation) first;
            //    mach.executeOperation(prog.operationID);
            //    this.subprograms.RemoveAt(0);
            //}

            //else if (firstType == typeof(Test))
            //{
            //    // A test will be executed then the correct program will be substituted for it
            //    Test prog = (Test) first;
            //    if (mach.executeTest(prog.testID))
            //        this.subprograms[0] = prog.thenCase;
            //    else
            //        this.subprograms[0] = prog.elseCase;
            //}

            //else if (firstType == typeof(Composition))
            //{
            //    // A composition will have its programs incorporated into this' list
            //    Composition prog = (Composition) first;
            //    this.subprograms.RemoveAt(0);
            //    this.subprograms = (IList<Program>) prog.subprograms.Concat(this.subprograms);
            //}

            //else if (firstType == typeof(While))
            //{
            //    /* A while loop will be ignored if the test result is false,
            //     * or its subprogram will be prepended to this' list w/o
            //     * taking the actual 'while' program.
            //     */
            //    While prog = (While) first;
                    
            //    if ( mach.executeTest(prog.testID) )
            //        this.subprograms.Insert(0, prog.repeatedProg);
            //    else
            //        this.subprograms.RemoveAt(0);
            //}

            //else if (firstType == typeof(Until))
            //{
            //    /* An until loop will be ignored if the test result is true,
            //     * or its subprogram will be prepended to this' list w/o
            //     * taking the actual 'until' program.
            //     */
            //    Until prog = (Until) first;
                    
            //    if ( mach.executeTest(prog.testID) )
            //        this.subprograms.RemoveAt(0);
            //    else
            //        this.subprograms.Insert(0, prog.repeatedProg);
            //}
        }
    }
}
