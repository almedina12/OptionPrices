using System;


namespace OptionPrices
{
    class Program
    {
        

        static void Main(string[] args)
        {
            //First Question
            double InitialStockPrice = 100;
            double RiskFreeRate = 0.05;
            double Sigma = 0.1;
            double OptionStrikePrice = 100;
            double OptionMaturity = 1;

            BlackScholesFormula MyOption = new BlackScholesFormula(InitialStockPrice, RiskFreeRate, Sigma, OptionStrikePrice, OptionMaturity);

            double MyCall = MyOption.CalculateCallOptionPrice();
            double MyPut = MyOption.CalculatePutOptionPrice();
            //Console.WriteLine(MyCall);
            //Console.WriteLine(MyPut);
            //Console.ReadKey();

            //Second Question
            BlackScholesImpliedVol MyOptionVol = new BlackScholesImpliedVol(InitialStockPrice, RiskFreeRate, OptionStrikePrice, OptionMaturity);
            double OptionCallPrice = MyOptionVol.CalculateImpliedVol(10,0.5, 0.01, 10000, true);
            double OptionPutPrice = MyOptionVol.CalculateImpliedVol(3, 0.5, 0.001, 10000, false);
            Console.WriteLine(OptionCallPrice);
            Console.WriteLine(OptionPutPrice);
            Console.ReadKey();

            //Third Question
            BlackScholesMonteCarlo MyOptionMonteCarlo = new BlackScholesMonteCarlo(InitialStockPrice, RiskFreeRate, Sigma, OptionStrikePrice, OptionMaturity);


            double MyCallMonteCarlo;
            double MyPutMonteCarlo;

            MyCallMonteCarlo = MyOptionMonteCarlo.CalculateCallOptionPrice(10000);
            MyPutMonteCarlo = MyOptionMonteCarlo.CalculatePutOptionPrice(10000);
            //Console.WriteLine(MyCallMonteCarlo);
            //Console.WriteLine(MyPutMonteCarlo);
            //Console.WriteLine(Math.Abs(MyCallMonteCarlo-MyCall));
            //Console.WriteLine(Math.Abs(MyPutMonteCarlo-MyPut));
            //Console.ReadKey();
        }
    }
}
