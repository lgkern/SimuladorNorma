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
        /// <summary>
        /// Called by the computation every time a step is ran.
        /// </summary>
        /// <param name="progState">String representation of the current </param>
        /// <param name="machState">String representation of the current machine state</param>
        void UpdateStepDone(string progState, string machState);
        
        /// <summary>
        /// Called by the computation when the result is obtained. 
        /// The result may be looked up on the machine.
        /// </summary>
        /// <param name="observed">The computation whose result is ready.</param>
        void UpdateResult(Computation observed);
    }
}
