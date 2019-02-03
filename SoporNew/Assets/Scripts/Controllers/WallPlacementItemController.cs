using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class WallPlacementItemController : GroundPlacementItemController
    {
        public Vector3 DefaultRotation;
        protected override void UpdatePosition()
        {
            Vector3 pos = transform.position;

            var hitRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hitInfo;

            if(Physics.Raycast(hitRay, out hitInfo, 5.0f))
            {
                if (hitInfo.collider != null &&
                    (hitInfo.collider.gameObject.tag == "wall" || hitInfo.collider.gameObject.tag == "wallOut"))
                {
                    transform.position = hitInfo.collider.gameObject.transform.position;
                    transform.rotation = hitInfo.collider.gameObject.transform.rotation;

                    if (CheckCanPlace)
                    {
                        if (!CanPlace)
                        {
                            CanPlace = true;
                            foreach (var material in Materials)
                                material.SetColor("_Color", CanPlaceColor);
                        }
                    }
                }
                else
                {
                    pos.y = GameManager.CurrentTerain.SampleHeight(transform.position) + ObjectHeight;
                    transform.position = new Vector3(transform.parent.position.x, pos.y, transform.parent.position.z);
                    if (CheckCanPlace)
                    {
                        if (CanPlace)
                        {
                            CanPlace = false;
                            transform.localEulerAngles = DefaultRotation;
                            foreach (var material in Materials)
                                material.SetColor("_Color", CantPlaceColor);
                        }
                    }
                }
            }
            else
            {
                pos.y = GameManager.CurrentTerain.SampleHeight(transform.position) + ObjectHeight;
                transform.position = new Vector3(transform.parent.position.x, pos.y, transform.parent.position.z);
                if (CheckCanPlace)
                {
                    if (CanPlace)
                    {
                        CanPlace = false;
                        transform.localEulerAngles = DefaultRotation;
                        foreach (var material in Materials)
                            material.SetColor("_Color", CantPlaceColor);
                    }
                }
            }
        }
    }
}
