using System;
using System.Collections.Generic;
using System.Linq;

namespace ChipSecuritySystem
{
    internal class Program
    {
        private static readonly List<ColorChip> Chips = new List<ColorChip>();
        private static readonly Random RandomGen = new Random();

        private static void Main()
        {
            Console.Clear();
            Console.WriteLine("Unlock the Master Control Panel\n");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Generate random chips.");
            Console.WriteLine("2. Enter chips manually.");
            Console.WriteLine("3. Use example set: [Blue, Yellow] [Red, Green] [Yellow, Red] [Orange, Purple]");

            int option = GetOption();

            if (option == 1) GenerateChips();
            else if (option == 2) InputChips();
            else LoadExampleChips();

            DisplayChips();

            var chipSet = new HashSet<ColorChip>();
            var validSequence = new List<ColorChip>();

            if (FindLongestChain(Color.Blue, Color.Green, chipSet, validSequence))
            {
                ShowSuccessSequence(validSequence);
            }
            else
            {
                Console.WriteLine(Constants.ErrorMessage);
            }
        }

        private static int GetOption()
        {
            int option;
            while (!int.TryParse(Console.ReadLine(), out option) || (option < 1 || option > 3))
            {
                Console.WriteLine("Please enter a valid option (1, 2, or 3).");
            }
            return option;
        }

        private static void GenerateChips()
        {
            for (var i = 0; i < 4; i++)
            {
                var startColor = (Color)RandomGen.Next(0, 6);
                var endColor = (Color)RandomGen.Next(0, 6);
                Chips.Add(new ColorChip(startColor, endColor));
            }
        }

        private static void LoadExampleChips()
        {
            Chips.Add(new ColorChip(Color.Blue, Color.Yellow));
            Chips.Add(new ColorChip(Color.Red, Color.Green));
            Chips.Add(new ColorChip(Color.Yellow, Color.Red));
            Chips.Add(new ColorChip(Color.Orange, Color.Purple));
        }

        private static void InputChips()
        {
            Console.WriteLine("\nAvailable Colors:");
            var values = Enum.GetValues(typeof(Color));
            for (var j = 0; j < values.Length; j++)
            {
                Console.WriteLine($"{j + 1}. {values.GetValue(j)}");
            }

            for (var i = 0; i < 4; i++)
            {
                Console.WriteLine($"\nDefine Chip {i + 1}");
                Color startColor = GetColorInput("Starting Color");
                Color endColor = GetColorInput("Ending Color");
                Chips.Add(new ColorChip(startColor, endColor));
            }
        }

        private static Color GetColorInput(string colorLabel)
        {
            Console.Write($"{colorLabel}: ");
            int input;
            while (!int.TryParse(Console.ReadLine(), out input)
                || input < 1 || input > Enum.GetValues(typeof(Color)).Length)
            {
                Console.WriteLine("Invalid input. Try again.");
            }
            return (Color)(input - 1);
        }

        private static bool FindLongestChain(Color startColor, Color endColor, HashSet<ColorChip> chipSet, List<ColorChip> result)
        {
            var maxChain = new List<ColorChip>();

            foreach (var chip in Chips.Where(chip => chip.StartColor == startColor && !chipSet.Contains(chip)))
            {
                chipSet.Add(chip);
                var currentChain = new List<ColorChip> { chip };

                if (chip.EndColor == endColor || FindLongestChain(chip.EndColor, endColor, chipSet, currentChain))
                {
                    if (currentChain.Count > maxChain.Count)
                    {
                        maxChain = new List<ColorChip>(currentChain);
                    }
                }
                chipSet.Remove(chip);
            }

            result.AddRange(maxChain);
            return maxChain.Count > 0;
        }

        private static void DisplayChips()
        {
            Console.WriteLine("\nChips in Play\n-------------\n");
            foreach (var chip in Chips) Console.WriteLine("[" + chip + "]");
            Console.WriteLine();
        }

        private static void ShowSuccessSequence(IEnumerable<ColorChip> sequence)
        {
            Console.WriteLine("Correct Sequence\n----------------");
            foreach (var chip in sequence) Console.WriteLine("[" + chip + "]");
            Console.WriteLine("\nAccess Granted!\n");
        }
    }
}
