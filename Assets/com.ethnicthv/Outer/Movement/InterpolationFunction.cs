using com.ethnicthv.Outer.Util;
using UnityEngine;
using UnityEngine.ProBuilder;

namespace com.ethnicthv.Outer.Movement
{
    public delegate Vector3 InterpolationFunction(Vector3 start, Vector3 end, float t, float d);

    public class InterpolationFunctions
    {
        public static InterpolationFunction Linear = (start, end, t, d) =>
        {
            return Vector3.MoveTowards(start, end, EasingFunction.EaseOutQuartic(t) * d);
        };

        public static InterpolationFunction CurveUp100 = (start, end, t, d) =>
        {
            var temp = Linear(start, end, t, d);
            temp.y += Mathf.Sin(EasingFunction.EaseInOutQuint(t) * Mathf.PI) * 100;
            return temp;
        };
    }
}