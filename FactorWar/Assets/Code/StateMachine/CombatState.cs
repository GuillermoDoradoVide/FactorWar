using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatState : State {

    protected StateManager owner;

    protected virtual void Awake()
    {
        owner = GetComponent<StateManager>();
    }

    protected override void AddListeners()
    {
        base.AddListeners();
    }

    protected override void RemoveListeners()
    {
        base.RemoveListeners();
    }

    // Update is called once per frame
    private void Update () {
		
	}
}
