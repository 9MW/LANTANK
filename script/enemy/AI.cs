using UnityEngine;

public class AI: BaseContol {
    int n ;//n为选择下一步动作的随机数字
    
    public int hp=1;
    public int defense;
    public int attack;
    public int paodanforce = 200;
    //float t = 0;
    public GameObject pre;
    public string type;
    public int paodantime=10;
    public float maxvelocity=0.3f;
    Vector2 x;
    bool col;
    Transform zszbx ;
    int waitime=3;//ai改变动作的时间
	// Use this for initialization
	void Start () {
        manipulate = gameObject;
        type = this.name;
	gameObject.AddComponent<BoxCollider2D>();
    gameObject.AddComponent<Rigidbody2D>();
    gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
     /*
    GameObject shuapaodan=new GameObject("spd");
    shuapaodan.transform.parent = this.transform;
    shuapaodan.transform.localPosition = new Vector2(0,0.25f);
    shuapaodan.AddComponent("epd");*/
   // address_paodan = transform.localPosition;
        
    InvokeRepeating("think", 0, waitime);
        
	}
    
    void think() {
      //  ++ii;
     
         waitime = Random.Range(2, 5);
        
        this.n = Random.Range(1, 5);

      //  Debug.Log(this.name+"开始第"+ii+"次调用think方法"+"\n"+"AI中的n是" + n + "\n"+ "waittim是" + waitime);
        paodan();
    }
    void OnCollisionEnter2D(Collision2D coll) { 
    if(coll.transform.CompareTag("enemy")){
        col =true;
    }
    }
	// Uvoid OnCollisionEnter2D(Collision2D coll) pdate is called once per frame
    void FixedUpdate()
    {
        //equip();
       //Debug.Log(address_paodan);
       
        if ( GetComponent<Rigidbody2D>().velocity == new Vector2(0,0)||
            col)
        {
            this.n = Random.Range(1, 5);
            paodan();
            col = false ;
         }
        movement(n, maxvelocity);
    }
    void equip()//检测坦克前方是否为玩家
    {
        Vector3 y = transform.TransformDirection(0, 1, 0);
        RaycastHit2D hit2d=Physics2D.Raycast(transform.position,y,9,9);
        
            if (hit2d.rigidbody.GetComponent<Rigidbody2D>().gameObject.tag == "Player")
            { Debug.Log("这是" + hit2d.collider.GetComponent<Rigidbody2D>().gameObject.name); }
        
    }
    
        void paodan()
    {
        
        //Debug.Log("AI.paodan.randomfair是"+randomfair);
        if (waitime == 3) { fire( pre); }
       
          /*  zszbx = this.transform;
            x = transform.TransformDirection(0, 1,0);
            //Vector2 address_paodan = transform.TransformDirection(transform.position);
            Vector2 address_paodan;
           // float y = transform.TransformDirection(transform.position).y + 1;
          //  float x1 = transform.TransformDirection(transform.position).x;
            address_paodan = transform.TransformDirection(new Vector2(x1, y));
            transform.localPosition = new Vector3(0, 1, 0);
            //Debug.Log(address_paodan);
            //Debug.Log("transform.position" + transform.position);
            GameObject prefab = (GameObject)pool.get(pre, transform.localPosition, transform.rotation);
         
             prefab.transform.eulerAngles = new Vector3(0, 0, 90);
            prefab.rigidbody2D.AddForce(x * paodanforce);
            
 */
        
        }
        void fire(GameObject pre)
        {
        bullect = 1;
            Vector3 x = transform.TransformDirection(0, 1, 0);
            pool.AIPD = true;
            GameObject prefab = pool.getGMOBJ(pre, transform.position, transform.rotation);
            prefab.GetComponent<Rigidbody2D>().AddForce(x * paodanforce);
         }
      /*  void player() {
            Ray2D ra2 = new Ray2D(x,transform.position);
            RaycastHit2D rh2;
            Physics2D.Raycast(ra2,rh2,t);
        }*/
	
}
