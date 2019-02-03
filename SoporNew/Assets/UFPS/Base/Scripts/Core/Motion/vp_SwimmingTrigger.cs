/////////////////////////////////////////////////////////////////////////////////
//
//	vp_SwimmingTrigger.cs
//	© VisionPunk. All Rights Reserved.
//	https://twitter.com/VisionPunk
//	http://www.visionpunk.com
//
//	description:	A very simple implementation demonstrating how to use the free
//					fly mode to make a player start swimming when the player enters
//					the trigger. This should be placed on an object that has a
//					collider with trigger enabled
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class vp_SwimmingTrigger : MonoBehaviour
{

    public string StateName = "Swimming";

    protected static string m_IsPlayerKey = "Is Player";	// key for checking if the collider is the player
    protected static string m_PlayerKey = "Player";			// key for the vp_FPPlayerEventHandler

    private LayerMask LayerMask = 1 << vp_Layer.LocalPlayer;	// layer mask to check for


    protected virtual void Start()
    {

        GetComponent<Collider>().isTrigger = true;

    }


    /// <summary>
    /// Check if this object is in the layer mask
    /// and if so, start swimming
    /// </summary>
    protected virtual void OnTriggerEnter(Collider col)
    {
        Dictionary<string, object> data = GetDataForCollider(col);
        if (!((bool)data[m_IsPlayerKey]))
            return;

        vp_FPPlayerEventHandler player = (vp_FPPlayerEventHandler)data[m_PlayerKey];
        player.SetState(StateName);
        player.MotorThrottle.Set(Vector3.zero);
        player.Jump.TryStop();
        Vector3 softForce = new Vector3(0, player.Velocity.Get().normalized.y * .25f, 0);
        player.Stop.Send();
        player.GetComponent<vp_FPController>().AddSoftForce(softForce, 10);

    }


    /// <summary>
    /// Check if this object is in the layer mask
    /// and if so, stop swimming
    /// </summary>
    protected virtual void OnTriggerExit(Collider col)
    {

        Dictionary<string, object> data = GetDataForCollider(col);
        if (!((bool)data[m_IsPlayerKey]))
            return;

        vp_FPPlayerEventHandler player = (vp_FPPlayerEventHandler)data[m_PlayerKey];
        player.SetState(StateName, false);

    }


    /// <summary>
    /// This method just does some checking to see if the object is in the mask
    /// and if so if it has a vp_FPPlayerEventHandler component and then returns
    /// the data
    /// </summary>
    protected virtual Dictionary<string, object> GetDataForCollider(Collider col)
    {

        // check if collider is in layermask
        if ((LayerMask.value & 1 << col.gameObject.layer) == 0)
            return new Dictionary<string, object>() { { m_IsPlayerKey, false } };

        // check if player component exists
        vp_FPPlayerEventHandler player = col.gameObject.GetComponent<vp_FPPlayerEventHandler>();
        if (player == null)
            return new Dictionary<string, object>() { { m_IsPlayerKey, false } }; ;

        // send data. includes player.
        return new Dictionary<string, object>() { { m_IsPlayerKey, true }, { m_PlayerKey, player } };

    }

}