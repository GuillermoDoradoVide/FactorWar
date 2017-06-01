using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRegion", menuName = "MAPS/Region", order = 0)]
public class Region : ScriptableObject { 

    public List<Province> provinces;

    private void InitRegion()
    {
    }

}
