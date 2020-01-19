using System;
using System.Collections.Generic;
using System.Linq;

namespace tropensturm.playgrounds
{
    public static class MontyHallNET
    {
        private static readonly Random getrandom = new Random();

        private static int getRandomDoor(int min = 0, int max = 3)
        {
            lock (getrandom) // synchronize
            {
                return getrandom.Next(min, max);
            }
        }

        private static Func<List<int>> doorPool = () => { return new List<int>() { 0, 1, 2 }; };

        #region host strategies
        public static Func<List<int>, (int, string)> Crawl = leftPossibilities =>
        {
            return (leftPossibilities.Last(), "crawl");
        };

        public static Func<List<int>, (int, string)> Fallen = leftPossibilities =>
        {
            var name = "fallen";

            if (leftPossibilities.Count == 1)
                return (leftPossibilities.Single(), name);

            int pick = getRandomDoor(0, 2); // we flip a coin with 50/50 chance if we pick the first or the last door
            if (pick == 0)
                return (leftPossibilities.First(), name);

            return (leftPossibilities.Last(), name);
        };

        public static Func<List<int>, (int, string)> Hall = leftPossibilities =>
        {
            var name = "hall";

            if (leftPossibilities.Count == 1)
                return (leftPossibilities.Single(), name);

            // each door is picked with a 33.3% chance but only one that is possible will be opened in the end
            while (true)
            {
                int pick = getRandomDoor();
                if (leftPossibilities.Contains(pick))
                    return (pick, name);
            }
        };
        #endregion host strategies
        
        public static bool Play(Func<List<int>, (int, string)> hostStrategy, bool switching = false, bool spottedHostStrategy = true)
        {
            List<int> possibilities = doorPool();
            List<int> gameField = doorPool();

            // game is hiding the price
            int price = getRandomDoor();

            // players choice
            int choice = getRandomDoor();

            // the host wont open either the door with the price nor the players selected door
            possibilities.Remove(price);
            possibilities.Remove(choice);

            // apply host strategy 
            (int open, string strategy) = hostStrategy(possibilities);
            var spottedStrategy = strategy; // safe name back
            if (!spottedHostStrategy)
                spottedStrategy = string.Empty;

            // choices left in the game for the player
            gameField.Remove(choice);
            gameField.Remove(open);

            #region rendering
            switch (spottedStrategy)
            {
                case "crawl":
                    strategy += "-spotted" + (switching ? "-switched" : "");
                    var win = false;

                    if (open == 1 && choice == 0)
                        win = true;
                    else if (open == 0 && choice == 2)
                        win = true;
                    else if (switching)
                        win = price == gameField.Single();
                    else
                        win = price == choice;

                    MontyHelpers.Renderer(strategy, price, choice, open, win);

                    break;
                default:
                    strategy += (switching ? "-switched" : "");
                    var finalChoice = switching ? gameField.Single() : choice;

                    MontyHelpers.Renderer(strategy, price, finalChoice, open, price == finalChoice);
                    
                    break;
            }
            #endregion rendering

            // return result
            switch (spottedStrategy)
            {
                case "crawl":
                    // logical switch when we know the host strategy:
                    // we know the host picked the highest numbered door without a price
                    // when it was 2 and we picked door 1 the price must be behind 3
                    if (open == 1 && choice == 0)
                        return true;
                    // logical switch when we know the host strategy:
                    // when the host picked door 1 but we selected door 3 than the price 
                    // must be door 2
                    else if (open == 0 && choice == 2)
                        return true;
                    else if (switching)
                        choice = gameField.Single();

                    return price == choice;
                default:
                    if (switching)
                        choice = gameField.Single();

                    return price == choice;
            }
        }
    }
}
