using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketChecker : MonoBehaviour
{
    public void ReceivePackets(int value)
    {
        Debug.LogFormat("Receive packets : {0}", value);
    }
}
