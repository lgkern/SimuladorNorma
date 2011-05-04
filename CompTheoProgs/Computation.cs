using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompTheoProgs
{
    public abstract class Computation
    {
        protected bool finished;
        protected string result;
        protected IList<string> steps;
        protected IMachine machine;
        private ICollection<IComputationObserver> observers;

        public bool Finished
        {
            get { return finished; }
        }

        public string Result
        {
            get 
            {
                if (!finished)
                    // TODO: implement proper exception
                    throw new Exception("Computation didn't finish executing");

                return result;
            }
        }

        public Computation()
        {
            result = null;
            steps = new List<string>();
            observers = new LinkedList<IComputationObserver>();
        }

        public override string ToString()
        {
            string result = "";

            foreach (string step in steps)
            {
                result += step;
                result += "\n";
            }

            return result;
        }

        protected void NotifyObserversOfStep(string step)
        {
            foreach (IComputationObserver o in observers)
            {
                o.UpdateStepDone(step);
            }
        }

        protected void NotifyObserversOfResult(string result)
        {
            foreach (IComputationObserver o in observers)
            {
                o.UpdateResult(result);
            }
        }

        public void AddObserver(IComputationObserver obs)
        {
            observers.Add(obs);
        }

        public void RemoveObserver(IComputationObserver obs)
        {
            observers.Remove(obs);
        }

        public void AddStep(string program, string state)
        {
            string step;
                
            step = "( " + program + ", " + state + " )";
            steps.Add(step);
            NotifyObserversOfStep(step);
        }

        public void Run()
        {
            while (!finished)
            {
                RunStep();
            }
        }

        public void Run(int maxSteps)
        {
            int i;

            for (i = 0; i < maxSteps && !finished; i++)
            {
                RunStep();
            }
        }

        public abstract void RunStep();
    }

    public class EndOfComputationException : System.Exception
    {
        public EndOfComputationException() : base() { }
        public EndOfComputationException(string message) : base(message) { }
        public EndOfComputationException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is necessary for serialization, whatever that is
        protected EndOfComputationException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }
}
