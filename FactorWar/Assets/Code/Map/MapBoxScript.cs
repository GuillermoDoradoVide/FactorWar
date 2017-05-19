using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBoxScript : MonoBehaviour {

    public MapBox mapbox;
    private MeshRenderer meshRenderer;
    private MeshFilter meshFilter;
	// Use this for initialization
	private void Start () {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mapbox.getMesh();
        meshRenderer.materials = mapbox.getMaterials();
    }
}
