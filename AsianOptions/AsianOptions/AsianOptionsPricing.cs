using System;

namespace AsianOptions
{
    /// <summary>
    /// Calculates the price of an asian options
    /// based on the given statistic context.
    /// </summary>
    internal class AsianOptionsPricing
    {
        /// <summary>
        /// Calculates the price of an option with the given statistical info.
        /// The algorithm used for calculation is the Monte Carlo method.
        /// </summary>
        /// <param name="rand">Random number generator</param>
        /// <param name="initial">Initial price</param>
        /// <param name="exercise">Excercise price</param>
        /// <param name="up">Up growth</param>
        /// <param name="down">Down growth</param>
        /// <param name="interest">Interest rate</param>
        /// <param name="periods">Number of periods to maturity</param>
        /// <param name="sims">Number of simulations to perform</param>
        /// <returns>The calculated value for an option with the given
        /// statistical context using the Monte Carlo method.</returns>
        internal static double Simulation(
            Random rand, double initial, double exercise,
            double up, double down, double interest, long periods, long sims)
        {
            //TODO Risk-neutral probabilities:

            double sum = 0.0;

            //TODO Run simulations:

            // Return average across all simulations:
            return (sum / Math.Pow(interest, periods)) / sims;
        }
    }
}
