using System;

namespace TicTacToe.Environment
{
    /// <summary>
    /// Generic interface for RL environments similar to OpenAI Gym
    /// </summary>
    /// <typeparam name="TAction">Type of actions</typeparam>
    /// <typeparam name="TObservation">Type of observations</typeparam>
    /// <typeparam name="TInfo">Type of info dictionary</typeparam>
    public interface IEnvironment<TAction, TObservation, TInfo>
    {
        /// <summary>
        /// Reset the environment to initial state
        /// </summary>
        /// <returns>Initial observation and info</returns>
        Tuple<TObservation, TInfo> Reset();
        
        /// <summary>
        /// Take a step in the environment
        /// </summary>
        /// <param name="action">Action to take</param>
        /// <returns>Next observation, reward, done flag, and info</returns>
        Tuple<TObservation, float, bool, TInfo> Step(TAction action);
        
        /// <summary>
        /// Render the environment
        /// </summary>
        void Render();
    }
}