using System;
using System.Net;
using UnityEngine;

public class BroadcastServerStrategy : IServerStrategy
{
    public BroadcastServerStrategy()
    {
    }
    public void Start()
    {
        Debug.Log("Starting BroadcastServerStrategy");
    }
    public override void processText(string text, IPEndPoint ipEndPoint)
    {
        if (text == null || text.Length == 0)
        {
            return;
        }
        udpServer.broadCast(text);
    }

    public override void setUdpServer(UDPServer udpServer)
    {
        this.udpServer = udpServer;
    }
}
