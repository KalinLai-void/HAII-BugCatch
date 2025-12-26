using Mediapipe.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AimComtroller : MonoBehaviour
{
    public static AimComtroller instance;

    public HandLandmarkerResultAnnotationController handController;

    [Header("Raycast Settings")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image aimImage;
    [SerializeField] private float rayDist = 1000f;
    [SerializeField] private float sphereCastingRadius = 0.1f;

    private GameObject target;
    public GameObject Target { get { return target; } }

    [Header("Bot Settings")]
    [SerializeField] private GameObject bot;
    [SerializeField] private Vector3 botToAimDist;

    private Vector3 botOriginRot;

    [Header("SFX")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickAudio;
    [SerializeField] private AudioClip catchAudio;

    [Header("MediaPipe UI Camera")]
    [SerializeField] private GameObject cameraCanvas;
    bool isCameraViewOn = true;

    private void Start()
    {
        if (instance == null) instance = this;
        if (bot != null) botOriginRot = bot.transform.eulerAngles;
        isCameraViewOn = cameraCanvas.activeSelf;
    }

    private void Update()
    {
        aimImage.color = Color.white;
        MoveAim();
        CheckRayCast();
    }

    private void MoveAim()
    {
        if (handController == null) return;

        try
        {
        aimImage.transform.localPosition = new Vector3(
            -UnityEngine.Screen.currentResolution.width / 2 + handController.CurrentTarget.handLandmarks[0].landmarks[5].x * UnityEngine.Screen.currentResolution.width,
            UnityEngine.Screen.currentResolution.height / 2 - handController.CurrentTarget.handLandmarks[0].landmarks[5].y * UnityEngine.Screen.currentResolution.height,
            0
            );
        }
        catch (System.Exception e) { };
    }

    private void CheckRayCast()
    {
        if (GameManager.instance.isPaused) return;

        Vector3 aimWorldPos = aimImage.rectTransform.position;
        Vector3 aimScreenPos;

        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            aimScreenPos = aimWorldPos;
        }
        else
        {
            aimScreenPos = Camera.main.WorldToScreenPoint(aimWorldPos);
        }

        Ray aimRay = Camera.main.ScreenPointToRay(aimScreenPos);

        if (Physics.SphereCast(aimRay, sphereCastingRadius, out RaycastHit hitBug, rayDist))
        {
            MoveBot(aimRay, hitBug);
            if (hitBug.collider.gameObject.layer == LayerMask.NameToLayer("Bug"))
            {
                aimImage.color = Color.green;
                target = hitBug.collider.gameObject;
                Debug.Log("[Target Bug] " + hitBug.collider.gameObject.name);
            }
            else
            {
                aimImage.color = Color.white;
                Debug.Log("[Target Other] " + hitBug.collider.gameObject.name);
            }
        }
        else
        {
            aimImage.color = Color.white;
            target = null;
        }
    }

    public void CatchTarget()
    {
        if (!target || target.layer != LayerMask.NameToLayer("Bug")) return;
        //Destroy(target);
        //target = null;
        bot.GetComponent<Animator>().SetTrigger("isCatching");
        audioSource.PlayOneShot(catchAudio);
        UIManager.instance.TurnOnUI(UIinGame.bugCatchedUI);
        GameManager.instance.PauseGame();
    }

    public void SimulateMouseClickAt()
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

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = aimScreenPos;
        
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        if (results.Count > 0)
        {
            audioSource.PlayOneShot(clickAudio);
            GameObject clickedObject = results[0].gameObject;
            Debug.Log("[Click]" + clickedObject.name);

            ExecuteEvents.Execute(clickedObject, eventData, ExecuteEvents.pointerDownHandler);
            ExecuteEvents.Execute(clickedObject, eventData, ExecuteEvents.pointerUpHandler);
            ExecuteEvents.Execute(clickedObject, eventData, ExecuteEvents.pointerClickHandler);
        }
    }

    public void TurnCameraViewOnOff()
    {
        if (!cameraCanvas) return;

        isCameraViewOn = !isCameraViewOn;
        cameraCanvas.SetActive(isCameraViewOn);
    }

    private void MoveBot(Ray ray, RaycastHit hit)
    {
        if (bot == null || GameManager.instance.isPaused) return;

        try
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
        catch (System.Exception e) { }
    }
}
