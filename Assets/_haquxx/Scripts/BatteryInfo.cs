using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryInfo : MonoBehaviour
{
    [SerializeField]
    private Text _batteryLevelText;
    [SerializeField]
    private Text _batteryStatusText;

    private void Update()
    {
        var level = SystemInfo.batteryLevel;
        var status = SystemInfo.batteryStatus;

        if(_batteryLevelText != null)
        {
            _batteryLevelText.text = (level * 100f).ToString() + "%";
        }

        if(_batteryStatusText != null)
        {
            _batteryStatusText.text = status.ToString();
        }
    }
}
