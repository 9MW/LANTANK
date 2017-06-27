using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;


public class UDPServer : MonoBehaviour
{
    Thread receiveThread;
    UdpClient udpServer;

    private NetworkHandler serverStrategy;

    public int port = 9000;
    public bool startListening = true;
//    private String lastMessage = "";

    private Dictionary<string, IPEndPoint> clientsList = new Dictionary<string, IPEndPoint>();

    //private static void Main()
    //{
    //    UDPServer receiveObj = new UDPServer();
    //    receiveObj.init();

    //    string text = "";
    //    do
    //    {
    //        text = Console.ReadLine();
    //    }
    //    while (!text.Equals("exit"));
    //}
    // start from unity3d
    public void Start()
    {
        Debug.Log("Starting: UDPServer"+startListening);
        if (startListening)
        {
            init();
        }
    }
    public void init()
    {
        print("Listening " + port);
        serverStrategy = GetComponent<NetworkHandler>();
      //  serverStrategy.setUdpServer(this);
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }
    private int countErrors = 0;
    private void ReceiveData()
    {
        //Server loop
        udpServer = new UdpClient(port);
        udpServer.EnableBroadcast = true;
        while (true)
        {
            try
            {
              
                // Client msg arrived.
                IPEndPoint senderIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

                byte[] data = udpServer.Receive(ref senderIpEndPoint);
               
                int senderPort = senderIpEndPoint.Port;
                string senderIp = senderIpEndPoint.Address.ToString();
                string playerKey = senderIp + ":" + senderPort;
                if (!clientsList.ContainsKey(playerKey))
                {
                    clientsList.Add(playerKey, senderIpEndPoint);
                }

                // Converting data to string
               // string text = Encoding.UTF8.GetString(data);
                serverStrategy.processMessage(data, senderIpEndPoint);
            }
            catch (ObjectDisposedException err)
            {
                Debug.LogError("SERVER: Object disposed. Exiting.");
                Debug.LogError(err.StackTrace);
                break;
            }
            catch (SocketException err)
            {
                Debug.LogError("SERVER: Connection error, retrying...");
                Debug.LogError(err.StackTrace);
                //break;
            }
            catch (Exception err)
            {
                Debug.LogError("SERVER: Exception");
                Debug.LogError(err.ToString());
                Debug.LogError(err.StackTrace);
                if (countErrors++ > 10)
                {
                    Debug.LogError("Too many errors");
                    Application.Quit();
                    break;
                }
            }
        }
    }

    public void broadCast(string msg, IPEndPoint ipEndPoint)
    {
        IPEndPoint[] players = UDPServer.toArray(this.clientsList);

       
        foreach (IPEndPoint player in players)
        {
            if (player != null && !player.Equals(ipEndPoint))
            {
                sendMessage(msg, player);
            }
        }
    }
    public void broadCast(byte[] msg)
    {
        IPEndPoint[] players = UDPServer.toArray(this.clientsList);
        print("players=" + clientsList.Count);
        
        foreach (IPEndPoint player in players)
        {
            if (player != null )
            {
                udpServer.Send(msg, msg.Length, player);
            }
        }
    }

    public void broadCast(string msg)
    {
        this.broadCast(msg, null);
    }

    public void sendMessage(string msg, IPEndPoint ipEndPoint)
    {
        byte[] msgBytes = Encoding.UTF8.GetBytes(msg);
        udpServer.Send(msgBytes, msgBytes.Length, ipEndPoint);
    }


    private static IPEndPoint[] toArray(Dictionary<string, IPEndPoint> dictionary)
    {
        IPEndPoint[] ipEndpointList = new IPEndPoint[dictionary.Count];
        dictionary.Values.CopyTo(ipEndpointList, 0);
        return ipEndpointList;
    }

    private void OnApplicationQuit()
    {
        try
        {
            udpServer.Close();
            receiveThread.Abort();
        }
        catch (Exception e)
        {
            Debug.Log("Errror closing server connection. \r" + e.StackTrace);
        }
    }
}
