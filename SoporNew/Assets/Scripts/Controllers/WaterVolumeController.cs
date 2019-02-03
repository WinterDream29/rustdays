using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class WaterVolumeController : MonoBehaviour 
    {
        void OnTriggerEnter(Collider otherCollider)
        {
            var player = otherCollider.GetComponent<PlayerController>();
            if (player != null)
            {
                //player.GetComponentInChildren<PlayerHeadObject>().InWaterMode(true);
            }

            //var playerHeadObj = otherCollider.GetComponent<PlayerHeadObject>();
            //if (playerHeadObj != null)
            //{
            //    //WaterMesh.enabled = false;
            //    playerHeadObj.UnderWater(true);
            //}
        }

        void OnTriggerExit(Collider otherCollider)
        {
            //var playerVC = otherCollider.GetComponent<PlayerViewController>();
            //if (playerVC != null)
            //{
            //    playerVC.GetComponentInChildren<PlayerHeadObject>().InWaterMode(false);
            //}

            //var playerHeadObj = otherCollider.GetComponent<PlayerHeadObject>();
            //if (playerHeadObj != null)
            //{
            //    // WaterMesh.enabled = true;
            //    playerHeadObj.UnderWater(false);
            //}
        }
    }
}
