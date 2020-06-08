using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class csThwackDetails : MonoBehaviour
{
    [FormerlySerializedAs("DropCount")] public int dropCount = 10;

    public FruitResourceType dropResouceType = FruitResourceType.ForestApple;
}
