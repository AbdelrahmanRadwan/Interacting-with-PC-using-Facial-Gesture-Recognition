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
        double[] MeanSquaredError;
        double[,] TrainingFeatures;
        string[] TrainingLabels;
        Dictionary<int, double[,]> Weights; // mapping the weights layer, to its real weights.
        Dictionary<string, int> LabelsEncoding; // to transform the labels from strings to integers. // 0 based
        string[] Labels = { "Closing Eyes", "Looking Down", "Looking Front", "Looking Left" }; // 0 based
        public MultilayerPerceptronModel()
        {
            NumberOfInputNeurons = 19;
            NumberOfFeatures = 19;
            NumberOfOutputNeorons = 4;
            NumberOfTrainingSamples = 60;
            Weights = new Dictionary<int, double[,]>();
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
            // Initialize the Eta 
            ////
            double[] temp = Annealing(0.1, 1E-5, NumberOfEpochs);
            for (int i = 0; i < NumberOfHiddenLayers + 1; i++)
                for (int j = 0; j < NumberOfEpochs; j++)
                    Eta[i, j] = temp[j];
            ////
            //Initialize the Labels Encoding
            ////
            LabelsEncoding["Closing Eyes"] = 0;
            LabelsEncoding["Looking Down"] = 1;
            LabelsEncoding["Looking Front"] = 2;
            LabelsEncoding["Looking Left"] = 3;
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
                for(int i=0;i<NumberOfTrainingSamples;i++)
                {
                    ////
                    // Forward Computation
                    ////
                    
                    for(int j=0;j<NumberOfHiddenLayers+1;j++)
                    {
                        double[] X;
                        if(j==0)
                            X = new double(RowSlicer(i));
                        X = MathematicalOperations.MatrixMatrixMultiplycation(Weights[j].GetLength(0), Weights[j].GetLength(1), X.Length, X, Weights[j]);
                    }

                }
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
