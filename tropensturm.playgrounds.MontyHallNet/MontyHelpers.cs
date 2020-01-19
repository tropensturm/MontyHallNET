using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tropensturm.playgrounds
{
    public static class MontyHelpers
    {
        public static Dictionary<string, Dictionary<string, int>> Output = new Dictionary<string, Dictionary<string, int>>();

        public static void Renderer(string strategy, int price, int choice, int opened, bool win)
        {
            if (!Output.Keys.Contains(strategy))
                Output.Add(strategy, new Dictionary<string, int>());

            string[] field = new string[] { string.Empty, string.Empty, string.Empty };

            if (choice != price)
            {
                field[price] += "P ";
                field[choice] += "x ";
            }
            else
            {
                field[price] += "P";
                field[choice] += "x";
            }
            field[opened] += "O ";

            var outcome = field.Aggregate((x, y) =>
                string.IsNullOrWhiteSpace(y) ?
                    (string.IsNullOrWhiteSpace(x) ?
                        "__" :
                        x)
                    + "|__" :
                    (string.IsNullOrWhiteSpace(x) ?
                        "__" :
                        x)
                    + "|" + y
                ) + (win ? "| win  " : "| loose");

            if (!Output[strategy].Keys.Contains(outcome))
                Output[strategy].Add(outcome, 1);
            else
                Output[strategy][outcome]++;
        }

    }
}
