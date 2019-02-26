using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBoxScript : MonoBehaviour {

    public MapBox mapbox;
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
    [SerializeField]
    private Vector3 cell;
	// Use this for initialization
	private void Awake () {
        mapbox = GetComponent<MapBox>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        //meshFilter.mesh = mapbox.getMesh();
        //meshRenderer.materials = mapbox.getMaterials();
        cell = mapbox.cell;
    }


    public void changeColorToRed()
    {
        meshRenderer.materials = mapbox.changeColorRed();
    }

    public void changeColorToGreen()
    {
        meshRenderer.materials = mapbox.changeColorGreen();
    }
}
