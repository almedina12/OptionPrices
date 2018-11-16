using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptionPrices
{

    public class BlackScholesFormula
    {

        protected double Sigma { get; set; }
        protected double InitialStockPrice { get; set; }
        protected double RiskFreeRate { get; set; }
        protected double OptionStrikePrice { get; set; }
        protected double OptionMaturity { get; set; }

        public BlackScholesFormula(double InitialStockPrice,
        double RiskFreeRate,
        double Sigma,
        double OptionStrikePrice,
        double OptionMaturity) 
        {
            this.InitialStockPrice = InitialStockPrice;
            this.RiskFreeRate = RiskFreeRate;
            this.Sigma = Sigma;
            this.OptionStrikePrice = OptionStrikePrice;
            this.OptionMaturity = OptionMaturity;
        }

        public BlackScholesFormula(double InitialStockPrice,
        double RiskFreeRate,
        double OptionStrikePrice,
        double OptionMaturity)
        {
            this.InitialStockPrice = InitialStockPrice;
            this.RiskFreeRate = RiskFreeRate;
            this.OptionStrikePrice = OptionStrikePrice;
            this.OptionMaturity = OptionMaturity;
        }

        public double CalculateCallOptionPrice()
        {
            double d1 = (Math.Log(InitialStockPrice / OptionStrikePrice) + (RiskFreeRate + Math.Pow(Sigma, 2) / 2) * OptionMaturity) /
                  (Sigma * Math.Sqrt(OptionMaturity));
            double d2 = d1 - Sigma * Math.Sqrt(OptionMaturity);

            double N_of_d1 = Normal.CDF(0, 1, d1);
            double N_of_d2 = Normal.CDF(0, 1, d2);

            return InitialStockPrice * N_of_d1 - OptionStrikePrice * Math.Exp(-RiskFreeRate * OptionMaturity) * N_of_d2;
        }

        public double CalculatePutOptionPrice()
        {
            double CallPrice = CalculateCallOptionPrice();
            return CallPrice + OptionStrikePrice * Math.Exp(-RiskFreeRate * OptionMaturity) - InitialStockPrice;

        }

        public double CalculateCallOptionPrice(double sigma)
        {
            double d1 = (Math.Log(InitialStockPrice / OptionStrikePrice) + (RiskFreeRate + Math.Pow(sigma, 2) / 2) * OptionMaturity) /
                  (sigma * Math.Sqrt(OptionMaturity));
            double d2 = d1 - sigma * Math.Sqrt(OptionMaturity);

            double N_of_d1 = Normal.CDF(0, 1, d1);
            double N_of_d2 = Normal.CDF(0, 1, d2);

            return InitialStockPrice * N_of_d1 - OptionStrikePrice * Math.Exp(-RiskFreeRate * OptionMaturity) * N_of_d2;
        }

        public double CalculatePutOptionPrice(double sigma)
        {
            double CallPrice = CalculateCallOptionPrice(sigma);
            return CallPrice + OptionStrikePrice * Math.Exp(-RiskFreeRate * OptionMaturity) - InitialStockPrice;

        }



    }


}
