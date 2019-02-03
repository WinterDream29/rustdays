using Assets.Scripts.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    public class GroundPlacementItemController : MonoBehaviour
    {
        public float ObjectHeight;
        public bool CheckCanPlace = false;
        public Color CanPlaceColor;
        public Color CantPlaceColor;
        public List<Material> Materials;
        public string CantPlaceLocalizationKey;

        public bool CanPlace { get; protected set; }
        public float AdditionalHeigth { get; set; }
        public UiSlot Slot { get; private set; }

        protected GameManager GameManager;

        private Transform _cachedTransform;
        private bool _isInitialized;
        private float _raycastTimer = 0.0f;

        public void Init(GameManager gameManager, UiSlot slot)
        {
            CanPlace = true;
            _cachedTransform = transform;
            Slot = slot;
            AdditionalHeigth = 0.0f;
            GameManager = gameManager;
            if (CheckCanPlace)
            {
                foreach (var material in Materials)
                    material.SetColor("_Color", CanPlaceColor);
            }

            _isInitialized = true;
        }

        public void Up()
        {
            AdditionalHeigth += 0.5f;
        }

        public void Down()
        {
            AdditionalHeigth -= 0.5f;
        }

        void LateUpdate()
        {
            UpdatePosition();
        }

        protected virtual void UpdatePosition()
        {
            Vector3 pos = transform.position;

            RaycastHit hit;
            Physics.Raycast(transform.position, -Vector3.up, out hit, 10);
            if (hit.collider != null && hit.collider.gameObject.tag != "Terrain")
            {
                pos.y = hit.collider.transform.position.y + ObjectHeight;
            }
            else
            {
                //pos.y = Terrain.activeTerrain.SampleHeight(transform.position) + ObjectHeight;
                pos.y = GameManager.CurrentTerain.SampleHeight(transform.position) + ObjectHeight;
            }
            
            transform.position = pos;
        }
    }
}
