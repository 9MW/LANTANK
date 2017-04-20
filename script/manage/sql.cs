/*using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
//using Mono.Data.Sqlite;

public class sql : MonoBehaviour {
    Text jb;
    dbaccess db;
    int money=0;
    string table1 = "玩家数据";
   // string tbnm = "存档1";
	// Use this for initialization
	void Start () {
        //save("se1");
        //read("se1");专业版才可以用，所以线注释了
        bool i = false;
        jb = GameObject.Find("民币").GetComponent<Text>();
       
        dbaccess db = getdb();
         if (i){
            Debug.Log("开始删除表”table1“中所有数据");
            db.DeleteContents("table1");
           Debug.Log("删除完毕");
        }
    
     //if (db.existtable("table1"))//判断数据库表"table1"是否存在，不存在则创建一个
        {
            
            //创建数据库表，与字段
            db.CreateTable(table1, new string[] { "名字", "金币" }, new string[] { "text", "real" });
            
           }
    Debug.Log("进入获取SqliteDataReader对象阶段");
    SqliteDataReader sqReader = db.SelectWhere(table1, new string[] { "名字", "金币" }, new string[] { "名字" }, new string[] { "=" }, new string[] { "玩家1" });
    Debug.Log("已获取SqliteDataReader对象");
    Debug.Log(sqReader.GetOrdinal("名字"));
    bool slt = sqReader.IsDBNull(sqReader.GetOrdinal("名字"));//.detvalue是获取以本机格式表示的指定列的值。
    Debug.Log(slt);
    if (slt)//判断是否有字段玩家1，若有则不再添加
    {
        Debug.Log("进入添加数据阶段");
        db.InsertInto(table1, new string[] { "'玩家1'", "0" });
        Debug.Log("添加数据完毕");
    }

   // Debug.Log(sqReader.Read());
       while (sqReader.Read())
        {
            Debug.Log("进入读取阶段");
            Debug.Log(sqReader.IsDBNull(0));
           
            Debug.Log(sqReader.GetString(sqReader.GetOrdinal("名字")) + sqReader.GetDouble(sqReader.GetOrdinal("金币")));
        } 
        //关闭对象
        db.CloseSqlConnection();
	}
    dbaccess getdb()
    {
#if UNITY_EDITOR
        string appDBPath = Application.dataPath + "/" + "wy.db";
        dbaccess db = new dbaccess("URI=file:" + appDBPath);
#elif UNITY_ANDROID
   
        string appDBPath = Application.persistentDataPath  + "/"+"wy.db";

        dbaccess db = new dbaccess("URI=file:" +  appDBPath);
      //  Debug.Log(  db.existtable("table1"));
        if (db.existtable("table1"))//判断数据库文件是否存在，不存在则创建一个
        {
           
            //创建数据库表，与字段
             db.CreateTable("table1", new string[] { "名字", "金币" }, new string[] { "text", "real" });
           }
          
        
#endif
        return db;
    
     }
    bool save(string tbnm) {
       
        db = getdb();
        db.UpdateInto(table1, new string[] { "金币" }, new string[] { ""+money }, "名字", "玩家1");
        if (db.existtable(tbnm))//判断数据库中是否已存在表tbnm
        {
            db.CreateTable(tbnm, new string[] { "类型", "位置", "旋转" }, new string[] { "text", "BLOB", "BLOB" });
            foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject)))
            {
                string wz = "" + obj.transform.position;
                Debug.Log("开始保存场景数据");
                Debug.Log("位置数据为" + wz);
                Debug.Log("物体名称是" + obj.name);
                db.InsertInto(tbnm, new string[] { "'" + obj.name + "'", "'" + obj.transform.position + "'", "'" + obj.transform.rotation + "'" });
                Debug.Log("数据保存完毕");

            }
            db.CloseSqlConnection();
            return true;
        }
        else {
            Debug.Log("表已存在");
            return false;
        }
    
    }
    bool read(string tbnm)
    {
        db = getdb();
        Debug.Log("进入获取read中 SqliteDataReader对象阶段");
        SqliteDataReader sqReader = db.ReadFullTable(tbnm);
        Debug.Log("已获取read中 SqliteDataReader对象");
        while (sqReader.Read()) {
            Debug.Log("开始读取数据");
            string nam = sqReader.GetString(sqReader.GetOrdinal("类型"));
            GameObject objPrefab = (GameObject)Resources.Load("Prefab/"+nam);
            Debug.Log("已加载gamobject");
            byte [] vector=(byte [])sqReader.GetValue(sqReader.GetOrdinal("位置"));
            ArrayList vtr = db.to0bj(vector, new char[] { '(', ')' });
            float x = (float)vtr[0];
            float y = (float)vtr[1];
            float z = (float)vtr[2];
            Vector3 wz = new Vector3(x, y, z);
            byte[] quality = (byte[])sqReader.GetValue(sqReader.GetOrdinal("旋转"));
            ArrayList qty = db.to0bj(quality, new char[] { '(', ')' });
            Quaternion quayt = new Quaternion((float)qty[0], (float)qty[1], (float)qty[2], (float)qty[3]);
           
           // for (int i = 0; i < vtr.Count;i++ )
            {
                Debug.Log(" Vector3 wz 为:" + "-----" + wz);
            }
           // Vector3 w =new Vector3( Convert.ChangeType(sqReader.GetString(sqReader.GetOrdinal("位置")), typeof(Vector3)));
            Debug.Log("获取完成");
            
        }
        return true;
    
    }
	// Update is called once per frame
    void onenemykill(GameObject killtk)
    {
        
        switch (killtk.name)
        {
            case "装甲车":
                money += Random.Range(1,4);
                break;
            case "中型坦克":
                money+= Random.Range(3, 7);
                break;
            case "重型坦克":
                money += Random.Range(5, 10);
                break;
        }
      

    
    }
    void Update() {

        jb.text = "金币"+money ;
    }
	}

*/