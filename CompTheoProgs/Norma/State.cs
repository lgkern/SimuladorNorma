using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompTheoProgs.Norma
{
    /* The state of a Norma machine: a set of registers.
     * 
     * Actually maps a string to each 
     */
    public class State
    {
        private Dictionary<string, uint> registers;

        /* Public register property, defaults all
         * registers to zero.
         */
        public uint this[string key]
        {
            get
            {
                try
                {
                    return registers[key];
                }
                catch (KeyNotFoundException)
                {
                    registers[key] = 0;
                    return 0;
                }
            }

            set { registers[key] = value; }
        }

        // Constructor initializes an empty dictionary of registers
        public State()
        {
            registers = new Dictionary<string, uint>();
        }

        // Generates a string with all defined registers/values
        public override string ToString()
        {
            string result = "{ ";

            foreach (string k in this.Keys)
            {
                result += k + ":" + this[k] + ", ";
            }

            return result.Substring(0, result.Length-2) + " }";
        }


    }
}
