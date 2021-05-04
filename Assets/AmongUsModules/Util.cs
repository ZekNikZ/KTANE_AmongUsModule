using System;

namespace AmongUsModules {
    public class Util {
        public static int constrain(int min, int val, int max) {
            return Math.Max(Math.Min(max, val), min);
        }

        public static long constrain(long min, long val, long max) {
            return Math.Max(Math.Min(max, val), min);
        }

        public static float constrain(float min, float val, float max) {
            return Math.Max(Math.Min(max, val), min);
        }

        public static double constrain(double min, double val, double max) {
            return Math.Max(Math.Min(max, val), min);
        }
    }
}
