﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    static T _instance = null;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load(typeof(T).Name) as T;
            }
            return _instance;
        }
    }
}