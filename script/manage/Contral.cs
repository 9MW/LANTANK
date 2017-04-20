using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contral : MonoBehaviour, MYMSG
{
    public static int north = 1;//北
    public static int south = 2;//南
    public static int west = 3;//西
    public static int east = 4;//东
    Vector3 zhuan;
    float btn = 0.5f;
    // Use contObj for initialization
    //public Vector2 speed = new Vector2(50, 50);
    public AudioSource as_Audio;
    public float sudu = 0.5f;
    public int li = 365;
    float X;
    public GameObject contObj;
    // Use this for initialization
    public void Awake()
    {

        isPass = false;
    }
    void FixedUpdate()
    {


        if (isPass == true)
        {
            switch (whichBTN)
            {
                case 1:
                    zhuan = new Vector3(0, 0, 0);
                    contObj.transform.eulerAngles = zhuan;
                    contObj.GetComponent<Rigidbody2D>().AddForce(Vector2.up * li);
                    break;
                case 2:
                    zhuan = new Vector3(0, 0, 180);
                    contObj.transform.eulerAngles = zhuan;
                    contObj.GetComponent<Rigidbody2D>().AddForce(Vector2.up * -li);
                    break;
                case 3:

                    zhuan = new Vector3(0, 0, 90);
                    contObj.transform.eulerAngles = zhuan;
                    contObj.GetComponent<Rigidbody2D>().AddForce(Vector2.right * -li);
                    break;
                case 4:
                    contObj.GetComponent<Rigidbody2D>().AddForce(Vector2.right * li);
                    zhuan = new Vector3(0, 0, 270);
                    contObj.transform.eulerAngles = zhuan;
                    break;
            }
        }

    }
    int whichBTN;
    bool isPass;
    public void BtnDW(int o)
    {
        isPass = true;
        whichBTN = o;
    }

    public void BtnUp(int o)
    {
        isPass = false;
        whichBTN = o;
    }
}
