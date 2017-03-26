using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FSMCalc
{
    public delegate void ChangeDisplay(string result);

    public class States
    {
        public ChangeDisplay invoker;

        public string number = "";
        public string result = "";
        public char op = '.';

        public char[] operations = { '+', '-' ,'/','x'};
        public char[] digits = { '0', '1','2','3','4','5','6','7','8','9' };
        public char[] nozerodigits = { '1','2','3','4','5','6','7','8','9' };
        public char[] zero = { '0' };
        public char[] separators = { '.', ',' };
        public char[] clear = { 'c' };
        public char[] equals = { '=' };
        public char[] singleoperations = { 's', 'i', '√' };
        public string stateName = "Zero";

        public void Process(char item)
        {
            switch (stateName)
            {
                case "Zero":
                    Zero(item, false);
                    break;
                case "AccumulateDigits":
                    AccumulateDigits(item, false);
                    break;
          
                case "ComputePending":
                    ComputePending(item, false);
                    break;
                case "Compute":
                    Compute(item, false);
                    break;
            }
        }

        public void Zero(char item,bool isInput)
        {
            if (isInput)
            {
                result = "";
                stateName = "Zero";
                invoker.Invoke(result);
            }
            else
            {
                if (nozerodigits.Contains(item))
                {
                    AccumulateDigits(item, true);
                }
            }
        }

        public void AccumulateDigits(char item, bool isInput)
        {
            if (isInput)
            {
                result += item;
                stateName = "AccumulateDigits";
                invoker.Invoke(result);
            }
            else
            {
                if (digits.Contains(item) || separators.Contains(item))
                {
                    AccumulateDigits(item,true);
                }
                else if (operations.Contains(item))
                {
                    ComputePending(item,true);
                }
                else if (equals.Contains(item))
                {
                    Compute(item,true);
                }
                else if (singleoperations.Contains(item))
                {
                    Compute(item, true);
                }
            }
        }

        public void ComputePending(char item, bool isInput)
        {
            if (isInput)
            {
                op = item;
                number = result;
                result = "";
                stateName = "ComputePending";
                invoker.Invoke(result);
            }
            else
            {
                if (digits.Contains(item))
                {
                    AccumulateDigits(item, true);
                }
            }
        }
        public void Compute(char item, bool isInput)
        {
            if (isInput)
            {
                
                double a2 = double.Parse(result);
                double r = 0;
                if (item == '=')
                {
                    double a1 = double.Parse(number);
                    if (op == '+')
                    {
                        r = a1 + a2;
                    }
                    else if (op == '-')
                    {
                        r = a1 - a2;
                    }
                    else if (op == '/')
                    {
                        r = a1 / a2;
                    }
                    else if (op == 'x')
                    {
                        r = a1 * a2;
                    }
                }
                else
                {
                    if(item == 'i')
                    {
                        r = 1 / a2;
                    }
                   else if (item == 's')
                    {
                        r = a2* a2;
                    }
                   else if (item == '√')
                    {
                        r = Math.Sqrt(a2);
                    }
                }
                result = r.ToString();
                stateName = "Compute";
                invoker.Invoke(result);
                number = result;
                
            }
            else
            {
                if (zero.Contains(item))
                {
                    Zero(item,true);
                }
                else if (nozerodigits.Contains(item))
                {
                   /// number = result;
                    result = "";
                    AccumulateDigits(item,true);
                }
            }
        }
    }
}
