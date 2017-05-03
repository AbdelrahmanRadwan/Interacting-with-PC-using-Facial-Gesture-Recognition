using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public class Data_Loader
    {
        ////
        //Generic
        ////
        int NumberOfClasses;
        int NumberOfFeatures;
        string[] FileLines; //[Temp variable] To load the content of the Json object to memory.
        string[] Labels = {"Closing Eyes", "Looking Down", "Looking Front", "Looking Left" };
        string[] Tokens; // [Temp variable]To hold the X and Y coordinated of the records.
        string Path; // [Temp variable] Generic variable to hold the cumulative path
        string ShufflingAlgorithm; // Variable to hold the name of the suffling algorithm we wanna use.
        ////
        //Training
        ////
        string PathOfTrainingMetaData;
        int NumberOfTrainingSamples;
        int NumberOfTrainingSamplesPerClass;
        double[,] TrainingFeatures;
        double[, ,] TrainingMetaData;
        string[] TrainingLabels;
        ////
        //Training
        ////
        string PathOfTestingMetaData;
        int NumberOfTestingSamples;
        int NumberOfTestingSamplesPerClass;
        double[,] TestingFeatures;
        double[, ,] TestingMetaData;
        string[] TestingLabels;
        ////////////////////////////////////////////////////
        ////////////////////////////////////////////////////
        #region Class Constructor
        public Data_Loader()
        {
            //////
            //Generic
            /////
            NumberOfClasses = 4;
            NumberOfFeatures = 19;
            ShufflingAlgorithm = "Fisher Yates Shuffle";
            ////
            //Training
            ////
            NumberOfTrainingSamples = 60; //15 picture per class
            NumberOfTrainingSamplesPerClass = 15;
            PathOfTrainingMetaData = @"Dataset\Shuffled\Training Dataset\Metadata";
            TrainingFeatures = new double[NumberOfTrainingSamples, NumberOfFeatures];
            TrainingLabels = new string[NumberOfTrainingSamples];
            TrainingMetaData = new double[NumberOfTrainingSamples, NumberOfFeatures + 1, 2];
            ////
            //Testing
            ////
            NumberOfTestingSamples = 20; //5 picture per class
            NumberOfTestingSamplesPerClass = 5;
            PathOfTestingMetaData = @"Dataset\Shuffled\Testing Dataset\Metadata";
            TestingFeatures = new double[NumberOfTestingSamples, NumberOfFeatures];
            TestingLabels = new string[NumberOfTestingSamples];
            TestingMetaData = new double[NumberOfTestingSamples, NumberOfFeatures + 1, 2];
        }
        public void RunTheDataLoader(Form1 TheMainForm)
        {
            
            PreProcessing(); // load the data from the files to the memory.
            FeatureExtraction(); // Extract the features from the data.
            PostProcessing();// Shuffle the training records to be ready for training AND Normalizing the features values.
        }
        #endregion
        #region Preprocessing
        private void PreProcessing()
        {
            ReadTrainingMetaData();
            ReadTestingMetaData();
        }
        private void ReadTrainingMetaData()
        {
            for (int i = 0; i < NumberOfTrainingSamples; i++)
            {
                // loading the metadata to the memory in the FileLines variable.
                Path = PathOfTrainingMetaData + @"\" + (i + 1).ToString() + ".pts";
                FileLines = System.IO.File.ReadAllLines(Path);
                // looping through the real records in the file (the XY points) to save them in the 3D array of records.
                for (int Line = 3; Line < 23; Line++) // 20 XY points starting from line 3
                {
                    Tokens = FileLines[Line].Split(' ');
                    TrainingMetaData[i, Line - 3, 0] = double.Parse(Tokens[0]); // parsing the X-axis record.
                    TrainingMetaData[i, Line - 3, 1] = double.Parse(Tokens[1]); // parsing the Y-axis record.
                }
                // swap the " tip of nose" with "tip of chin", to make it easier in processing :D
                // the first parameter is the place in which you want to swap the two features.
                // The second parameter is the array name.
                Swap(i, "TrainingMetaData");
            }
        }
        private void ReadTestingMetaData()
        {
            for (int i = 0; i < NumberOfTestingSamples; i++)
            {
                // loading the metadata to the memory in the FileLines variable.
                Path = PathOfTestingMetaData + @"\" + (i + 1).ToString() + ".pts";
                FileLines = System.IO.File.ReadAllLines(Path);
                // looping through the real records in the file (the XY points) to save them in the 3D array of records.
                for (int Line = 3; Line < 23; Line++) // 20 XY points starting from line 3
                {
                    Tokens = FileLines[Line].Split(' ');
                    TestingMetaData[i, Line - 3, 0] = double.Parse(Tokens[0]); // parsing the X-axis record.
                    TestingMetaData[i, Line - 3, 1] = double.Parse(Tokens[1]); // parsing the Y-axis record.
                }
                // swap the " tip of nose" with "tip of chin", to make it easier in processing :D
                // the first parameter is the place in which you want to swap the two features.
                // The second parameter is the array name.
                Swap(i, "TestingMetaData");
            }
        }
        #endregion

        #region Feature Extraction
        private void FeatureExtraction()
        {
            TrainingFeatureExtraction();
            TestingFeatureExtraction();
        }
        private void TrainingFeatureExtraction()
        {
            for (int Record=0;Record <NumberOfTrainingSamples;Record++)
            {
                for (int point = 0; point<NumberOfFeatures;point++)
                {
                    TrainingFeatures[Record, point] = EuclideanDistance(
                            TrainingMetaData[Record,point,0],TrainingMetaData[Record,point,1], // the current point
                            TrainingMetaData[Record, 19, 0], TrainingMetaData[Record, 19, 1]// the last point (tip of nose)
                                                                        );
                }
                TrainingLabels[Record]=Labels[Record/NumberOfTrainingSamplesPerClass];
            }
        }
        private void TestingFeatureExtraction()
        {
            for (int Record = 0; Record < NumberOfTestingSamples; Record++)
            {
                for (int point = 0; point < NumberOfFeatures; point++)
                {
                    TestingFeatures[Record, point] = EuclideanDistance(
                            TestingMetaData[Record, point, 0], TestingMetaData[Record, point, 1], // the current point
                            TestingMetaData[Record, 19, 0], TestingMetaData[Record, 19, 1]// the last point (tip of nose)
                                                                       );
                }
                TestingLabels[Record] = Labels[Record / NumberOfTestingSamplesPerClass];
            }
        }
        #endregion

        #region PostProcessing
        private void PostProcessing()
        {
            /////
            //Shuffle
            ////
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
            ////
            // Normalize
            ////

        }
        #region Shuffling Algorithms

        //Algorithm I
        // Knuth Shuffle Algorithm
        private void KnuthShuffle()
        {
            Random random = new Random();
            for (int i = 0; i < this.NumberOfTrainingSamples; i++)
            {
                int index = random.Next(i, this.NumberOfTrainingSamples); // Don't select from the entire array on subsequent loops
                ShufflingSwaper(i, index);
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
                ShufflingSwaper(i, index);
            }
        }
        #endregion
        #region Normalization Algorithms
        #endregion
        #endregion

        #region Helper Methods
        private void ShufflingSwaper(int index1, int index2)
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
        private void Swap(int index, string ArrayName)
        {
            double TempDouble = 0;
            if (ArrayName == "TrainingMetaData")
            {
                // Swap the X feature
                TempDouble = TrainingMetaData[index, 19, 0];
                TrainingMetaData[index, 19, 0] = TrainingMetaData[index, 14, 0];
                TrainingMetaData[index, 14, 0] = TempDouble;
                //////
                //Swap the Y feature.
                TempDouble = TrainingMetaData[index, 19, 1];
                TrainingMetaData[index, 19, 1] = TrainingMetaData[index, 14, 1];
                TrainingMetaData[index, 14, 1] = TempDouble;
            }
            else if (ArrayName == "TestingMetaData")
            {
                // Swap the X feature
                TempDouble = TestingMetaData[index, 19, 0];
                TestingMetaData[index, 19, 0] = TestingMetaData[index, 14, 0];
                TestingMetaData[index, 14, 0] = TempDouble;
                //////
                //Swap the Y feature.
                TempDouble = TestingMetaData[index, 19, 1];
                TestingMetaData[index, 19, 1] = TestingMetaData[index, 14, 1];
                TestingMetaData[index, 14, 1] = TempDouble;
            }
            else
            {
                MessageBox.Show("Ops! something went wrong.");
            }
        }

        private double EuclideanDistance(double x1, double y1, double x2, double y2)
        {
            double Distance = 0;
            Distance = Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
            return Distance;
        }

        #endregion
    }
}
