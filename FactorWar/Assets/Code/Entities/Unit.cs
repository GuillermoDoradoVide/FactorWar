using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Entity {

    [Header("Unit Animation")]
    public bool isIdle;
    public bool isMoving;
    public bool isAttacking;

    private void Start () {
        entityType = EntityType.UNIT;
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        Debugger.printLog("'" + name + "(" + entityType + ")' is beeing disabled.");
    }

    // MIN BEHAVIOUR LOOP
    private void Update ()
    {
	}

}
