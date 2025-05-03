using System;
using System.Collections.Generic;
using TicTacToe.Domain;
using TicTacToe.Players;
using TicTacToe.Rendering;

namespace TicTacToe.Environment
{
    /// <summary>
    /// Tic Tac Toe environment following OpenAI Gym-like interface
    /// </summary>
    public class TicTacToeEnvironment : IEnvironment<int, float[], Dictionary<string, object>>
    {
        private readonly GameState _gameState;
        private readonly IPlayer _humanPlayer;
        private readonly IPlayer _aiPlayer;
        private readonly IRenderer _renderer;

        public TicTacToeEnvironment(IPlayer humanPlayer, IPlayer aiPlayer, IRenderer renderer)
        {
            _gameState = new GameState();
            _humanPlayer = humanPlayer;
            _aiPlayer = aiPlayer;
            _renderer = renderer;
        }

        /// <summary>
        /// Reset the environment to initial state
        /// </summary>
        /// <returns>Initial observation and info</returns>
        public Tuple<float[], Dictionary<string, object>> Reset()
        {
            _gameState.Reset();
            return new Tuple<float[], Dictionary<string, object>>(
                _gameState.GetObservation(),
                new Dictionary<string, object> { { "legal_actions", _gameState.GetLegalActions() } }
            );
        }

        /// <summary>
        /// Take a step in the environment using the given action
        /// </summary>
        /// <param name="action">Action index (0-8)</param>
        /// <returns>Next state, reward, done flag, and info</returns>
        public Tuple<float[], float, bool, Dictionary<string, object>> Step(int action)
        {
            try
            {
                // Human takes action
                if (!_gameState.MakeMove(action))
                {
                    throw new InvalidOperationException("Invalid action");
                }
                
                float humanReward = _gameState.GetReward(Player.Human);
                bool isDone = _gameState.IsTerminated();
                
                // If game not over, AI takes turn
                if (!isDone)
                {
                    int aiAction = _aiPlayer.MakeMove(_gameState);
                    _gameState.MakeMove(aiAction);
                    
                    // Recalculate after AI move
                    humanReward = _gameState.GetReward(Player.Human);
                    isDone = _gameState.IsTerminated();
                }
                
                Dictionary<string, object> info = new Dictionary<string, object>
                {
                    { "legal_actions", _gameState.GetLegalActions() },
                    { "game_status", _gameState.GameStatus }
                };
                
                return new Tuple<float[], float, bool, Dictionary<string, object>>(
                    _gameState.GetObservation(),
                    humanReward,
                    isDone,
                    info
                );
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error during environment step: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Render the current state of the environment
        /// </summary>
        public void Render()
        {
            _renderer.Render(_gameState);
        }
        
        /// <summary>
        /// Play a full episode where human player makes moves interactively
        /// </summary>
        public void PlayEpisode()
        {
            Reset();
            Render();
            
            while (!_gameState.IsTerminated())
            {
                if (_gameState.CurrentPlayer == Player.Human)
                {
                    int humanAction = _humanPlayer.MakeMove(_gameState);
                    Step(humanAction);
                    Render();
                }
            }
            
            // Display final status
            Console.WriteLine($"Game ended with status: {_gameState.GameStatus}");
        }
    }
}