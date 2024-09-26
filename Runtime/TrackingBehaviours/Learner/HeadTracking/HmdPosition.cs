using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OmiLAXR.TrackingBehaviours.Learner
{
    public class HmdPosition
    {
        private readonly Camera _mMainCamera;

        //create singleton to make sure that only one instance of this class is given
        private static HmdPosition _instance = null;

        private HmdPosition()
        {
            _mMainCamera = Camera.main;
            try
            {
                
            }
            catch (Exception e)
            {
                _mMainCamera = null;
            }
        }
        public static HmdPosition SharedInstance => _instance ??= new HmdPosition();

        //collect position of the hmd via a main camera. Does not work when no main camera is given in a project/application
        public HmdTimedPosition GetHmdPosition() => _mMainCamera != null ? new HmdTimedPosition(DateTime.Now, _mMainCamera.transform.position) : default;

        //get the maximum/minima hmdPosition in a list of hmdPositions depending on the axis 
        public (HmdTimedPosition hmdPosition, int index) GetMaxMinPosition(string maxmin, string axis, List<HmdTimedPosition> list)
        {
            if (list.Count > 0)
            {
                //create new list which only exist the given axis values 
                var values = new List<float>();
                switch (axis)
                {
                    case "x":
                    {
                        values.AddRange(list.Select(pos => pos.Position.x));
                        break;
                    }
                    case "y":
                    {
                        values.AddRange(list.Select(pos => pos.Position.y));
                        break;
                    }
                    case "z":
                    {
                        values.AddRange(list.Select(pos => pos.Position.z));
                        break;
                    }
                }

                switch (maxmin)
                {
                    //look for the maximum value in list of axis values
                    case "max":
                    {
                        var maxValue = values.Max();
                        var maxIndex = values.IndexOf(maxValue);

                        return (list[maxIndex], maxIndex);
                    }
                    //look for the minimum value in list of axis values
                    case "min":
                    {
                        var minValue = values.Min();
                        var minIndex = values.IndexOf(minValue);

                        return (list[minIndex], minIndex);
                    }
                }
            }
            //if list has no values, just return a default value
            var defaultHmdPosition = new HmdTimedPosition();
            return (defaultHmdPosition, -1);
        }

    }
}