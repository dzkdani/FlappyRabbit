using System;
using UnityEngine;

public class CustomMonobehavior : MonoBehaviour
{
    protected virtual void DispatchEvent(EventHandler _event, object _sender, EventArgs _eventArgs)
    {
        if (_event != null)
        {
            _event(_sender, _eventArgs);
        }
    }
}
