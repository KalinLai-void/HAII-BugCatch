using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe.Unity;
using UnityEngine.UI;

public class AimComtroller : MonoBehaviour
{
    [SerializeField] private HandLandmarkerResultAnnotationController handController;

    [SerializeField] private Canvas canvas;
    [SerializeField] private Image aimImage;

    private void Update()
    {
        MoveAim();
        CheckRayCast();
    }

    private void MoveAim()
    {
        if (handController == null) return;

        aimImage.transform.localPosition = new Vector3(
            -UnityEngine.Screen.currentResolution.width / 2 + handController.CurrentTarget.handLandmarks[0].landmarks[5].x * UnityEngine.Screen.currentResolution.width,
            UnityEngine.Screen.currentResolution.height / 2 - handController.CurrentTarget.handLandmarks[0].landmarks[5].y * UnityEngine.Screen.currentResolution.height,
            0
            );
    }

    private void CheckRayCast()
    {
        Vector3 aimScreenPos;
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            aimScreenPos = aimImage.GetComponent<RectTransform>().transform.position;
        }
        else
        {
            aimScreenPos = Camera.main.WorldToScreenPoint(aimImage.GetComponent<RectTransform>().transform.position);
        }


        if (Physics.Raycast(Camera.main.ScreenPointToRay(aimScreenPos), out RaycastHit hit, 1000f))
        {
            Debug.Log(hit.collider.gameObject.name);
        }
    }
}
