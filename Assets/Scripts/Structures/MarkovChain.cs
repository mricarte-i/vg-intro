using System;
using System.Collections.Generic;

public class MarkovChain
{
    private class MarkovChainEntry
    {
        private string _currentState;
        private event Action _action;
        private double _accumProba = 0;
        private Dictionary<string, StateTransitionProba> _nextStates = new Dictionary<string, StateTransitionProba>();

        public MarkovChainEntry(string currentState)
        {
            _currentState = currentState;
        }
        
        public void AddStateAction(Action action)
        {
            _action += action;
        }

        public void AddPossibleNextState(string nextStateName, double nextStateProba)
        {
            if (_accumProba >= 1f) throw new Exception("Accumulated probabilities exceed 1");
            if (!_nextStates.ContainsKey(nextStateName))
            {
                _nextStates[nextStateName] = new StateTransitionProba(nextStateName, nextStateProba);
                _accumProba += nextStateProba;
                return;
            }

            _accumProba -= _nextStates[nextStateName]._endStateProba;
            _nextStates[nextStateName]._endStateProba = nextStateProba;
            _accumProba += nextStateProba;
        }

        public void RunActions()
        {
            _action.Invoke();
        }

        public string GetNextState(double roll)
        {
            double accum = 0;
            foreach (StateTransitionProba transition in _nextStates.Values)
            {
                accum += transition._endStateProba;
                if (roll < accum)
                {
                    return transition._endStateName;
                }
            }

            return _currentState;
        }
        
        private class StateTransitionProba
        {
            public string _endStateName { get; }
            public double _endStateProba { get; set; }

            public StateTransitionProba(string endStateName, double endStateProba)
            {
                _endStateName = endStateName;
                _endStateProba = endStateProba;
            }
        }

    }

    private Dictionary<string, MarkovChainEntry> _chain = new Dictionary<string, MarkovChainEntry>();
    private string _currentState = null;
    private Random _random = new Random();

    public void AddState(string stateName, Action action)
    {
        if (!_chain.ContainsKey(stateName))
        {
            _chain.Add(stateName, new MarkovChainEntry(stateName));
        }

        var entry = _chain[stateName];
        entry.AddStateAction(action);
    }

    public void AddTransition(string startStateName, string endStateName, double proba)
    {
        if (!_chain.ContainsKey(startStateName)) throw new Exception("Start state not found");
        if (!_chain.ContainsKey(endStateName)) throw new Exception("End state not found");
        
        var entry = _chain[startStateName];
        entry.AddPossibleNextState(endStateName, proba);
    }

    public void SetState(string state)
    {
        if (!_chain.ContainsKey(state)) throw new Exception("Cannot set a state which does not exist");
        _currentState = state;
    }

    public void RunActions()
    {
        if (!_chain.ContainsKey(_currentState)) throw new Exception("Set a state before running actions");
        _chain[_currentState].RunActions();
    }

    public void NextStep()
    {
        if (!_chain.ContainsKey(_currentState)) throw new Exception("Set a state before transitioning");
        double stateRoll = _random.NextDouble();
        
        MarkovChainEntry entry = _chain[_currentState];
        string nextState = entry.GetNextState(stateRoll);

        _currentState = nextState;
    }
}
