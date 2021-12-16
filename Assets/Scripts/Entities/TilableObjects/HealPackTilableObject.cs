using System;
using System.Collections;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Entities
{
    public class HealPackTilableObject : TilableObject
    {
        [Header("HealPackProps")]
        [SerializeField] private float _baseHeal = 1;

        [SerializeField] private MMFeedbacks _mmFeedbacks;

        protected override IEnumerator InteractionWithPlayer(TileBox box, TurnState state)
        {
            //base.InteractionWithPlayer(box, state);
            if (state == TurnState.Player)
            {
                for (float i = 0; i < 1f; i += 0.01f * _jumpSpeed)
                {
                    TempVector3 = Vector3.Lerp(_currentTileBox.transform.position, box.transform.position,
                        i);
                    TempVector3.y = Mathf.Sin(Mathf.Clamp(i, 0f, 0.5f) * Mathf.PI) * _jumpHeight;
                    transform.position = TempVector3;

                    yield return null;
                }
                (box.TiledObject as PlayerTilableObject).GetHeal(_baseHeal);
                PickupObject();
            }
        }

        
        public override IEnumerator SpawnAnimation(Action<BaseTilableObject> OnEndSpawn)
        {
            transform.DOMoveY(_currentTileBox.transform.position.y+0.5f, 0.2f).OnComplete(() => OnEndSpawn.Invoke(this));
            
            yield break;
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
    }
}