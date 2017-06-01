using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewProvince", menuName = "MAPS/Province", order = 1)]
public class Province : ScriptableObject
{
    [SerializeField]
    private string provinceName;
    [Header("Province resources")]
    public int electricityPower;
    public int creditsGain;
    public int techPoints;

    public enum FactionOwner { TRINUM, ETERNUM, NEUTRAL };
    [Header("Faction")]
    public FactionOwner factionOwner;
    private Map map;

    public void initProvince()
    {
        factionOwner = FactionOwner.NEUTRAL;
        electricityPower = 0;
        creditsGain = 0;
        techPoints = 0;
        if(map == null)
        {
            Debugger.printErrorLog("This province (" + provinceName + ") has no map.");

            //Proceso de busqueda del mapa en cuestion
        }
    }

    public void updateProvinceData()
    {
    }

    public void setMap(Map newMap)
    {
        map = newMap;
    }
}
