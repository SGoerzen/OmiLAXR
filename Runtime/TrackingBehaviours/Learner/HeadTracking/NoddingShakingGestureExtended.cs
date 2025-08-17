/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours.Learner
{
    // public class NoddingShakingGestureExtended : MonoBehaviour
    // {
    //     // value type for collectinga line in 3d-space consisting out of a location vector and a direction vector
    //     public struct Line3D
    //     {
    //         public Vector3 LocationVector { get; private set; }
    //         public Vector3 DirectionVector { get; private set; }
    //
    //         public Line3D(Vector3 locationVector, Vector3 directionVector)
    //         {
    //             LocationVector = locationVector;
    //             DirectionVector = directionVector;
    //         }
    //     }
    //     
    //     private HeadUtils _headUtils;
    //
    //     //Arrays to collect samples
    //     private List<HmdTimedPosition> _hmdPositions = new List<HmdTimedPosition>();
    //     private List<HmdTimedPosition> _hmdPositionsManipulated = new List<HmdTimedPosition>();
    //     private List<float> _distanceHmdPositionsManipulated = new List<float>();
    //     private Vector3 _nullVector;
    //
    //     //Boooleans to identify nods/shakes
    //     private bool _enoughSamplesCollected;
    //     private bool _trackingCurrentlyRunning = true;
    //
    //     private bool _currentlyNodding;
    //     private bool _currentlyShaking;
    //
    //     /***************
    //     * Changeable variables for nodding/shaking detection
    //     * For best result, parameters still need to be optimized => Futurw Work
    //     * minDistanceNodding: Minimal distance between local extrema to identify a nod
    //     * minDistanceShaking: Minimal distance between local extrema to identify a shake
    //     * minNormalHeadMovements: Minimal distance of corresponding axis to identify a normal wlak through the room
    //     * continuousTimeBetweenNods: Maximal amount of time to identify a sequence of nods
    //     * continuousTimeBetweenShakes: Maximal amount of time to identify a sequence of shakes
    //     * minTimeForGestures: Minimal amount of time to have enough samples collected and be able to identify nods/shakes
    //     ***************/
    //     private const double MinDistanceNodding = 0.01;
    //     private const double MinDistanceShaking = 0.01;
    //     private const double MinNormalHeadMovements = 0.05;
    //     private readonly TimeSpan _continuousTimeBetweenNods = new TimeSpan(0, 0, 0, 1, 0);
    //     private readonly TimeSpan _continuousTimeBetweenShakes = new TimeSpan(0, 0, 0, 1, 0);
    //     private readonly TimeSpan _minTimeForGestures = new TimeSpan(0, 0, 0, 1, 0);
    //
    //     private List<Vector3> _pointsOfFocus = new List<Vector3>();
    //
    //     public NoddingShakingGestureExtended()
    //     {
    //         _currentlyShaking = false;
    //     }
    //
    //     /***************
    //      * Changeable variables for nodding/shaking detection with eye tracking
    //      ***************/
    //     private const float MaxDistancePointOfFocus = 5.0f;
    //
    //     // Start is called before the first frame update
    //     private void Start()
    //     {
    //         _nullVector = Vector3.zero;
    //         _headUtils = GetComponent<HeadUtils>();
    //         _eyeTrackingSystem = MainTrackingSystem.Instance.GetSubSystem<EyeTrackingSystem>();
    //         
    //         if (_eyeTrackingSystem && _eyeTrackingSystem.enabled)
    //             _adapter = _eyeTrackingSystem.Adapter;
    //     }
    //
    //     // Update is called once per frame
    //     private void Update()
    //     {
    //         if (_adapter == null)
    //             return;
    //         
    //         var error = _adapter.GetEyeData(ref _eyeData);
    //         var addHmdPosition = HmdPosition.SharedInstance.GetHmdPosition();
    //         if (object.Equals(addHmdPosition, default(HmdTimedPosition))) return;
    //         _hmdPositions.Add(addHmdPosition);
    //         //hmdPositionsOriginal.Add(addHmdPosition);
    //
    //         //add same hmdPosition to hmdPositionsManipulated but with zero y coordinate for correct distance 
    //         addHmdPosition.Position.y = 0;
    //         _hmdPositionsManipulated.Add(addHmdPosition);
    //
    //         //add distance 
    //         _distanceHmdPositionsManipulated.Add(Vector3.Distance(_nullVector, _hmdPositionsManipulated[_hmdPositionsManipulated.Count - 1].Position));
    //
    //         //add point of focus to pointsOfFocus
    //         var leftLine = new Line3D(_eyeData.VerboseData.Left.GazeOrigin_mm, _eyeData.VerboseData.Left.GazeDirectionNormalized);
    //         var rightLine = new Line3D(_eyeData.VerboseData.Right.GazeOrigin_mm, _eyeData.VerboseData.Right.GazeDirectionNormalized);
    //         _pointsOfFocus.Add(MathCalculations.IntersectionOfTwoLines(leftLine, rightLine));
    //
    //         //check if currently a nod or shake has been recognized
    //         NodsOrShakes(ref _currentlyNodding, ref _currentlyShaking, ref _hmdPositions);
    //
    //         switch (_currentlyNodding)
    //         {
    //             //nodding recongized
    //             case true when !_currentlyShaking:
    //                 Nodding(ref _currentlyNodding, ref _currentlyShaking);
    //                 break;
    //             //shaking recognized
    //             case false when _currentlyShaking:
    //                 Shaking(ref _currentlyNodding, ref _currentlyShaking);
    //                 break;
    //         }
    //     }
    //
    //     //check for either nodding or shaking
    //     private void NodsOrShakes(ref bool currentlyNodding, ref bool currentlyShaking, ref List<HmdTimedPosition> hmdPositions)
    //     {
    //         //calculate if timespan between first and last entry is enough for recognizing possible noddings
    //         HeadUtils.EnoughSamplesCollected(ref hmdPositions, _minTimeForGestures, ref _trackingCurrentlyRunning, ref _enoughSamplesCollected);
    //
    //         //nodding or shaking can be possible
    //         if (!_enoughSamplesCollected) return;
    //         var averagePoint = MathCalculations.FindAveragePoint(_pointsOfFocus);
    //         var currentPointOfFocus = _pointsOfFocus[_pointsOfFocus.Count - 1];
    //
    //         //check if the focus point is constantly "the same"
    //         if (!(object.Equals(currentPointOfFocus, default(Vector3))) && Vector3.Distance(currentPointOfFocus, averagePoint) > MaxDistancePointOfFocus)
    //         {
    //             currentlyNodding = false;
    //             currentlyShaking = false;
    //         }
    //         //check for x, y and z coordinate to see if the person ist just watching around 
    //         else if (HeadUtils.NormalHeadMovement("x", hmdPositions, MinNormalHeadMovements) &&
    //                  HeadUtils.NormalHeadMovement("y", hmdPositions, MinNormalHeadMovements) &&
    //                  HeadUtils.NormalHeadMovement("z", hmdPositions, MinNormalHeadMovements))
    //         {
    //             currentlyNodding = false;
    //             currentlyShaking = false;
    //         }
    //         else
    //         {
    //             //Nodding
    //             //check for x and z cooridnate to see if the person ist just watching around (no nodding)
    //             if (HeadUtils.NormalHeadMovement("x", hmdPositions, MinNormalHeadMovements) ||
    //                 HeadUtils.NormalHeadMovement("z", hmdPositions, MinNormalHeadMovements))
    //             {
    //                 currentlyNodding = false;
    //             }
    //             //distances between min and max values not enough for nodding
    //             else if (!(HeadUtils.EnoughAmplitude(hmdPositions, MinDistanceNodding, "y")))
    //             {
    //                 currentlyNodding = false;
    //             }
    //             else
    //             {
    //                 currentlyNodding = true;
    //             }
    //
    //             if (!currentlyNodding)
    //             {
    //                 //Shaking
    //                 //check y coordinate first to exclude head shaking
    //                 if (HeadUtils.NormalHeadMovement("y", hmdPositions, MinNormalHeadMovements))
    //                 {
    //                     currentlyShaking = false;
    //                 }
    //                 //distances between min and max values not enough for shaking
    //                 else if (!(HeadUtils.EnoughAmplitude(_distanceHmdPositionsManipulated, MinDistanceShaking)))
    //                 {
    //                     currentlyShaking = false;
    //                 }
    //                 else
    //                 {
    //                     currentlyShaking = true;
    //                 }
    //             }
    //
    //             //if no shake or nods has been recognized, empty all lists for new recognition
    //             if (!currentlyNodding && !currentlyShaking)
    //             {
    //                 HeadUtils.ClearAllItems(ref hmdPositions, ref _hmdPositionsManipulated, ref _distanceHmdPositionsManipulated, ref _pointsOfFocus);
    //             }
    //             //if both a shake and a nod has been recognized, empty all lists for new recognition because a shake and nod at the same time is not possible
    //             else if (currentlyNodding && currentlyShaking)
    //             {
    //                 currentlyNodding = false;
    //                 currentlyShaking = false;
    //                 HeadUtils.ClearAllItems(ref hmdPositions, ref _hmdPositionsManipulated, ref _distanceHmdPositionsManipulated, ref _pointsOfFocus);
    //             }
    //         }
    //     }
    //
    //     private void Nodding(ref bool currentlyNodding, ref bool currentlyShaking)
    //     {
    //         if (!_headUtils.trackNodding)
    //             return;
    //         //nodded still going on or finished
    //         var positionContinuousNodding = HeadUtils.GetPositionContinuousGesture(_hmdPositions, _continuousTimeBetweenNods);
    //         if (positionContinuousNodding == -1) return;
    //         //nodding finished
    //         if (!HeadUtils.GestureFinished(_hmdPositions, positionContinuousNodding, MinDistanceNodding, "y"))
    //             return;
    //         var numNods = _headUtils.GetNumOfGestures(_hmdPositions, MinDistanceNodding, "y");
    //         if(numNods > 0)
    //         {
    //             _headUtils.LogHeadGesture(ref _hmdPositions, ref _hmdPositionsManipulated, ref _distanceHmdPositionsManipulated, ref _pointsOfFocus, ref currentlyNodding, ref currentlyShaking, MinDistanceNodding, xAPI_Definitions.gestures.verbs.nodded, xAPI_Definitions.gestures.activities.head, "NumHeadNodding", numNods);
    //         }
    //     }
    //
    //     private void Shaking(ref bool currentlyNodding, ref bool currentlyShaking)
    //     {
    //
    //         if (!_headUtils.trackShaking)
    //             return;
    //         //shaking still going on or finished
    //         var positionContinuousShaking = HeadUtils.GetPositionContinuousGesture(_hmdPositionsManipulated, _continuousTimeBetweenShakes);
    //         if (positionContinuousShaking == -1) return;
    //         //shaking finished
    //         if (!HeadUtils.GestureFinished(_distanceHmdPositionsManipulated, positionContinuousShaking,
    //                 MinDistanceShaking)) return;
    //         var numShakes = HeadUtils.GetNumOfGestures(_distanceHmdPositionsManipulated, MinDistanceShaking);
    //         if(numShakes > 0)
    //         {
    //             _headUtils.LogHeadGesture(ref _hmdPositions, ref _hmdPositionsManipulated, ref _distanceHmdPositionsManipulated, ref _pointsOfFocus, ref currentlyNodding, ref currentlyShaking, MinDistanceNodding, xAPI_Definitions.gestures.verbs.shaked, xAPI_Definitions.gestures.activities.head, "NumHeadShaking", numShakes);
    //         }
    //     }
    // }
    
    // find average point in a list of points
        // public static Vector3 FindAveragePoint(List<Vector3> listOfPoints)
        // => new Vector3(listOfPoints.Average(x => x.x), listOfPoints.Average(x => x.y), listOfPoints.Average(x => x.z));
        //
        // // find the points on both lines via the minimal distance between two lines if no intersection point exist, see http://geomalgorithms.com/a07-_distance.html
        // private static (Vector3, Vector3, float) MinimalDistanceNoIntersection(NoddingShakingGestureExtended.Line3D line1, NoddingShakingGestureExtended.Line3D line2)
        // {
        //     var intersectionPointLeft = new Vector3();
        //     var intersectionPointRight = new Vector3();
        //
        //     var w0 = line1.LocationVector - line2.LocationVector;
        //     var a = Vector3.Dot(line1.DirectionVector, line1.DirectionVector);
        //     var b = Vector3.Dot(line1.DirectionVector, line2.DirectionVector);
        //     var c = Vector3.Dot(line2.DirectionVector, line2.DirectionVector);
        //     var d = Vector3.Dot(line1.DirectionVector, line1.DirectionVector);
        //     var e = Vector3.Dot(line1.DirectionVector, line1.DirectionVector);
        //
        //     if ((a * c - b * b) == 0)
        //     {
        //         return (intersectionPointLeft, intersectionPointRight, -1);
        //     }
        //     var sc = (b * e - c * d) / (a * c - b * b);
        //     var tc = (a * e - b * d) / (a * c - b * b);
        //
        //     var distance = ((line1.LocationVector - line2.LocationVector) + (((b * e - c * d) * line1.DirectionVector) - (a * e - b * d) * line2.DirectionVector) / (a * c - b * b)).magnitude;
        //
        //     var s = line1.LocationVector + 3 * line1.DirectionVector;
        //     var t = line2.LocationVector + 1 * line2.DirectionVector;
        //
        //     intersectionPointLeft = line1.LocationVector + sc * line1.DirectionVector;
        //     intersectionPointRight = line2.LocationVector + tc * line2.DirectionVector;
        //
        //     return (intersectionPointLeft, intersectionPointRight, distance);
        // }
        //
        //
        // // minimal distance between two lines in 3d, see https://stackoverflow.com/questions/45897542/how-to-find-the-intersection-of-two-lines-in-a-3d-space-using-jmonkeyengine3-or
        // // line1 = line1.locationVector + t_1 * line1.directionVector =  r_1 + t_1 * d_1
        // // line2 = line2.locationVector + t_1 * line2.directionVector = r_2 + t_2 * d_2
        // private static float MinimalDistanceBetweenTwoLines(NoddingShakingGestureExtended.Line3D line1, NoddingShakingGestureExtended.Line3D line2)
        // {
        //     // minimalDistance = |(r_2 - r_1) * (d_2 x d_1)| / |(d_2 x d_1)|
        //     var minimalDistance = Math.Abs(Vector3.Dot((line2.LocationVector - line1.LocationVector), (Vector3.Cross(line2.DirectionVector, line1.DirectionVector)))) / Vector3.Cross(line2.DirectionVector, line1.DirectionVector).magnitude;
        //     return minimalDistance;
        // }
        //
        // // two lines are not parallel if they are linearly independent:
        // // line1.directionVector = k * line2.directionVector not solvable for any k in R
        // private static bool ParallelLines(NoddingShakingGestureExtended.Line3D line1, NoddingShakingGestureExtended.Line3D line2)
        // {
        //     var kX = line1.DirectionVector.x / line2.DirectionVector.x;
        //     var kY = line1.DirectionVector.y / line2.DirectionVector.y;
        //     var kZ = line1.DirectionVector.z / line2.DirectionVector.z;
        //
        //     return (kX == kY && kY == kZ && kX == kZ);
        // }
        //
        // // intersection Point for two lines in 3d
        // // line1 = line1.locationVector + t_1 * line1.directionVector =  r_1 + t_1 * d_1
        // // line2 = line2.locationVector + t_1 * line2.directionVector = r_2 + t_2 * d_2
        // public static Vector3 IntersectionOfTwoLines(NoddingShakingGestureExtended.Line3D line1, NoddingShakingGestureExtended.Line3D line2)
        // {
        //     var intersectionPoint = new Vector3();
        //
        //     var minimalDistance = MinimalDistanceBetweenTwoLines(line1, line2);
        //
        //     if (minimalDistance > 0 && !(ParallelLines(line1, line2)))
        //     {
        //         Vector3 minDistancePointLine1;
        //         Vector3 minDistancePointLine2;
        //
        //         (minDistancePointLine1, minDistancePointLine2, minimalDistance) = MinimalDistanceNoIntersection(line1, line2);
        //         var middlePoint = (minDistancePointLine1 + minDistancePointLine2) / 2;
        //         return middlePoint;
        //     }
        //
        //     if (ParallelLines(line1, line2) || !(minimalDistance <= 50.0f)) return intersectionPoint;
        //     var successfull_t_2 = false;
        //     var t_2 = 0.0f;
        //     var t_1 = 0.0f;
        //
        //     // prevent the calculation from division by zero
        //
        //     // first test
        //     // t_1 = (r_2.x + t_2 * d_2.x - r_1.x) / d_1.x
        //     // t_2 = (r_1.y + ((r_2.x * d_1.y) / d_1.x) - ((r_1.x * d_1.y) / d_1.x) - r_2.y) / (d_2.y - (d_2.x * d_1.y) / d_1.x)
        //     // t_2 = (r_1.z + ((r_2.x * d_1.z) / d_1.x) - ((r_1.x * d_1.z) / d_1.x) - r_2.z) / (d_2.z - (d_2.x * d_1.z) / d_1.x)
        //     if (line1.DirectionVector.x != 0)
        //     {
        //         if ((line2.DirectionVector.y - (line2.DirectionVector.x * line1.DirectionVector.y) / line1.DirectionVector.x) != 0)
        //         {
        //             t_2 = (line1.LocationVector.y + ((line2.LocationVector.x * line1.DirectionVector.y) / line1.DirectionVector.x) - ((line1.LocationVector.x * line1.DirectionVector.y) / line1.DirectionVector.x) - line2.LocationVector.y) / (line2.DirectionVector.y - (line2.DirectionVector.x * line1.DirectionVector.y) / line1.DirectionVector.x);
        //             successfull_t_2 = true;
        //         }
        //         else if ((line2.DirectionVector.z - (line2.DirectionVector.x * line1.DirectionVector.z) / line1.DirectionVector.x) != 0)
        //         {
        //             t_2 = (line1.LocationVector.z + ((line2.LocationVector.x * line1.DirectionVector.z) / line1.DirectionVector.x) - ((line1.LocationVector.x * line1.DirectionVector.z) / line1.DirectionVector.x) - line2.LocationVector.z) / (line2.DirectionVector.z - (line2.DirectionVector.x * line1.DirectionVector.z) / line1.DirectionVector.x);
        //             successfull_t_2 = true;
        //         }
        //
        //         if (successfull_t_2)
        //         {
        //             t_1 = (line2.LocationVector.x + t_2 * line2.DirectionVector.x - line1.LocationVector.x) / line1.DirectionVector.x;
        //         }
        //
        //     }
        //
        //     // second test
        //     // t_2 = (r_1.x + ((r_2.y * d_1.x) / d_1.y) - ((r_1.y * d_1.x) / d_1.y) - r_2.x) / (d_2.x - (d_2.y * d_1.x) / d_1.y)
        //     // t_1 = (r_2.y + t_2 * d_2.y - r_1.y) / d_1.y
        //     // t_2 = (r_1.z + ((r_2.y * d_1.z) / d_1.y) - ((r_1.y * d_1.z) / d_1.y) - r_2.z) / (d_2.z - (d_2.y * d_1.z) / d_1.y)
        //     if (!successfull_t_2 && line1.DirectionVector.y != 0)
        //     {
        //         if ((line2.DirectionVector.x - (line2.DirectionVector.y * line1.DirectionVector.x) / line1.DirectionVector.y) != 0)
        //         {
        //             t_2 = (line1.LocationVector.x + ((line1.LocationVector.y * line1.DirectionVector.x) / line1.DirectionVector.y) - ((line1.LocationVector.y * line1.DirectionVector.x) / line1.DirectionVector.y) - line1.LocationVector.x) / (line2.DirectionVector.x - (line2.DirectionVector.y * line1.DirectionVector.x) / line1.DirectionVector.y);
        //             successfull_t_2 = true;
        //         }
        //         else if ((line2.DirectionVector.z - (line2.DirectionVector.y * line1.DirectionVector.z) / line1.DirectionVector.y) != 0)
        //         {
        //             t_2 = (line1.LocationVector.z + ((line1.LocationVector.y * line1.DirectionVector.z) / line1.DirectionVector.y) - ((line1.LocationVector.y * line1.DirectionVector.z) / line1.DirectionVector.y) - line1.LocationVector.z) / (line2.DirectionVector.z - (line2.DirectionVector.y * line1.DirectionVector.z) / line1.DirectionVector.y);
        //             successfull_t_2 = true;
        //         }
        //
        //         if (successfull_t_2)
        //         {
        //             t_1 = (line2.LocationVector.y + t_2 * line2.DirectionVector.y - line1.LocationVector.y) / line1.DirectionVector.y;
        //         }
        //
        //     }
        //
        //     // third test
        //     // t_2 = (r_1.x + ((r_2.z * d_1.x) / d_1.z) - ((r_1.z * d_1.x) / d_1.z) - r_2.x) / (d_2.x - (d_2.z * d_1.x) / d_1.z)
        //     // t_2 = (r_1.y + ((r_2.z * d_1.y) / d_1.z) - ((r_1.z * d_1.y) / d_1.z) - r_2.y) / (d_2.y - (d_2.z * d_1.y) / d_1.z)
        //     // t_1 = (r_2.z + t_2 * d_2.z - r_1.z) / d_1.z
        //     if (!successfull_t_2 && line1.DirectionVector.z != 0)
        //     {
        //
        //         if ((line2.DirectionVector.x - (line2.DirectionVector.z * line1.DirectionVector.x) / line1.DirectionVector.z) != 0)
        //         {
        //             t_2 = (line1.LocationVector.x + ((line2.LocationVector.z * line1.DirectionVector.x) / line1.DirectionVector.z) - ((line1.LocationVector.z * line1.DirectionVector.x) / line1.DirectionVector.z) - line2.LocationVector.x) / (line2.DirectionVector.x - (line2.DirectionVector.z * line1.DirectionVector.x) / line1.DirectionVector.z);
        //             successfull_t_2 = true;
        //         }
        //         else if ((line2.DirectionVector.y - (line2.DirectionVector.z * line1.DirectionVector.y) / line1.DirectionVector.z) != 0)
        //         {
        //             t_2 = (line1.LocationVector.y + ((line2.LocationVector.z * line1.DirectionVector.y) / line1.DirectionVector.z) - ((line1.LocationVector.z * line1.DirectionVector.y) / line1.DirectionVector.z) - line2.LocationVector.y) / (line2.DirectionVector.y - (line2.DirectionVector.z * line1.DirectionVector.y) / line1.DirectionVector.z);
        //             successfull_t_2 = true;
        //         }
        //
        //         if (successfull_t_2)
        //         {
        //             t_1 = (line2.LocationVector.z + t_2 * line2.DirectionVector.z - line1.LocationVector.z) / line1.DirectionVector.z;
        //         }
        //
        //     }
        //
        //     if (successfull_t_2 && (line1.LocationVector + t_1 * line1.DirectionVector) == (line2.LocationVector + t_2 * line2.DirectionVector))
        //     {
        //         intersectionPoint = line1.LocationVector + t_1 * line1.DirectionVector;
        //     }
        //     return intersectionPoint;
        // }

}