using System;
using System.Collections;
using UnityEngine;

namespace Core.Entities
{
    public class HealPackTilableObject : TilableObject
    {
        [Header("HealPackProps")]
        [SerializeField] private float _baseHeal = 1;

        protected override IEnumerator PlayerInteraction(TileBox box, TurnState state)
        {

            //base.PlayerInteraction(box, state);
            if (state == TurnState.Player)
            {
                for (float i = 0; i < 0.5f; i += 0.01f * _jumpSpeed)
                {
                    TempVector3 = Vector3.Lerp(_currentTileBox.transform.position, box.transform.position,
                        i);
                    TempVector3.y = Mathf.Sin(Mathf.Clamp(i, 0f, 0.5f) * Mathf.PI) * _jumpHeight;
                    transform.position = TempVector3;

                    yield return null;
                }

                (box.TiledObject as PlayerTilableObject).GetHeal(_baseHeal);
                for (float i = 0; i < 0.5f; i += 0.01f * _jumpSpeed)
                {
                    TempVector3 = Vector3.Lerp(_currentTileBox.transform.position, box.transform.position,
                        (0.5f - i));
                    TempVector3.y = Mathf.Sin(Mathf.Clamp(0.5f - i, 0f, 0.5f) * Mathf.PI) * _jumpHeight;
                    transform.position = TempVector3;

                    yield return null;
                }
            }
        }
        
        public override IEnumerator DestroyAnimation()
        {
            
            TilableObjectsController.Instance.RemoveObjectFromList(this);
            //TODO Destroy Death Animation
            Destroy(this.gameObject);
            yield break;
            
        }

        private void PickupObject()
        {
            StartCoroutine(DestroyAnimation());
        }
        
        public override void PlayerCallBack(PlayerCallbackType callbackType, PlayerTilableObject player)
        {
            //base.PlayerCallBack(callbackType, player);
            switch (callbackType)
            {
                case PlayerCallbackType.Pickup:
                {
                    PickupObject();
                    break;
                }
                case PlayerCallbackType.Attack:
                {
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException(nameof(callbackType), callbackType, null);
            }
        }
    }
}