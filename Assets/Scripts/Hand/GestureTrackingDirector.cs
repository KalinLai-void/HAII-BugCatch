using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe;
using Mediapipe.Unity;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GestureTrackingDirector : MonoBehaviour
{
    public static GestureTrackingDirector instance;

    HandState gesture = new HandState();

    public MeaningfulGesture preGesture;
    public MeaningfulGesture nowGesture;

    private void Start()
    {
        if (instance == null) instance = this;
        gesture.OnStateChanged += HandleOnStateChanged;
    }

    private void Update()
    {
        HandleHandController();
    }

    private void HandleHandController()
    {
        try
        {
            if (AimComtroller.instance.handController == null) return;
            if (AimComtroller.instance.handController.CurrentTarget.handLandmarks.Count <= 0) return;

            gesture.Process(AimComtroller.instance.handController.CurrentTarget.handLandmarks[0]);
        }
        catch (System.Exception e) { }
    }

    private void HandleOnStateChanged(HandState.FingerState previousState, HandState.FingerState currentState)
    {
        preGesture = previousState.Analyze();
        nowGesture = currentState.Analyze();

        if (preGesture != MeaningfulGesture.ThumbAndIndexClosed &&
            nowGesture == MeaningfulGesture.ThumbAndIndexClosed)
        {
            Debug.Log("Click!");
            AimComtroller.instance.SimulateMouseClickAt();
        }
        else if (preGesture == MeaningfulGesture.Eight &&
                 nowGesture == MeaningfulGesture.Seven)
        {
            AimComtroller.instance.TurnCameraViewOnOff();
        }
        else if (preGesture == MeaningfulGesture.Three &&
                 nowGesture == MeaningfulGesture.One)
        {
            Destroy(GameManager.instance.gameObject);
            SceneManager.LoadScene(0);
        }
        else if (preGesture == MeaningfulGesture.Five &&
                 nowGesture == MeaningfulGesture.None &&
                 UIManager.instance.nowActiveUI == UIinGame.HUD)
        {
            if (GameManager.instance.isPaused) return;
            AimComtroller.instance.CatchTarget();
        }

        if (UIManager.instance.nowActiveUI == UIinGame.bugCatchedUI)
        {
            // do something
        }
    }
}

