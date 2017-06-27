using UnityEngine;

public  class IUIHandler : MonoBehaviour
{
    protected UDPServer udpServer;
   public recData receiveData;
    public virtual void setUdpServer(UDPServer server) {
        this.udpServer = server;
    }
  
    public virtual void processMessage(string message) { }
    public virtual void processMessage(byte[] message) {
        if (receiveData.GetInvocationList().Length != 0)
            receiveData(message, null);
    }
    public virtual void processMessage(byte[] data, System.Net.IPEndPoint address) {
    
        if (receiveData.GetInvocationList().Length != 0)
            receiveData(data, address);
    }
}
public delegate void recData(byte[] data, System.Net.IPEndPoint address);
