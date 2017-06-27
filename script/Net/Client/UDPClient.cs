using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPClient : MonoBehaviour
{
    //TODO: Feed server from config file to allow dynamic server|port
    public string serverIP = "127.0.0.1";
    public int serverPort = 9000;
    public int clientListeningPort = 9001;

    IPEndPoint remoteEndPoint;
    UdpClient server;
    NetworkHandler serverHandler;
    Thread receiveThread;

    public void Start()
    {
        Application.runInBackground = true;
        serverHandler = GetComponent<NetworkHandler>();
        //init();
    }
    
    public void init()
    {
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);
        while (true) { 
            try
            {
                server = new UdpClient(clientListeningPort);
                break;
            }
            catch (SocketException e) {
                clientListeningPort++;
                continue;
            }
        }
        server.EnableBroadcast = true;

        print("Sending to " + serverIP + " : " + serverPort);

        receiveThread = new Thread(new ThreadStart(serverListener));
        receiveThread.IsBackground = true;
        receiveThread.Start();
        //sendString(NetworkConstants.ACTION_SERVER_LOGIN);
    }
    int countErrors = 0;
    
    /**
     * TODO: Handle Reconnect
     * 
     * Receives the message from the server
     * */
    private void serverListener()
    {
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = server.Receive(ref anyIP);
                print("data is " + BitConverter.ToString(data) + " data length=" + data.Length);
                serverHandler.processMessage(data);//delivery data to main thread
            }
            catch (ObjectDisposedException err)
            {
                Debug.LogError("Object disposed");
                Debug.LogError(err.StackTrace);
                break;
            }
            catch (SocketException err)
            {
                Debug.LogError("Connection lost");
                Debug.LogError(err.ErrorCode);
                //break;
            }
            catch (Exception err)
            {
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
    public void sendString(int msg) {
        this.sendString(msg + " ");
    }

    public void sendString(string message)
    {
        try
        {
            if (message != "")
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                server.Send(data, data.Length, remoteEndPoint);
            }
        }
        catch (Exception err)
        {
            print(err.ToString());
        }
    }
    int frequency = 0;
    public void sendByte(byte[] data)
    {
//        print("already send " + frequency + " times");
        try
        {
            if (data.Length != 0)
            {
              
                server.Send(data, data.Length, remoteEndPoint);
                frequency++;
            }
        }
        catch (Exception err)
        {
            print(err.ToString());
        }
        if (frequency % 10 == 0)
            print("already send " + frequency + " times");
    }
    private void OnApplicationQuit()
    {
        try
        {
            server.Close();
            receiveThread.Abort();
        }
        catch (Exception e)
        {
            Debug.Log("Errror closing server connection. \r" + e.StackTrace);
        }
    }


}