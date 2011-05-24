using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompTheoProgs
{
    /* An interface for classes that implement machines.
     * 
     * Must define some formal aspects of the machine definition:
     *  - Interpretation of operations and tests
     *  - Input/Output function
     *  
     *  The set of valid input and output values is a subset
     * of all possible strings, and may be defined implicitly
     * (rejecting some of the input values by throwing an
     * InvalidMachineValueException).
     * 
     *  The set of values for memory may be private, the
     * only requirement is that a state may be represented
     * as a string.
     */
    public interface IMachine
    {
        /*  Interpretation of operations and tests, may
         * throw an InvalidMachineInstructionException
         * if the given ID is not defined for the machine.
         */
        void executeOperation(string operationID);
        bool executeTest(string testID);

        /*  Input and output functions for the machine,
         * throw an InvalidMachineValueException
         * if given an invalid value for this machine.
         */
        void PutValue(string input);
        void PutValues(IList<string> input);
        string GetValue();
        IList<string> GetValues();
        IList<string> GetValues(int num);

        // A string representation of the current state.
        string CurrentState
        { get; }
    }

    [Serializable()]
    public class InvalidMachineInstructionException : Exception
    {
        public InvalidMachineInstructionException() : base() { }
        public InvalidMachineInstructionException(string message) : base(message) { }
        public InvalidMachineInstructionException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is necessary for serialization, whatever that is
        protected InvalidMachineInstructionException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }

    [Serializable()]
    public class InvalidMachineValueException : Exception
    {
        public InvalidMachineValueException() : base() { }
        public InvalidMachineValueException(string message) : base(message) { }
        public InvalidMachineValueException(string message, System.Exception inner) : base(message, inner) { }

        // A constructor is necessary for serialization, whatever that is
        protected InvalidMachineValueException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }
}
