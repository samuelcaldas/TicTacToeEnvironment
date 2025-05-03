using System;
using System.Collections.Generic;
using System.Linq;
using TicTacToe.Domain;

namespace TicTacToe.Players
{
    /// <summary>
    /// AI player implementation that can be replaced with Actor-Critic model
    /// </summary>
    public class AIPlayer : IPlayer
    {
        private readonly Random _random = new Random();
        
        /// <summary>
        /// Make a move based on the current game state
        /// </summary>
        /// <param name="gameState">Current game state</param>
        /// <returns>Chosen action index (0-8)</returns>
        public int MakeMove(GameState gameState)
        {
            try
            {
                List<int> legalActions = gameState.GetLegalActions();
                
                if (legalActions.Count == 0)
                    throw new InvalidOperationException("No valid moves available");
                
                // For demonstration, this uses a simple random strategy
                // In practice, this would use your trained Actor-Critic model
                return legalActions[_random.Next(legalActions.Count)];
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error in AI move selection: {ex.Message}", ex);
            }
        }
    }
}