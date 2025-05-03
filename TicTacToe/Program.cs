using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TicTacToe.Domain;
using TicTacToe.Environment;
using TicTacToe.Players;
using TicTacToe.Rendering;

namespace TicTacToe
{
    /// <summary>
    /// Main entry point for the Tic Tac Toe application
    /// </summary>
    internal static class Program
    {
        private const int RendererSelectionTimeoutSeconds = 3;

        [STAThread]
        static void Main()
        {
            try
            {
                Console.WriteLine("Tic Tac Toe Environment for Actor-Critic Training");
                Console.WriteLine("===============================================");
                Console.WriteLine($"Choose renderer type (you have {RendererSelectionTimeoutSeconds} seconds):");
                Console.WriteLine("1. Console Renderer (Default)");
                Console.WriteLine("2. WinForms Renderer");

                // Set up renderer selection with timeout
                IRenderer renderer = SelectRendererWithTimeout();

                // Create players
                IPlayer humanPlayer = new HumanPlayer();
                IPlayer aiPlayer = new AIPlayer();

                // Create and initialize environment
                var environment = new TicTacToeEnvironment(humanPlayer, aiPlayer, renderer);

                // Run the environment
                ApplicationConfiguration.Initialize();
                RunEnvironment(environment, renderer);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal error: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Selects renderer with a timeout, defaulting to console renderer
        /// </summary>
        /// <returns>The selected renderer implementation</returns>
        private static IRenderer SelectRendererWithTimeout()
        {
            IRenderer renderer = null;
            var rendererSelected = new ManualResetEvent(false);

            // Start renderer selection thread
            var selectionThread = new Thread(() =>
            {
                try
                {
                    string input = Console.ReadLine()?.Trim();

                    renderer = input switch
                    {
                        "2" => new WinFormsRenderer(),
                        _ => new ConsoleRenderer()
                    };
                }
                catch (Exception)
                {
                    // Default to console renderer on error
                    renderer = new ConsoleRenderer();
                }
                finally
                {
                    rendererSelected.Set();
                }
            });

            selectionThread.Start();

            // Wait for selection with timeout
            bool selected = rendererSelected.WaitOne(RendererSelectionTimeoutSeconds * 1000);

            if (!selected)
            {
                Console.WriteLine("\nSelection timeout. Using default Console Renderer.");
                renderer = new ConsoleRenderer();
            }

            return renderer;
        }

        /// <summary>
        /// Runs the environment with the selected renderer
        /// </summary>
        /// <param name="environment">Tic Tac Toe environment</param>
        /// <param name="renderer">Selected renderer</param>
        private static void RunEnvironment(TicTacToeEnvironment environment, IRenderer renderer)
        {
            if (renderer is WinFormsRenderer winFormsRenderer)
            {
                // For WinForms, we need to run on UI thread
                Task.Run(() => environment.PlayEpisode());
                Application.Run(winFormsRenderer);
            }
            else
            {
                // For console, just run the episode
                environment.PlayEpisode();

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}