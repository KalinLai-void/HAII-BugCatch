using Mediapipe;
using Mediapipe.Tasks.Components.Containers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandState
{
    [Flags]
    public enum FingerState
    {
        Closed = 0,
        ThumbOpen = 1,
        IndexOpen = 2,
        MiddleOpen = 4,
        RingOpen = 8,
        PinkyOpen = 16,
        ThumbAndIndexClosed = 32,
    }

    public delegate void HandStateEvent(FingerState previousState, FingerState currentState);
    public event HandStateEvent OnStateChanged = (p, c) => { };
    FingerState m_FingerState;
    public void Process(NormalizedLandmarks landmarkList)
    {
        FingerState fingerState = FingerState.Closed;
        /* Analyse Fingers */
        
        float pseudoFixKeyPoint = landmarkList.landmarks[2].x;
        if ((landmarkList.landmarks[0].x > landmarkList.landmarks[1].x && landmarkList.landmarks[3].x < pseudoFixKeyPoint && landmarkList.landmarks[4].x < pseudoFixKeyPoint) ||
         (landmarkList.landmarks[0].x < landmarkList.landmarks[1].x && landmarkList.landmarks[3].x > pseudoFixKeyPoint && landmarkList.landmarks[4].x > pseudoFixKeyPoint))
        {
            fingerState |= FingerState.ThumbOpen;

            double thumbIndexDist = Math.Sqrt(
                Math.Pow(landmarkList.landmarks[4].x - landmarkList.landmarks[8].x, 2) +
                Math.Pow(landmarkList.landmarks[4].y - landmarkList.landmarks[8].y, 2)
            );
            if (thumbIndexDist < 0.1f)
            {
                fingerState |= FingerState.ThumbAndIndexClosed;
            }
        }
        pseudoFixKeyPoint = landmarkList.landmarks[6].y;
        if (landmarkList.landmarks[7].y < pseudoFixKeyPoint && landmarkList.landmarks[8].y < pseudoFixKeyPoint)
        {
            fingerState |= FingerState.IndexOpen;
        }
        pseudoFixKeyPoint = landmarkList.landmarks[10].y;
        if (landmarkList.landmarks[11].y < pseudoFixKeyPoint && landmarkList.landmarks[12].y < pseudoFixKeyPoint)
        {
            fingerState |= FingerState.MiddleOpen;
        }
        pseudoFixKeyPoint = landmarkList.landmarks[14].y;
        if (landmarkList.landmarks[15].y < pseudoFixKeyPoint && landmarkList.landmarks[16].y < pseudoFixKeyPoint)
        {
            fingerState |= FingerState.RingOpen;
        }
        pseudoFixKeyPoint = landmarkList.landmarks[18].y;
        if (landmarkList.landmarks[19].y < pseudoFixKeyPoint && landmarkList.landmarks[20].y < pseudoFixKeyPoint)
        {
            fingerState |= FingerState.PinkyOpen;
        }

        if (m_FingerState != fingerState)
        {
            OnStateChanged(m_FingerState, fingerState);
            m_FingerState = fingerState;
        }

        Debug.Log(fingerState);
    }
}
