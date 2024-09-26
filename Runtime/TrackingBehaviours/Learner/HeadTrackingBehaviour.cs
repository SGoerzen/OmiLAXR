using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OmiLAXR.TrackingBehaviours.Learner
{
    public class HeadTrackingBehaviour : TrackingBehaviour
    {
        public struct HeadTrackingBehaviourArgs
        {
            public readonly TimeSpan TimeSpan;
            public readonly int NumberOfGesture;
            public readonly HmdTimedPosition[] HmdPositions;
            public readonly HmdTimedPosition[] HmdPositionsManipulated;
            public readonly float[] DistanceHmdPositionsManipulated;
            public readonly double MinDistanceOfGesture;

            public HeadTrackingBehaviourArgs(
                HmdTimedPosition[] hmdPositions, 
                HmdTimedPosition[] hmdPositionsManipulated, 
                float[] distanceHmdPositionsManipulated,
                double minDistanceOfGesture, 
                TimeSpan timesSpan, 
                int numberOfGesture)
            {
                HmdPositions = hmdPositions;
                HmdPositionsManipulated = hmdPositionsManipulated;
                DistanceHmdPositionsManipulated = distanceHmdPositionsManipulated;
                MinDistanceOfGesture = minDistanceOfGesture;
                TimeSpan = timesSpan;
                NumberOfGesture = numberOfGesture;
            }
        }
        
        [Gesture("Head"), Action("Nod")]
        public TrackingBehaviourEvent<HeadTrackingBehaviourArgs> OnNodded =
            new TrackingBehaviourEvent<HeadTrackingBehaviourArgs>();
        
        [Gesture("Head"), Action("Shake")]
        public TrackingBehaviourEvent<HeadTrackingBehaviourArgs> OnShook =
            new TrackingBehaviourEvent<HeadTrackingBehaviourArgs>();

        public bool trackNoddingEnabled = true;
        public bool trackShakingEnabled = true;
        
        //Arrays to collect samples
        private List<HmdTimedPosition> _hmdPositions = new List<HmdTimedPosition>();
        private List<HmdTimedPosition> _hmdPositionsManipulated = new List<HmdTimedPosition>();
        private List<float> _distanceHmdPositionsManipulated = new List<float>();
        private Vector3 _nullVector;

        //Boooleans to identify nods/shakes
        private bool _enoughSamplesCollected = false;
        private bool _trackingCurrentlyRunning = true;

        private bool _currentlyNodding = false;
        private bool _currentlyShaking = false;

        /***************
         * Changeable variables for nodding/shaking detection
         * For best result, parameters still need to be optimized => Futurw Work
         * minDistanceNodding: Minimal distance between local extrema to identify a nod
         * minDistanceShaking: Minimal distance between local extrema to identify a shake
         * minNormalHeadMovements: Minimal distance of corresponding axis to identify a normal wlak through the room
         * continuousTimeBetweenNods: Maximal amount of time to identify a sequence of nods
         * continuousTimeBetweenShakes: Maximal amount of time to identify a sequence of shakes
         * minTimeForGestures: Minimal amount of time to have enough samples collected and be able to identify nods/shakes
         ***************/
        private const double MinDistanceNodding = 0.01;
        private const double MinDistanceShaking = 0.01;
        private const double MinNormalHeadMovements = 0.05;
        private readonly TimeSpan _continuousTimeBetweenNods = new TimeSpan(0, 0, 0, 1, 0);
        private readonly TimeSpan _continuousTimeBetweenShakes = new TimeSpan(0, 0, 0, 1, 0);
        private readonly TimeSpan _minTimeForGestures = new TimeSpan(0, 0, 0, 1, 0);
        
        protected override void AfterFilteredObjects(Object[] objects)
        {
            
        }

        // Start is called before the first frame update
        private void Start()
        {
            _nullVector = Vector3.zero;
        }

        // Update is called once per frame
        private void Update()
        {
            var addHmdPosition = HmdPosition.SharedInstance.GetHmdPosition();
            if (Equals(addHmdPosition, default(HmdTimedPosition))) return;
            //add hmdPosition to list of hmdPositions
            _hmdPositions.Add(addHmdPosition);

            //add same hmdPosition to hmdPositionsManipulated but with zero y coordinate for correct distance 
            addHmdPosition.Position.y = 0;
            _hmdPositionsManipulated.Add(addHmdPosition);

            //add distance 
            _distanceHmdPositionsManipulated.Add(Vector3.Distance(_nullVector,
                _hmdPositionsManipulated[_hmdPositionsManipulated.Count - 1].Position));

            //check if currently a nod or shake has been recognized
            NodsOrShakes(ref _currentlyNodding, ref _currentlyShaking, ref _hmdPositions);

            switch (_currentlyNodding)
            {
                //nodding recongized
                case true when !_currentlyShaking:
                    Nodding(ref _currentlyNodding, ref _currentlyShaking);
                    break;
                //shaking recognized
                case false when _currentlyShaking:
                    Shaking(ref _currentlyNodding, ref _currentlyShaking);
                    break;
            }
        }

        //check for either nodding or shaking
        private void NodsOrShakes(ref bool currentlyNodding, ref bool currentlyShaking,
            ref List<HmdTimedPosition> hmdPositions)
        {
            //calculate if timespan between first and last entry is enough for recognizing possible noddings
            HeadUtils.EnoughSamplesCollected(ref hmdPositions, _minTimeForGestures, ref _trackingCurrentlyRunning,
                ref _enoughSamplesCollected);

            //nodding or shaking can be possible
            if (!_enoughSamplesCollected) return;
            //check for x, y and z coordinate to see if the person ist just watching around 
            if (HeadUtils.NormalHeadMovement("x", hmdPositions, MinNormalHeadMovements) &&
                HeadUtils.NormalHeadMovement("y", hmdPositions, MinNormalHeadMovements) &&
                HeadUtils.NormalHeadMovement("z", hmdPositions, MinNormalHeadMovements))
            {
                currentlyNodding = false;
                currentlyShaking = false;
            }
            else
            {
                //Nodding
                //check for x and z cooridnate to see if the person ist just watching around (no nodding)
                if (HeadUtils.NormalHeadMovement("x", hmdPositions, MinNormalHeadMovements) ||
                    HeadUtils.NormalHeadMovement("z", hmdPositions, MinNormalHeadMovements))
                {
                    currentlyNodding = false;
                }
                //distances between min and max values not enough for nodding
                else if (!(HeadUtils.EnoughAmplitude(hmdPositions, MinDistanceNodding, "y")))
                {
                    currentlyNodding = false;
                }
                else
                {
                    currentlyNodding = true;
                }

                if (!currentlyNodding)
                {
                    //Shaking
                    //check y coordinate first to exclude head shaking
                    if (HeadUtils.NormalHeadMovement("y", hmdPositions, MinNormalHeadMovements))
                    {
                        currentlyShaking = false;
                    }
                    //distances between min and max values not enough for shaking
                    else if (!HeadUtils.EnoughAmplitude(_distanceHmdPositionsManipulated, MinDistanceShaking))
                    {
                        currentlyShaking = false;
                    }
                    else
                    {
                        currentlyShaking = true;
                    }
                }

                switch (currentlyNodding)
                {
                    //if no shake or nods has been recognized, empty all lists for new recognition
                    case false when !currentlyShaking:
                        HeadUtils.ClearAllItems(ref hmdPositions, ref _hmdPositionsManipulated,
                            ref _distanceHmdPositionsManipulated);
                        break;
                    //if both a shake and a nod has been recognized, empty all lists for new recognition because a shake and nod at the same time is not possible
                    case true when currentlyShaking:
                        currentlyNodding = false;
                        currentlyShaking = false;
                        HeadUtils.ClearAllItems(ref hmdPositions, ref _hmdPositionsManipulated,
                            ref _distanceHmdPositionsManipulated);
                        break;
                }
            }
        }
        
        private static TimeSpan GetLogInfo(ref List<HmdTimedPosition> hmdPositions, ref List<HmdTimedPosition> hmdPositionsManipulated, ref List<float> distanceHmdPositionsManipulated, ref bool currentlyNodding, ref bool currentlyShaking, double minDistanceNodding, int numGestures)
        {
            var timeSpan = hmdPositions[hmdPositions.Count - 1].Timestamp - hmdPositions[0].Timestamp;

            currentlyNodding = false;
            currentlyShaking = false;

            //clear all items for new detection
            HeadUtils.ClearAllItems(ref hmdPositions, ref hmdPositionsManipulated, ref distanceHmdPositionsManipulated);
            return timeSpan;
        }
        
        private void Nodding(ref bool currentlyNodding, ref bool currentlyShaking)
        {
            if (!trackNoddingEnabled)
                return;
            //nodded still going on or finished
            var positionContinuousNodding = HeadUtils.GetPositionContinuousGesture(_hmdPositions, _continuousTimeBetweenNods);
            if (positionContinuousNodding == -1) return;
            //nodding finished
            if (!HeadUtils.GestureFinished(_hmdPositions, positionContinuousNodding, MinDistanceNodding, "y"))
                return;
            var numNods = HeadUtils.GetNumOfGestures(_hmdPositions, MinDistanceNodding, "y");
            if (numNods <= 0) return;
            var timeSpan = GetLogInfo(ref _hmdPositions, ref _hmdPositionsManipulated, ref _distanceHmdPositionsManipulated, ref currentlyNodding, ref currentlyShaking, MinDistanceNodding, numNods);
            OnNodded?.Invoke(this, new HeadTrackingBehaviourArgs(_hmdPositions.ToArray(), _hmdPositionsManipulated.ToArray(), _distanceHmdPositionsManipulated.ToArray(), MinDistanceNodding, timeSpan, numNods));
        }
        
        private void Shaking(ref bool currentlyNodding, ref bool currentlyShaking)
        {
            if (!trackShakingEnabled)
                return;
            //shaking still going on or finished
            var positionContinuousShaking = HeadUtils.GetPositionContinuousGesture(_hmdPositionsManipulated, _continuousTimeBetweenShakes);
            if (positionContinuousShaking == -1) return;
            //shaking finished
            if (!HeadUtils.GestureFinished(_distanceHmdPositionsManipulated, positionContinuousShaking,
                    MinDistanceShaking)) return;
            var numShakes = HeadUtils.GetNumOfGestures(_distanceHmdPositionsManipulated, MinDistanceShaking);
            if (numShakes <= 0)
                return;
            var timeSpan = GetLogInfo(ref _hmdPositions, ref _hmdPositionsManipulated, ref _distanceHmdPositionsManipulated, ref currentlyNodding, ref currentlyShaking, MinDistanceShaking, numShakes);
            OnShook?.Invoke(this, new HeadTrackingBehaviourArgs(_hmdPositions.ToArray(), _hmdPositionsManipulated.ToArray(), _distanceHmdPositionsManipulated.ToArray(), MinDistanceShaking, timeSpan, numShakes));
        }
    }
}