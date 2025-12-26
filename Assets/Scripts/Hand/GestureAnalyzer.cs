using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe;

public enum MeaningfulGesture
{
    None,
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    ThumbAndIndexClosed,
    ThumbAndIndexOpen,
    Good
}

public static class GestureAnalyzer
{
    public static MeaningfulGesture Analyze(this HandState.FingerState state)
    {
        /* Analyze Gesture */

        if ((state & HandState.FingerState.ThumbAndIndexClosed) != 0)
        {
            return MeaningfulGesture.ThumbAndIndexClosed;
        }    
        else if (state == (
         HandState.FingerState.ThumbOpen |
         HandState.FingerState.IndexOpen |
         HandState.FingerState.MiddleOpen |
         HandState.FingerState.RingOpen))
        {
            return MeaningfulGesture.Nine;
        }
        else if (state == (
         HandState.FingerState.ThumbOpen |
         HandState.FingerState.IndexOpen |
         HandState.FingerState.MiddleOpen))
        {
            return MeaningfulGesture.Eight;
        }
        else if (state == (
         HandState.FingerState.ThumbOpen |
         HandState.FingerState.IndexOpen))
        {
            return MeaningfulGesture.Seven;
        }
        else if (state == (
         HandState.FingerState.ThumbOpen |
         HandState.FingerState.PinkyOpen))
        {
            return MeaningfulGesture.Six;
        }
        else if (state == (
         HandState.FingerState.ThumbOpen |
         HandState.FingerState.IndexOpen |
         HandState.FingerState.MiddleOpen |
         HandState.FingerState.RingOpen |
         HandState.FingerState.PinkyOpen))
        {
            return MeaningfulGesture.Five;
        }
        else if (state == (
         HandState.FingerState.IndexOpen |
         HandState.FingerState.MiddleOpen |
         HandState.FingerState.RingOpen |
         HandState.FingerState.PinkyOpen))
        {
            return MeaningfulGesture.Four;
        }
        else if (state == (
         HandState.FingerState.IndexOpen |
         HandState.FingerState.MiddleOpen |
         HandState.FingerState.RingOpen))
        {
            return MeaningfulGesture.Three;
        }
        else if (state == (
         HandState.FingerState.IndexOpen |
         HandState.FingerState.MiddleOpen))
        {
            return MeaningfulGesture.Two;
        }
        else if (state == (
         HandState.FingerState.IndexOpen))
        {
            return MeaningfulGesture.One;
        }
        else if (state == (
         HandState.FingerState.ThumbOpen))
        {
            return MeaningfulGesture.Good;
        }
        else
        {
            return MeaningfulGesture.None;
        }
    }
}
