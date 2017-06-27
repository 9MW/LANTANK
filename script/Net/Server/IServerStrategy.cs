using System;
using System.Net;
using UnityEngine;

public abstract class IServerStrategy : NetManager
{
    protected UDPServer udpServer;
    public virtual void processText(string text, IPEndPoint ipEndPoint) { }
    public virtual void processText(byte[] text, IPEndPoint ipEndPoint) { }
    public virtual void setUdpServer(UDPServer server) {
        this.udpServer = server;
    }
}
