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
    [SerializeField] private float rayDist = 1000f;

    [Header("Bot Settings")]
    [SerializeField] private GameObject bot;
    [SerializeField] private Vector3 botToAimDist;

    private Vector3 botOriginRot;

    private void Start()
    {
        if (bot != null) botOriginRot = bot.transform.eulerAngles;
    }

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

        Ray aimRay = Camera.main.ScreenPointToRay(aimScreenPos);
        if (Physics.Raycast(aimRay, out RaycastHit hit, rayDist))
        {
            MoveBot(aimRay, hit);
            //Debug.Log(hit.collider.gameObject.name);
        }
    }

    private void MoveBot(Ray ray, RaycastHit hit)
    {
        Vector3 pos = ray.GetPoint(
            Vector3.Distance(hit.collider.transform.position, Camera.main.transform.position) - 1f
            );

        float direction = 1;
        if (handController.GetRightOrLeftHand() == "Left") direction -= 2;

        bot.transform.position = new Vector3(
            pos.x + botToAimDist.x * direction, 
            pos.y + botToAimDist.y,
            pos.z + botToAimDist.z
            );

        bot.transform.eulerAngles = new Vector3(
            botOriginRot.x,
            botOriginRot.y * direction,
            botOriginRot.z
            );
    }
}
