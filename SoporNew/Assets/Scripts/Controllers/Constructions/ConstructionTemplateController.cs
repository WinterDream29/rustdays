using Assets.Scripts.Models.Constructions;
using UnityEngine;

namespace Assets.Scripts.Controllers.Constructions
{
    public class ConstructionTemplateController : GroundPlacementItemController
    {
        public ConstructionType ConstructionType;

        protected override void UpdatePosition()
        {
            var hitRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hitInfo;
            int waterLayerMask = 1 << 4;//берем слой воды
            if (ConstructionType == ConstructionType.Foundation || ConstructionType == ConstructionType.Ceiling || ConstructionType == ConstructionType.Stairs) //не райкастим коллайдеры стен
                waterLayerMask |= (1 << 13);
            if (ConstructionType == ConstructionType.Wall || ConstructionType == ConstructionType.StreetStairs)
                waterLayerMask |= (1 << 14);
            waterLayerMask = ~waterLayerMask;//инвентируем, теперь в переменной все слои крое воды
            if (Physics.Raycast(hitRay, out hitInfo, 10.0f, waterLayerMask))
            {
                if (hitInfo.collider != null)
                {
                    var controller = hitInfo.collider.GetComponent<ConstructionColliderController>();
                    if (controller != null && controller.ConstructionType == ConstructionType)
                    {
                        transform.position = hitInfo.collider.transform.position;
                        transform.rotation = hitInfo.collider.transform.rotation;
                    }
                    else
                    {
                        transform.position = transform.parent.position;
                        transform.rotation = transform.parent.rotation;

                        Vector3 pos = transform.position;
                        //pos.y = Terrain.activeTerrain.SampleHeight(transform.position) + ObjectHeight + AdditionalHeigth;
                        pos.y = GameManager.CurrentTerain.SampleHeight(transform.position) + ObjectHeight + AdditionalHeigth;
                        transform.position = pos;
                    }
                }
                else
                {
                    transform.position = transform.parent.position;
                    transform.rotation = transform.parent.rotation;

                    Vector3 pos = transform.position;
                    //pos.y = Terrain.activeTerrain.SampleHeight(transform.position) + ObjectHeight + AdditionalHeigth;
                    pos.y = GameManager.CurrentTerain.SampleHeight(transform.position) + ObjectHeight + AdditionalHeigth;
                    transform.position = pos;
                }
            }
        }
    }
}
