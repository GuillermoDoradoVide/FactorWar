﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class GameManager : SingletonComponent<GameManager>
{
    private SceneController sceneController;
    private EventManager eventManager;
    private SoundManager soundManager;

	[Header("Pause Setting")]
	public float pauseTriggerTimer = 3.0f;
    [ReadOnly]
	public float currentPauseTimer;
	public bool isPaused = false;

    private void Awake()
    {
        initGameGrpahicsRenderOptions();
        initEventTriggers();
    }

	private void Start() {
        initManagers();
    }

    private void OnDisable()
    {
        EventManager.stopListening(Events.EventList.GAMEMANAGER_Pause, pauseGame);
        EventManager.stopListening(Events.EventList.GAMEMANAGER_Continue, continueGame);
    }

    private void OnApplicationQuit()
    {
        MonoBehaviour[] scripts = FindObjectsOfType<MonoBehaviour>();
        foreach (MonoBehaviour script in scripts)
        {
            script.enabled = false;
        }
    }

    /* INIT METHODS */

    private void initEventTriggers()
    {
        EventManager.startListening(Events.EventList.GAMEMANAGER_Pause, pauseGame);
        EventManager.startListening(Events.EventList.GAMEMANAGER_Continue, continueGame);
    }

    public void initManagers()
    {
        resetGameManager();
        sceneController = SceneController.Instance;
		eventManager = EventManager.Instance;
		soundManager = SoundManager.Instance;
    }

    private void resetGameManager()
    {
        isPaused = false;
    }

    // UPDATE *************************
    private void Update()
    {
       
    }
    //*************************

    public void initGameGrpahicsRenderOptions()
    {
    }

    private void pauseGame()
    {
        EventManager.triggerEvent(Events.EventList.STATE_Pause);
		Debugger.printLog("Pausa del juego. abriendo el menu.");
    }

    private void continueGame()
    {
        EventManager.triggerEvent(Events.EventList.STATE_Continue);
		Debugger.printLog("Reanudar el juego.");
    }
}
