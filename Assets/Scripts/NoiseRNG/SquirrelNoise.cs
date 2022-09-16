using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SquirrelNoise
{
    public static uint Noise1D(int position, uint seed) {
        uint noise1 = 0xB5297A4D;
        uint noise2 = 0x68E31DA4;
        uint noise3 = 0x1B56C4E9;

        uint mangled = (uint)(int.MaxValue/2 + position);
        mangled *= noise1;
        mangled += seed;
        mangled ^= (mangled >> 8);
        mangled += noise2;
        mangled ^= (mangled << 8);
        mangled *= noise3;
        mangled ^= (mangled >> 8);

        return mangled;
    }

    public static uint Noise2D(int x, int y, uint seed) {
        int prime = 198491317;
        return Noise1D(x + y*prime, seed);
    }

    public static uint Noise3D(int x, int y, int z, uint seed) {
        int prime1 = 198491317;
        int prime2 = 6542989;
        return Noise1D(x + y*prime1 + z*prime2, seed);
    }

    public static float GetFloatValueBetween(float min, float max, int position, uint seed) {
        return min + (max - min) *
            ((float)Noise1D(position, seed) / (float)uint.MaxValue) ;
    }

    public static int GetIntValueBetween(int min, int max, int position, uint seed) {
        return (int)GetFloatValueBetween(min, max, position, seed);
    }

    public static bool GetResultGivenProbability(float prob, int position, uint seed) {
        return (prob > GetFloatValueBetween(0, 1, position, seed));
    }
}
