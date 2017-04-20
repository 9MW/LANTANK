using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class control : BaseContol, MYMSG
{
    public int whichBTN;
    public bool isPass;
    public bool isOnNet;
    public static int north = 1;//北
    public static int south = 2;//南
    public static int west = 3;//西
    public static int east = 4;//东
    float btn = 0.5f;
    // Use contObj for initialization
    //public Vector2 speed = new Vector2(50, 50);
    public AudioSource as_Audio;
    public float sudu = 0.5f;
    public float li = 0.5f;
    float X;
    private void Start()
    {
        isPass = false;
    }

    float Y;
    public AudioClip ac_move;
    // 2 - Store the movement
    
    bool s;
    bool x;
    bool z;
    bool r;
    float ctbtn;
    public struct MovingJoystick
    {
        public Vector2 joystickAxis;

    }
   
    void FixedUpdate()
    {
        if (isPass == true )
        {
            if (manipulate == null)
            {
                Debug.Log("contObj 等于 null");
                return;
            }
            movement(whichBTN, li);
                //move(whichBTN);
        }
    }

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


