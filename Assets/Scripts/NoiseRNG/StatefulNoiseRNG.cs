using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatefulNoiseRNG
{
    private int state = 0;
    private int increment = 1;

    public StatefulNoiseRNG(){}
    
    public StatefulNoiseRNG(int increment) {
        this.increment = increment;
    }

    public float GetFloatValueBetween(float min, float max) {
        state += increment;
        return SeededNoiseRNG.GetFloatValueBetween(min, max, state);
    }

    public int GetIntValueBetween(int min, int max) {
        state += increment;
        return SeededNoiseRNG.GetIntValueBetween(min, max, state);
    }

    public bool GetResultGivenProbability(float prob) {
        state += increment;
        return SeededNoiseRNG.GetResultGivenProbability(prob, state);
    }
}
