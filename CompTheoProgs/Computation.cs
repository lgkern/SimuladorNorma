using System;
using System.Collections.Generic;
using System.Linq;

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
        private IList<string> progStates, machStates;
        private bool firstStateDisplayed;
        protected IMachine machine;
        private string result;
        private ICollection<IComputationObserver> observers;

        // Public getter for whether or not it has finished
        public bool Finished { get { return result != null; } }

        // Public getter for the machine
        public IMachine Machine { get { return machine; } }



        /*
         *  Public, common methods
         */
        /// <summary>
        /// Constructor that initializes internal structures. Subclasses must create the first step.
        /// </summary>
        /// <param name="mach">The machine to be used as is on the computation</param>
        public Computation(IMachine mach)
        {
            result = null;
            progStates = new List<string>();
            machStates = new List<string>();
            observers = new LinkedList<IComputationObserver>();
            machine = mach;
            firstStateDisplayed = false;
        }

        // Creates a string representing the computation,
        // listing each step already ran.
        public override string ToString()
        {
            string result = "";

            for (int i = 0; i < progStates.Count; i++)
            {
                result += "( " + progStates[i] + "," + machStates[i] + " )";
                result += "\n";
            }

            return result;
        }



        /*
         *  Methods for running the computation
         */
        
        /// <summary>
        /// Runs until finished
        /// </summary>
        public void Run()
        {
            while (!Finished)
            {
                RunStep();
            }
        }

        /*  
         * 
         */
        /// <summary>
        /// Runs at most maxSteps (may finish execution before that),
        /// returns the number of steps actually executed.
        /// </summary>
        /// <param name="maxSteps">The maximum number of steps being ran</param>
        /// <returns></returns>
        public int Run(int maxSteps)
        {
            int i;

            for (i = 0; i < maxSteps && !Finished; i++)
            {
                RunStep();
            }

            return i;
        }

        /// <summary>
        /// Runs a single step. 
        /// Implemented as a template method, so
        /// the execution itself is defined by 
        /// each concrete subclass.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <exception cref="EndOfComputationException">When the computation has already finished.</exception>
        /*  Subclasses must define the method 
         * 'ExecuteSingleStep' for this to work.
         * 
         *  There are also two hooks:
         *   - StepWhenFinishedHook
         *   - EndOfComputationHook
         */
        public void RunStep()
        {
            // Will just display the first state at the first run
            if (!firstStateDisplayed)
            {
                NotifyObserversOfStep(progStates.First(), machStates.First());
                firstStateDisplayed = true;
                return;
            }

            // Must not run step if already finished
            if (Finished)
            {
                StepWhenFinishedHook();
                throw new EndOfComputationException();
            }

            // Actually runs the step
            bool finishedAfterExecution = ExecuteSingleStep();

            // If the computation ended, sets the result
            if (finishedAfterExecution)
            {
                EndOfComputationHook();
                SetResult(machine.GetValue());
            }
        }

        /// <summary>
        /// Abstract method for processing the execution of
        /// a single step, different for each kind of program
        /// </summary>
        /// <returns>true if running a step caused the computation to end</returns>
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

        /// <summary>
        /// Adds a step to the computation and notifies observers of it
        /// </summary>
        /// <param name="program">A string representing the current program state.</param>
        /// <param name="state">A string representing the current machine state.</param>
        protected void AddStep(string program, string state)
        {
            progStates.Add(program);
            machStates.Add(state);
            NotifyObserversOfStep(program, state);
        }

        /// <summary>
        /// Sets the result of the computation and notifies observers of it
        /// </summary>
        /// <param name="result"></param>
        protected void SetResult(string result)
        {
            this.result = result;
            NotifyObserversOfResult();
        }

        private void NotifyObserversOfStep(string progState, string machState)
        {
            foreach (IComputationObserver o in observers)
            {
                o.UpdateStepDone(progState, machState);
            }
        }

        private void NotifyObserversOfResult()
        {
            foreach (IComputationObserver o in observers)
            {
                o.UpdateResult(this);
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
