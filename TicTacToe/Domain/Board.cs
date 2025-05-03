using System;

namespace TicTacToe.Domain
{
    /// <summary>
    /// Represents the Tic Tac Toe game board
    /// </summary>
    public class Board
    {
        private const int Size = 3;
        private readonly Player[,] _cells;

        public Board()
        {
            _cells = new Player[Size, Size];
            Reset();
        }

        /// <summary>
        /// Reset the board to initial state
        /// </summary>
        public void Reset()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    _cells[i, j] = Player.None;
                }
            }
        }

        /// <summary>
        /// Get the current state of the board as a flat array suitable for ML
        /// </summary>
        /// <returns>A flat array representing the board state</returns>
        public float[] GetState()
        {
            float[] state = new float[Size * Size];
            int index = 0;

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    state[index++] = (float)_cells[i, j];
                }
            }

            return state;
        }

        /// <summary>
        /// Check if a move is valid at the specified position
        /// </summary>
        /// <param name="x">Row index</param>
        /// <param name="y">Column index</param>
        /// <returns>True if move is valid, false otherwise</returns>
        public bool IsMoveValid(int x, int y)
        {
            if (x < 0 || x >= Size || y < 0 || y >= Size)
                return false;

            return _cells[x, y] == Player.None;
        }

        /// <summary>
        /// Make a move on the board
        /// </summary>
        /// <param name="x">Row index</param>
        /// <param name="y">Column index</param>
        /// <param name="player">Player making the move</param>
        /// <returns>True if move was successful, false otherwise</returns>
        public bool MakeMove(int x, int y, Player player)
        {
            if (!IsMoveValid(x, y))
                return false;

            _cells[x, y] = player;
            return true;
        }

        /// <summary>
        /// Check if there's a winner on the board
        /// </summary>
        /// <returns>The winning player or Player.None if no winner</returns>
        public Player CheckWinner()
        {
            // Check rows
            for (int i = 0; i < Size; i++)
            {
                if (_cells[i, 0] != Player.None && 
                    _cells[i, 0] == _cells[i, 1] && 
                    _cells[i, 1] == _cells[i, 2])
                {
                    return _cells[i, 0];
                }
            }

            // Check columns
            for (int j = 0; j < Size; j++)
            {
                if (_cells[0, j] != Player.None && 
                    _cells[0, j] == _cells[1, j] && 
                    _cells[1, j] == _cells[2, j])
                {
                    return _cells[0, j];
                }
            }

            // Check diagonals
            if (_cells[0, 0] != Player.None && 
                _cells[0, 0] == _cells[1, 1] && 
                _cells[1, 1] == _cells[2, 2])
            {
                return _cells[0, 0];
            }

            if (_cells[0, 2] != Player.None && 
                _cells[0, 2] == _cells[1, 1] && 
                _cells[1, 1] == _cells[2, 0])
            {
                return _cells[0, 2];
            }

            return Player.None;
        }

        /// <summary>
        /// Check if the board is full
        /// </summary>
        /// <returns>True if the board is full, false otherwise</returns>
        public bool IsFull()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (_cells[i, j] == Player.None)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Get the value at a specific position
        /// </summary>
        /// <param name="x">Row index</param>
        /// <param name="y">Column index</param>
        /// <returns>The player at the specified position</returns>
        public Player GetAt(int x, int y)
        {
            if (x < 0 || x >= Size || y < 0 || y >= Size)
                throw new ArgumentOutOfRangeException();
                
            return _cells[x, y];
        }
    }
}