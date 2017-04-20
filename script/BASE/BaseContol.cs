using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseContol : MonoBehaviour {
    public byte bullect=0;
    public GameObject manipulate;
    public Transform cannonOutput;
    Rigidbody2D rigid;
    public int WhichDirection;
    public int NetworkId=-1;
    public delegate void Shot(byte whichBollet);
    public Shot frie;
    public GameObject cannonball;
    void shot(byte bolletType)
    {
        if (cannonball == null)
            return;
        Vector2 x = transform.TransformDirection(0, 1, 0);
        GameObject prefab = pool.getGMOBJ(cannonball, transform.position, transform.rotation);
        //  OnDrawGizmosSelected();`
        // prefab.transform.eulerAngles = new Vector3(0, 0, 90);
        prefab.GetComponent<Rigidbody2D>().AddForce(x * 200);

        pool.put(prefab, 4);
    }
    private void Awake()
    {
        frie += shot;
        manipulate = gameObject;
    }
    // Use this for initialization
    void Start () {
       
        rigid= manipulate.GetComponent<Rigidbody2D>();
            }
    private void FixedUpdate()
    {

    }
    public void movement(int direction,float maxvelocity)
    {
        WhichDirection = direction;
        switch (direction)
        {

            case 4:

                // rigidbody2D.AddForce(Vector2.right * li);
                 manipulate.GetComponent<Rigidbody2D>().velocity = new Vector2(maxvelocity, 0);
                 manipulate.transform.eulerAngles = new Vector3(0, 0, 270);

                break;
            case 1:

                // rigidbody2D.AddForce(Vector2.up * li);
                 manipulate.GetComponent<Rigidbody2D>().velocity = new Vector2(0, maxvelocity);
                 manipulate.transform.eulerAngles = new Vector3(0, 0, 0);

                break;
            case 3:

                // rigidbody2D.AddForce(Vector2.right * -li);
                 manipulate.GetComponent<Rigidbody2D>().velocity = new Vector2(-maxvelocity, 0);
                 manipulate.transform.eulerAngles = new Vector3(0, 0, 90);

                break;
            case 2:

                // rigidbody2D.AddForce(Vector2.up * -li);
                 manipulate.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -maxvelocity);
                 manipulate.transform.eulerAngles = new Vector3(0, 0, 180);

                break;

        }
    }
    public void setDirection(byte d)
    {
        switch (d)
        {
            case 1:
                zhuan = Vector3.zero;
                manipulate.transform.eulerAngles = zhuan;
                break;
            case 2:
                zhuan = new Vector3(0, 0, 180);
                manipulate.transform.eulerAngles = zhuan;
                break;
            case 3:

                zhuan = new Vector3(0, 0, 90);
                manipulate.transform.eulerAngles = zhuan;
                break;
            case 4:
                zhuan = new Vector3(0, 0, 270);
                manipulate.transform.eulerAngles = zhuan;
                break;
        }
    }
    Vector3 zhuan;
   public void move(int whichpress,int li)
    {
        WhichDirection = whichpress;
        switch (whichpress)
        {
            case 1:
                zhuan = Vector3.zero;
                manipulate.transform.eulerAngles = zhuan;
                manipulate.GetComponent<Rigidbody2D>().AddForce(Vector2.up * li);
                break;
            case 2:
                zhuan = new Vector3(0, 0, 180);
                manipulate.transform.eulerAngles = zhuan;
                manipulate.GetComponent<Rigidbody2D>().AddForce(Vector2.up * -li);
                break;
            case 3:

                zhuan = new Vector3(0, 0, 90);
                manipulate.transform.eulerAngles = zhuan;
                manipulate.GetComponent<Rigidbody2D>().AddForce(Vector2.right * -li);
                break;
            case 4:
                manipulate.GetComponent<Rigidbody2D>().AddForce(Vector2.right * li);
                zhuan = new Vector3(0, 0, 270);
                manipulate.transform.eulerAngles = zhuan;
                break;
        }
    }
}
public class GameObjectType
{
    public static readonly byte LightTank=1, MiddleTank=2, Heavy=3;
    
}