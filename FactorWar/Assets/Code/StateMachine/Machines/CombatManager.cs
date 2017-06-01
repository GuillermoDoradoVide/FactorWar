using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : StateManager {

    public void initCombat()
    {
        ChangeState<InitCombatState>();
    }

    public void EndCOmbat()
    {
        ChangeState<EndCombatState>();
    }
}
