using System;

namespace CompTheoProgs
{
    public interface IProgram
    {
        Computation NewComputation(IMachine mach, string input);
    }
}
