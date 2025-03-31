using Controller.Coin;
using Factory.Pool.Model;
using Manager.Score.Events;
using Service;
using Service.CameraService;
using Service.Concrete;
using Service.GameStateService.Events;
using Service.GameStateService.Models;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Manager.Coin
{
    public class CoinManager : MonoBehaviour
    {
        [SerializeField] private PoolDataSo coinPool;

        private void OnEnable()
        {
            ServiceLocator.Get<EventService>().Register<GameStateChangedEvent>(OnGameStateChanged);
            ServiceLocator.Get<EventService>().Register<SpawnCoinEvent>(OnCoinSpawn);
        }

        private void OnDisable()
        {
            ServiceLocator.Get<EventService>().Unregister<GameStateChangedEvent>(OnGameStateChanged);
            ServiceLocator.Get<EventService>().Unregister<SpawnCoinEvent>(OnCoinSpawn);
        }

        private void OnCoinSpawn(SpawnCoinEvent data)
        {
            Spawn(data.ID);
        }

        private void OnGameStateChanged(GameStateChangedEvent data)
        {
            if (data.GameState != GameState.Playing) return;

            for (var i = 0; i < 10; i++)
                Spawn();
        }

        private void Spawn(int id = 0)
        {
            var screenBounds = ServiceLocator.Get<CameraService>().GetScreenBounds();

            var min = screenBounds.min;
            var max = screenBounds.max;
            var val = new Vector3(Random.Range(min.x, max.x), 0, Random.Range(min.z, max.z));
            var coin = ServiceLocator.Get<PoolService>().Get<GameObject>(coinPool).GetComponent<CoinController>();
            coin.transform.position = val;
            coin.Initialize(id);
        }
    }
}