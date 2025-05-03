using System;
using System.Drawing;
using System.Windows.Forms;
using TicTacToe.Domain;

namespace TicTacToe.Rendering
{
    /// <summary>
    /// WinForms-based renderer for a visually appealing Tic Tac Toe game
    /// </summary>
    public class WinFormsRenderer : Form, IRenderer
    {
        private readonly Panel[,] _cells;
        private readonly Label _statusLabel;
        private GameState _currentState;
        private const int CellSize = 100;
        private const int BoardMargin = 20;
        
        public WinFormsRenderer()
        {
            // Initialize form
            Text = "Tic Tac Toe";
            Size = new Size(CellSize * 3 + BoardMargin * 2, CellSize * 3 + BoardMargin * 3 + 40);
            BackColor = Color.FromArgb(25, 25, 50);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            
            // Create cells
            _cells = new Panel[3, 3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Panel cell = new Panel
                    {
                        Size = new Size(CellSize, CellSize),
                        Location = new Point(j * CellSize + BoardMargin, i * CellSize + BoardMargin),
                        BackColor = Color.FromArgb(45, 45, 75),
                        BorderStyle = BorderStyle.None,
                        Margin = new Padding(5),
                        Tag = Player.None
                    };
                    
                    _cells[i, j] = cell;
                    Controls.Add(cell);
                }
            }
            
            // Create status label
            _statusLabel = new Label
            {
                Location = new Point(BoardMargin, 3 * CellSize + BoardMargin + 10),
                Size = new Size(3 * CellSize, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.White
            };
            
            Controls.Add(_statusLabel);
        }
        
        /// <summary>
        /// Render the current game state
        /// </summary>
        /// <param name="gameState">Current game state</param>
        public void Render(GameState gameState)
        {
            _currentState = gameState;
            
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateUI()));
            }
            else
            {
                UpdateUI();
            }
        }
        
        private void UpdateUI()
        {
            // Update cells
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Player player = _currentState.Board.GetAt(i, j);
                    _cells[i, j].Paint -= CellPaint;
                    _cells[i, j].Tag = player;
                    _cells[i, j].Paint += CellPaint;
                    _cells[i, j].Invalidate();
                }
            }
            
            // Update status
            string status = _currentState.GameStatus switch
            {
                GameStatus.InProgress => $"Current player: {_currentState.CurrentPlayer}",
                GameStatus.HumanWon => "Game Over: You Won!",
                GameStatus.AIWon => "Game Over: AI Won!",
                GameStatus.Draw => "Game Over: It's a Draw!",
                _ => string.Empty
            };
            
            _statusLabel.Text = status;
            _statusLabel.ForeColor = _currentState.GameStatus switch
            {
                GameStatus.HumanWon => Color.LightGreen,
                GameStatus.AIWon => Color.LightCoral,
                GameStatus.Draw => Color.LightYellow,
                _ => Color.White
            };
        }
        
        private void CellPaint(object sender, PaintEventArgs e)
        {
            Panel cell = (Panel)sender;
            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            
            Player player = (Player)cell.Tag;
            int padding = 20;
            
            switch (player)
            {
                case Player.Human:
                    // Draw X with animation effect
                    using (Pen pen = new Pen(Color.LightBlue, 8))
                    {
                        g.DrawLine(pen, padding, padding, cell.Width - padding, cell.Height - padding);
                        g.DrawLine(pen, cell.Width - padding, padding, padding, cell.Height - padding);
                    }
                    break;
                    
                case Player.AI:
                    // Draw O with smooth circle
                    using (Pen pen = new Pen(Color.LightCoral, 8))
                    {
                        g.DrawEllipse(pen, padding, padding, cell.Width - 2 * padding, cell.Height - 2 * padding);
                    }
                    break;
            }
        }
        
        /// <summary>
        /// Show the form as a modal dialog
        /// </summary>
        public new void Show()
        {
            Application.Run(this);
        }
    }
}