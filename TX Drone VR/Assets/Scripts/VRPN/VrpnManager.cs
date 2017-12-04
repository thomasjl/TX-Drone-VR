using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VrpnManager : MonoBehaviour {

    public string address;  
    public int channel;

    private bool isConnected;

    void Start()
    {
        isConnected = false;
    }


    public void vrpnConnection()
    {
        if (VRPN.vrpnButton(address, channel))
        {
            isConnected = true;
            Debug.Log("VRPN connected");
        }
        else
        {
            Debug.LogError("Can't connect to VRPN");
        }

    }

    public Vector3 getDronePosition()
    {
        return VRPN.vrpnTrackerPos(address, channel);         
    }

    public Quaternion getDroneRotation()
    {
        return VRPN.vrpnTrackerQuat(address, channel);         
    }

    void Update()
    {
        if (isConnected )
        {
            Vector3 dronePos = getDronePosition();
            Quaternion droneRot = getDroneRotation();

            Debug.Log(dronePos.x + " " + dronePos.y + " " + dronePos.z + " - " + droneRot.w + " " + droneRot.x + " " + droneRot.y + " " + droneRot.z);
        }

    }

}
