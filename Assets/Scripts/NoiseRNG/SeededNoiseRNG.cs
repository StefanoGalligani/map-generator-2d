using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SeededNoiseRNG
{
    public static float GetFloatValueBetween(float min, float max, int position) {
        return SquirrelNoise.GetFloatValueBetween(min, max, position, SeedSingleton.instance.seedNumber);
    }

    public static int GetIntValueBetween(int min, int max, int position) {
        return SquirrelNoise.GetIntValueBetween(min, max, position, SeedSingleton.instance.seedNumber);
    }

    public static bool GetResultGivenProbability(float prob, int position) {
        return SquirrelNoise.GetResultGivenProbability(prob, position, SeedSingleton.instance.seedNumber);
    }
}
