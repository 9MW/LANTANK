using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class UIHandler : IUIHandler
{
    INetworkHandler ultimatelyClient;

    private void Start()
    {
        ultimatelyClient = GetComponent<INetworkHandler>();
    }

    public override void processMessage(string message)
    {
        throw new NotImplementedException();
    }
    public override void processMessage(byte[] message)
    {
        ultimatelyClient.processMessage(message);
    }
    public override void processMessage(byte[] data, IPEndPoint address)
    {
        base.processMessage(data, address);
    }

}
