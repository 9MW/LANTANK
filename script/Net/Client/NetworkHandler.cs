using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkHandler :MonoBehaviour,INetworkHandler
{
    //protected UDPServer udpServer;
    //public virtual void setUdpServer(UDPServer server)
    //{
    //    this.udpServer = server;
    //}
    public void processMessage(string text)
    {
        if (text == null || text.Length == 0)
        {
            return;
        }
        Debug.Log(">> " + text);

        MainThreadProcessor.Instance().Enqueue(MainThreadProcessor.Instance().processMessage(text));
    }
     public void processMessage(byte[] text)
    {
        if (text == null || text.Length == 0)
        {
            return;
        }
     //   Debug.Log(">> " +BitConverter.ToString(text));

        MainThreadProcessor.Instance().Enqueue(MainThreadProcessor.Instance().processMessage(text));
    }
    public void processMessage(byte[] text, System.Net.IPEndPoint ip)
    {
        if (text == null || text.Length == 0)
        {
            return;
        }
      //  Debug.Log(">> " + text);

        MainThreadProcessor.Instance().Enqueue(MainThreadProcessor.Instance().processMessage(text,ip));
    }
}
