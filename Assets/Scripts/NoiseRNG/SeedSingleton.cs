using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class SeedSingleton
{
    public string seedString {get; private set;}
    public uint seedNumber {get; private set;}

    public static SeedSingleton instance {get; private set;}

    private SeedSingleton(string ss, uint s) {
        this.seedString = ss;
        this.seedNumber = s;
    }

    public static void InitSeed(string seedString){
        if (instance == null)
            instance = new SeedSingleton(seedString, (uint)seedString.GetHashCode());//altra funzione hash per avere uint
        else throw new System.Exception("Seed cannot be changed");
    }
}
