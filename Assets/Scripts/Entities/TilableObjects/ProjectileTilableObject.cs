
using System;
using System.Collections;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Entities
{
    public class ProjectileTilableObject : TilableObject
    {
        [Header("ProjectileProps")] [SerializeField]
        private float BaseDamage = 1f;
        private bool _endAttack = false;

        public void EndAttack()
        {
            _endAttack = true;
            Debug.LogWarning("PlayerEndAttack");
        }

        protected override IEnumerator InteractionWithPlayer(TileBox box, TurnState state)
        {
            if (state == TurnState.Enemy)
            {
                
                for (float i = 0; i < 0.5f; i += 0.01f * _jumpSpeed)
                {
                    TempVector3 = Vector3.Lerp(_currentTileBox.transform.position, box.transform.position,
                        i);
                    TempVector3.y = Mathf.Sin(Mathf.Clamp(i, 0f, 0.5f) * Mathf.PI) * _jumpHeight;
                    transform.position = TempVector3;

                    yield return null;
                }
                (box.TiledObject as PlayerTilableObject).GetDamage(BaseDamage);
                StartCoroutine(DestroyAnimation());
            }
        }

        public override IEnumerator DestroyAnimation()
        {
            TilableObjectsController.Instance.RemoveObjectFromList(this);
            //TODO Destroy Death Animation
            yield return null;
            Destroy(this.gameObject);
            yield break;
        }
        
        public override IEnumerator SpawnAnimation(Action<BaseTilableObject> OnEndSpawn)
        {
            transform.DOMoveY(_currentTileBox.transform.position.y + 0.5f, 0.2f)
                .OnComplete(() => OnEndSpawn.Invoke(this));
            yield break;
        }
    }
}