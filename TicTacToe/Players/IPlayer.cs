using TicTacToe.Domain;

namespace TicTacToe.Players
{
    /// <summary>
    /// Interface for a player that can make moves in Tic Tac Toe
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// Decide on and return the next move
        /// </summary>
        /// <param name="gameState">Current game state</param>
        /// <returns>Action index (0-8) representing the chosen move</returns>
        int MakeMove(GameState gameState);
    }
}