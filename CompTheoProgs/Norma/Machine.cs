using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CompTheoProgs.Norma
{

    /* Implementation of the NORMA machine
     *
     * May use as many registers as required, each
     * containing a natural number and identified
     * by a string. All registers are initialized
     * on zero.
     * 
     * Accepts two kinds of operations: 'ad_X' and 
     * 'sub_X', where X is the string identifying
     * a register.
     * 
     * Accepts one kind of tests: 'X_zero', where
     * X is the string identifying a register.
     * 
     * Uses the register 'X' for input and
     * the register 'Y' for output.
     */
    public class Machine : CompTheoProgs.IMachine
    {
        private State registers;
        private static Regex operRE = new Regex("(ad|sub)_([a-zA-Z0-9]+)", RegexOptions.Compiled);
        private static Regex testRE = new Regex("([a-zA-Z0-9]+)_zero", RegexOptions.Compiled);

        /* Constructor only initializes all registers on zero.
         */
        public Machine()
        {
            registers = new State();
        }

        /* Executes 'ad_X' by incrementing the register X
         * or 'sub_X' by decrementing it.
         * Throws an exception for other operations
         */
        public void executeOperation(string operationID)
        {
            Match opMatch = operRE.Match(operationID);

            if (!opMatch.Success)
                throw new InvalidMachineInstructionException("Invalid operation for NORMA machine");

            else if (opMatch.Groups[1].Value == "ad")
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

        /* Executes 'X_zero', returning true if the register
         * X has value 0. Throws an exception for other tests.
         */
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

        /* Input function: must receive a string representation
         * of a natural number. Initializes all registers on zero,
         * then assigns the given value to register 'X'.
         */
        public void PutValue(string input)
        {
            uint value;

            try
            {
                value = Convert.ToUInt32(input);
            }
            catch (Exception)
            {
                throw new InvalidMachineValueException();
            }

            registers = new State();
            registers["X"] = value;
        }

        /* Output function: returns the string representation
         * of the value of register Y.
         */
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

        /* String representation of current state
         */
        public string CurrentState
        {
            get { return registers.ToString(); }
        }
    }
}
