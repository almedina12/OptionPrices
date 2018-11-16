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
            BlackScholesFormula MyOption = new BlackScholesFormula(InitialStockPrice, RiskFreeRate, OptionStrikePrice, OptionMaturity);
            Func<double, double> SolveforSigma;

            if (isCall)
                SolveforSigma = Sigma => MyOption.CalculateCallOptionPrice(Sigma) - optionPrice;
            else
                SolveforSigma = Sigma => MyOption.CalculatePutOptionPrice(Sigma) - optionPrice;
            
            double Volatility = MyNewtonSolver.Solve(SolveforSigma, null, initialGuess);

            return Volatility;


        }

    }
}
