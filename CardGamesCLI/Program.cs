using System;
using System.Collections.Generic;
using System.Linq;
using CardGames.Core.Durak;
using static CardGames.Core.Durak.DurakGame;

namespace CardGamesCLI
{
    class Program
    {
        static DurakGame game;
        static int previousDefender = -1;

        static void Main(string[] args)
        {
            Setup();
            MainLoop();
        }

        static void Setup()
        {
            int playerCount = 3;
            game = new DurakGame();

            for (int i = 0; i < playerCount; i++)
                game.AddPlayer(i);

            game.Start();
            previousDefender = game.DefenderIndex;
        }

        static void MainLoop()
        {
            while (true)
            {
                Console.WriteLine("========================================================");
                SortHands();
                PrintHands();
                PrintAttacks();

                Console.WriteLine($"\nTrump: ---{game.Trump}---");
                Console.WriteLine($"Deck: {game.Deck.Count}");
                Console.WriteLine($"Defender: Player{game.DefenderIndex}\n");

                if (previousDefender != game.DefenderIndex)
                {
                    Console.WriteLine("--- NEW DEFENDER ---");
                    Console.WriteLine("--- NEW DEFENDER ---");
                    Console.WriteLine("--- NEW DEFENDER ---");
                    previousDefender = game.DefenderIndex;
                }

                int playerId;
                PlayerRole role;
                do 
                {
                    playerId = ReadInt("Select player", game.Players.Count);
                    role = game.GetPlayerRole(playerId);
                }
                while (role == PlayerRole.None);

                Player player = game.Players[playerId];
                Console.WriteLine($"\nRole: {role}");
                Console.WriteLine($"Your hand:");
                PrintHand(playerId);

                int cardIndex = ReadInt("Card index", player.Hand.Count);

                Card card = player.Hand[cardIndex];
                IReadOnlyList<Card> unbeaten = game.Attacks.Unbeaten();

                if (role == PlayerRole.Attacker || unbeaten.Count < 2)
                {
                    Try(() => game.Turn(playerId, card));
                }
                else
                {
                    Console.WriteLine("Unbeaten cards:");
                    
                    for (int i = 0; i < unbeaten.Count; i++)
                        Console.WriteLine($"{i}. {unbeaten[i]}");

                    int targetIndex = ReadInt($"Select target", unbeaten.Count);
                    Card target = unbeaten[targetIndex];

                    Try(() => game.Turn(playerId, card, target));
                }

                Console.WriteLine();
            }
        }

        static void Try(Action action)
        {
            try
            {
                action();
            }
            catch (GameException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static void PrintAttacks()
        {
            if (game.Attacks.Any())
            {
                Console.WriteLine("\nAttack entries:");

                for (int i = 0; i < game.Attacks.Count; i++)
                {
                    var attack = game.Attacks[i];

                    Console.WriteLine(
                        $"{i + 1}. {attack.Attacker} -> " +
                        $"{(attack.IsBeaten ? attack.Defender.ToString() : "None")}");
                }
            }
            else
                Console.WriteLine("\nNo attack entries");
        }

        static void PrintHands()
        {
            var hands = game.Players.Select(p => p.Hand.ToArray());
            var forTurn = game.Players.Select(p => game.GetCardsForTurn(p.Id)).ToArray();

            int max = hands.Max(h => h.Length);
            var players = game.Players.Select(p => $"{$"Player{p.Id}", 16}");
            Console.WriteLine(string.Join("", players));

            for (int i = 0; i < max; i++)
            {
                var set = hands
                    .Select(h => h.Length > i ? h[i] : null)
                    .Select((c, j) => $"{c,16} {(forTurn[j].Contains(c) ? "+" : " ")}");

                Console.WriteLine(string.Join("", set));
            }
        }

        static void PrintHand(int playerId)
        {
            Player player = game.Players[playerId];
            var forTurn = game.GetCardsForTurn(playerId);

            for (int i = 0; i < player.Hand.Count; i++)
            {
                Card card = player.Hand[i];
                Console.WriteLine($"{i}. {card} {(forTurn.Contains(card) ? "+" : "")}");
            }
            Console.WriteLine();
        }

        static void SortHands()
        {
            foreach (var player in game.Players)
            {
                player.Hand = player.Hand
                    .OrderBy(c => c.Suit)
                    .ThenByDescending(c => c.Value)
                    .ToList();
            }
        }
    
        static int ReadInt(string msg, int count)
        {
            Console.Write($"{msg}: ");

            int num;
            do 
            {
                while (!int.TryParse(Console.ReadLine(), out num))
                    Console.WriteLine("Incorrect input, try again");
            }
            while (num < 0 || num >= count);
                
            return num;
        }
    }
}
