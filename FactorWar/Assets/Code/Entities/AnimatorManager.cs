using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    private Animator animator;
    public bool isBeeingDestroyed;
    public bool isDestroyed;

    private static int isBeeingDestroyedID;

    // Use this for initialization
    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debugger.printErrorLog("'" + name + "' doesn't have a <Animator> COMPONENT.");
        }
        isBeeingDestroyedID = Animator.StringToHash("isBeeingDestroyed");
        isBeeingDestroyed = false;
        isDestroyed = false;
    }

    public void setAnimationBeeingDestroyed()
    {
        isBeeingDestroyed = true;
        animator.SetBool(isBeeingDestroyedID, true);
    }

    private void setIsDestroyed()
    {
        isDestroyed = true;
    }

}