using System;
using System.Collections.Generic;
using Service.GameStateService.Abstract;
using Service.GameStateService.Concrete.States;
using Service.GameStateService.Models;

namespace Service.GameStateService.Concrete
{
    public class GameStateMachine : IDisposable
    {
        private IGameState _currentGameState;
        private Dictionary<GameState, IGameState> _stateMap;

        public GameStateMachine()
        {
            SetupStateMap();
            _currentGameState = _stateMap[GameState.Starting];
            _currentGameState.Enter();
        }

        public void Dispose()
        {
            _stateMap.Clear();
        }

        private void SetupStateMap()
        {
            _stateMap = new Dictionary<GameState, IGameState>
            {
                { GameState.Starting, new StartingState() },
                { GameState.Playing, new PlayingState() },
                { GameState.Win, new SuccessState() },
                { GameState.Fail, new FailState() }
            };
        }

        public void SetState(GameState newState)
        {
            if (_currentGameState == null)
                return;

            _currentGameState.Exit();
            _currentGameState = _stateMap[newState];
            _currentGameState.Enter();
        }
    }
}