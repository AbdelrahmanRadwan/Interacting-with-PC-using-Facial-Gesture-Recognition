using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    public static class MathematicalOperations
    {
        static private Random random = new Random();
        static public double[] DifferentiationOfSoftmaxFunction(double []x)
        {
            double[] derivative= new double[x.Length];
            for(int i=0;i<x.Length;i++)
                derivative [i]= (1 - x[i]) * x[i];
            return derivative;
        }
        public static double GetRandomNumber(double minimum, double maximum)
        {           
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
        public static double[] softmax(double[] oSums)
        {
            // determine max output sum
            // does all output nodes at once so scale doesn't have to be re-computed each time
            double max = oSums[0];
            for (int i = 0; i < oSums.Length; ++i)
                max = Math.Max(max, oSums[i]);

            // determine scaling factor -- sum of exp(each val - max)
            double scale = 0.0;
            for (int i = 0; i < oSums.Length; ++i)
                scale += Math.Exp(oSums[i] - max);

            double[] result = new double[oSums.Length];
            for (int i = 0; i < oSums.Length; ++i)
                result[i] = Math.Exp(oSums[i] - max) / scale;
            return result;
        }
        public static double CalculateErrorUsingSoftmax(double[] oSums, double []DesiredOutput)
        {
            // determine max output sum
            // does all output nodes at once so scale doesn't have to be re-computed each time
            double max = oSums[0];
            for (int i = 0; i < oSums.Length; ++i)
                max = Math.Max(max,oSums[i]);

            // determine scaling factor -- sum of exp(each val - max)
            double scale = 0.0;
            for (int i = 0; i < oSums.Length; ++i)
                scale += Math.Exp(oSums[i] - max);

            double[] result = new double[oSums.Length];
            for (int i = 0; i < oSums.Length; ++i)
                result[i] = Math.Exp(oSums[i] - max) / scale;
            //Result now scaled so that xi sum to 1.0
            //////////////////////////////////////////////////////////////////////////////
            ////
            //calculate the error (e) = sum((desired - softmax result)^2)/2
            ////
            double sum = 0;
            for (int i = 0; i < oSums.Length; i++)
            {
                DesiredOutput[i] = (DesiredOutput[i] - result[i]) * (DesiredOutput[i] - result[i]);
                sum += DesiredOutput[i];
            }
                return sum/2.0; 
        }
        ////
        // The tanh function
        ////
        public static double HyperbolicFunction(double X)
        {
            double y = Math.Tanh(X);
            return y;
        }
        public static double DifferentiationOfHyperbolicFunction(double x)
        {
            double y;
            y = 4 * Math.Exp(2 * x);
            y = y / ((1 + Math.Exp(2 * x)) * (1 + Math.Exp(2 * x)));
            return y;
        }

            /////
            // Matrix Operations
            /////
        private static double[,] MatrixTranspose(int n, int m, double[,] Matrix)
        {
            double[,] TransposedMatrix = new double[m, n];

            for (int i = 0; i < m; i++)
                for (int j = 0; j < n; j++)
                    TransposedMatrix[i, j] = Matrix[j, i];
            return TransposedMatrix;
        }
        public static double[] MatrixMatrixMultiplycation(int WeightN, int WeightM, int XN, double[] X, double[,] Weights)
        {
            double[,] NewWeights = MatrixTranspose(WeightN, WeightM, Weights);
            double[] result = new double[WeightM];
            double[] NewX = new double[XN + 1];
            for (int i = 0; i < XN; i++)
                NewX[i] = X[i];
            NewX[XN] = 1; // Adding the baise.

            // the mutrix vector multiplycation.
            for (int i = 0; i < WeightM; i++)
            {
                double Sum = 0;
                for (int j = 0; j < WeightN; j++)
                    Sum = Sum + NewWeights[i, j] * NewX[j];
                result[i] = Sum;
            }

            return result;
        }

    }
}
