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
        int CurrentEpoch;
        int NumberOfHiddenLayers;
        int[] NumberOfNeuronsInEachHiddenLayer; // the number of neurons in each hidden layer.
        double MeanSquaredErrorThreshold;
        double TrainingMeanSquaredError;  // MSE for training data, 1 is just an initalization value.
        double Error; // a counter to denote the number of error outputs
        double[,] Eta;
        double SumOfErrorPerEpoch;
        double EtaSingleValue;
        double[] MeanSquaredError;
        double[,] TrainingFeatures;
        double[] TrainingError;
        string[] TrainingLabels;
        Dictionary<int, double[,]> Weights; // mapping the weights layer, to its real weights.
        Dictionary<int, double[]> X; // The feature vector
        Dictionary<int, double[]> NeoronsSignalErrors; // The Signal Error vector
        Dictionary<string, double[]> DesiredOutput; // to transform the labels from strings to integers. // 0 based
        string[] Labels = { "Closing Eyes", "Looking Down", "Looking Front", "Looking Left" }; // 0 based

        public MultilayerPerceptronModel()
        {
            NumberOfInputNeurons = 19;
            NumberOfFeatures = 19;
            NumberOfOutputNeorons = 4;
            NumberOfTrainingSamples = 60;
            Weights = new Dictionary<int, double[,]>();
            TrainingError = new double[NumberOfTrainingSamples];
        }
        ////
        // The entry point of the Algorithms (the run of it), which is called from the main form.
        ////
        public void RunMLP(Form1 TheMainForm)
        {
            RunMLPInitializations(TheMainForm);
            TrainTheModel();
        }
        ////
        // The initialization of the new experiment.
        ////
        private void RunMLPInitializations(Form1 TheMainForm)
        {
            ////
            //General initializations
            ////
            MeanSquaredErrorThreshold = 1E-3;
            TrainingMeanSquaredError = 1;     // MSE for training data, 1 is just an initalization value.
            Error = 0;    // a counter to denote the number of error outputs
            Eta = new double[NumberOfHiddenLayers + 1, NumberOfEpochs]; // used in the backpropagation
            MeanSquaredError = new double[NumberOfEpochs];
            CurrentEpoch = 1;
            ////////////////////////////////////////////////////////////////////////////
            ////
            //Initializaqing the weights dimentions
            ////
            NumberOfNeuronsInEachHiddenLayer = new int[NumberOfHiddenLayers];
            // the weights between the input and the first hidden layer.
            Weights[0] = new double[NumberOfFeatures + 1, NumberOfNeuronsInEachHiddenLayer[0]];
            // the weights between the first and the last hidden layers.
            for (int i = 1; i < NumberOfHiddenLayers; i++)
                Weights[i] = new double[NumberOfNeuronsInEachHiddenLayer[i - 1], NumberOfNeuronsInEachHiddenLayer[i]];
            // the weights between the last hidden layer and the output layer.
            Weights[NumberOfHiddenLayers] = new double[NumberOfNeuronsInEachHiddenLayer[NumberOfHiddenLayers - 1], NumberOfOutputNeorons];
            ////////////////////////////////////////////////////////////////////////////
            ////
            // Initialize the Weigts by valuse between -1 and 1
            ////
            for (int i = 0; i < NumberOfHiddenLayers + 1; i++)
                for (int j = 0; j < Weights[i].GetLength(0); j++)
                    for (int k = 0; k < Weights[i].GetLength(1); k++)
                        Weights[i][j, k] = MathematicalOperations.GetRandomNumber(-1, 1);
            ////////////////////////////////////////////////////////////////////////////
            ////
            // Initialize the Eta 
            ////
            double[] temp = Annealing(0.1, 1E-5, NumberOfEpochs);
            for (int i = 0; i < NumberOfHiddenLayers + 1; i++)
                for (int j = 0; j < NumberOfEpochs; j++)
                    Eta[i, j] = temp[j];
            ////
            //Initialize the Labels Encoding
            ////
            DesiredOutput["Closing Eyes"] = new double[] { 1.0, 0.0, 0.0, 0.0 };
            DesiredOutput["Looking Down"] = new double[] { 0.0, 1.0, 0.0, 0.0 };
            DesiredOutput["Looking Front"] = new double[] { 0.0, 0.0, 1.0, 0.0 };
            DesiredOutput["Looking Left"] = new double[] { 0.0, 0.0, 0.0, 1.0 };
            ////
            //Load the training data set and labels
            ////
            TrainingFeatures = TheMainForm.Loader.TrainingFeatures;
            TrainingLabels = TheMainForm.Loader.TrainingLabels;
        }

        ////
        // The model trainig main loop
        ////
        private void TrainTheModel()
        {
            while (TrainingMeanSquaredError > MeanSquaredErrorThreshold && CurrentEpoch <= NumberOfEpochs)
            {
                ////
                // Forward Computation
                ////     
                for (int i = 0; i < NumberOfTrainingSamples; i++)
                {
                    X[0] = RowSlicer(i);
                    for (int j = 0; j < NumberOfHiddenLayers + 1; j++)
                    {

                        X[j + 1] = MathematicalOperations.MatrixMatrixMultiplycation(
                            Weights[j].GetLength(0), // weight n
                            Weights[j].GetLength(1), // weight m
                            X[j].Length, // vector of features' size
                            X[j], // vector of featurtes
                            Weights[j]);
                        if (j != NumberOfHiddenLayers)
                        {
                            ////
                            //Run the Hyperbolic Function over the array data
                            ////
                            for (int k = 0; k < X[j + 1].Length; k++)
                                X[j + 1][k] = MathematicalOperations.HyperbolicFunction(X[j + 1][k]);
                        }
                        else
                        {
                            ////
                            //Run the softmax Function over the array data
                            ////
                            X[j + 1] = MathematicalOperations.softmax(X[j + 1]);
                        }
                    }
                    //Calculate the error of the feedforward part.
                    TrainingError[i] = MathematicalOperations.CalculateErrorUsingSoftmax(
                                                                                        X[NumberOfHiddenLayers],
                                                                                        DesiredOutput[TrainingLabels[i]]
                                                                                        );

                    ////
                    //  Backward computation
                    //// 
                    ///////////////////////////////////////
                    ////
                    // Calculate the Neorons Signal Errors
                    ////
                    /////For the output layer using softmax
                    for (int j = 0; j < NumberOfOutputNeorons; j++)
                        NeoronsSignalErrors[NumberOfHiddenLayers] = MathematicalOperations.DifferentiationOfSoftmaxFunction(X[NumberOfHiddenLayers]);
                    //// for the other layers
                    for (int j = NumberOfHiddenLayers - 1; j >= 0; j--)
                    {
                        NeoronsSignalErrors[j] = new double[X[j].Length];
                        // pick one node of the hidden layer.
                        for (int k = 0; k < X[j].Length; k++)
                        {
                            double SumOfErrorSignals = 0;
                            for (int l = 0; l < NeoronsSignalErrors[j + 1].Length; l++)
                                SumOfErrorSignals = SumOfErrorSignals + NeoronsSignalErrors[j + 1][l] * Weights[j + 1][k, l];
                            // Update the Signal Error
                            NeoronsSignalErrors[j][k] = SumOfErrorSignals;
                        }
                    }
                    ///////////////////////////////////////////////////////////////////////////////////
                    //////Update the Weights
                    ///////////////////////////////////////////////////////////////////////////////////
                    for (int j = 0; j < NumberOfHiddenLayers + 1; j++)
                        for (int k = 0; k < Weights[i].GetLength(0); k++)
                            for (int l = 0; l < Weights[i].GetLength(1); l++)
                                for (int m = 0; m < NeoronsSignalErrors[j].Length; m++)
                                    Weights[j][k, l] =
                                        Weights[j][k, l] + // Weight
                                        EtaSingleValue * // eta
                                        NeoronsSignalErrors[j][k]* // Signal Error
                                        MathematicalOperations.DifferentiationOfHyperbolicFunction(X[j + 1][k]) * // Diffrentiation of the next feature.
                                        X[j][k]; // Feature

                }
                /////
                // Calculate the mean squared error
                ////
                for(int j=0;j<NumberOfTrainingSamples;j++)
                    SumOfErrorPerEpoch= SumOfErrorPerEpoch + TrainingError[j]*TrainingError[j];
                SumOfErrorPerEpoch/=NumberOfTrainingSamples;
                MeanSquaredError[CurrentEpoch] = SumOfErrorPerEpoch / 2;
            }
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

        private double[] RowSlicer(int Index)
        {
            double[] Slice = new double[NumberOfFeatures];
            for (int i = 0; i < NumberOfFeatures; i++)
                Slice[i] = TrainingFeatures[Index, i];
            return Slice;
        }
        #endregion
    }
}
