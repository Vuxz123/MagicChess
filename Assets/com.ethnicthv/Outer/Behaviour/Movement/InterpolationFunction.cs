using com.ethnicthv.Outer.Util;
using UnityEngine;
using Debug = com.ethnicthv.Other.Debug;

namespace com.ethnicthv.Outer.Behaviour.Movement
{
    /// <summary>
    /// signature of the interpolation function
    /// start: start position
    /// end: end position
    /// t: current distance walked
    /// d: total distance
    /// delta: current step
    /// </summary>
    public delegate Vector3 InterpolationFunction(Vector3 start, Vector3 end, float t, float d, float delta);

    public static class InterpolationFunctions
    {
        public static InterpolationFunction Linear = (start, end, t, d, delta) =>
        {
            return Vector3.MoveTowards(start, end, EasingFunction.EaseOutQuart(t) * d);
        };

        public static InterpolationFunction CurveUp1 = (start, end, t, d, delta) =>
        {
            var temp = Linear(start, end, t, d, delta);
            var sin = Mathf.Sin(EasingFunction.EaseOutQuart(t) * Mathf.PI);
            var y = temp.y + sin * 1;
            return new Vector3(temp.x, y, temp.z);
        };
    }
}