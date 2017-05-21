using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Item, IDamageable<int>,  IKillable,  ISelectable {

	// ENABLE METHODS
	private void Start () {
	}

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    // MIN BEHAVIOUR LOOP
    private void Update () {
		
	}
    /////////////////////

    // INTERFACES
    public void select() { }

    public void damaged(int damagePoints) {
        recieveDamage(damagePoints);
    }

    public void kill() { }


}
