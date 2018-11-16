using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptionPrices
{
    public class BlackScholesImpliedVol
    {
        protected double InitialStockPrice { get; set; }
        protected double RiskFreeRate { get; set; }
        protected double OptionStrikePrice { get; set; }
        protected double OptionMaturity { get; set; }


        public BlackScholesImpliedVol(double initialStockPrice,
        double riskFreeRate,
        double optionStrikePrice,
        double optionMaturity)
        {
            InitialStockPrice = initialStockPrice;
            RiskFreeRate = riskFreeRate;
            OptionStrikePrice = optionStrikePrice;
            OptionMaturity = optionMaturity;
        }

        public double CalculateImpliedVol(double optionPrice, double initialGuess, double tolerance, int maxIterations, Boolean isCall)
        {

            NewtonSolver MyNewtonSolver = new NewtonSolver(tolerance, maxIterations);
            Func<double, double> SolveforSigma;

            if (isCall) {
                SolveforSigma = Sigma => InitialStockPrice * Normal.CDF(0, 1, Math.Log(InitialStockPrice / OptionStrikePrice) + 
                    (RiskFreeRate + Math.Pow(Sigma, 2) / 2) * OptionMaturity) /(Sigma * Math.Sqrt(OptionMaturity)) - 
                    OptionStrikePrice * Math.Exp(-RiskFreeRate * OptionMaturity) * Normal.CDF(0, 1, Math.Log(InitialStockPrice / OptionStrikePrice) + 
                    (RiskFreeRate + Math.Pow(Sigma, 2) / 2) * OptionMaturity - Sigma * Math.Sqrt(OptionMaturity)) /
                 (Sigma * Math.Sqrt(OptionMaturity)) - optionPrice;
            }
            else
            {
                SolveforSigma = Sigma => InitialStockPrice * Normal.CDF(0, 1, Math.Log(InitialStockPrice / OptionStrikePrice) +
                    (RiskFreeRate + Math.Pow(Sigma, 2) / 2) * OptionMaturity) / (Sigma * Math.Sqrt(OptionMaturity)) -
                    OptionStrikePrice * Math.Exp(-RiskFreeRate * OptionMaturity) * Normal.CDF(0, 1, Math.Log(InitialStockPrice / OptionStrikePrice) +
                    (RiskFreeRate + Math.Pow(Sigma, 2) / 2) * OptionMaturity - Sigma * Math.Sqrt(OptionMaturity)) /
                 (Sigma * Math.Sqrt(OptionMaturity)) + OptionStrikePrice * Math.Exp(-RiskFreeRate * OptionMaturity) - InitialStockPrice - optionPrice;
            }
            double Volatility = MyNewtonSolver.Solve(SolveforSigma, null, initialGuess);

            return Volatility;


        }

    }
}
