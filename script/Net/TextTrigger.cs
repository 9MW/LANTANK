
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class TextTrigger : MonoBehaviour, IPointerClickHandler {
    [HideInInspector]
    public NetWorkAdapter n;
    public void OnPointerClick(PointerEventData eventData)
    {
        n.connectTosever(gameObject.GetComponent<Text>());
    }

}
/*
public class DirectSetup : MonoBehaviour
{
    // Matchmaker related
    List<UnityEngine.Networking.Match.MatchInfoSnapshot> m_MatchList = new List<MatchInfoSnapshot>();
    bool m_MatchCreated;
    bool m_MatchJoined;
    UnityEngine.Networking.Match.MatchInfo m_MatchInfo;
    string m_MatchName = "NewRoom";
    NetworkMatch m_NetworkMatch;

    // Connection/communication related
    int m_HostId = -1;
    // On the server there will be multiple connections, on the client this will only contain one ID
    List<int> m_ConnectionIds = new List<int>();
    byte[] m_ReceiveBuffer;
    string m_NetworkMessage = "Hello world";
    string m_LastReceivedMessage = "";
    NetworkWriter m_Writer;
    NetworkReader m_Reader;
    bool m_ConnectionEstablished;

    const int k_ServerPort = 25000;
    const int k_MaxMessageSize = 65535;

    void Awake()
    {
        m_NetworkMatch = gameObject.AddComponent<NetworkMatch>();
    }

    void Start()
    {
        m_ReceiveBuffer = new byte[k_MaxMessageSize];
        m_Writer = new NetworkWriter();
        // While testing with multiple standalone players on one machine this will need to be enabled
        Application.runInBackground = true;
    }

    void OnApplicationQuit()
    {
        NetworkTransport.Shutdown();
    }

    void OnGUI()
    {
        if (string.IsNullOrEmpty(Application.cloudProjectId))
            GUILayout.Label("You must set up the project first. See the Multiplayer tab in the Service Window");
        else
            GUILayout.Label("Cloud Project ID: " + Application.cloudProjectId);

        if (m_MatchJoined)
            GUILayout.Label("Match joined '" + m_MatchName + "' on Matchmaker server");
        else if (m_MatchCreated)
            GUILayout.Label("Match '" + m_MatchName + "' created on Matchmaker server");

        GUILayout.Label("Connection Established: " + m_ConnectionEstablished);

        if (m_MatchCreated || m_MatchJoined)
        {
            GUILayout.Label("Relay Server: " + m_MatchInfo.address + ":" + m_MatchInfo.port);
            GUILayout.Label("NetworkID: " + m_MatchInfo.networkId + " NodeID: " + m_MatchInfo.nodeId);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Outgoing message:");
            m_NetworkMessage = GUILayout.TextField(m_NetworkMessage);
            GUILayout.EndHorizontal();
            GUILayout.Label("Last incoming message: " + m_LastReceivedMessage);

            if (m_ConnectionEstablished && GUILayout.Button("Send message"))
            {
                m_Writer.SeekZero();
                m_Writer.Write(m_NetworkMessage);
                byte error;
                for (int i = 0; i < m_ConnectionIds.Count; ++i)
                {
                    NetworkTransport.Send(m_HostId,
                        m_ConnectionIds[i], 0, m_Writer.AsArray(), m_Writer.Position, out error);
                    if ((NetworkError)error != NetworkError.Ok)
                        Debug.LogError("Failed to send message: " + (NetworkError)error);
                }
            }

            if (GUILayout.Button("Shutdown"))
            {
                m_NetworkMatch.DropConnection(m_MatchInfo.networkId,
                    m_MatchInfo.nodeId, 0, OnConnectionDropped);
            }
        }
        else
        {
            if (GUILayout.Button("Create Room"))
            {
                m_NetworkMatch.CreateMatch(m_MatchName, 4, true, "", "", "", 0, 0, OnMatchCreate);
            }

            if (GUILayout.Button("Join first found match"))
            {
                m_NetworkMatch.ListMatches(0, 1, "", true, 0, 0, (success, info, matches) =>
                {
                    if (success && matches.Count > 0)
                        m_NetworkMatch.JoinMatch(matches[0].networkId, "", "", "", 0, 0, OnMatchJoined);
                });
            }

            if (GUILayout.Button("List rooms"))
            {
                m_NetworkMatch.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
            }

            if (m_MatchList.Count > 0)
            {
                GUILayout.Label("Current rooms:");
            }
            foreach (var match in m_MatchList)
            {
                if (GUILayout.Button(match.name))
                {
                    m_NetworkMatch.JoinMatch(match.networkId, "", "", "", 0, 0, OnMatchJoined);
                }
            }
        }
    }

    public void OnConnectionDropped(bool success, string extendedInfo)
    {
        Debug.Log("Connection has been dropped on matchmaker server");
        NetworkTransport.Shutdown();
        m_HostId = -1;
        m_ConnectionIds.Clear();
        m_MatchInfo = null;
        m_MatchCreated = false;
        m_MatchJoined = false;
        m_ConnectionEstablished = false;
    }

    public virtual void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            Debug.Log("Create match succeeded");
            Utility.SetAccessTokenForNetwork(matchInfo.networkId, matchInfo.accessToken);

            m_MatchCreated = true;
            m_MatchInfo = matchInfo;

            StartServer(matchInfo.address, matchInfo.port, matchInfo.networkId,
                matchInfo.nodeId);
        }`
        else
        {
            Debug.LogError("Create match failed: " + extendedInfo);
        }
    }

    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        if (success && matches != null)
        {
            m_MatchList = matches;
        }
        else if (!success)
        {
            Debug.LogError("List match failed: " + extendedInfo);
        }
    }

    // When we've joined a match we connect to the server/host
    public virtual void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            Debug.Log("Join match succeeded");
            Utility.SetAccessTokenForNetwork(matchInfo.networkId, matchInfo.accessToken);

            m_MatchJoined = true;
            m_MatchInfo = matchInfo;

            Debug.Log("Connecting to Address:" + matchInfo.address +
                " Port:" + matchInfo.port +
                " NetworKID: " + matchInfo.networkId +
                " NodeID: " + matchInfo.nodeId);
            ConnectThroughRelay(matchInfo.address, matchInfo.port, matchInfo.networkId,
                matchInfo.nodeId);
        }
        else
        {
            Debug.LogError("Join match failed: " + extendedInfo);
        }
    }

    void SetupHost(bool isServer)
    {
        Debug.Log("Initializing network transport");
        NetworkTransport.Init();
        var config = new ConnectionConfig();
        config.AddChannel(QosType.Reliable);
        config.AddChannel(QosType.Unreliable);
        var topology = new HostTopology(config, 4);
        if (isServer)
            m_HostId = NetworkTransport.AddHost(topology, k_ServerPort);
        else
            m_HostId = NetworkTransport.AddHost(topology);
    }

    void StartServer(string relayIp, int relayPort, NetworkID networkId, NodeID nodeId)
    {
        SetupHost(true);

        byte error;
        NetworkTransport.ConnectAsNetworkHost(
            m_HostId, relayIp, relayPort, networkId, Utility.GetSourceID(), nodeId, out error);
    }

    void ConnectThroughRelay(string relayIp, int relayPort, NetworkID networkId, NodeID nodeId)
    {
        SetupHost(false);

        byte error;
        NetworkTransport.ConnectToNetworkPeer(
            m_HostId, relayIp, relayPort, 0, 0, networkId, Utility.GetSourceID(), nodeId, out error);
    }

    void Update()
    {
        if (m_HostId == -1)
            return;

        var networkEvent = NetworkEventType.Nothing;
        int connectionId;
        int channelId;
        int receivedSize;
        byte error;

        // Get events from the relay connection
        networkEvent = NetworkTransport.ReceiveRelayEventFromHost(m_HostId, out error);
        if (networkEvent == NetworkEventType.ConnectEvent)
            Debug.Log("Relay server connected");
        if (networkEvent == NetworkEventType.DisconnectEvent)
            Debug.Log("Relay server disconnected");

        do
        {
            // Get events from the server/client game connection
            networkEvent = NetworkTransport.ReceiveFromHost(m_HostId, out connectionId, out channelId,
                m_ReceiveBuffer, (int)m_ReceiveBuffer.Length, out receivedSize, out error);
            if ((NetworkError)error != NetworkError.Ok)
            {
                Debug.LogError("Error while receiveing network message: " + (NetworkError)error);
            }

            switch (networkEvent)
            {
                case NetworkEventType.ConnectEvent:
                    {
                        Debug.Log("Connected through relay, ConnectionID:" + connectionId +
                            " ChannelID:" + channelId);
                        m_ConnectionEstablished = true;
                        m_ConnectionIds.Add(connectionId);
                        break;
                    }
                case NetworkEventType.DataEvent:
                    {
                        Debug.Log("Data event, ConnectionID:" + connectionId +
                            " ChannelID: " + channelId +
                            " Received Size: " + receivedSize);
                        m_Reader = new NetworkReader(m_ReceiveBuffer);
                        m_LastReceivedMessage = m_Reader.ReadString();
                        break;
                    }
                case NetworkEventType.DisconnectEvent:
                    {
                        Debug.Log("Connection disconnected, ConnectionID:" + connectionId);
                        break;
                    }
                case NetworkEventType.Nothing:
                    break;
            }
        } while (networkEvent != NetworkEventType.Nothing);
    }
}

 #if ENABLE_UNET

namespace UnityEngine.Networking
{
    public struct NetworkBroadcastResult
    {
        public string serverAddress;
        public byte[] broadcastData;
    }

    [DisallowMultipleComponent]
    [AddComponentMenu("Network/NetworkDiscovery")]
    public class NetworkDiscovery : MonoBehaviour
    {
        const int k_MaxBroadcastMsgSize = 1024;

        // config data
        [SerializeField]
        int m_BroadcastPort = 47777;

        [SerializeField]
        int m_BroadcastKey = 2222;

        [SerializeField]
        int m_BroadcastVersion = 1;

        [SerializeField]
        int m_BroadcastSubVersion = 1;

        [SerializeField]
        int m_BroadcastInterval = 1000;

        [SerializeField]
        bool m_UseNetworkManager = true;

        [SerializeField]
        string m_BroadcastData = "HELLO";

        [SerializeField]
        bool m_ShowGUI = true;

        [SerializeField]
        int m_OffsetX;

        [SerializeField]
        int m_OffsetY;

        // runtime data
        int m_HostId = -1;
        bool m_Running;

        bool m_IsServer;
        bool m_IsClient;

        byte[] m_MsgOutBuffer;
        byte[] m_MsgInBuffer;
        HostTopology m_DefaultTopology;
        Dictionary<string, NetworkBroadcastResult> m_BroadcastsReceived;

        public int broadcastPort
        {
            get { return m_BroadcastPort; }
            set { m_BroadcastPort = value; }
        }

        public int broadcastKey
        {
            get { return m_BroadcastKey; }
            set { m_BroadcastKey = value; }
        }

        public int broadcastVersion
        {
            get { return m_BroadcastVersion; }
            set { m_BroadcastVersion = value; }
        }

        public int broadcastSubVersion
        {
            get { return m_BroadcastSubVersion; }
            set { m_BroadcastSubVersion = value; }
        }

        public int broadcastInterval
        {
            get { return m_BroadcastInterval; }
            set { m_BroadcastInterval = value; }
        }

        public bool useNetworkManager
        {
            get { return m_UseNetworkManager; }
            set { m_UseNetworkManager = value; }
        }

        public string broadcastData
        {
            get { return m_BroadcastData; }
            set
            {
                m_BroadcastData = value;
                m_MsgOutBuffer = StringToBytes(m_BroadcastData);
                if (m_UseNetworkManager)
                {
                    if (LogFilter.logWarn) { Debug.LogWarning("NetworkDiscovery broadcast data changed while using NetworkManager. This can prevent clients from finding the server. The format of the broadcast data must be 'NetworkManager:IPAddress:Port'."); }
                }
            }
        }

        public bool showGUI
        {
            get { return m_ShowGUI; }
            set { m_ShowGUI = value; }
        }

        public int offsetX
        {
            get { return m_OffsetX; }
            set { m_OffsetX = value; }
        }

        public int offsetY
        {
            get { return m_OffsetY; }
            set { m_OffsetY = value; }
        }

        public int hostId
        {
            get { return m_HostId; }
            set { m_HostId = value; }
        }

        public bool running
        {
            get { return m_Running; }
            set { m_Running = value; }
        }

        public bool isServer
        {
            get { return m_IsServer; }
            set { m_IsServer = value; }
        }

        public bool isClient
        {
            get { return m_IsClient; }
            set { m_IsClient = value; }
        }

        public Dictionary<string, NetworkBroadcastResult> broadcastsReceived
        {
            get { return m_BroadcastsReceived; }
        }

        static byte[] StringToBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static string BytesToString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public bool Initialize()
        {
            if (m_BroadcastData.Length >= k_MaxBroadcastMsgSize)
            {
                if (LogFilter.logError) { Debug.LogError("NetworkDiscovery Initialize - data too large. max is " + k_MaxBroadcastMsgSize); }
                return false;
            }

            if (!NetworkTransport.IsStarted)
            {
                NetworkTransport.Init();
            }

            if (m_UseNetworkManager && NetworkManager.singleton != null)
            {
                m_BroadcastData = "NetworkManager:" + NetworkManager.singleton.networkAddress + ":" + NetworkManager.singleton.networkPort;
                if (LogFilter.logInfo) { Debug.Log("NetwrokDiscovery set broadbast data to:" + m_BroadcastData); }
            }

            m_MsgOutBuffer = StringToBytes(m_BroadcastData);
            m_MsgInBuffer = new byte[k_MaxBroadcastMsgSize];
            m_BroadcastsReceived = new Dictionary<string, NetworkBroadcastResult>();

            ConnectionConfig cc = new ConnectionConfig();
            cc.AddChannel(QosType.Unreliable);
            m_DefaultTopology = new HostTopology(cc, 1);

            if (m_IsServer)
                StartAsServer();

            if (m_IsClient)
                StartAsClient();

            return true;
        }

        // listen for broadcasts
        public bool StartAsClient()
        {
            if (m_HostId != -1 || m_Running)
            {
                if (LogFilter.logWarn) { Debug.LogWarning("NetworkDiscovery StartAsClient already started"); }
                return false;
            }

            m_HostId = NetworkTransport.AddHost(m_DefaultTopology, m_BroadcastPort);
            if (m_HostId == -1)
            {
                if (LogFilter.logError) { Debug.LogError("NetworkDiscovery StartAsClient - addHost failed"); }
                return false;
            }

            byte error;
            NetworkTransport.SetBroadcastCredentials(m_HostId, m_BroadcastKey, m_BroadcastVersion, m_BroadcastSubVersion, out error);

            m_Running = true;
            m_IsClient = true;
            if (LogFilter.logDebug) { Debug.Log("StartAsClient Discovery listening"); }
            return true;
        }

        // perform actual broadcasts
        public bool StartAsServer()
        {
            if (m_HostId != -1 || m_Running)
            {
                if (LogFilter.logWarn) { Debug.LogWarning("NetworkDiscovery StartAsServer already started"); }
                return false;
            }

            m_HostId = NetworkTransport.AddHost(m_DefaultTopology, 0);
            if (m_HostId == -1)
            {
                if (LogFilter.logError) { Debug.LogError("NetworkDiscovery StartAsServer - addHost failed"); }
                return false;
            }

            byte err;
            if (!NetworkTransport.StartBroadcastDiscovery(m_HostId, m_BroadcastPort, m_BroadcastKey, m_BroadcastVersion, m_BroadcastSubVersion, m_MsgOutBuffer, m_MsgOutBuffer.Length, m_BroadcastInterval, out err))
            {
                if (LogFilter.logError) { Debug.LogError("NetworkDiscovery StartBroadcast failed err: " + err); }
                return false;
            }

            m_Running = true;
            m_IsServer = true;
            if (LogFilter.logDebug) { Debug.Log("StartAsServer Discovery broadcasting"); }
            DontDestroyOnLoad(gameObject);
            return true;
        }

        public void StopBroadcast()
        {
            if (m_HostId == -1)
            {
                if (LogFilter.logError) { Debug.LogError("NetworkDiscovery StopBroadcast not initialized"); }
                return;
            }

            if (!m_Running)
            {
                Debug.LogWarning("NetworkDiscovery StopBroadcast not started");
                return;
            }
            if (m_IsServer)
            {
                NetworkTransport.StopBroadcastDiscovery();
            }

            NetworkTransport.RemoveHost(m_HostId);
            m_HostId = -1;
            m_Running = false;
            m_IsServer = false;
            m_IsClient = false;
            m_MsgInBuffer = null;
            m_BroadcastsReceived = null;
            if (LogFilter.logDebug) { Debug.Log("Stopped Discovery broadcasting"); }
        }

        void Update()
        {
            if (m_HostId == -1)
                return;

            if (m_IsServer)
                return;

            NetworkEventType networkEvent;
            do
            {
                int connectionId;
                int channelId;
                int receivedSize;
                byte error;
                networkEvent = NetworkTransport.ReceiveFromHost(m_HostId, out connectionId, out channelId, m_MsgInBuffer, k_MaxBroadcastMsgSize, out receivedSize, out error);

                if (networkEvent == NetworkEventType.BroadcastEvent)
                {
                    NetworkTransport.GetBroadcastConnectionMessage(m_HostId, m_MsgInBuffer, k_MaxBroadcastMsgSize, out receivedSize, out error);

                    string senderAddr;
                    int senderPort;
                    NetworkTransport.GetBroadcastConnectionInfo(m_HostId, out senderAddr, out senderPort, out error);

                    var recv = new NetworkBroadcastResult();
                    recv.serverAddress = senderAddr;
                    recv.broadcastData = new byte[receivedSize];
                    Buffer.BlockCopy(m_MsgInBuffer, 0, recv.broadcastData, 0, receivedSize);
                    m_BroadcastsReceived[senderAddr] = recv;

                    OnReceivedBroadcast(senderAddr, BytesToString(m_MsgInBuffer));
                }
            }
            while (networkEvent != NetworkEventType.Nothing);
        }

        void OnDestroy()
        {
            if (m_IsServer && m_Running && m_HostId != -1)
            {
                NetworkTransport.StopBroadcastDiscovery();
                NetworkTransport.RemoveHost(m_HostId);
            }

            if (m_IsClient && m_Running && m_HostId != -1)
            {
                NetworkTransport.RemoveHost(m_HostId);
            }
        }

        public virtual void OnReceivedBroadcast(string fromAddress, string data)
        {
            //Debug.Log("Got broadcast from [" + fromAddress + "] " + data);
        }

        void OnGUI()
        {
            if (!m_ShowGUI)
                return;

            int xpos = 10 + m_OffsetX;
            int ypos = 40 + m_OffsetY;
            const int spacing = 24;

            if (UnityEngine.Application.platform == RuntimePlatform.WebGLPlayer)
            {
                GUI.Box(new Rect(xpos, ypos, 200, 20), "( WebGL cannot broadcast )");
                return;
            }

            if (m_MsgInBuffer == null)
            {
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Initialize Broadcast"))
                {
                    Initialize();
                }
                return;
            }
            string suffix = "";
            if (m_IsServer)
                suffix = " (server)";
            if (m_IsClient)
                suffix = " (client)";

            GUI.Label(new Rect(xpos, ypos, 200, 20), "initialized" + suffix);
            ypos += spacing;

            if (m_Running)
            {
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Stop"))
                {
                    StopBroadcast();
                }
                ypos += spacing;

                if (m_BroadcastsReceived != null)
                {
                    foreach (var addr in m_BroadcastsReceived.Keys)
                    {
                        var value = m_BroadcastsReceived[addr];
                        if (GUI.Button(new Rect(xpos, ypos + 20, 200, 20), "Game at " + addr) && m_UseNetworkManager)
                        {
                            string dataString = BytesToString(value.broadcastData);
                            var items = dataString.Split(':');
                            if (items.Length == 3 && items[0] == "NetworkManager")
                            {
                                if (NetworkManager.singleton != null && NetworkManager.singleton.client == null)
                                {
                                    NetworkManager.singleton.networkAddress = items[1];
                                    NetworkManager.singleton.networkPort = Convert.ToInt32(items[2]);
                                    NetworkManager.singleton.StartClient();
                                }
                            }
                        }
                        ypos += spacing;
                    }
                }
            }
            else
            {
                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Start Broadcasting"))
                {
                    StartAsServer();
                }
                ypos += spacing;

                if (GUI.Button(new Rect(xpos, ypos, 200, 20), "Listen for Broadcast"))
                {
                    StartAsClient();
                }
                ypos += spacing;
            }
        }
    }
}
#endif
public class DirectSetup : MonoBehaviour
{
    // Matchmaker related
    List<MatchInfoSnapshot> m_MatchList = new List<MatchInfoSnapshot>();
    bool m_MatchCreated;
    bool m_MatchJoined;
    MatchInfo m_MatchInfo;
    string m_MatchName = "NewRoom";
    NetworkMatch m_NetworkMatch;

    // Connection/communication related
    int m_HostId = -1;
    // On the server there will be multiple connections, on the client this will only contain one ID
    List<int> m_ConnectionIds = new List<int>();
    byte[] m_ReceiveBuffer;
    string m_NetworkMessage = "Hello world";
    string m_LastReceivedMessage = "";
    NetworkWriter m_Writer;
    NetworkReader m_Reader;
    bool m_ConnectionEstablished;

    const int k_ServerPort = 25000;
    const int k_MaxMessageSize = 65535;

    void Awake()
    {
        m_NetworkMatch = gameObject.AddComponent<NetworkMatch>();
    }

    void Start()
    {
        m_ReceiveBuffer = new byte[k_MaxMessageSize];
        m_Writer = new NetworkWriter();
        // While testing with multiple standalone players on one machine this will need to be enabled
        Application.runInBackground = true;
    }

    void OnApplicationQuit()
    {
        NetworkTransport.Shutdown();
    }

    void OnGUI()
    {
        if (string.IsNullOrEmpty(Application.cloudProjectId))
            GUILayout.Label("You must set up the project first. See the Multiplayer tab in the Service Window");
        else
            GUILayout.Label("Cloud Project ID: " + Application.cloudProjectId);

        if (m_MatchJoined)
            GUILayout.Label("Match joined '" + m_MatchName + "' on Matchmaker server");
        else if (m_MatchCreated)
            GUILayout.Label("Match '" + m_MatchName + "' created on Matchmaker server");

        GUILayout.Label("Connection Established: " + m_ConnectionEstablished);

        if (m_MatchCreated || m_MatchJoined)
        {
            GUILayout.Label("Relay Server: " + m_MatchInfo.address + ":" + m_MatchInfo.port);
            GUILayout.Label("NetworkID: " + m_MatchInfo.networkId + " NodeID: " + m_MatchInfo.nodeId);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Outgoing message:");
            m_NetworkMessage = GUILayout.TextField(m_NetworkMessage);
            GUILayout.EndHorizontal();
            GUILayout.Label("Last incoming message: " + m_LastReceivedMessage);

            if (m_ConnectionEstablished && GUILayout.Button("Send message"))
            {
                m_Writer.SeekZero();
                m_Writer.Write(m_NetworkMessage);
                byte error;
                for (int i = 0; i < m_ConnectionIds.Count; ++i)
                {
                    NetworkTransport.Send(m_HostId,
                        m_ConnectionIds[i], 0, m_Writer.AsArray(), m_Writer.Position, out error);
                    if ((NetworkError)error != NetworkError.Ok)
                        Debug.LogError("Failed to send message: " + (NetworkError)error);
                }
            }

            if (GUILayout.Button("Shutdown"))
            {
                m_NetworkMatch.DropConnection(m_MatchInfo.networkId,
                    m_MatchInfo.nodeId, 0, OnConnectionDropped);
            }
        }
        else
        {
            if (GUILayout.Button("Create Room"))
            {
                m_NetworkMatch.CreateMatch(m_MatchName, 4, true, "", "", "", 0, 0, OnMatchCreate);
            }

            if (GUILayout.Button("Join first found match"))
            {
                m_NetworkMatch.ListMatches(0, 1, "", true, 0, 0, (success, info, matches) =>
                {
                    if (success && matches.Count > 0)
                        m_NetworkMatch.JoinMatch(matches[0].networkId, "", "", "", 0, 0, OnMatchJoined);
                });
            }

            if (GUILayout.Button("List rooms"))
            {
                m_NetworkMatch.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
            }

            if (m_MatchList.Count > 0)
            {
                GUILayout.Label("Current rooms:");
            }
            foreach (var match in m_MatchList)
            {
                if (GUILayout.Button(match.name))
                {
                    m_NetworkMatch.JoinMatch(match.networkId, "", "", "", 0, 0, OnMatchJoined);
                }
            }
        }
    }

    public void OnConnectionDropped(bool success, string extendedInfo)
    {
        Debug.Log("Connection has been dropped on matchmaker server");
        NetworkTransport.Shutdown();
        m_HostId = -1;
        m_ConnectionIds.Clear();
        m_MatchInfo = null;
        m_MatchCreated = false;
        m_MatchJoined = false;
        m_ConnectionEstablished = false;
    }

    public virtual void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            Debug.Log("Create match succeeded");
            Utility.SetAccessTokenForNetwork(matchInfo.networkId, matchInfo.accessToken);

            m_MatchCreated = true;
            m_MatchInfo = matchInfo;

            StartServer(matchInfo.address, matchInfo.port, matchInfo.networkId,
                matchInfo.nodeId);
        }
        else
        {
            Debug.LogError("Create match failed: " + extendedInfo);
        }
    }

    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        if (success && matches != null)
        {
            m_MatchList = matches;
        }
        else if (!success)
        {
            Debug.LogError("List match failed: " + extendedInfo);
        }
    }

    // When we've joined a match we connect to the server/host
    public virtual void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            Debug.Log("Join match succeeded");
            Utility.SetAccessTokenForNetwork(matchInfo.networkId, matchInfo.accessToken);

            m_MatchJoined = true;
            m_MatchInfo = matchInfo;

            Debug.Log("Connecting to Address:" + matchInfo.address +
                " Port:" + matchInfo.port +
                " NetworKID: " + matchInfo.networkId +
                " NodeID: " + matchInfo.nodeId);
            ConnectThroughRelay(matchInfo.address, matchInfo.port, matchInfo.networkId,
                matchInfo.nodeId);
        }
        else
        {
            Debug.LogError("Join match failed: " + extendedInfo);
        }
    }

    void SetupHost(bool isServer)
    {
        Debug.Log("Initializing network transport");
        NetworkTransport.Init();
        var config = new ConnectionConfig();
        config.AddChannel(QosType.Reliable);
        config.AddChannel(QosType.Unreliable);
        var topology = new HostTopology(config, 4);
        if (isServer)
            m_HostId = NetworkTransport.AddHost(topology, k_ServerPort);
        else
            m_HostId = NetworkTransport.AddHost(topology);
    }

    void StartServer(string relayIp, int relayPort, NetworkID networkId, NodeID nodeId)
    {
        SetupHost(true);

        byte error;
        NetworkTransport.ConnectAsNetworkHost(
            m_HostId, relayIp, relayPort, networkId, Utility.GetSourceID(), nodeId, out error);
    }

    void ConnectThroughRelay(string relayIp, int relayPort, NetworkID networkId, NodeID nodeId)
    {
        SetupHost(false);

        byte error;
        NetworkTransport.ConnectToNetworkPeer(
            m_HostId, relayIp, relayPort, 0, 0, networkId, Utility.GetSourceID(), nodeId, out error);
    }

    void Update()
    {
        if (m_HostId == -1)
            return;

        var networkEvent = NetworkEventType.Nothing;
        int connectionId;
        int channelId;
        int receivedSize;
        byte error;

        // Get events from the relay connection
        networkEvent = NetworkTransport.ReceiveRelayEventFromHost(m_HostId, out error);
        if (networkEvent == NetworkEventType.ConnectEvent)
            Debug.Log("Relay server connected");
        if (networkEvent == NetworkEventType.DisconnectEvent)
            Debug.Log("Relay server disconnected");

        do
        {
            // Get events from the server/client game connection
            networkEvent = NetworkTransport.ReceiveFromHost(m_HostId, out connectionId, out channelId,
                m_ReceiveBuffer, (int)m_ReceiveBuffer.Length, out receivedSize, out error);
            if ((NetworkError)error != NetworkError.Ok)
            {
                Debug.LogError("Error while receiveing network message: " + (NetworkError)error);
            }

            switch (networkEvent)
            {
                case NetworkEventType.ConnectEvent:
                    {
                        Debug.Log("Connected through relay, ConnectionID:" + connectionId +
                            " ChannelID:" + channelId);
                        m_ConnectionEstablished = true;
                        m_ConnectionIds.Add(connectionId);
                        break;
                    }
                case NetworkEventType.DataEvent:
                    {
                        Debug.Log("Data event, ConnectionID:" + connectionId +
                            " ChannelID: " + channelId +
                            " Received Size: " + receivedSize);
                        m_Reader = new NetworkReader(m_ReceiveBuffer);
                        m_LastReceivedMessage = m_Reader.ReadString();
                        break;
                    }
                case NetworkEventType.DisconnectEvent:
                    {
                        Debug.Log("Connection disconnected, ConnectionID:" + connectionId);
                        break;
                    }
                case NetworkEventType.Nothing:
                    break;
            }
        } while (networkEvent != NetworkEventType.Nothing);
    }
}
*/