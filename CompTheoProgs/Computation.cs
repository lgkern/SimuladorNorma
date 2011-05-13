using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompTheoProgs
{
    /*  Abstract class for a computation, which keeps
     * track of all steps of execution for a program.
     * 
     *  Doesn't execute automatically, has a few methods
     * for doing it in a controlled way.
     * 
     *  Implements the observer pattern, being an observable
     * class. Already implements all the subscription, so 
     * subclasses don't need to.
     * 
     *  Also implements some protected convenience methods
     * so subclasses don't have to do much to notify observers.
     */
    public abstract class Computation
    {
        /*
         *  Fields and Properties
         */
        private IList<string> steps;
        protected IMachine machine;
        private string result;
        private ICollection<IComputationObserver> observers;

        // Public getter for whether or not it has finished
        public bool Finished { get { return result != null; } }

        // Public getter for the result, reuturns null when not finished
        public string Result { get { return result; } }



        /*
         *  Public, common methods
         */
        // Constructor that initializes internal structures
        public Computation(IMachine mach)
        {
            result = null;
            steps = new List<string>();
            observers = new LinkedList<IComputationObserver>();
            machine = mach;
        }

        /*  Creates a string representing the computation,
         * listing each step already ran.
         */
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



        /*
         *  Methods for running the computation
         */
        
        // Runs until finished
        public void Run()
        {
            while (!Finished)
            {
                RunStep();
            }
        }

        /*  Runs at most maxSteps (may finish execution before that),
         * returns the number of steps actually executed.
         */
        public int Run(int maxSteps)
        {
            int i;

            for (i = 0; i < maxSteps && !Finished; i++)
            {
                RunStep();
            }

            return i;
        }

        /*  Template method for running a step.
         * 
         *  Throws an EnfOfComputationException when
         * called after the computation has ended.
         * 
         *  Subclasses must define the method 
         * 'ExecuteSingleStep' for this to work.
         * 
         *  There are also two hooks:
         *   - StepWhenFinishedHook
         *   - EndOfComputationHook
         */
        public void RunStep()
        {
            bool finishedAfterExecution;

            // Must not run step if already finished
            if (Finished)
            {
                StepWhenFinishedHook();
                throw new EndOfComputationException();
            }

            finishedAfterExecution = ExecuteSingleStep();

            // If the computation ended, sets the result
            if (finishedAfterExecution)
            {
                EndOfComputationHook();
                SetResult(machine.GetValue());
            }
        }

        /* Abstract method for processing the execution of
         * a single step, different for each kind of program.
         * 
         * Returns true if running step caused the computation to end
         */
        protected abstract bool ExecuteSingleStep();

        // Hooks for the RunStep template method
        protected virtual void StepWhenFinishedHook() { }
        protected virtual void EndOfComputationHook() { }

        /*
         *  Methods for the observer pattern
         *   1. Public subscription methods
         *   2. Protected setters that notify observers
         *   3. Private notification methods
         */
        public void AddObserver(IComputationObserver obs)
        {
            observers.Add(obs);
        }

        public void RemoveObserver(IComputationObserver obs)
        {
            observers.Remove(obs);
        }

        // Adds a step to the computation and notifies observers of it
        protected void AddStep(string program, string state)
        {
            string step;
                
            step = "( " + program + ", " + state + " )";
            steps.Add(step);
            NotifyObserversOfStep(step);
        }

        // Sets the result of the computation and notifies observers of it
        protected void SetResult(string result)
        {
            this.result = result;
            NotifyObserversOfResult(result);
        }

        private void NotifyObserversOfStep(string step)
        {
            foreach (IComputationObserver o in observers)
            {
                o.UpdateStepDone(step);
            }
        }

        private void NotifyObserversOfResult(string result)
        {
            foreach (IComputationObserver o in observers)
            {
                o.UpdateResult(result);
            }
        }
    }

    /* The exception class for when running a computation 
     * is asked after it has ended
     */
    [Serializable()]
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
