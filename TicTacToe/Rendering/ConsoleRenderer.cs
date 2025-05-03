using System;
using TicTacToe.Domain;

namespace TicTacToe.Rendering
{
    /// <summary>
    /// Console-based renderer for Tic Tac Toe game
    /// </summary>
    public class ConsoleRenderer : IRenderer
    {
        /// <summary>
        /// Render the game state to the console
        /// </summary>
        /// <param name="gameState">Current game state</param>
        public void Render(GameState gameState)
        {
            Console.Clear();
            Console.WriteLine("Tic Tac Toe");
            Console.WriteLine("===========\n");
            
            for (int i = 0; i < 3; i++)
            {
                Console.Write("  ");
                for (int j = 0; j < 3; j++)
                {
                    string symbol = GetSymbol(gameState.Board.GetAt(i, j));
                    Console.Write($" {symbol} ");
                    if (j < 2) Console.Write("|");
                }
                Console.WriteLine();
                
                if (i < 2)
                {
                    Console.WriteLine("  -----------");
                }
            }
            
            Console.WriteLine();
            Console.WriteLine($"Current player: {gameState.CurrentPlayer}");
            Console.WriteLine($"Game status: {gameState.GameStatus}");
            Console.WriteLine();
        }
        
        private string GetSymbol(Player player)
        {
            return player switch
            {
                Player.Human => "X",
                Player.AI => "O",
                _ => " "
            };
        }
    }
}