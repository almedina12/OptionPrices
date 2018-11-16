using MathNet.Numerics.LinearAlgebra;
using System;


namespace OptionPrices
{
    class NewtonSolver
    {
        private const double delta = 0.01; // for approximating partial derivatives
        private double tol;
        private int maxIt;

        /// <summary>
        /// Creates new instance of NewtonSolver
        /// </summary>
        /// <param name="tolerance">Error tolerance below which solution is accepted</param>
        /// <param name="maximumIterations">Maximum number of iterations that can be carried out</param>
        public NewtonSolver(double tolerance, int maximumIterations)
        {
            this.tol = tolerance;
            this.maxIt = maximumIterations;
        }

        /// <summary>
        /// Returns the approximation to the solution. An exception if thrown if solution doesn't satisfy the accuracy requirements within the specified number of iterations.
        /// </summary>
        /// <param name="f">A scalar valued function for which the root is to be approxiamted</param>
        /// <param name="fPrime">Optional: Exact expression for the derivative. Finite difference approximation is used if set to null</param>
        /// <param name="x0">Starting point</param>
        /// <returns>An approximate solution</returns>
        public double Solve(
            Func<double, double> f,
            Func<double, double> fPrime,
            double x0)
        {
            return Solve(f, fPrime, x0, tol, maxIt);
        }

        /// <summary>
        /// Returns the approximation to the solution. An exception if thrown if solution doesn't satisfy the accuracy requirements within the specified number of iterations.
        /// </summary>
        /// <param name="f">A scalar valued function for which the root is to be approxiamted</param>
        /// <param name="fPrime">Optional: Exact expression for the derivative. Finite difference approximation is used if set to null</param>
        /// <param name="x0">Starting point</param>
        /// <param name="maxError">Error tolerance below which solution is accepted</param>
        /// <param name="maxIter">Maximum number of iterations that can be carried out</param>
        /// <returns>An approximate solution</returns>
        public static double Solve(
            Func<double, double> f,
            Func<double, double> fPrime,
            double x0,
            double maxError,
            int maxIter)
        {
            if (fPrime == null)
            {
                double fPrim;
                double delta = 0.1;
                int n = 0;
                double xn = f(x0);
                while (Math.Abs(f(xn)) > maxError)
                {
                    fPrim = (f(xn + delta) - f(xn - delta)) / (2 * delta);
                    xn = xn - f(xn) / fPrim;
                    n++;
                    if (n > maxIter)
                    {
                        Console.WriteLine("Exceded maximum number of iterations.");
                        return x0;
                    }
                }

                return xn;
            }
            else
            {
                int n = 0;
                double xn = f(x0);
                while (Math.Abs(f(xn)) > maxError)
                {
                    xn = xn - f(xn) / fPrime(xn);
                    n++;
                    if (n > maxIter)
                    {
                        Console.WriteLine("Exceded maximum number of iterations.");
                        return x0;
                    }
                }

                return xn;
            }
        }

        public Matrix<double> ApproximateJacobian(Func<Vector<double>, Vector<double>> F, Vector<double> x)
        {
            Vector<double> evaluated;
            Vector<double> vec = Vector<double>.Build.Dense(x.Count);
            Matrix<double> J_F = Matrix<double>.Build.Dense(x.Count, x.Count);
            for (int i = 0; i < x.Count; i++)
            {
                vec = Vector<double>.Build.Dense(x.Count);
                vec[i] = delta;
                evaluated = (F(x + vec) - F(x - vec)) / (2 * delta);
                J_F.SetColumn(i, evaluated);
            }

            return J_F;

        }

        /// <summary>
        /// Returns the approximation to the solution. An exception if thrown if solution doesn't satisfy the accuracy requirements within the specified number of iterations.
        /// </summary>
        /// <param name="F">A vector valued function for which the root is to be approxiamted</param>
        /// <param name="J_F">Optional: Exact expression for the Jacobian. Finite difference approximation is used if set to null</param>
        /// <param name="x_0">Starting point</param>
        /// <returns>Vector with the approximate solution</returns>
        public Vector<double> Solve(
            Func<Vector<double>, Vector<double>> F,
            Func<Vector<double>, Matrix<double>> J_F,
            Vector<double> x_0)
        {
            if (J_F == null)
            {
                Vector<double> xn = x_0;
                Vector<double> aux = CreateVector.Dense<double>(xn.Count);
                Matrix<double> jacobian;
                int n = 0;
                while (Math.Abs(F(xn).L2Norm()) > tol)
                {
                    jacobian = ApproximateJacobian(F, xn);
                    Console.WriteLine(jacobian);
                    xn.CopyTo(aux);
                    xn = jacobian.Solve(-F(xn));
                    xn = xn + aux;
                    n++;
                    if (n > maxIt)
                    {
                        Console.WriteLine("Exceded maximum number of iterations.");
                        return xn;
                    }
                }
                return xn;
            }
            else
            {
                Vector<double> xn = x_0;
                Vector<double> aux = CreateVector.Dense<double>(xn.Count);
                int n = 0;
                while (Math.Abs(F(xn).L2Norm()) > tol)
                {

                    xn.CopyTo(aux);
                    xn = J_F(xn).Solve(-F(xn));
                    xn = xn + aux;
                    n++;
                    if (n > maxIt)
                    {
                        Console.WriteLine("Exceded maximum number of iterations.");
                        return xn;
                    }
                }
                return xn;
            }
        }

    }
}
