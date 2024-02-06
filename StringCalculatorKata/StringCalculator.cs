using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace StringCalculatorKata
{
    public class StringCalculator
    {
        public StringCalculator()
        {
            this.AddOccured += AddOccured_addCounter;
        }

        private void AddOccured_addCounter(string input, int result)
        {
            _addCounter++;
        }

        private int _addCounter = 0;
        public int Add(string numbers)
        {
            int result = 0;
            if (!String.IsNullOrWhiteSpace(numbers))
            {
                //var delimiter = getDelimiter(numbers);
                string delim = String.Empty;
                if (numbers.StartsWith("//"))
                {
                    delim = numbers.Substring(2, 1);
                    numbers = numbers.Substring(3);
                }

                var numberslists = numbers.Split(new[] { ",", "\n", $"{delim}" }, StringSplitOptions.RemoveEmptyEntries);

                bool hasNegatives = false;
                List<string> negativeNumbers = new List<string>();
                foreach (var number in numberslists)
                {
                    if (int.TryParse(number, out int intresult))
                    {
                        if (intresult < 0)
                        {
                            hasNegatives = true;
                            negativeNumbers.Add(intresult.ToString());
                        }
                        else if (intresult > 1000)
                        {
                            continue;
                        }
                        result += intresult;
                    }
                    else
                    {
                        throw new Exception("Invalid User Input");
                    }
                }

                if (hasNegatives) throw new Exception($"negatives not allowed: {string.Join(",", negativeNumbers)}");
            }
            AddOccured?.Invoke(numbers, result);
            return result;

        }

        private string getDelimiter(string numbers)
        {
            throw new NotImplementedException();
        }

        public int GetCalledCount()
        {
            return _addCounter;
        }

        public event AddOccuredDelegate? AddOccured;

        public delegate void AddOccuredDelegate(string input, int result);
    }
}
