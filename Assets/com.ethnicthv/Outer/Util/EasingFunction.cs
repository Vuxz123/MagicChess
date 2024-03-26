using System;

namespace com.ethnicthv.Outer.Util
{
    public class EasingFunction
    {
        public static float EaseOutQuartic(float number)
        {
            if (number >= 1) return 1;
            return 1 - MathF.Pow(1 - number, 4);
        }
        
        public static float EaseInOutQuint(float x) {
            return x < 0.5 ? 16 * x * x * x * x * x : 1 - MathF.Pow(-2 * x + 2, 5) / 2;
        }
    }
}