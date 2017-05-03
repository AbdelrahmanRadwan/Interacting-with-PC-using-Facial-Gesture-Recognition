using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public class Data_Loader
    {
        int NumberOfClasses;
        int NumberOfTrainingSamples;
        int NumberOfTrainingSamplesPerClass;
        int NumberOfTestingSamples;
        int NumberOfTestingSamplesPerClass;
        int NumberOfFeatures;
        double[,] TrainingFeatures;
        string[] TrainingLabels;
        string PathOfTrainingMetaData;
        string PathOfTestingMetaData;
        string ShufflingAlgorithm;
        public Data_Loader()
        {
            //////
            //Initializiation
            //////
            NumberOfClasses = 4;
            NumberOfFeatures = 19;
            NumberOfTrainingSamples = 60; //15 picture per class
            NumberOfTestingSamples = 20; //5 picture per class
            NumberOfTrainingSamplesPerClass = 15;
            NumberOfTestingSamplesPerClass = 5;
            TrainingFeatures = new double[NumberOfTrainingSamples, NumberOfFeatures];
            TrainingLabels = new string[NumberOfTrainingSamples];
            PathOfTrainingMetaData = @"Dataset\Shuffled\Training Dataset\Metadata";
            PathOfTestingMetaData = @"Dataset\Shuffled\Testing Dataset\Metadata";
            ShufflingAlgorithm = "sda";

            //////
            //Processing
            /////
            PreProcessing(); // load the dara from the files to the memory.
            FeatureExtraction(); // Extract the features from the data.
            PostProcessing();// Shuffle the training records to be ready for training AND Normalizing the features values.


        }
        private void PreProcessing()
        {

        }
        private void FeatureExtraction()
        {

        }
        private void PostProcessing()
        {
            if (ShufflingAlgorithm == "Knuth Shuffle")
            {
                KnuthShuffle();
            }
            else if (ShufflingAlgorithm == "Fisher Yates Shuffle")
            {
                FisherYates();
            }
            else
            {
                MessageBox.Show("This Algorithms Is Not Supported Yet.");
            }

        }
        #region Feature Extraction
        private void TrainingFeatureExtraction()
        {

        }
        private void TrainingFeatureExtraction()
        {

        }
        #endregion
        #region Helper Methods
        private void SWAP(int index1, int index2)
        {
            ////
            //Swap the labels
            ////
            string TempString = TrainingLabels[index1];
            TrainingLabels[index1] = TrainingLabels[index2];
            TrainingLabels[index2] = TempString;
            ////
            //Swap the features
            ////
            for (int i = 0; i < NumberOfFeatures; i++)
            {
                double TempDouble = TrainingFeatures[index1, i];
                TrainingFeatures[index1, i] = TrainingFeatures[index2, i];
                TrainingFeatures[index2, i] = TempDouble;
            }
        }

        private double EuclideanDistance(double x1, double y1, double x2, double y2)
        {
            double Distance = 0;


            return Distance;
        }

        #endregion
        #region Shuffling Algorithms

        //Algorithm I
        // Knuth Shuffle Algorithm
        private void KnuthShuffle()
        {
            Random random = new Random();
            for (int i = 0; i < this.NumberOfTrainingSamples; i++)
            {
                int index = random.Next(i, this.NumberOfTrainingSamples); // Don't select from the entire array on subsequent loops
                SWAP(i, index);
            }
        }
        //Algorithm II
        //Fisher Yates Shuffle Algorithm
        private void FisherYates()
        {
            Random random = new Random();
            for (int i = this.NumberOfTrainingSamples - 1; i > 0; i--)
            {
                int index = random.Next(i);
                SWAP(i, index);
            }
        }
        #endregion
        #region Normalization Algorithms
        #endregion

    }
}
