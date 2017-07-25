using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using System.Net;

public class Sever : NetManager
{
    //public Shot Botfire;
    UDPServer Usever;
    private void Start()
    {
       // Botfire += sendshotAction;
        Usever = adapter.UDPS;
    }
    Queue<Color> availableColor = new Queue<Color>(27);
    public override void init()
    {
        generateDifferColor();//use color to identify players
        uint PN = 0;
        Debug.Log("Entry to init method adapter.connectionClient=" + adapter.connectionClient.Count);
        //instance player
        foreach (int id in adapter.connectionClient.Keys)
        {
            Color tmpc;
            Debug.Log("clientId=" + id + " byteId=" + (byte)id);
            tmpc = availableColor.Dequeue();
            Vector3 spawnPOS = new Vector3(-synchronizationObj.Count + PN, 0);
            byte[] colinfo = NetWorkAdapter.GetBytes(tmpc.r + "," + tmpc.g + "," + tmpc.b
               + "," +spawnPOS.x + "," +spawnPOS.y );
            adapter.sendmessage(id, colinfo, msgType.color);
            GameObject player = Instantiate(PlayePrefab, spawnPOS, gameObject.transform.rotation);
            
            string which = adapter.connectionClient[id].Stext;
            string ip = which.Replace("::ffff:", "");
            int IPid = int.Parse(ip.Split('.')[3])+1;
            adapter.sendmessage(id, NetWorkAdapter.GetBytes(IPid+","), msgType.SeverSideId);
            player.GetComponent<BaseContol>().NetworkId = IPid;
            player.GetComponent<SpriteRenderer>().color = tmpc;
            player.name = "玩家1" + IPid;
            IpToObj.Add(ip, id);
            synchronizationObj.Add(player.name , player);
        }

    }
    void generateDifferColor()
    {
        Color c;
        int times = 0;
        for (float cr = 0; cr <= 1; cr += 0.5f)
        {

            for (float cb = 0; cb <= 1; cb += 0.5f)
            {

                for (float cg = 0; cg <= 1; cg += 0.5f)
                {
                    times++;
                    
                    c = new Color(cr, cg, cb,255);
                    availableColor.Enqueue(c);
                }
            }
        }
    }
    public override void syncData()
    {
        byte[] bytes = collectData2();
        Usever.broadCast(bytes);
      /*  foreach (int id in adapter.connectionClient.Keys)
        {
            adapter.sendmessage(id,bytes, msgType.Data);
        }
        */
    }
    BaseContol operatingObj;
    GameObject obj;
    public override void receiveDataHandel(int outConnectionId, byte[] buffer, int receivedSize, int MType)
    {
        if (MType == msgType.shoot)
        {
            string[] shotmsg = NetWorkAdapter.splitString(buffer);//element 0 position is key,1 is bulletCategory,client key was placed null parameter 
            byte bullet = byte.Parse(shotmsg[1]);
            byte[] reaibleData = new byte[buffer.Length + 1];
            Array.Copy(buffer, 0, reaibleData, 1, buffer.Length);
            reaibleData[0] = msgType.shoot;
            synchronizationObj[shotmsg[0]].GetComponent<BaseContol>().frie(bullet);
        }
       // operatingObj.frie(clientInfo.state[0]);

            }
    public override void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "scene2")
        {

            playerObject = GameObject.Find("玩家1");
            init();
            GaneStart = true;
        }
    }
    GameObject[] players;
    GameObject[] enemys;
    int enemycounter = 0;
     byte[] collectData2()
    {

        GameObject[] activeObj;
        GameObject[][] tmpContainer;
        byte[] syncbytes;
        //if (isSever && isPlaying)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            enemys = GameObject.FindGameObjectsWithTag("enemy");
            // weapon= GameObject.FindGameObjectsWithTag("weapon");
            tmpContainer = new GameObject[][] { players, enemys };
            int activeGobjnum = players.Length + enemys.Length;
            activeObj = new GameObject[activeGobjnum];
            // SyncPosition = new Vector3[activeGobjnum];
            players.CopyTo(activeObj, 0);
            enemys.CopyTo(activeObj, players.Length);
            int id;

            BaseContol objectstate;
            SynchronizationMessage.Clear();
            for (int i = 0; i < tmpContainer.Length; i++)
            {

                GameObject[] g = tmpContainer[i];
                foreach (GameObject g1 in g)
                {
                    objectstate = g1.GetComponent<BaseContol>();
                    id = objectstate.NetworkId;
                    byte FireState = objectstate.bullect;
                    if (id == -1)
                    {
                        enemycounter += 1;
                        objectstate.NetworkId = enemycounter;
                        SynchronizationMessage.Add(g1.name + enemycounter, new SerializeUDP(objectstate.NetworkId, g1.transform.position, g1.transform));
                        // Debug.Log(g1.name + "-counter=" + enemycounter);
                    }
                    else
                    {
                        SynchronizationMessage.Add(g1.name + id, new SerializeUDP(id, g1.transform.position, g1.transform));
                    }

                    objectstate.bullect = 0;

                }
            }

        }
        syncbytes = ObjectToByteArray(SynchronizationMessage);

        return syncbytes;
        // controlData[1] = c.whichBTN;
    }
    private void OnDestroy()
    {
        Destroy(Usever);
    }
    Dictionary<IPEndPoint, int> IPtoId = new Dictionary<IPEndPoint, int>(10);
    //process incoming position data 
    public override void processMessage(byte[] Udpdata, IPEndPoint ipinfo)
    {
        /* print("ipinfo.Address.ToString() " + ipinfo.Address.ToString() );
         Debug.Log(
             " IpToObj[ipinfo.Address.ToString()]="+IpToObj[ipinfo.Address.ToString()].ToString());
         */
        if (!IPtoId.ContainsKey(ipinfo))
        {
            string address = ipinfo.Address.ToString();
           int IPid = int.Parse(address.Split('.')[3]);
            IPtoId.Add(ipinfo, IPid+1);
        }
        obj = synchronizationObj["玩家1" + IPtoId[ipinfo]];
        SerializeUDP clientInfo = (SerializeUDP)BytesToOBJ(Udpdata);
        operatingObj = obj.GetComponent<BaseContol>();
        // obj.transform.position = new Vector3(clientInfo.x, clientInfo.y,clientInfo.z);
        SyncPosition(operatingObj.transform.gameObject, new Vector3(clientInfo.x, clientInfo.y, clientInfo.z));
        operatingObj.setDirection(clientInfo.angle);
    }

}
