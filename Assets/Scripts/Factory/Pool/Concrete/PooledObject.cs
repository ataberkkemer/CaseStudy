using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Factory.Pool.Abstract;
using Service;
using Service.Concrete;
using Service.GameStateService.Events;
using Service.GameStateService.Models;
using UnityEditor;
using UnityEngine;

namespace Factory.Pool.Concrete
{
    public class PooledObject : MonoBehaviour, IPooledGameObject
    {
        private List<IPooledGameObject> _children;
        private bool _hasPooledParent;
        private bool _isApplicationQuitting;

        private bool _isReleasing;
        private PoolService _poolService;

        private void OnEnable()
        {
            ServiceLocator.Get<EventService>().Register<GameStateChangedEvent>(OnGameStateChanged);
        }

        protected virtual void OnDisable()
        {
#if UNITY_EDITOR
            if (Undo.isProcessing) return;
#endif
            if (_isReleasing) return;
            if (_isApplicationQuitting) return;
            if (transform.parent == Parent) return;
            ServiceLocator.Get<EventService>().Unregister<GameStateChangedEvent>(OnGameStateChanged);

            ManualRelease();
        }

        protected virtual void OnApplicationQuit()
        {
            _isApplicationQuitting = true;
        }

        protected virtual void OnTransformChildrenChanged()
        {
            FindChildren();
        }

        protected virtual void OnTransformParentChanged()
        {
            _hasPooledParent = GetComponentsInParent<IPooledObject>().Where(x => !ReferenceEquals(x, this)).ToList()
                .Count > 0;
        }

        public int Id { get; set; }
        public Transform Parent { get; set; }

        public virtual void OnGet(bool activateOnGet)
        {
            _isReleasing = false;
            if (activateOnGet)
                gameObject.SetActive(true);
        }

        public virtual void SetTransform(Vector3 position, Quaternion rotation, Transform parent)
        {
            transform.SetPositionAndRotation(position, rotation);
            transform.SetParent(parent != null ? parent : Parent);
        }

        public virtual void OnRelease()
        {
            _isReleasing = true;
            gameObject.SetActive(false);
            transform.SetParent(Parent);
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        public virtual void ManualRelease(bool releaseChildren = true)
        {
            if (_isReleasing) return;

            if (releaseChildren)
                ReleaseChildren();
            _poolService = ServiceLocator.Get<PoolService>();
            _poolService.Release(Id, gameObject, null);
            if (enabled == false) enabled = true;
        }

        private void OnGameStateChanged(GameStateChangedEvent data)
        {
            if (data.GameState is GameState.Fail or GameState.Win)
            {
                DOTween.KillAll();
                GetComponent<IDisposable>().Dispose();
                ManualRelease();
                ServiceLocator.Get<EventService>().Unregister<GameStateChangedEvent>(OnGameStateChanged);
            }
        }

        protected virtual void FindChildren()
        {
            _children = GetComponentsInChildren<IPooledGameObject>().Where(x => !ReferenceEquals(x, this)).Reverse()
                .ToList();
        }

        protected virtual void ReleaseChildren()
        {
            FindChildren();

            if (_children is not { Count: > 0 }) return;

            foreach (var child in _children)
                child.ManualRelease(false);
        }
    }
}