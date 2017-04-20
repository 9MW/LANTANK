using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class BtnControlState : MonoBehaviour
{
    string loadname;
    Button bt;
   
    // Use this for initialization
    void Start () {
        
        bt = GetComponent<Button>();
        bt.onClick.AddListener(onClick);
        loadname = transform.name;
	}
    void onClick() {
        if (loadname=="quit")
        Application.Quit();
        SceneManager.LoadScene(loadname);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
public class MyIntEvent : UnityEvent<int>
{
    
}
