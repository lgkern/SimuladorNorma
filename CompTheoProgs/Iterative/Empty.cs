using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompTheoProgs.Iterative
{
    class Empty : Program
    {
        public const string TextRepr = "*vazio*";

        public override string ToString()
        {
            return TextRepr;
        }

        internal override IList<Program> EvalAndGetProgramsToPrepend(IMachine mach)
        {
            return new List<Program>();
        }

    }
}
