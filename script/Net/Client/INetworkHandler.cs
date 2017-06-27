public interface INetworkHandler
{
     void processMessage(byte[] message);
     void processMessage(string message);
     void processMessage(byte[] text, System.Net.IPEndPoint ip);
}