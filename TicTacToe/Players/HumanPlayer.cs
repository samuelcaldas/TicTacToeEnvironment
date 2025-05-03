using System;
using System.Collections.Generic;
using TicTacToe.Domain;

namespace TicTacToe.Players
{
    /// <summary>
    /// Implementation of a human player that takes input from console
    /// </summary>
    public class HumanPlayer : IPlayer
    {
        /// <summary>
        /// Get the human's move from console input
        /// </summary>
        /// <param name="gameState">Current game state</param>
        /// <returns>Chosen action index (0-8)</returns>
        public int MakeMove(GameState gameState)
        {
            List<int> legalActions = gameState.GetLegalActions();
            if (legalActions.Count == 0)
                throw new InvalidOperationException("No valid moves available");
                
            int action = -1;
            bool validInput = false;
            
            while (!validInput)
            {
                try
                {
                    Console.WriteLine("Enter your move (row [0-2] and column [0-2], e.g., '1 2'): ");
                    string input = Console.ReadLine();
                    
                    string[] coordinates = input?.Split(new[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (coordinates == null || coordinates.Length != 2)
                    {
                        Console.WriteLine("Please enter two coordinates separated by space.");
                        continue;
                    }
                    
                    int row = int.Parse(coordinates[0]);
                    int col = int.Parse(coordinates[1]);
                    
                    if (row < 0 || row > 2 || col < 0 || col > 2)
                    {
                        Console.WriteLine("Coordinates must be between 0 and 2.");
                        continue;
                    }
                    
                    action = row * 3 + col;
                    
                    if (!legalActions.Contains(action))
                    {
                        Console.WriteLine("That cell is already occupied. Try again.");
                        continue;
                    }
                    
                    validInput = true;
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid input. Please enter numbers for row and column.");
                }
            }
            
            return action;
        }
    }
}