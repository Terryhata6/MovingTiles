using System;
using System.Collections;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Core.Entities
{
    public class ExitDoorTilableObject : TilableObject
    {
        

        [SerializeField] private MMFeedbacks _mmFeedbacks;

        protected override IEnumerator InteractionWithPlayer(TileBox box, TurnState state)
        {
            //base.InteractionWithPlayer(box, state);
            if (state == TurnState.Player)
            {
                DoorUsed();
                (box.TiledObject as PlayerTilableObject).GoToExitDoor();
                yield return null;
            }
        }


        public override IEnumerator SpawnAnimation(Action<BaseTilableObject> OnEndSpawn)
        {
            transform.DOMoveY(_currentTileBox.transform.position.y + 0.5f, 0.2f)
                .OnComplete(() => OnEndSpawn.Invoke(this));

            yield break;
        }

        public override IEnumerator DestroyAnimation()
        {
            TilableObjectsController.Instance.RemoveObjectFromList(this);
            //TODO Destroy Death Animation
            Debug.Log("Открытие двери, конец уровня");
            yield break;
        }

        private void DoorUsed()
        {
            StartCoroutine(DestroyAnimation());
        }
    }
}
