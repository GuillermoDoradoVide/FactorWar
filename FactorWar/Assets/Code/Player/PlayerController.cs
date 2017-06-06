using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// Use this for initialization
	private void Start () {
		
	}
	
	// Update is called once per frame
	private void Update () {
        if (Input.GetMouseButton(0))
        {
            checkSelected();
        }
    }

    private void checkSelected()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.green);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.GetComponent<ISelectable>() != null)
            {
                hit.transform.gameObject.GetComponent<ISelectable>().select();
            }

            /*if (ExecuteEvents.CanHandleEvent<ISelectable>(hit.transform.gameObject))
                ExecuteEvents.Execute(targetGazedObject, null, (IElement element, BaseEventData data) => element.selectElement());*/
            // si se selecciona el objeto se manda el evento > EventManager.triggerEvent(Events.EventList.MV_Active);
        }
    }
}
