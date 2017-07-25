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
    // Use contObj for initialization
    //public Vector2 speed = new Vector2(50, 50);
    public AudioSource as_Audio;
    public float li = 0.5f;
    private void Start()
    {
        isPass = false;
    }
    public AudioClip ac_move;
    // 2 - Store the movement
    
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
            move(whichBTN, li);
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


