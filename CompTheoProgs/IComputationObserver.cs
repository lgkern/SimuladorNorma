using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompTheoProgs
{
    /*
     *  An observer interface for a computation. May
     * subscribe to a computation, then is notified
     * when a step is ran and when the result is obtained
     */
    public interface IComputationObserver
    {
        /*  Called when a step is ran, receives a string
         * representation of the reached program and state
         */
        void UpdateStepDone(string step);

        /*  Called when the result is obtained, receives
         * the result of the machine's output function.
         */
        void UpdateResult(string result);
    }
}
