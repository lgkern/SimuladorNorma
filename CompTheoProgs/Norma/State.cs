using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompTheoProgs.Norma
{
    public class State : Dictionary<string, int>
    {
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
