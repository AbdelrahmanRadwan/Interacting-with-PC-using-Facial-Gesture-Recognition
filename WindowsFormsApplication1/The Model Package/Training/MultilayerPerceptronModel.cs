using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    class MultilayerPerceptronModel
    {
        int NumberOfInputNeurons;
        int NumberOfOutputNeorons;
        int NumberOfFeatures;
        int NumberOfTrainingSamples;
        int NumberOfEpochs;
        int NumberOfHiddenLayers;
        int[] NumberOfNeuronsInEachHiddenLayer; // the number of neurons in each hidden layer.
        double MeanSquaredErrorThreshold;
        double TrainingMeanSquaredError;  // MSE for training data, 1 is just an initalization value.
        double Error; // a counter to denote the number of error outputs
        double[] MeanSquaredError;
        Dictionary<string, double[,]> Weights; // mapping the weights layer, to its real weights.

        public MultilayerPerceptronModel()
        {
            NumberOfInputNeurons = 19;
            NumberOfFeatures = 19;
            NumberOfOutputNeorons = 4;
            NumberOfTrainingSamples = 60;
            MeanSquaredErrorThreshold = 1E-3;
            TrainingMeanSquaredError = 1;     // MSE for training data, 1 is just an initalization value.
            Error = 0;    // a counter to denote the number of error outputs
            MeanSquaredError = new double[NumberOfEpochs];

            Weights = new Dictionary<string, double[,]>();
        }


        #region Helper Methods

        private double[] Annealing(double start, double end, int number)
        {
            double[] Result = new double[number];
            double step = (end - start) / (number - 1);
            for (int i = 0; i < number; i++)
                Result[i] = i * step + start;
            return Result;
        }
        #endregion
    }
}
