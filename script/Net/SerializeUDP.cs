using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SerializeUDP  {
    public int id;
    public byte angle;
    public float x, y, z = 0;
    public SerializeUDP(int identification, Vector3 v, Quaternion rotation)
    {
        id = identification;
        angle = 0;
        this.x = v.x;
        this.y = v.y;
        switch ((int)rotation.eulerAngles.z)
        {
            case 0:
                angle = 1;
                break;
            case 180:
                angle = 2;
                break;
            case 90:
                angle = 3;
                break;
            case 270:
                angle = 4;
                break;

        }
    }
}
