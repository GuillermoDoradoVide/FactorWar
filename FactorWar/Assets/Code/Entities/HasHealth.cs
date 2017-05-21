using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasHealth : MonoBehaviour {

    public int maxHP;
    public int currentHP;
    public bool isDead;

    private void Start()
    {
        isDead = false;
        if (maxHP == 0  || currentHP == 0)
        {
            Debugger.printErrorLog("'" + name + "' health stats aren't set.");
        }
    }

    public bool takeDamage(int damageTaken)
    {
        currentHP -= damageTaken;
        if (currentHP <= 0)
        {
            isDead = true;
        }
        return isDead;
    }

}
