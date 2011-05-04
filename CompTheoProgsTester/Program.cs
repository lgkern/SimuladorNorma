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
            Iterative.Program prog;
            Norma.Machine mach;
            Computation comp;
            ComputationViewer view;

            mach = new Norma.Machine();

            prog = new Iterative.Until(
                "X_zero",
                new Iterative.Composition(
                    new Iterative.Operation("ad_Y"),
                    new Iterative.Operation("sub_X")));

            comp = prog.NewComputation(mach, "2");
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
