using System;
using System.Collections.Generic;

namespace TicTacToe.Domain
{
    /// <summary>
    /// Represents the complete state of a Tic Tac Toe game
    /// </summary>
    public class GameState
    {
        private readonly Board _board;
        private Player _currentPlayer;
        private GameStatus _gameStatus;
        
        private const int BoardSize = 3;

        public GameState()
        {
            _board = new Board();
            Reset();
        }

        /// <summary>
        /// Reset the game to initial state
        /// </summary>
        public void Reset()
        {
            _board.Reset();
            _currentPlayer = Player.Human; // Human starts first
            _gameStatus = GameStatus.InProgress;
        }

        /// <summary>
        /// Get the current board state as observation for ML agents
        /// </summary>
        /// <returns>Flat array representing the board and current player</returns>
        public float[] GetObservation()
        {
            // Combine board state and current player
            float[] boardState = _board.GetState();
            float[] observation = new float[boardState.Length + 1];
            
            Array.Copy(boardState, 0, observation, 0, boardState.Length);
            observation[boardState.Length] = (float)_currentPlayer;
            
            return observation;
        }

        /// <summary>
        /// Get all legal moves as action indices (0-8)
        /// </summary>
        /// <returns>List of valid action indices</returns>
        public List<int> GetLegalActions()
        {
            List<int> legalActions = new List<int>();
            
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    if (_board.IsMoveValid(i, j))
                    {
                        legalActions.Add(i * BoardSize + j);
                    }
                }
            }
            
            return legalActions;
        }

        /// <summary>
        /// Make a move using flat action index (0-8)
        /// </summary>
        /// <param name="actionIndex">Action index from 0 to 8</param>
        /// <returns>True if the move was successful</returns>
        public bool MakeMove(int actionIndex)
        {
            if (_gameStatus != GameStatus.InProgress)
                return false;
                
            int row = actionIndex / BoardSize;
            int col = actionIndex % BoardSize;
            
            if (!_board.MakeMove(row, col, _currentPlayer))
                return false;
                
            // Update game status
            UpdateGameStatus();
            
            // Switch players if game is still in progress
            if (_gameStatus == GameStatus.InProgress)
            {
                _currentPlayer = _currentPlayer == Player.Human ? Player.AI : Player.Human;
            }
            
            return true;
        }

        private void UpdateGameStatus()
        {
            Player winner = _board.CheckWinner();
            
            if (winner == Player.Human)
            {
                _gameStatus = GameStatus.HumanWon;
            }
            else if (winner == Player.AI)
            {
                _gameStatus = GameStatus.AIWon;
            }
            else if (_board.IsFull())
            {
                _gameStatus = GameStatus.Draw;
            }
        }

        /// <summary>
        /// Check if the game is terminated (won or draw)
        /// </summary>
        /// <returns>True if game is over</returns>
        public bool IsTerminated()
        {
            return _gameStatus != GameStatus.InProgress;
        }

        /// <summary>
        /// Get reward for the given player based on current game state
        /// </summary>
        /// <param name="player">Player to calculate reward for</param>
        /// <returns>Reward value</returns>
        public float GetReward(Player player)
        {
            switch (_gameStatus)
            {
                case GameStatus.HumanWon:
                    return player == Player.Human ? 1.0f : -1.0f;
                case GameStatus.AIWon:
                    return player == Player.AI ? 1.0f : -1.0f;
                case GameStatus.Draw:
                    return 0.0f;
                default:
                    return 0.0f; // No reward during gameplay
            }
        }

        public Board Board => _board;
        public Player CurrentPlayer => _currentPlayer;
        public GameStatus GameStatus => _gameStatus;
    }
}