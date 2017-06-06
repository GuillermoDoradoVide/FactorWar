using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : Entity, ICanAttack {

    [Header("Unit Base Stats")]
    public int ActionPoints;
    public int movementRange;

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

    public void attack()
    {
        EventManager.Instance.TriggerEvent(new EventEntityAttack(2));
    }

    public override void select()
    {
        // EVENT TRIGGER, UNIT SELECTED
        if (unitCellPosition != null)
        {
            EventManager.Instance.TriggerEvent(new EventEntitySelected(unitCellPosition, movementRange));
        }
    }



}
