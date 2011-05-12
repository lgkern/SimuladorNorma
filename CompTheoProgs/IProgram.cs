using System;

namespace CompTheoProgs
{
    /*  An interface for program classes.
     * 
     *  Since the internal structure varies broadly,
     * this interface imposes no constraint on it.
     * 
     *  Must only know how to create a computation
     * instance for the current program, given a
     * machine and an input value for it.
     */
    public interface IProgram
    {
        /*  Creates a computation for the program instance,
         * on the given machine which receives the given input
         * value.
         * 
         *  May throw an InvalidMachineValueException if
         * the given input value is invalid for the machine.
         */
        Computation NewComputation(IMachine mach, string input);
    }
}
