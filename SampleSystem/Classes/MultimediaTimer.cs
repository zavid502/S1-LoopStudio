// <copyright file="MultimediaTimer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SampleSystem;

using System;
using System.Runtime.InteropServices;

/// <summary>
/// Hoge resolutie timer.
/// </summary>
public class MultimediaTimer
{
    [DllImport("winmm.dll", SetLastError = true)]
    private static extern uint timeSetEvent(uint uDelay, uint uResolution, TimerCallback lpTimeProc, uint dwUser, uint fuEvent);

    [DllImport("winmm.dll", SetLastError = true)]
    private static extern uint timeKillEvent(uint uTimerId);

    private delegate void TimerCallback(uint uTimerID, uint uMsg, IntPtr dwUser, IntPtr dw1, IntPtr dw2);

    private uint timerId = 0;
    private Action callback;

    public MultimediaTimer(uint interval, Action callback)
    {
        this.callback = callback;
        timerId = timeSetEvent(interval, 0, TimerCallbackMethod, 0, 1);
    }

    public void Stop()
    {
        if (timerId != 0)
        {
            uint error = timeKillEvent(timerId);

            timerId = 0;
        }
    }

    private void TimerCallbackMethod(uint id, uint msg, IntPtr user, IntPtr param1, IntPtr param2)
    {
        callback.Invoke();
    }
}