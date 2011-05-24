using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CompTheoProgs;

namespace CompTheoProgs
{
    class ComputationViewer : IComputationObserver
    {
        public ComputationViewer(Computation comp)
        {
            comp.AddObserver( (IComputationObserver)this );
        }

        public void UpdateStepDone(string step)
        {
            Console.WriteLine(step);
        }

        public void UpdateResult(string result)
        {
            Console.WriteLine(result);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Iterative.Program prog1;
            Monolithic.SimpleInstructions.Program prog2;
            ICollection<Monolithic.SimpleInstructions.Instruction> instrs = new List<Monolithic.SimpleInstructions.Instruction>();
            Norma.Machine mach;
            Computation comp;
            ComputationViewer view;

            mach = new Norma.Machine();

            prog1 = new Iterative.Until(
                "X_zero",
                new Iterative.Composition(
                    new Iterative.Operation("ad_Y"),
                    new Iterative.Operation("sub_X")));

            instrs.Add(new Monolithic.SimpleInstructions.Test("1", "X_zero", "0", "2"));
            instrs.Add(new Monolithic.SimpleInstructions.Operation("2", "ad_Y", "3"));
            instrs.Add(new Monolithic.SimpleInstructions.Operation("3", "sub_X", "1"));

            prog2 = new Monolithic.SimpleInstructions.Program(instrs, "1");

            Console.Write(prog1);
            Console.WriteLine();
            Console.Write(prog1.toSimpleInstructions());
            Console.WriteLine();
            Console.Write(prog2);
            Console.WriteLine();

            comp = prog1.NewComputation(mach, "2");
            view = new ComputationViewer(comp);
            
            while (!comp.Finished)
            {
                Console.ReadKey(true);
                comp.RunStep();
            }

            comp = prog1.toSimpleInstructions().NewComputation(mach, "2");
            view = new ComputationViewer(comp);

            while (!comp.Finished)
            {
                Console.ReadKey(true);
                comp.RunStep();
            }
            
            comp = prog2.NewComputation(mach, "2");
            view = new ComputationViewer(comp);

            while (!comp.Finished)
            {
                Console.ReadKey(true);
                comp.RunStep();
            }

            Console.ReadKey(true);
        }
    }
}
