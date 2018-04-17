using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class KeepScreenAlive : MonoBehaviour {
    
    private void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    
}
