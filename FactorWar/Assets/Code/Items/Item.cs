using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

    public int hp;
    public bool damageable;
	// Use this for initialization
	private void Start () {
    }
	
	// Update is called once per frame
	private void Update () {
		
	}

    public void recieveDamage(int value)
    {
        hp -= value;
    }
}
