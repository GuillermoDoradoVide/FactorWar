using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class EventManager : SingletonComponent<EventManager>
{
    [SerializeField]
    public Dictionary<Events.EventList, UnityEvent> eventDictionary;
    public delegate void setAchievementUnlocked(AchievementKeysList.AchievementList key);
    static setAchievementUnlocked achievementUnlocked;
	public delegate void TeleportPlayer(Transform position);
	static TeleportPlayer teleportPlayer;

    public bool LimitQueueProcesing = false;
    public float QueueProcessTime = 0.0f;
    private Queue m_eventQueue = new Queue();
    ///<summary>
    /// Delagate for game event data sent to listeners.
    /// </summary>
    /// <typeparam name="T">Game Event Sub Class</typeparam>
    public delegate void EventGenericDelegate<T>(T e) where T : GameEvent;
    private delegate void EventGenericDelegate(GameEvent gameEvent);
	
	private Dictionary<System.Type, EventGenericDelegate> delegates = new Dictionary<System.Type, EventGenericDelegate>();
    private Dictionary<System.Delegate, EventGenericDelegate> delegateLookup = new Dictionary<System.Delegate, EventGenericDelegate>();
    private Dictionary<System.Delegate, System.Delegate> onceLookups = new Dictionary<System.Delegate, System.Delegate>();

    private void Awake()
    {
       eventDictionary = new Dictionary<Events.EventList, UnityEvent>();
    }

    private void Start() {}

    private void Update()
    {
        float timer = 0.0f;
        while (m_eventQueue.Count > 0)
        {
            if (LimitQueueProcesing)
            {
                if (timer > QueueProcessTime)
                    return;
            }

            GameEvent evt = m_eventQueue.Dequeue() as GameEvent;
            TriggerEvent(evt);

            if (LimitQueueProcesing)
                timer += Time.deltaTime;
        }
    }

    private EventGenericDelegate addDelegate<T> (EventGenericDelegate<T> del) where T : GameEvent {

        // Early-out if we've already registered this delegate
        if (delegateLookup.ContainsKey(del))
            return null;

        // Create a new non-generic delegate which calls our generic one.
        // This is the delegate we actually invoke.
        EventGenericDelegate internalDelegate = (e) => del((T)e);
        delegateLookup[del] = internalDelegate;

        EventGenericDelegate thisEvent = null;
        if (delegates.TryGetValue(typeof(T), out thisEvent)) {
            delegates[typeof(T)] = thisEvent += internalDelegate; 
        } else {
            delegates[typeof(T)] = internalDelegate;
        }
        Debugger.printLog("Start listening to event: " + internalDelegate.ToString());
        return internalDelegate;
    }

    ///<summary>
    /// Adds method to be invoked when event raised.
    /// </summary>
    /// <typeparam name="T">The event associated with the event delegate.</typeparam>
    /// <param name="del">The method to be stored and invoked if the event is raised.</param>
    public void AddListener<T>(EventGenericDelegate<T> del) where T : GameEvent
    {
        addDelegate<T>(del);
    }

    public void AddListenerOnce<T>(EventGenericDelegate<T> del) where T : GameEvent
    {
        EventGenericDelegate result = addDelegate<T>(del);
        if (result != null)
        {
            // remember this is only called once
            onceLookups[result] = del;
        }
    }
    /// <summary>
    /// Removes method to be invoked when event raised.
    /// </summary>
    /// <typeparam name="T">The event associated with the event delegate.</typeparam>
    /// <param name="del">The method to be removed.</param>
    public void RemoveListener<T>(EventGenericDelegate<T> del) where T : GameEvent
    {
        EventGenericDelegate internalDelegate;
        if (delegateLookup.TryGetValue(del, out internalDelegate))
        {
            EventGenericDelegate tempDel;
            if (delegates.TryGetValue(typeof(T), out tempDel))
            {
                tempDel -= internalDelegate;
                if (tempDel == null)
                {
                    delegates.Remove(typeof(T));
                }
                else
                {
                    delegates[typeof(T)] = tempDel;
                }
            }
            Debugger.printLog("Remove event: " + del.ToString());
            delegateLookup.Remove(del);
        }
    }

    public void RemoveAll()
    {
        delegates.Clear();
        delegateLookup.Clear();
        onceLookups.Clear();
    }

    public bool HasListener<T>(EventGenericDelegate<T> del) where T : GameEvent
    {
        return delegateLookup.ContainsKey(del);
    }

    ///<summary>
    /// Raises an event. ALl methods associated with event are invoked.
    /// </summary>
    /// <param name="e">THe event to raise. This is passed to associated delegates.</param>
    public void TriggerEvent(GameEvent e)
    {
        EventGenericDelegate del;
        if (delegates.TryGetValue(e.GetType(), out del))
        {
            del.Invoke(e);
            // remove listeners which should only be called once
            foreach (EventGenericDelegate k in delegates[e.GetType()].GetInvocationList())
            {
                if (onceLookups.ContainsKey(k))
                {
                    delegates[e.GetType()] -= k;
                    if (delegates[e.GetType()] == null)
                    {
                        delegates.Remove(e.GetType());
                    }
                    delegateLookup.Remove(onceLookups[k]);
                    onceLookups.Remove(k);
                }
            }
        }
        else
        {
            Debugger.printErrorLog("Event: " + e.GetType() + " has no listeners");
        }
    }

    public bool QueueEvent(GameEvent evt)
    {
        if (!delegates.ContainsKey(evt.GetType()))
        {
            Debug.LogWarning("EventManager: QueueEvent failed due to no listeners for event: " + evt.GetType());
            return false;
        }

        m_eventQueue.Enqueue(evt);
        return true;
    }

    private void OnDestroy()
    {
        Debugger.printLog("OnDestroy EventManager");
        RemoveAll();
        m_eventQueue.Clear();
        //instance = null;
    }

    // OLD EventManager
    //Generic event EventManager
    public static void startListening (Events.EventList eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
		Debugger.printLog("Start listening to event: " + eventName);
    }

    public static void stopListening (Events.EventList eventName, UnityAction listener)
    {
        if (instance == null) return;
        UnityEvent thisEvent = null;
        if(Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
		Debugger.printLog("Stop listening to event: " + eventName);
    }

    public static void triggerEvent(Events.EventList eventName)
    {
        UnityEvent thisEvent = null;
        if(Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
			Debugger.printLog("Trigger this event: " + eventName);
        }
    }

    //achievements
    public static void addAchievementListener(setAchievementUnlocked method)
    {
        achievementUnlocked += method;
    }

    public static void removeAchievementListener(setAchievementUnlocked method)
    {
        achievementUnlocked -= method;
    }

    public static void unlockAchievementEvent(AchievementKeysList.AchievementList achievement)
    {
        if (achievementUnlocked != null) achievementUnlocked(achievement);
    }

	//teleport
	public static void addTeleportListener(TeleportPlayer method)
	{
		teleportPlayer += method;
	}

	public static void removeTeleportListener(TeleportPlayer method)
	{
		teleportPlayer -= method;
	}

	public static void teleportPlayerToPosition(Transform position)
	{
		if (teleportPlayer != null) teleportPlayer(position);
	}

}
