using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompTheoProgs;
using Pretty;

namespace CompTheoProgs
{
    class ComputationViewer : IComputationObserver
    {
        public ComputationViewer(Computation comp)
        {
            comp.AddObserver( this );
        }

        public void UpdateStepDone(string progState, string machState)
        {
            Console.WriteLine("( " + progState + ", " + machState + " )");
        }

        public void UpdateResult(Computation observed)
        {
            Console.WriteLine(observed.Machine.GetValue());
        }
    }

    class Program
    {
        static void Main(string[] args)
        {            
            /* Creates a stupid iterative program
             */
            IList<Iterative.Program> subprogs = new List<Iterative.Program>();
            subprogs.Add(new Iterative.Empty());
            subprogs.Add(new Iterative.Operation("ad_Y"));
            subprogs.Add(new Iterative.Empty());
            subprogs.Add(new Iterative.Operation("sub_X"));
            subprogs.Add(new Iterative.Empty());
            subprogs.Add(new Iterative.Test("C_zero", new Iterative.Empty(), new Iterative.Empty()));
            subprogs.Add(new Iterative.Empty());
            Iterative.Program prog1 = new Iterative.Until("X_zero", new Iterative.Composition(subprogs));

            /* Prints the program on several configurations
             */
            Console.WriteLine(PrettyPrinter.prettify(prog1, 10));
            Console.WriteLine(PrettyPrinter.prettify(prog1, 35));
            Console.WriteLine(PrettyPrinter.prettify(prog1, 72));
            Console.WriteLine(PrettyPrinter.prettify(prog1, 80));
            Console.WriteLine();
            Console.WriteLine(PrettyPrinter.prettierfy(prog1, 80, 20));
            Console.WriteLine(PrettyPrinter.prettierfy(prog1, 80, 30));
            Console.WriteLine(PrettyPrinter.prettierfy(prog1, 80, 80));
            Console.WriteLine();
            Console.WriteLine(PrettyPrinter.prettify(prog1.toSimpleInstructions(), 30));
            Console.WriteLine();
            Console.WriteLine(PrettyPrinter.prettify(prog1.toSimpleInstructions(), 50));
            Console.WriteLine();
            Console.WriteLine(PrettyPrinter.prettify(prog1.toSimpleInstructions(), 80));
            Console.WriteLine();
            Console.ReadKey(true); // Pause

            /* Creates a less stupid iterative program
             */
            prog1 = 
                new Iterative.Until("X_zero", 
                    new Iterative.Composition(
                        new Iterative.Operation("ad_Y"),
                        new Iterative.Operation("sub_X")));

            /* Creates a monolithic program
             */
            ICollection<Monolithic.SimpleInstructions.Instruction> instrs = new List<Monolithic.SimpleInstructions.Instruction>();
            instrs.Add(new Monolithic.SimpleInstructions.Test("1", "X_zero", "0", "2"));
            instrs.Add(new Monolithic.SimpleInstructions.Operation("2", "ad_Y", "3"));
            instrs.Add(new Monolithic.SimpleInstructions.Operation("3", "sub_X", "1"));
            Monolithic.SimpleInstructions.Program prog2 = new Monolithic.SimpleInstructions.Program(instrs, "1");

            /* Executes the iterative program
             */
            Computation comp = prog1.NewComputation(new Norma.Machine("2"));
            ComputationViewer view = new ComputationViewer(comp);
            while (!comp.Finished)
            {
                Console.ReadKey(true);
                comp.RunStep();
            }

            /* Executes the monolithic program
             * created from the iterative one
             */
            comp = prog1.toSimpleInstructions().NewComputation(new Norma.Machine("2"));
            view = new ComputationViewer(comp);
            while (!comp.Finished)
            {
                Console.ReadKey(true);
                comp.RunStep();
            }
            
            /* Executes the monolithic program
             * created from scratch
             */
            comp = prog2.NewComputation(mach, new Norma.Machine(2));
            view = new ComputationViewer(comp);
            while (!comp.Finished)
            {
                Console.ReadKey(true);
                comp.RunStep();
            }

            Console.ReadKey(true); // Pause
        }
    }
}
