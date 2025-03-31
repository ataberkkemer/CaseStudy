using System;
using Controller.Coin.Events;
using Controller.Coin.Model;
using DG.Tweening;
using Factory.Pool.Concrete;
using Input.Abstract;
using Service;
using Service.Concrete;
using Service.Timer;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controller.Coin
{
    public class CoinController : MonoBehaviour, IInputReceiver, IDisposable
    {
        [SerializeField] private CoinMaterialMap coinMaterialMap;
        [SerializeField] private Transform meshContainer;
        [SerializeField] private MeshRenderer meshRenderer;
        private readonly Vector3 _startPosition = new(0, 0.25f, 0);
        private bool _canReceiveInput = true;
        private int _id;

        private IPooledGameObject _pooledGameObject;
        private GameTimer _timer;

        public void Dispose()
        {
            DOTween.Kill(transform);
            if (_timer != null)
            {
                _timer.Stop();
                _timer.OnComplete -= JumpAnimation;
                _timer = null;
            }

            _canReceiveInput = false;
        }

        public void ReceiveInput()
        {
            if (!_canReceiveInput) return;
            ServiceLocator.Get<EventService>().Fire(new CoinCollectedEvent(_id));

            Dispose();
            DespawnAnimation();
        }

        public void Initialize(int id)
        {
            _id = id;
            _pooledGameObject = GetComponent<IPooledGameObject>();
            meshRenderer.material = coinMaterialMap.GetMaterial(id);
            meshContainer.localPosition = _startPosition;
            DOTween.Kill(transform);
            SpawnAnimation();
            JumpTimer();
            _canReceiveInput = true;
        }

        private void JumpTimer()
        {
            _timer = null;
            _timer = new GameTimer(Random.Range(3f, 5f));
            _timer.OnComplete += JumpAnimation;
        }

        private void JumpAnimation()
        {
            meshContainer.DOJump(transform.position, 2f, 1, 0.5f).SetEase(Ease.OutQuad).SetId(transform)
                .OnComplete(() => meshContainer.localPosition = _startPosition);
            JumpTimer();
        }

        private void SpawnAnimation()
        {
            DOTween.Kill(transform);
            meshContainer.localScale = Vector3.zero;
            meshContainer.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).SetId(transform);
            meshContainer.DORotate(new Vector3(0, 0, 360), 2f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Incremental).SetId(transform).SetDelay(0.5f);
        }

        private void DespawnAnimation()
        {
            DOTween.Kill(transform);
            meshContainer.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack)
                .OnComplete(() => { _pooledGameObject.ManualRelease(); }).SetId(transform);
        }
    }
}