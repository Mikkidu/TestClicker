using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCounter : MonoBehaviour
{
    public delegate void OnUpdate(float deltatime);
    public static OnUpdate OnFrameUpdateIvent;

    // Update is called once per frame
    void Update()
    {
        if (OnFrameUpdateIvent != null)
            OnFrameUpdateIvent.Invoke(Time.deltaTime);
    }
}
