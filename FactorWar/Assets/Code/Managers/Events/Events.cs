﻿using UnityEngine;
using System.Collections;

public class GameEvent
{

}

///<summary>
/// Raised when a dialog option is selected.
/// </summary>
public class EventEntitySelected : GameEvent
{
    private MapBox unitMapbox;
    public MapBox UnitMapbox { get { return unitMapbox; } }
    private int attackVisionRange;
    public int AttackVisionRange { get { return attackVisionRange; } }
    public EventEntitySelected(MapBox mapbox, int vision)
    {
        unitMapbox = mapbox;
        attackVisionRange = vision;
    }
}

///<summary>
/// Raised when a dialog option is selected.
/// </summary>
public class EventEntityAttack : GameEvent
{
    private int damage;
    public int Damage { get { return damage; } }
    public EventEntityAttack(int damageAmount)
    {
        damage = damageAmount;
    }
}

[CreateAssetMenu(fileName ="EventsList", menuName ="EventsList", order =1)]
public class Events  : ScriptableObject
{
    /// <summary>
    /// [Terminology]
    /// <para>SV = StateEvent</para>
    /// <para>MV = MenuEvent</para>
    /// </summary>
    public enum EventList {
        STATE_Next, STATE_Pause, STATE_Continue,
        MENU_Show, MENU_Hide, MENU_Active, MENU_Select,
        MV_Hide_Active, DIALOG_Selected,
        // sistema de eventos para gestionar los menus. Si se selecciona 1 se desactivan el resto.
        // opciones --> triggerEvent desactivar el resto.
        ACHIEVEMENT_TriggerUnlocked_Achievement,
        PET_NewDialog, NPC_DIALOG_FINISHED,
		PLAYER_FadeIn, PLAYER_FadeOut, PLAYER_Teleport, PLAYER_PickObject, PLAYER_Inspect,
        LEVEL_Activity_Completed, NEW_USER,
        GAMEMANAGER_Pause, GAMEMANAGER_Continue,
        Count };
}

public class AchievementKeysList : ScriptableObject {
    public enum AchievementList
    {
		FIRST_LOGGIN, SESSION_1, S1_LEVEL1, S1_LEVEL2, S1_LEVEL3, SESSION_2, S2_LEVEL1, S2_LEVEL2, S2_LEVEL3,
        Count
    };

}
