using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

    public CombatManager CombatMachine;
    public Region mapRegion;

    private void Awake()
    {
        GetCombatMachine();
        InitGame();
    }
   
    private void GetCombatMachine()
    {
        if (CombatMachine == null)
        {
            CombatMachine = CombatManager.Instance as CombatManager;
        }
    }

    private void InitGame()
    {

    }

    public void setNewCombat()
    {
        CombatMachine.initCombat();
    }

    public void PlayerManagment()
    {

    }

    public void EndTurn()
    {

    }
}
