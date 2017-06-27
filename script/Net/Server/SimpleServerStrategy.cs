using System;
using System.Net;
using UnityEngine;

public class SimpleServerStrategy : IServerStrategy
{
    private int currentPlayer = 0;

    public SimpleServerStrategy()
    {
        Debug.Log("Starting: SimpleServerStrategy");
    }

    public override void setUdpServer(UDPServer udpServer) {
        this.udpServer = udpServer;
    }

    public override void processText(string text, IPEndPoint senderIpEndPoint)
    {
        int senderPort = senderIpEndPoint.Port;
        string senderIp = senderIpEndPoint.Address.ToString();
        string playerKey = senderIp + ":" + senderPort;

        if (text.StartsWith(NetworkConstants.ACTION_BROADCAST))
        {
            udpServer.broadCast(text);
        }
        else if (text.StartsWith(NetworkConstants.ACTION_UPDATE_POSITION + " ") || text.StartsWith(NetworkConstants.INPUT_POSITION))
        {
            udpServer.broadCast(text, senderIpEndPoint);
        }
        else if (text.StartsWith(NetworkConstants.ACTION_SERVER_LOGIN + " "))
        {
            udpServer.sendMessage(getNextPlayerMessage(), senderIpEndPoint);
        }
        else
        {
            Console.WriteLine(playerKey + " - UNKNOWN MESSAGE :: " + text);
        }
    }
    private string getNextPlayerMessage()
    {
        //TODO: Rework this assign of player
        String nextPlayerMessage = NetworkConstants.ACTION_SERVER_LOGIN + " " + currentPlayer++;
        return nextPlayerMessage;
    }
}
