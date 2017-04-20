using UnityEngine;
using System.Collections;

public class camerafollow : MonoBehaviour {
    Vector3 q;
    float x;
    float y;
    public  GameObject tk;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (tk == null)
            return;
       x= tk.transform.position.x;
       y = tk.transform.position.y;
       q = new Vector3(x,y,transform.position.z);
       transform.position = q;
	}
}
