using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuAnimationEvent : MonoBehaviour
{
    [SerializeField] PauseMenuManager manager;

    public void AnimationEventStart()
    {
        manager.PauseMenuAnimationStart();
    }

    public void AnimationEventOpenDone()
    {
        manager.PauseMenuAnimationOpenDone();
    }

    public void AnimationEventCloseDone()
    {
        manager.PauseMenuAnimationCloseDone();
    }
}
