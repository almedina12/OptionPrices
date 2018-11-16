using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptionPrices
{
    public class BlackScholesMonteCarlo
    {

        protected double InitialStockPrice { get; set; }
        protected double RiskFreeRate { get; set; }
        protected double Sigma { get; set; }
        protected double OptionStrikePrice { get; set; }
        protected double OptionMaturity { get; set; }


        public BlackScholesMonteCarlo(double initialStockPrice,
        double riskFreeRate,
        double sigma,
        double optionStrikePrice,
        double optionMaturity)
        {
            InitialStockPrice = initialStockPrice;
            RiskFreeRate = riskFreeRate;
            Sigma = sigma;
            OptionStrikePrice = optionStrikePrice;
            OptionMaturity = optionMaturity;
        }

        public double CalculateCallOptionPrice(int N)
        {
            double[] Z = new double[N];
            Normal.Samples(Z, 0, 1); // will fill Z with samples from standard normal
            double total = 0;
            double aux;

            for (int i = 0; i < N; i++)
            {
                aux = OptionStrikePrice * Math.Exp((RiskFreeRate
                    - 0.5 * Math.Pow(Sigma, 2)) * OptionMaturity + Sigma *
                    Math.Sqrt(OptionMaturity) * Z[i]) - InitialStockPrice;

                total = total + Math.Exp(-RiskFreeRate * OptionMaturity) * Math.Max(0, aux);

            }
            return total / N;
        }

        public double CalculatePutOptionPrice(int N)
        {
            double CallPrice = this.CalculateCallOptionPrice(N);
            return CallPrice + OptionStrikePrice * Math.Exp(-RiskFreeRate * OptionMaturity) - InitialStockPrice;
        }
    }

    

}
