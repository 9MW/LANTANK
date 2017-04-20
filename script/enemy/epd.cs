using UnityEngine;
using System.Collections;

public class epd : MonoBehaviour {

    
    
    public int paodanforce = 200;
	// Use this for initialization
	
    void fire( GameObject pre) {
        
            Vector3 x = transform.TransformDirection(0, 1, 0);
            GameObject prefab = (GameObject)Instantiate(pre, transform.position, transform.rotation);
            prefab.GetComponent<Rigidbody2D>().AddForce(x * paodanforce);
            //DestroyObject(pre,10);
        
    
    
    }
}
