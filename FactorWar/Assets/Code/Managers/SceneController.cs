using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class SceneController : SingletonComponent<SceneController>
{
    private string currentSceneName;
    private string nextSceneName;
    private AsyncOperation resourceUnloadTask;
    private AsyncOperation sceneLoadTask;
    private enum SceneState { Reset, Preload, Load, Unload, Postload, Ready, Run, Count };
    private SceneState sceneState;
    private delegate void SceneStateUpdate();
    private SceneStateUpdate[] sceneStateUpdates;
    private List<string> scenes;
    private int numScenes;

    public Scene getCurrentScene()
    {
        return SceneManager.GetActiveScene();
    }

	public void resetScene() {
		if (instance != null)
		{
            instance.sceneState = SceneState.Reset;
		}
	}

    public void SwitchScene(string nextSceneName)
    {
        if (instance != null)
        {
            if (instance.currentSceneName != nextSceneName)
            {
                instance.nextSceneName = nextSceneName;
            }
        }
    }

    public static void loadAditiveScene(string addedAditiveScene)
    {
        instance = Instance;
        Debugger.printLog("AdditiveScene");
        if (instance != null)
        {
            if (!SceneManager.GetSceneByName(addedAditiveScene).IsValid())
            {
                Instance.sceneLoadTask = SceneManager.LoadSceneAsync(addedAditiveScene, LoadSceneMode.Additive);
				Debugger.printLog("Carga");
            }
        }
    }

    public static void unloadAditiveScene(string addedAditiveScene)
    {
        instance = Instance;
        Debugger.printLog("AdditiveScene");
        if (instance != null)
        {
            if (SceneManager.GetSceneByName(addedAditiveScene).IsValid())
            {
                SceneManager.UnloadScene(addedAditiveScene);
            }
        }
    }

    private void Awake()
    {
		currentSceneName = SceneManager.GetActiveScene().name;
		nextSceneName = currentSceneName;
		Debugger.printLog("[SceneManager] loading '" + nextSceneName + "' scene.");
        sceneState = SceneState.Run; // first process
        sceneStateUpdates = new SceneStateUpdate[(int)SceneState.Count];
        //Set each scene state delegate
        sceneStateUpdates[(int)SceneState.Reset] = sceneStateReset;
        sceneStateUpdates[(int)SceneState.Preload] = sceneStatePreload;
        sceneStateUpdates[(int)SceneState.Load] = sceneStateLoad;
        sceneStateUpdates[(int)SceneState.Unload] = sceneStateUnload;
        sceneStateUpdates[(int)SceneState.Postload] = sceneStatePostload;
        sceneStateUpdates[(int)SceneState.Ready] = sceneStateReady;
        sceneStateUpdates[(int)SceneState.Run] = sceneStateRun;
    }

    protected void OnDestroy()
    {
        Debugger.printLog("OnDestroy SceneController");
        // clean up all the delegates
        if (sceneStateUpdates != null)
        {
            for (int x = 0; x < (int)SceneState.Count; x++)
            {
                sceneStateUpdates[x] = null;
            }
            sceneStateUpdates = null;
        }
        // clean up the singleton instance
        if (instance != null)
        {
            instance = null;
        }
    }

    private void Update()
    {
        if (sceneStateUpdates[(int)sceneState] != null)
        {
            sceneStateUpdates[(int)sceneState]();
        }
    }

    private void sceneStateReset()
    {
        Debugger.printLog("Reset Scene");
        // run a gc pass
        System.GC.Collect();
        sceneState = SceneState.Preload;
    }

    private void sceneStatePreload()
    {
        //Debug.Log("Preload");
		sceneLoadTask = SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Single);
        sceneState = SceneState.Load;
    }

    private void sceneStateLoad()
    {
        Debugger.printLog("Loading new Scene");
        //done loading?
        if (sceneLoadTask.isDone == true)
        {
            sceneState = SceneState.Unload;
        }
        else
        {
            // update scene loading process
            //sceneLoadTask.allowSceneActivation = true;
        }
    }

    private void sceneStateUnload()
    {
        // cleaning up resources yet?
        if (resourceUnloadTask == null)
        {
            resourceUnloadTask = Resources.UnloadUnusedAssets();
        }
        else
        {
            // done cleaning up ?
            if (resourceUnloadTask.isDone == true)
            {
                resourceUnloadTask = null;
                sceneState = SceneState.Postload;
                Debugger.printLog("Unload resources");
            }
        }
    }

    private void sceneStatePostload()
    {
        //Debug.Log("Postload");
        currentSceneName = nextSceneName;
        sceneState = SceneState.Ready;
    }

    private void sceneStateReady()
    {
        Debugger.printLog("Scene is ready");
        // run a gc pass 
        //if you have assets loaded in the scene
        // that are currently unnused but may be used later dont do this here
        //System.GC.Collect();
        sceneState = SceneState.Run;
    }

    private void sceneStateRun()
    {
        //Debug.Log("Run");
        if (currentSceneName != nextSceneName)
        {
            sceneState = SceneState.Reset;
        }
    }
}
