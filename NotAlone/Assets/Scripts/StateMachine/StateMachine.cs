using UnityEngine;

public class StateMachine 
{
    private IState _currentState;
    private IState _previousState;

    private bool _canExecuteState = false;

    public IState CurrentState => _currentState;
    public IState PreviousState => _previousState;

    public void SetStartState(IState startState)
    {
        _currentState = startState;
        _currentState.Enter();
        _canExecuteState = true;
        Debug.Log(_currentState.ToString());
    }

    public void ChangeState(IState newState)
    {
        _canExecuteState = false;
        if(_currentState != null)
        {
            _previousState = _currentState;
            _previousState.Exit();
        }
        _currentState = newState;
        _currentState?.Enter();
        _canExecuteState = true;
        Debug.Log(_currentState);
    }

    public void Tick()
    {
        UpdateTickCurrentState();
    }

    public void FixedTick()
    {
        FixedUpdateTickCurrentState();
    }

    private void UpdateTickCurrentState()
    {
         if (_canExecuteState)
            _currentState.ExecuteUpdate();
    }

    private void FixedUpdateTickCurrentState()
    {
       if (_canExecuteState)
            _currentState.ExecuteFixedUpdate();
    }
}
