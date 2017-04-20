using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Sever : NetManager
{
    
    public override void syncData()
    {
        byte[] bytes = collectData();
        
        foreach (int id in adapter.connectionClient.Keys)
        {
            adapter.sendmessage(id,bytes, msgType.Data);
        }

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
        operatingObj.frie(clientInfo.state[0]);

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

}
