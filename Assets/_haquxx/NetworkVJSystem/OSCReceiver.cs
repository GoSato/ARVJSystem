using OscJack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public enum CastType
{
    UniCast,
    MultiCast
}

public class OSCData
{
    public string Address;
    public float Value;

    public OSCData(string address, float value)
    {
        this.Address = address;
        this.Value = value;
    }
}

public class OSCReceiver : MonoBehaviour
{
    [SerializeField]
    private int _port;
    [SerializeField]
    private string _oscAddress;
    [SerializeField]
    private string _multiCastAddress;
    [SerializeField]
    private CastType _type = CastType.UniCast;
    [SerializeField]
    private Text _text;

    private String _debugText;
    private Queue<OSCData> _queue;

    public Action<OSCData> OnUpdate;

    private void OnEnable()
    {
        _queue = new Queue<OSCData>();

        var server = OscMaster.GetSharedServer(_port);
        switch (_type)
        {
            case CastType.UniCast:
                break;
            case CastType.MultiCast:
                server.JoinMulticastGroup(_multiCastAddress);
                break;
        }
        server.MessageDispatcher.AddCallback(_oscAddress, OnDataReceive);
    }

    private void OnDisable()
    {
        var server = OscMaster.GetSharedServer(_port);
        server.MessageDispatcher.RemoveCallback(_oscAddress, OnDataReceive);
    }

    private void Update()
    {
        while (_queue.Count > 0)
        {
            UpdateLog();
        }
    }

    private void OnDataReceive(string address, OscDataHandle data)
    {
        lock (_queue)
            _queue.Enqueue(new OSCData(address, data.GetElementAsFloat(0)));
    }

    private void UpdateLog()
    {
        OSCData data;

        lock (_queue)
            data = _queue.Dequeue();

        if (OnUpdate != null)
        {
            OnUpdate.Invoke(data);
        }

        _debugText = String.Format("{0}:{1} : {2} : {3}", DateTime.Now, DateTime.Now.Millisecond, data.Address, data.Value);
        _text.text = _debugText;
        switch(data.Address)
        {
            case "/1/recenable":
                _text.color = Color.red;
                break;
            case "/1/input":
                _text.color = Color.green;
                break;
            case "/1/solo":
                _text.color = Color.blue;
                break;
        }
    }
}
