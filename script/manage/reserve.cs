using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

public class reserve : MonoBehaviour {
    string filepath;
    
    
	// Use this for initialization
    void Awake(){
    #if UNITY_EDITOR
        filepath =Application.dataPath + @"/StreamingAssets/";
    #elif UNITY_ANDROID
    filepath =Application.persistentDataPath + @"/StreamingAssets/";
    #endif
    }
	void  ttt() {
        Dictionary<string, GameObject> gmj = new Dictionary<string, GameObject>();
        GameObject[] emy = GameObject.FindGameObjectsWithTag("enemy");
        foreach(GameObject g in emy)
        {
            gmj.Add(g.name, g);
        }
        var binFormatter = new BinaryFormatter();
        var mStream = new MemoryStream();
        binFormatter.Serialize(mStream, gmj);

        //This gives you the byte array.
        
        Debug.Log("counter是" + gmj.Count+" byte[]="+mStream.ToArray().Length);
        //     save("a1");
    }
    void save(string snm) {
        //if (File.Exists(path))
        {
            string path = filepath + snm + ".tk";
            var fs = new FileStream(path, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject)))
            {
                bw.Write(obj.name);
                bw.Write(obj.transform.position.ToString());
                bw.Write(obj.transform.rotation.ToString());
                bw.Write(obj.transform.localScale.ToString());
   
            }
            bw.Flush();
            bw.Close();
            fs.Close();
            fs.Dispose();
        }
       
    }
    List<float> strtofl(string[] str){
       List<float> rt = new List<float>();
       for (int t = 0; t < str.Length; t++) {
           float x = float.Parse(str[t]);
           rt.Add(x);
       }
       return rt;
       
    }
    void red(string rnm)
    {
        string rdph = filepath + rnm + ".tk";
        string objnm;
        string objps;
        string objrt;
        string objls;
        if(File.Exists(rdph)){
            using (FileStream rdfs = new FileStream(rdph, FileMode.Open)) 
            {
                using (BinaryReader br = new BinaryReader(rdfs)) {
                    int index = 0;
                    while(index<rdfs.Length)
                    try
                    {
                        objnm = br.ReadString();
                        objps = br.ReadString().Trim(new char[] { '(', ')' });
                        objrt = br.ReadString().Trim(new char[] { '(', ')' });
                        objls = br.ReadString().Trim(new char[] { '(', ')' });
                    string[] ps = objps.Split(new char[] { ',' });
                    List<float> v3list = strtofl(ps);
                    Vector3 v3 = new Vector3(v3list[0],v3list[1],v3list[2]);
                    string[] rt = objrt.Split(new char[] { ',' });
                        List<float> rt4list = strtofl(rt);
                        Quaternion rt4 = new Quaternion(rt4list[0], rt4list[1], rt4list[2], rt4list[3]);
                    string[] ls = objls.Split(new char[] { ',' });
                    List<float> ls3list = strtofl(rt);
                    Vector3 ls3 = new Vector3(ls3list[0], ls3list[1], ls3list[2]);
                    //byte[] data = br.ReadBytes((int)rdfs.Length);
                    //string meg = System.Text.Encoding.Default.GetString(data);
                        Debug.Log("读的游戏物体是"+objnm);
                       Debug.Log(objnm + "的位置是" + v3);
                       Debug.Log(objnm + "的角度是" +rt4);
                    Debug.Log(objnm + "的缩放是" + objls);
                             Debug.Log("index是" + index);
                    index ++;
                   // Resources.Load();
                    }
                   catch
                  {break;}
                    
                    }
                }
        }
           
            //while(index<data.Length){
            //    int stglength = data[index];//第一个元素为字符串长度
            //    index++; 
            //    byte[] buf = new byte[stglength];
            //    br.Read(buf, index, buf.Length);
                
            //    objnm = System.Text.Encoding.Default.GetString(buf);
            //   Debug.Log("objnm是"+objnm+"!!");
            //}

        }
      



    
	// Update is called once per frame
	void Update () {
	
	}
}
