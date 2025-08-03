using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public static class WeightedSelection
{
    
    public static GameObject ChooseRandom(List<WeightObject> weightObjects)
    {

        

        //sum of all weights
        float weightSum = 0;

        foreach (WeightObject weightObject in weightObjects)
        {
            weightSum += weightObject.weight;
        }

        float rand = Random.Range(0f, weightSum);

        float tempSum = 0;

        for(int i = 0; i < weightObjects.Count; i++)
        {
            tempSum += weightObjects[i].weight;

            if (tempSum >= rand)
            {
                return weightObjects[i].obj;
            }
        }

        return null;

    }

}
