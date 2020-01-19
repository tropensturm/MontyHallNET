using System;
using System.Collections.Generic;

namespace tropensturm.playgrounds
{
    class Program
    {
        /*
         * rules
         * 1. a price is randomly hidden behind one of three doors
         * 2. all doors are closed at the beginning
         * 3. the player chooses a door (but it keeps closed)
         * 4A. the player choosed the door with the price, the host is choosing one of the other doors with a hidden strategy
         * 4B. the player choosed a door without a win, the host has to open the other door without a win
         * 5. the host gives the player the choice to switch the door
         * 6. the finally choosen door is opened
         * 
         * 4A -> standard:
         * host is choosing the door to open randomly so long till he hit not the price nor the choosen one
         * 
         * 4A -> fallen host:
         * has the player choosed the door with the price, the host is picking the other door with an even probability
         * 
         * 4A -> crawling host:
         * the host is always opening door with the highest numbering*
         * 
         * *): the strategy how the host chooses the door to open is pointless for the outcome as long as the strategy is unknown 
         * to the player but when the player knows that when the host is crawling on the floor and in this case will always choice 
         * the highest numbering door, than he can optimize his winning chances in a non switching scenario to 5/9 (~55,55%)
         *      -> when the player choosed door 1 and the host opens door 2, the price must be behind door 3
         *      -> when the player choosed door 3 and the host opens door 1, the price must be behind door 2
         * but the irony is by switching he still will have better chances
         */
        static void Main(string[] args)
        {
            Func<Func<List<int>, (int, string)>, bool, bool, decimal> gameCycles = (f, switching, strategy) =>
            {
                int rounds = 12 * 9 * 10000; // because of the possible outcomes
                decimal denominator = rounds / 100;
                int cnt = 0;

                for (int i = 1; i < rounds; i++)
                {
                    cnt += MontyHallNET.Play(f, switching, strategy) ? 1 : 0;
                }

                return cnt / denominator;
            };

            Console.WriteLine($"monty hall without switching = {string.Format("{0:0.00}", gameCycles(MontyHallNET.Hall, false, false))}% (Bayes predicts ~33.33% (1/3))");
            Console.WriteLine($"monty hall with switching = {string.Format("{0:0.00}", gameCycles(MontyHallNET.Hall, true, false))}% (Bayes predicts ~66.66% (2/3))");

            Console.WriteLine($"monty fall without switching = {string.Format("{0:0.00}", gameCycles(MontyHallNET.Fallen, false, false))}% (Bayes predicts ~33.33% (1/3))");
            Console.WriteLine($"monty fall with switching = {string.Format("{0:0.00}", gameCycles(MontyHallNET.Fallen, true, false))}% (Bayes predicts ~66.66% (2/3))");

            Console.WriteLine($"monty crawl without switching (and player doesn't know hosts' strategy) = {string.Format("{0:0.00}", gameCycles(MontyHallNET.Crawl, false, false))}% (Bayes predicts ~33.33% (1/3))");
            Console.WriteLine($"monty crawl with switching (and player does recognize hosts' strategy)  = {string.Format("{0:0.00}", gameCycles(MontyHallNET.Crawl, true, true))}% (Bayes predicts ~66.66% (2/3))");
            Console.WriteLine($"monty crawl without switching (and player does recognize hosts' strategy) = {string.Format("{0:0.00}", gameCycles(MontyHallNET.Crawl, false, true))}% (Bayes predicts ~55,55% (5/9))");

            // render outputs
            Console.WriteLine(Environment.NewLine + "monty crawl (no switching + known strategy) the 9 possibilites are:");
            foreach (var r in MontyHelpers.Output["crawl-spotted"])
                Console.WriteLine(r.Key + " -> " + r.Value);

            Console.WriteLine(Environment.NewLine + "monty hall (before switching) the 12 possibilites are:");
            foreach (var r in MontyHelpers.Output["hall"])
                Console.WriteLine(r.Key + " -> " + r.Value);
            Console.WriteLine("result provides 6 looses with a double possibility, because 3 of the 6 wins are mirrored counterparts (|O|Px|_| & O|_|Px| & |Px|O|_|). When we merge these we have 3 wins and 6 losses equal weighted => 1/3");

            Console.WriteLine(Environment.NewLine + "monty hall (after switching) the 12 possibilites are:");
            foreach (var r in MontyHelpers.Output["hall-switched"])
                Console.WriteLine(r.Key + " -> " + r.Value);
            Console.WriteLine("we turned the 6 looses into 6 wins and 3 wins into 3x2 losses. So equal weighted this results in 9 possibilites with 6 winning outcomes => 2/3");
        }
    }
}
