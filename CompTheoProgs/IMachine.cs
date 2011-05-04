using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompTheoProgs
{
    public interface IMachine
    {
        void executeOperation(string operationID);
        bool executeTest(string testID);

        void PutValue(string input);
        string GetValue();

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
}
