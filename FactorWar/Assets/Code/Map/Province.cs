using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName="NewProvince", menuName ="MAPS/Province", order = 1 )]
public class Province : ScriptableObject {

    [SerializeField]
    private string provincename;
    [Header("Province resources")]
    public int electricityPower;
    public int creditsGain;
    public int techPoints;

    public enum FactionOwner {TRINUM, ETERNUM, NEUTRAL};
    [Header("Faction")]
    public FactionOwner factionOwner;
    private Map map;

    public void initProvince()
    {
    }

    public void updateProvinceData()
    {
    }

    public void setMap(Map newMap)
    {
        map = newMap;
    }
}
