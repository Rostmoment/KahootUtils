using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KahootUtils
{
    class KahootChoice(string text, bool correct)
    {
        public string Text { get; } = text;
        public bool Correct { get; } = correct;

        public override string ToString()
        {
            return $"{Text} {(Correct ? "(Correct)" : "")}";
        }
    }
}
