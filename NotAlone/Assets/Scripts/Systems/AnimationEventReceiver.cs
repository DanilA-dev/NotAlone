using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class AnimationEventReceiver : MonoBehaviour
{
    [SerializeField] private List<AnimationEvent> _animationEvents = new List<AnimationEvent>();

    private void InvokeEvent(string eventName)
    {
        _animationEvents.Where(a => a.EventName == eventName).FirstOrDefault().Event?.Invoke();
    }
}

[System.Serializable]
public class AnimationEvent
{
    public string EventName;
    public UnityEvent Event;
}
