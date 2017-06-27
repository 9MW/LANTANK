using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Permissions;

/**
 * Class that enqueues tasks to be executed in the main thread.
 * 
 * Credits: https://github.com/PimDeWitte/UnityMainThreadDispatcher/blob/master/UnityMainThreadDispatcher.cs
 * */
public class MainThreadProcessor : MonoBehaviour
{
    //This is the entity that must handle UI changes (if any) from the received message.
    private IUIHandler clientStrategy;
    private static readonly Queue<Action> _executionQueue = new Queue<Action>();
    private static MainThreadProcessor _instance = null;
   // [HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
    public static MainThreadProcessor Instance()
    {
        if (!Exists())
        {
            throw new Exception("MainThreadProcessor could not find the UnityMainThreadDispatcher object. Please ensure you have added the MainThreadExecutor Prefab to your scene.");
        }
        return _instance;
    }

    public static bool Exists()
    {
        return _instance != null;
    }

    private void Start()
    {
        init();
    }

    public   void init()
    {
        if (_instance == null)
        {
            _instance = gameObject.GetComponent<MainThreadProcessor>();
            clientStrategy = GetComponent<IUIHandler>();
            if (clientStrategy == null) {
                Application.Quit();
                throw new Exception("Plsss");
            }
            //inputManager = GetComponent<InputManager>();
            //DontDestroyOnLoad(this.gameObject);
        }
    }
    public void Update()
    {
        lock (_executionQueue)
        {
            while (_executionQueue.Count > 0)
            {

                _executionQueue.Dequeue().Invoke();
            }
        }
    }
    public void Enqueue(IEnumerator action)
    {
        lock (_executionQueue)
        {
            _executionQueue.Enqueue(() => {
                StartCoroutine(action);
            });
        }
    }

    public IEnumerator processMessage(byte[] text)
    {
        if (clientStrategy != null) {
            clientStrategy.processMessage (text);
        }
        yield return null;
    }
    public IEnumerator processMessage(string text)
    {
        if (clientStrategy != null)
        {
            clientStrategy.processMessage(text);
        }
        yield return null;
    }
    public IEnumerator processMessage(byte[] text, System.Net.IPEndPoint address)
    {
            clientStrategy.processMessage(text, address);
        
        yield return null;
    }
}
