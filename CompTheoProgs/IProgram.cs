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
        /// <summary>
        /// Creates a computation for the program instance.
        /// </summary>
        /// <param name="mach">The machine used as is on the computation.</param>
        /// <returns>The computation created.</returns>
        /// <exception cref="InvalidMachineValueException">When the input is invalid for the machine.</exception>
        Computation NewComputation(IMachine mach);
    }
}
