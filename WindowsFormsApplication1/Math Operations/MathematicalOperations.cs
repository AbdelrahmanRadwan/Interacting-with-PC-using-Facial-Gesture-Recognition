using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    public static class MathematicalOperations
    {
        ////
        // The tanh function
        ////
        public static double HyperbolicFunction(double X)
        {
            double y = Math.Tanh(X);
            return y;
        }
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
