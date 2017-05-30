using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IDamageable<int>, IKillable, ISelectable
{

    public enum EntityType {UNIT, STRUCTURE, DECORATION};
    public EntityType entityType;

    private HasHealth health;

    [SerializeField]
    private AnimatorManager unitAnimator;

    protected Transform c_transform;
    protected GameObject c_gameObject;
    protected MapBox unitCellPosition;

	// Use this for initialization
	private void Start () {
        c_transform = GetComponent<Transform>();
        c_gameObject = GetComponent<GameObject>();
        health = GetComponent<HasHealth>();
        unitAnimator = GetComponent<AnimatorManager>();
        if (health == null)
        {
            Debugger.printErrorLog("'" + name + "(" + entityType + ")' doesn't have a <HasHealth> COMPONENT.");
        }
        if (unitAnimator == null)
        {
            Debugger.printErrorLog("'" + name + "' doesn't have a <AnimatorManager(Script)> COMPONENT.");
        }
    }

    // Update is called once per frame
    private void Update () {
		
	}

    // CELL

    public void setMapCell(MapBox mapBox)
    {
        if(mapBox != null)
        {
            unitCellPosition = mapBox;
        }
        else
        {
            Debugger.printErrorLog("not a cell map element asigned to this <" + name + "> unit");
        }
    }

    // INTERFACES
    public void select()
    {
        // EVENT TRIGGER, UNIT SELECTED
        if(unitCellPosition != null)
        {
            EventManager.Instance.TriggerEvent(new EventEntitySelect(unitCellPosition));
        }
    }

    public void damaged(int damagePoints)
    {
        if (health.takeDamage(damagePoints))
        {
            StartCoroutine(kill());
        }
    }

    public IEnumerator kill()
    {
        Debugger.printLog(("'" + name + "(" + entityType + ")' is beeing killed."));
        unitAnimator.setAnimationBeeingDestroyed();
        while (!unitAnimator.isDestroyed)
        {
            yield return null;
        }
        c_gameObject.SetActive(false);
    }
}
