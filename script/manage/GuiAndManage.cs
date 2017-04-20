using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
    public class GuiAndManage : MonoBehaviour
    {
        public int enemy = 27;
        public int exist = 9;
        int existing = 0;
        public GameObject s1;
        public GameObject s2;
        public GameObject s3;//prefix with s is spown position
        public GameObject t1;//prefix with t is represent enemys prefab
        public GameObject t2;
        public GameObject t3;

        bool 是否暂停 = false;
        GameObject[] gt;//实例化的坦克
        Text tt;
        Text money;
        float tm = 0;
        GameObject[] pd;
        // Use this for initialization reference Object reference not set to an instance of an object
        //attribute.FixedUpdate () (at Assets/script/attribute.cs:52)

        public void tstfc(int n)
        {
            n++;
        }

        void Start()//自适应屏幕算法
        {

            
            tt = GameObject.Find("属性").GetComponent<Text>();
            money = GameObject.Find("民币").GetComponent<Text>();
            money.text = "金币";
        tt.text = "剩余敌人" + existing;
        gt = new GameObject[3] { t1, t2, t3 };
        //刷点与坦克的一维数组
        /*  int ManualWidth = 1920;
          int ManualHeight = 1080;
          int manualHeight;
          if (System.Convert.ToSingle(Screen.height) / Screen.width > System.Convert.ToSingle(ManualHeight) / ManualWidth)
              manualHeight = Mathf.RoundToInt(System.Convert.ToSingle(ManualWidth) / Screen.width * Screen.height);
          else
              manualHeight = ManualHeight;
          Camera camera = GetComponent<Camera>();
          float scale = System.Convert.ToSingle(manualHeight / 640f);
          camera.fieldOfView *= scale;*/
    }

        // Update is called once per frame
        void FixedUpdate()
        {
        //pdparent();



        spownEnemy();
          
        }
    void spownEnemy()
    {
        if (existing < exist && enemy > 9)
        {


            tm += Time.deltaTime;

            if (tm > 2)
            {
               
                int s = Random.Range(0, 3);
                int t = Random.Range(0, 3);
                //  Debug.Log("s是" + s);
                // Debug.Log("t是" + t);
                GameObject tk = gt[t];

                switch (s)
                {
                    case 0:
                        pool.getGMOBJ(tk, Vector3.zero, Quaternion.identity);
                        break;
                    case 1:
                        pool.getGMOBJ(tk, Vector3.zero, Quaternion.identity);
                        break;
                    case 2:
                        pool.getGMOBJ(tk, Vector3.zero, Quaternion.identity);
                        break;

                }
                existing++;
                tm = 0;
                //Debug.Log(existing);
            }

        }

    }
    void pdparent()///将炮弹作为摄像机子物体用broadcastmessage给此脚步传递碰撞物体信息
        {

            pd = GameObject.FindGameObjectsWithTag("weapon");
            for (int ps = 0; ps < pd.Length; ps++)
            {
                pd[ps].transform.parent = transform;
            }



        }
      public  void onenemykill(GameObject killtk)
        {
            switch (killtk.name)
            {
                case "装甲车":
                    money.text += 3;
                    break;
                case "中型坦克":
                    money.text += 3;
                    break;
                case "重型坦克":
                    money.text += 3;
                    break;

            }

            existing--;
            enemy--;
        }
        /* public void Pause(Animator m_Open) {
              if (是否暂停)
              {
                  Object[] objects = FindObjectsOfType(typeof(GameObject));
                  foreach (GameObject go in objects)
                  {
                      if (go.name == "摄像机")
                          continue;
                      go.SendMessage("OnPauseGame", SendMessageOptions.DontRequireReceiver);
                  }

              }
              else { Time.timeScale = 1; }
              //m_Open.SetBool(Open, 是否暂停);


              是否暂停 = !是否暂停;
              Debug.Log("点击了暂停按钮");


          }*/
      
    }

    public interface MYMSG : IEventSystemHandler
    {
        void BtnDW(int o);
        void BtnUp(int o);
    }
