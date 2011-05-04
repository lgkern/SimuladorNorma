using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CompTheoProgs.Norma
{
    public class Machine : CompTheoProgs.IMachine
    {
        private State registers;
        private static Regex operRE = new Regex("(ad|sub)_([a-zA-Z0-9]+)", RegexOptions.Compiled);
        private static Regex testRE = new Regex("([a-zA-Z0-9]+)_zero", RegexOptions.Compiled);

        public Machine()
        {
            registers = new State();
        }

        public void executeOperation(string operationID)
        {
            Match opMatch = operRE.Match(operationID);

            if (!opMatch.Success)
                
                throw new InvalidMachineInstructionException("Invalid operation for NORMA machine");

            else if (opMatch.Groups[1].Value == "soma")
            {
                try
                {
                    registers[opMatch.Groups[2].Value]++;
                }
                catch (KeyNotFoundException)
                {
                    registers[opMatch.Groups[2].Value] = 1;
                }
            }

            else
            {
                try
                {
                    registers[opMatch.Groups[2].Value]--;
                }
                catch (KeyNotFoundException)
                {
                    registers[opMatch.Groups[2].Value] = 1;
                }
            }
        }

        public bool executeTest(string testID)
        {
            Match testMatch = testRE.Match(testID);

            if (testMatch.Success)
            {
                try
                {
                    return registers[testMatch.Groups[1].Value] == 0;
                }
                catch (KeyNotFoundException)
                {
                    registers[testMatch.Groups[1].Value] = 0;
                    return true;
                }
            }
            else
            {
                throw new InvalidMachineInstructionException("Invalid test for Norma");
            }
        }

        public void PutValue(string input)
        {
            registers["X"] = Convert.ToInt32(input);
        }

        public string GetValue()
        {
            try
            {
                return registers["Y"].ToString();
            }
            catch (KeyNotFoundException)
            {
                registers["Y"] = 0;
                return "0";
            }
        }

        public string CurrentState
        {
            get { return registers.ToString(); }
        }

        public State Registers
        {
            get { return registers; }
        }
    }
}
