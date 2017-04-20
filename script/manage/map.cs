using UnityEngine;
using System.Collections;

public class map : MonoBehaviour {
    public GameObject prefab;
    string MapSeed;
    void Start()
    {
        GameObject map = new GameObject("map");

        for (int i = -50; i < 50; i++)
        {
            float f =30* Mathf.Sin(i);
               Vector2 positionx = new Vector2(i, f);
               Vector2 positiony = new Vector2(f, i);
               GameObject x=(GameObject) Instantiate(prefab, positionx, Quaternion.identity);
               GameObject y = (GameObject)Instantiate(prefab, positiony, Quaternion.identity);
               y.transform.parent = x.transform.parent = map.transform;
            
        }
    }
	// Use this for initialization
	
	
	// Update is called once per frame
	void Update () {
	
	}
}
