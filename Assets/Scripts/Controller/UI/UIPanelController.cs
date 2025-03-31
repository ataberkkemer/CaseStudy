using System.Collections.Generic;
using Controller.UI.Models;
using Service;
using Service.Concrete;
using Service.GameStateService.Events;
using Service.GameStateService.Models;
using UnityEngine;

namespace Controller.UI
{
    public class UIPanelController : MonoBehaviour
    {
        [SerializeField] private GameObject startPanel;
        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject losePanel;

        private Dictionary<PanelType, GameObject> _panelMap;

        private void Awake()
        {
            _panelMap = new Dictionary<PanelType, GameObject>
            {
                { PanelType.MainMenu, startPanel },
                { PanelType.Win, winPanel },
                { PanelType.Lose, losePanel }
            };

            startPanel.SetActive(false);
            winPanel.SetActive(false);
            losePanel.SetActive(false);
        }

        private void OnEnable()
        {
            var eventService = ServiceLocator.Get<EventService>();

            eventService.Register<OpenPanelEvent>(OnPanelOpenRequested);
            eventService.Register<ClosePanelEvent>(OnPanelCloseRequested);
        }

        private void OnPanelOpenRequested(OpenPanelEvent data)
        {
            foreach (var panelType in _panelMap.Keys) _panelMap[panelType].SetActive(data.PanelType == panelType);
        }

        private void OnPanelCloseRequested(ClosePanelEvent data)
        {
            _panelMap[data.PanelType].SetActive(false);
        }

        public void StartGame()
        {
            ServiceLocator.Get<EventService>().Fire(new GameStateChangedEvent(GameState.Playing));

            startPanel.SetActive(false);
        }

        public void PanelCallback()
        {
            ServiceLocator.Get<EventService>().Fire(new GameStateChangedEvent(GameState.Starting));

            winPanel.SetActive(false);
            losePanel.SetActive(false);
        }
    }
}