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
        [SerializeField] private Transform _openDoorTranform;

        protected override IEnumerator InteractionWithPlayer(TileBox box, TurnState state)
        {
            transform.DOLookAt(new Vector3(0, 0, -1), 0.1f);
            transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.1f);
            bool doorOpened = false;
            bool doorScaled = false;
            var player = (box.TiledObject as PlayerTilableObject);
            _openDoorTranform.transform.DORotate(new Vector3(0, 90, 0), 0.5f).OnComplete(() => { doorOpened = true; });
            yield return new WaitUntil(() => doorOpened);
            //base.InteractionWithPlayer(box, state);
            for (float i = 0; i < 1f; i += 0.01f * _jumpSpeed)
            {
                TempVector3 = Vector3.Lerp(_currentTileBox.transform.position, box.transform.position,
                    i);
                TempVector3.y = Mathf.Sin(Mathf.Clamp(i, 0f, 0.5f) * Mathf.PI) * _jumpHeight;
                transform.position = TempVector3;

                yield return null;
            }

            transform.position = box.transform.position;
            //DoorUsed();
            transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 1f);
            player.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 1f).OnComplete(() => { doorScaled = true;});
            transform.DORotate(Vector3.zero, 1f, RotateMode.FastBeyond360);
            doorScaled = false;
            yield return new WaitUntil(() => doorScaled);
            player.GoToExitDoor();
            yield return null;
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