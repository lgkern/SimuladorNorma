using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompTheoProgs
{
    public interface IComputationObserver
    {
        void UpdateStepDone(string step);
        void UpdateResult(string result);
    }
}
