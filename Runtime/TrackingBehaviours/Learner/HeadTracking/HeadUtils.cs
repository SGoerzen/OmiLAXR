using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours.Learner
{
    public static class HeadUtils
    {
        /***************
         * Overloading functions to turn head gesture into xApi
         * **************/
        public static int GetNumOfGestures(List<float> listOfCoordinates, double minAmplitude)
        {
            var doubleList = listOfCoordinates.ConvertAll(x => (double)x);
            return GetNumOfGestures(doubleList, minAmplitude);
        }

        public static int GetNumOfGestures(List<HmdTimedPosition> listOfCoordinates, double minAmplitude, string axis)
        {
            var doubleList = ConvertHmdPositionListIntoDoubleList(listOfCoordinates, axis);
            return GetNumOfGestures(doubleList, minAmplitude);
        }

        private static int GetNumOfGestures(List<double> listOfCoordinates, double minAmplitude)
        {
            //smooth line for correct calculation of number of head shakes
            var smoothedLine = SmoothLine(listOfCoordinates, minAmplitude);

            //get max number of maxima and minima values
            var maxPeaks = FindMaxPeaks(smoothedLine);
            var minPeaks = FindMinPeaks(smoothedLine);
            //return the max number of maxima/minima values
            return Mathf.Max(minPeaks.Count, maxPeaks.Count);
        }

        //identifying a completed gesture
        public static bool GestureFinished(List<HmdTimedPosition> listOfCoordinates, int positionContinuousElement, double minDistance, string axis)
        {
            var floatList = ConvertHmdPositionListIntoFloatList(listOfCoordinates, axis);

            return GestureFinished(floatList, positionContinuousElement, minDistance);
        }
        public static bool GestureFinished(List<float> listOfCoordinates, int positionContinuousElement, double minDistance)
        {
            //get the maximum and minimum of sublist for continuous gesture check
            var maxSubDistance = listOfCoordinates.GetRange(positionContinuousElement, listOfCoordinates.Count - positionContinuousElement).Max();
            var minSubDistance = listOfCoordinates.GetRange(positionContinuousElement, listOfCoordinates.Count - positionContinuousElement).Min();

            return Math.Abs(maxSubDistance - minSubDistance) < minDistance;
        }

        /***************
         * Overloading functions to check if the amplitude between maximum and minimum value is enough for possible gesture recognition
         * **************/
        public static bool EnoughAmplitude(List<HmdTimedPosition> listOfCoordinates, double minAmplitude, string axis)
        {
            var floatList = ConvertHmdPositionListIntoFloatList(listOfCoordinates, axis);

            return EnoughAmplitude(floatList, minAmplitude);
        }

        public static bool EnoughAmplitude(List<float> list, double minAmplitude)
        {
            if (list.Count <= 0) return false;
            var maxDistance = list.Max();
            var minDistance = list.Min();
            //calculate amplitude between minimum and maximum
            var amplitude = Mathf.Abs(maxDistance - minDistance);

            return amplitude >= minAmplitude;
        }

        //smooth line to calculate the correct number of head shakes/nods
        private static List<double> SmoothLine(IReadOnlyList<double> distances, double minDistanceGesture)
        {
            var distancesSmoothed = new List<double>();

            //add only values with a mini difference (minDistanceGesture) to the smooth line
            var startValue = distances[0];
            distancesSmoothed.Add(distances[0]);

            foreach (var t in distances)
            {
                var res = Math.Abs(startValue - t);
                if (!(res >= minDistanceGesture)) continue;
                distancesSmoothed.Add(t);
                startValue = t;
            }

            return distancesSmoothed;
        }
          public static int GetPositionContinuousGesture(List<HmdTimedPosition> list, TimeSpan continuousTimeBetweenGestures)
        {
            //reverse iteration via list.Reverse() gives error why foreach is replaced by normal for loop
            for (var i = list.Count - 1; i >= 0; i--)
            {
                if ((list[list.Count - 1].Timestamp - list[i].Timestamp) > continuousTimeBetweenGestures)
                {
                    return i;
                }
            }
            return -1;
        }

        /***************
         * Overloading function to empty all delivered lists 
         ***************/
        public static void ClearAllItems(ref List<HmdTimedPosition> hmdPositions)
        {
            var helpPointsOfFocus = new List<Vector3>();

            ClearAllItems(ref hmdPositions, ref helpPointsOfFocus);
        }

        private static void ClearAllItems(ref List<HmdTimedPosition> hmdPositions, ref List<Vector3> pointsOfFocus)
        {
            var helpHmdPositionsManipulated = new List<HmdTimedPosition>();
            var helpDistanceHmdPositionsManipulated = new List<float>();

            ClearAllItems(ref hmdPositions, ref helpHmdPositionsManipulated, ref helpDistanceHmdPositionsManipulated, ref pointsOfFocus);
        }

        public static void ClearAllItems(ref List<HmdTimedPosition> hmdPositions, ref List<HmdTimedPosition> hmdPositionsManipulated, ref List<float> distanceHmdPositionsManipulated)
        {
            var helpPointsOfFocus = new List<Vector3>();

            ClearAllItems(ref hmdPositions, ref hmdPositionsManipulated, ref distanceHmdPositionsManipulated, ref helpPointsOfFocus);
        }

        public static void ClearAllItems(ref List<HmdTimedPosition> hmdPositions, ref List<HmdTimedPosition> hmdPositionsManipulated, ref List<float> distanceHmdPositionsManipulated, ref List<Vector3> pointsOfFocus)
        {
            hmdPositions.Clear();
            hmdPositionsManipulated.Clear();
            distanceHmdPositionsManipulated.Clear();
            pointsOfFocus.Clear();
        }
        
        /// <summary>
        /// identify if the movement was a normal headmovement or could be a gesture
        /// </summary>
        public static bool NormalHeadMovement(string axis, List<HmdTimedPosition> hmdPositions, double minNormalHeadMovements)
        {
            // calculate if the difference between the maximum and minimum value of a list of hmdPositions is greater than a specific threshold (minNormalHeadMovements)
            return
                (HmdPosition.Instance.GetMaxMinPosition("max", axis, hmdPositions).hmdPosition.Position.x
                - HmdPosition.Instance.GetMaxMinPosition("min", axis, hmdPositions).hmdPosition.Position.x) > minNormalHeadMovements;
        }


        /***************
         * Overloading functions to check if enough time has passed (enough samples have been collected) for gesture (shaking/nodding) recognition
         * **************/
        public static void EnoughSamplesCollected(ref List<HmdTimedPosition> hmdPositions, TimeSpan minTimeForGestures, ref bool startApplication, ref Boolean timeSpanGestureReached)
        {
            var helpHmdPositionsManipulated = new List<HmdTimedPosition>();
            var helpDistanceHmdPositionsManipulated = new List<float>();

            EnoughSamplesCollected(ref hmdPositions, ref helpHmdPositionsManipulated, ref helpDistanceHmdPositionsManipulated, minTimeForGestures, ref startApplication, ref timeSpanGestureReached);
        }

        private static void EnoughSamplesCollected(ref List<HmdTimedPosition> hmdPositions, ref List<HmdTimedPosition> hmdPositionsManipulated, ref List<float> distanceHmdPositionsManipulated, TimeSpan minTimeForGestures, ref Boolean startApplication, ref Boolean timeSpanGestureReached)
        {
            //calculate if timespan between first and last entry is enough for recognizing possible shakes
            if ((hmdPositions[hmdPositions.Count - 1].Timestamp - hmdPositions[0].Timestamp) > minTimeForGestures)
            {
                //first time after appication hast started, therefore clear all Lists for a correct recognition
                if (startApplication)
                {
                    ClearAllItems(ref hmdPositions, ref hmdPositionsManipulated, ref distanceHmdPositionsManipulated);
                    startApplication = false;
                }
                timeSpanGestureReached = true;
            }
            else
            {
                timeSpanGestureReached = false;
            }
        }
        
        //find all maxima in a list
        private static List<double> FindMaxPeaks(IReadOnlyList<double> list)
        {
            var peakList = new List<double>();
            for (var i = 1; i < (list.Count - 1); i++)
            {
                if ((list[i - 1] < list[i]) && (list[i] > list[i + 1]))
                {
                    peakList.Add(list[i]);
                }
            }

            return peakList;
        }

        //find all minima in a list
        private static List<double> FindMinPeaks(IReadOnlyList<double> list)
        {
            var peakList = new List<double>();
            for (var i = 1; i < (list.Count - 1); i++)
            {
                if ((list[i - 1] > list[i]) && (list[i] < list[i + 1]))
                {
                    peakList.Add(list[i]);
                }
            }

            return peakList;
        }
        
        /// <summary>
        /// convert a list of hmdPositions into list of floats
        /// </summary>
        /// <param name="listHmdPositions"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        private static List<float> ConvertHmdPositionListIntoFloatList(List<HmdTimedPosition> listHmdPositions, string axis)
        {
            var floatList = new List<float>();
            foreach (var pos in listHmdPositions)
            {
                switch (axis)
                {
                    case "x":
                        floatList.Add(pos.Position.x);
                        break;
                    case "y":
                        floatList.Add(pos.Position.y);
                        break;
                    case "z":
                        floatList.Add(pos.Position.z);
                        break;
                }
            }
            return floatList;
        }
        
        /// <summary>
        /// convert a list of hmdPositions into list of doubles
        /// </summary>
        /// <param name="listHmdPositions"></param>
        /// <param name="axis"></param>
        /// <returns></returns>
        private static List<double> ConvertHmdPositionListIntoDoubleList(List<HmdTimedPosition> listHmdPositions, string axis)
        {
            var floatList = new List<double>();
            foreach (var pos in listHmdPositions)
            {
                switch (axis)
                {
                    case "x": floatList.Add(pos.Position.x);
                        break;
                    case "y": floatList.Add(pos.Position.y);
                        break;
                    case "z": floatList.Add(pos.Position.z);
                        break;
                }
            }
            return floatList;
        }
        
        /***************
         * Function to save a line for furhter research quests to optimize parameters of nodding/shaking
         ***************/
        public static void SaveLine(List<float> list, string listName)
        {
            var myFilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            using (var outputFile = new StreamWriter(Path.Combine(myFilePath, "Line_" + listName + ".txt"), true))
            {
                foreach (double elem in list)
                {
                    outputFile.WriteLine(elem.ToString());
                }
            }
        }
    }
    
}