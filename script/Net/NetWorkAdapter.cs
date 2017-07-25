using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using UnityEngine.Networking.Types;

public class NetWorkAdapter : NetworkDiscovery
{
    NetworkDiscovery ND;
    public GameObject Roomlist;
    public GameObject text;
    private ConnectionConfig config;
    public  delegate void receiveData(int outConnectionId, byte[] buffer, int receivedSize, int mstype);
    public  receiveData Receiver;
    // Use this for initialization
    void Start()
    {
        Debug.Log("HEAD != HEAD" + ("HEAD" != "HEAD"));
        LogFilter.currentLogLevel = 1;
        Roomlist = GameObject.Find("/Canvas/room");
        //   ND = FindObjectOfType<NetworkDiscovery>();
        DontDestroyOnLoad(this);
        if (Initialize())
        {
            Debug.Log("NetworkDiscovery initialize success");
        };
    }
    Queue<byte[]> recieveMsgQue=new Queue<byte[]>(128);
    public Dictionary<int, StrTxt> connectionClient = new Dictionary<int, StrTxt>();
    byte[] buffer = new byte[1024];
    void fresh()
    {
        int outHostId;
        int outConnectionId;
        int outChannelId;
        string outConnIp;
        int outPort;
        NetworkID outID;
        NodeID outNode;
        int receivedSize;
        byte error;
        int quesize;
        NetworkEventType evt;
        int dog = 0;
        do
        {
            dog++;
             
            evt =
NetworkTransport.Receive(out outHostId, out outConnectionId, out outChannelId, buffer, buffer.Length, out receivedSize, out error);
            quesize = NetworkTransport.GetIncomingMessageQueueSize(outHostId, out error);
            if (error != 0)
               print("Receive error=" + error.ToString() + "quesize=" + quesize);
            switch (evt)
            {
                case NetworkEventType.ConnectEvent:
                    {
                        NetworkTransport.GetConnectionInfo(outHostId, outConnectionId, out outConnIp, out outPort, out outID, out outNode, out error);
                        connectionClient.Add(outConnectionId, new StrTxt(Instantiate(text), outConnIp, true));
                        OnConnect(outHostId, outConnectionId, (NetworkError)error);
                        break;
                    }
                case NetworkEventType.DisconnectEvent:
                    {
                        OnDisconnect(outHostId, outConnectionId, (NetworkError)error);
                        break;
                    }
                case NetworkEventType.DataEvent:
                    {
                        
                        OnData(outHostId, outConnectionId, outChannelId,ref buffer, receivedSize, (NetworkError)error);
                        break;
                    }
                case NetworkEventType.BroadcastEvent:
                    {
                        OnBroadcast(outHostId, buffer, receivedSize, (NetworkError)error);
                        break;
                    }
                case NetworkEventType.Nothing:
                    break;

                default:
                    Debug.LogError("Unknown network message type received: " + evt);
                    break;
            }


        }
        while (dog<30&&quesize!=0);
    }
    int receiveMsgType;
    bool isHaed = true;
    //int  GetSize;
    //int conuter = 0;
    //byte[][] tmpReceive;
    private void OnData(int outHostId, int outConnectionId, int outChannelId,ref byte[] buffer, int receivedSize, NetworkError error)
    {
        Debug.Log("Entry OnData receive size="+buffer.Length+ "receivedSize"+ receivedSize);
      /*  
        if (isHaed)
        {
            byte[] reallyBytes = new byte[receivedSize];
            Array.Copy(buffer, 0, reallyBytes, 0, receivedSize);
            Debug.Log("!= HEAD?"+GetString(reallyBytes)+ "!= HEAD?");
            string[] s = splitString(reallyBytes);
          //  Debug.Log("s[2]=" + s[2] +"!= HEAD?");
         //   if (s[2] != "HEAD")
         //       return;
            isHaed = false;
            GetSize = int.Parse(s[1]);
            this.buffer = new byte[GetSize];
          //  if (GetSize>1024)
           //     tmpReceive = new byte[(GetSize / 1024) + 1][];
            //connectionClient[outConnectionId].setHead(false);
            receiveMsgType = int.Parse(s[0]);
            Debug.Log("receiveMsgType"+receiveMsgType);
         //   connectionClient[outConnectionId].setMessageType(receiveMsgType);
            Debug.Log("GetSize=" + GetSize);
            return;
        }
        Debug.Log("Entry Receiver");*/
        if (buffer[0] == msgType.testMessage)
        {
            Debug.Log("Receive test message=" + NetWorkAdapter.GetString(buffer));
            isHaed = true;
            return;
        }
        /*
         if (conuter< (GetSize / 1024)+1&&GetSize>1024)
         {

             tmpReceive[conuter] = buffer;
             conuter++;
             if(conuter== (GetSize / 1024) + 1)
             {
                 isHaed = true;
                 conuter = 0;
                 byte[] wholeMessage = combineBytes(tmpReceive, GetSize);
                 if (receiveMsgType == msgType.testMessage)
                 {
                     Debug.Log("Receive test message=" + NetWorkAdapter.GetString(wholeMessage));
                     return;
                 }
                 Receiver(outConnectionId, wholeMessage, receivedSize, receiveMsgType);

             }
             return;
             // GetSize -= conuter * 1024;
         }

             if (receiveMsgType == msgType.testMessage)
             {
                 Debug.Log("Receive test message=" + NetWorkAdapter.GetString(buffer));
                 isHaed = true;
                 return;
             }   */
        byte[] data = new byte[receivedSize-1];
        Array.Copy(buffer,  1, data, 0, data.Length);
        receiveMsgType = buffer[0];
        Receiver(outConnectionId, data, receivedSize, receiveMsgType);
        //     isHaed = true;
        //buffer = new byte[50];

    }
   public UDPClient UDPC;
    public void asClint()
    {
        if (isClient == true)
        {
            Debug.Log("Clienthostid=" + hostId);
            Debug.Log("client already start");
            return;
        }
        init();
        /*
        if (hostId == -1)
        {

            channelid = config.AddChannel(QosType.Reliable);
            HostTopology topology = new HostTopology(config, 1);
            hostId = NetworkTransport.AddHost(topology, broadcastPort);
        }*/
        byte error;

        if (NetworkTransport.IsBroadcastDiscoveryRunning())
            NetworkTransport.StopBroadcastDiscovery();
            NetworkTransport.SetBroadcastCredentials(hostId, broadcastKey, broadcastVersion, broadcastSubVersion, out error);
        running = true;
        isClient = true;
        Debug.Log("hostid=" + hostId + ",error=" + error);
        /*
        foreach (string info in broadcastsReceived.Keys)
        {
            text.GetComponent<Text>().text = info + System.Text.Encoding.Default.GetString(broadcastsReceived[info].broadcastData);
            /*worldPositionStays	If true, the parent-relative position,
            scale and rotation are modified such that the object keeps the same world space position,
            rotation and scale as before.
            Instantiate(text).transform.SetParent(Roomlist.transform, false);
        }
     */
        isServer = false;
        gameObject.AddComponent<Client>();
        UDPC=gameObject.AddComponent<UDPClient>();
        Destroy(gameObject.GetComponent<Sever>());
        
    }
   public UDPServer UDPS;
    public void establishHost()
    {
        if (isServer == true)
            return;
        gameObject.AddComponent<Sever>();
        Destroy(gameObject.GetComponent<Client>());
        UDPS=gameObject.AddComponent<UDPServer>();
        init();
        /*
        HostTopology topology = new HostTopology(config, 10);
        int hostId = NetworkTransport.AddHost(topology, 8898);*/
        int severport = UDPS.port;
        
        broadcastData = hostId + "," + broadcastPort + "," + channelid + ","+severport;
        byte[] massageout = GetBytes(broadcastData);
        byte err = new byte();
        if(!NetworkTransport.IsBroadcastDiscoveryRunning())
        NetworkTransport.StartBroadcastDiscovery(hostId, broadcastPort, broadcastKey, broadcastVersion, broadcastSubVersion, massageout, massageout.Length, broadcastInterval, out err);
        Debug.Log("broadcastPort" + severport + "hostId=" + hostId + "NetworkTransport.StartBroadcastDiscovery=" +
       "channelid=" + channelid);
        running = true;
        isServer = true;
        
        //         StartAsServer();

    }

    Dictionary<string, string> listed = new Dictionary<string, string>();
    public int connectionId;
    int[] connIdAy = new int[10];
    int channelid;
    int receiveHostID;
    public void connectTosever(Text t)
    {
        foreach (string info in listed.Keys)
        {
            Debug.Log("fromAddress= " + info);
        }
        // Debug.Log("listescountHash " + listed.GetHashCode());
        string which = t.text;
        byte error = new byte();
        string[] s = listed[which].Split(new char[] { ',' });
        string ip = which.Replace("::ffff:", "");
        receiveHostID = int.Parse(s[0]);//.Replace("hostId", "")
        int port = int.Parse(s[1]);
        channelid = int.Parse(s[2]);
        UDPC.serverIP = ip;
        UDPC.serverPort = int.Parse(s[3]);
        UDPC.init();
        connectionId = NetworkTransport.Connect(hostId, ip, port, channelid, out error);
        Debug.Log("hostid=" + hostId + "error="+error);
        Debug.Log(connectionId + "ip = " + ip + "channelID=" + channelid);
    }
    
    int e = 0;
    public void testFunction()
    {
        string form = 0 + "," + 1024+ "," + "HEAD";// + "," + "HEAD";
        Debug.Log("msgHEAD = " + form + '|');
        byte[] msgInfo = GetBytes(form);
        string[] s6 = splitString(msgInfo);
        Debug.Log("Reistare = " +GetString(msgInfo) + "|s.2="+s6[2]+" istrue="+(s6[2]=="HEAD"));
        
        foreach (int id in connectionClient.Keys)
        {

            string s = "长太息以掩涕兮,哀民生之多艰";
            byte[] b;
            string d = s;
            while (d.Length < 519)
            {
                d += s;
            }
            b = GetBytes(d);
            Debug.Log("b.length=" + b.Length+"Bcontent="+GetString(b));
            if (e % 2 == 0)
            {
                e = 0;
                while (e++ < 15)
                {
                    sendmessage(id, b, msgType.testMessage);
                }
                
            }
            sendmessage(id, b, msgType.testMessage);
        }
       
    }
    public void sendmessage(int ID, byte[] msg, int Type)
    {

        byte error = new byte();
        byte[] d = new byte[msg.Length + 1];
        d[0] = (byte)Type;
        Array.Copy(msg, 0, d, 1, msg.Length);
        /* Comment code for send big than 1024k data.
         * byte[] msg = GetBytes("背绳墨以追曲兮, 终不察夫民心");
        string form = Type + "," + msg.Length+","+"HEAD";// + "," + "HEAD";
        Debug.Log("msgHEAD = " + form+'|');
        byte[] msgInfo = GetBytes(form);
        NetworkTransport.Send(hostId, ID, channelid, msgInfo, msgInfo.Length, out error);
        Debug.Log("connectionIdHead  " + connectionId + " error= " + error.ToString()+" channelid="+channelid);
        NetworkTransport.Send(hostId, ID, channelid, msg, msg.Length, out error);
        return;*/
        NetworkTransport.Send(hostId, ID, channelid, d, d.Length, out error);
        /*
        if (msg.Length <= 1024)
        {
            NetworkTransport.Send(hostId, ID, channelid, msg, msg.Length, out error);
        }
        else
        {
            byte[][] bs = spelitBytes(msg, msg.Length);
            for (int i= 0; i< bs.Length; i++)
            {
                NetworkTransport.Send(hostId, ID, channelid, bs[i], bs[i].Length, out error);
                if (i > 2||error!=0)
                {
                    Debug.Log("sendI=" + i);
                    Debug.Log("connectionIdmsg  " + connectionId + " error= " + error);
                }
               
            }
        }
        Debug.Log("NetworkTransport.GetIncomingMessageQueueSize=" + NetworkTransport.GetOutgoingMessageQueueSize(hostId,out error));
        */
    }
    void init()
    {
        if (hostId != -1)
            return;
       // gameObject.AddComponent<MainThreadProcessor>().init();
        NetworkTransport.Init();
        config = new ConnectionConfig();
        channelid = config.AddChannel(QosType.ReliableSequenced);
        HostTopology topology = new HostTopology(config, 15);
        // hostId = NetworkTransport.AddHost(topology);
        Debug.Log("hostId1=" + hostId);
        hostId = NetworkTransport.AddHost(topology, broadcastPort);
    }
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        if (!listed.ContainsKey(fromAddress))
        {
            listed.Add(fromAddress, data);
            text.GetComponent<Text>().text = fromAddress;
            GameObject clientslist =
            Instantiate(text);
            clientslist.transform.SetParent(Roomlist.transform, false);
            clientslist.GetComponent<TextTrigger>().n = this;
            Debug.Log("count" + listed.Count);
        }

        // base.OnReceivedBroadcast(fromAddress, data);
    }
   public void OnConnect(int hostId, int connectionId, NetworkError error)
    {
        connectionClient[connectionId].Gtext.transform.SetParent(Roomlist.transform, false);
        Debug.Log("OnConnect(ReceivehostId = " + hostId + ", connectionId = "
            + connectionId + ", error = " + error.ToString() + ")");
    }
    void showText(string show)
    {
        GameObject clientslist =
          Instantiate(text);
        clientslist.GetComponent<Text>().text = show;
        clientslist.transform.SetParent(Roomlist.transform, false);
    }
   public void OnDisconnect(int hostId, int connectionId, NetworkError error)
    {
        Destroy(connectionClient[connectionId].Gtext);
        connectionClient.Remove(connectionId);
        Debug.Log("OnDisconnect(hostId = " + hostId + ", connectionId = "
            + connectionId + ", error = " + error.ToString() + ")");
    }

   public void OnBroadcast(int hostId, byte[] data, int size, NetworkError error)
    {
    }
   private void FixedUpdate()
    {
        if (hostId != -1)
        {
         fresh();
        }
    }
    public static byte[] GetBytes(string str)
    {
        byte[] bytes = new byte[str.Length * sizeof(char)];
        System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
        return bytes;
    }

    public static string GetString(byte[] bytes)
    {
        char[] chars = new char[bytes.Length / sizeof(char)];
        System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
        return new string(chars);
    }
    public static string[] splitString(byte[] b)
    {
        return GetString(b).Split( ',' );
    }
    public static byte[] combineBytes(byte[][] inbytes,int Size)
    {
        byte[] result = new byte[Size];
        int i = 0;
        while (++i < inbytes.Length)
        {

            Array.Copy(inbytes[i-1],0, result, (i - 1) * 1024, 1024);

        }
        Array.Copy(inbytes[i-1], 0, result, (i - 1) * 1024, Size-(i-1)*1024);
        return result;
    }
    public static byte[][] spelitBytes(byte[] inbytes, int UnitSize)
    {
        int spelitLength;
        if (inbytes.Length%UnitSize!=0)
         spelitLength = (inbytes.Length / UnitSize)+1;
        spelitLength = (inbytes.Length / UnitSize);
        byte[][] result = new byte[spelitLength][];
        int i = 0;
        while (i < spelitLength)
        {
            byte[] container;
            i++;
            if (i != spelitLength)
            {
                container = new byte[1024];
                Array.Copy(inbytes, (i - 1) * 1024, container, 0, 1024);
            }
            else
            {
                container = new byte[inbytes.Length - (i - 1) * 1024];
                Array.Copy(inbytes, (i - 1) * 1024, container, 0, container.Length);
            }
            result[i-1] = container;
        }
        return result;
    }
}
   public struct StrTxt
{
    public bool isHead;
    public  GameObject Gtext;
    public  string Stext;
    public int messageType;
  public StrTxt(GameObject g,string s,bool ishead)
    {
        messageType = 3;
        isHead = ishead;
        Gtext = g;
        Gtext.GetComponent<Text>().text = s;
        Stext = s;
    }
    public void setHead(bool i)
    {
        isHead = i;
    }
    public void setMessageType(int yp)
    {
        messageType = yp;
     }
}
