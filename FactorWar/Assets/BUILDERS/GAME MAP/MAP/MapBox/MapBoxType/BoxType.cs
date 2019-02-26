using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BoxType", menuName = "MAP/MapBox/MapBoxType", order = 0)]
public class BoxType : ScriptableObject
{
    [Header("- Features")]
    public List<BoxTypeFeature> boxFeatures;
}