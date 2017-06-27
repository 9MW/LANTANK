using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using System.Net;

public class Sever : NetManager
{
    
    UDPServer Usever;
    private void Start()
    {
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
            adapter.sendmessage(id, new byte[] { (byte)id }, msgType.SeverSideId);
            Debug.Log("clientId=" + id + " byteId=" + (byte)id);
            tmpc = availableColor.Dequeue();
            byte[] colinfo = NetWorkAdapter.GetBytes(tmpc.r + "," + tmpc.g + "," + tmpc.b);
            adapter.sendmessage(id, colinfo, msgType.color);
            Vector3 spawnPOS = new Vector3(-synchronizationObj.Count + PN, 0);
            GameObject player = Instantiate(PlayePrefab, spawnPOS, gameObject.transform.rotation);
            player.name = "player" + id;
            player.GetComponent<SpriteRenderer>().color = tmpc;
            string which = adapter.connectionClient[id].Stext;
            string ip = which.Replace("::ffff:", "");
            IpToObj.Add(ip, id);
            synchronizationObj.Add("player" + id, player);
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
                    
                    c = new Color(cr, cg, cb);
                    availableColor.Enqueue(c);
                    if (times > 50)
                        break;
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
    public override void receiveDataHandel(int outConnectionId, byte[] buffer, int receivedSize, int MT)
    {
        obj = synchronizationObj["player" + outConnectionId];
        Gamobjectsinfo clientInfo = (Gamobjectsinfo)BytesToOBJ(buffer);
        operatingObj = obj.GetComponent<BaseContol>();
        obj.transform.position = new Vector3(clientInfo.x, clientInfo.y,clientInfo.z);
        operatingObj.setDirection(clientInfo.angle);
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

    private void OnDestroy()
    {
        Destroy(Usever);
    }

    public override void processMessage(byte[] Udpdata, IPEndPoint ipinfo)
    {
        print("ipinfo.Address.ToString() " + ipinfo.Address.ToString() );
        Debug.Log(
            " IpToObj[ipinfo.Address.ToString()]="+IpToObj[ipinfo.Address.ToString()].ToString());
        receiveDataHandel(IpToObj[ipinfo.Address.ToString()], Udpdata,1,1);
    }
}
