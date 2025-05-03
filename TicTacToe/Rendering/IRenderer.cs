using TicTacToe.Domain;

namespace TicTacToe.Rendering
{
    /// <summary>
    /// Interface for rendering the game state
    /// </summary>
    public interface IRenderer
    {
        /// <summary>
        /// Render the current game state
        /// </summary>
        /// <param name="gameState">Current state of the game</param>
        void Render(GameState gameState);
    }
}