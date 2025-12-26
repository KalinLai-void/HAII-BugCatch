using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum UIinGame
{
    HUD, bugCatchedUI, ConverseUI, EndUI
}

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private Canvas HUD;
    [SerializeField] private GameObject botAndButPosForUI;
    [SerializeField] private GameObject InsectListObj;
    [SerializeField] private Canvas bugCatchedUI;
    [SerializeField] private Canvas converseUI;
    [SerializeField] private Canvas endUI;

    private void Start()
    {
        if (instance == null) instance = this;
    }

    public void PauseGame()
    {
        GameManager.instance.PauseGame();
    }

    public void ContinueGame()
    {
        GameManager.instance.ContinueGame();
    }

    private void TurnOffAllUI()
    {
        HUD.gameObject.SetActive(false);
        botAndButPosForUI.SetActive(false);
        bugCatchedUI.gameObject.SetActive(false);
        converseUI.gameObject.SetActive(false);
        endUI.gameObject.SetActive(false);

        for (int i = 0; i < InsectListObj.transform.childCount; i++)
        {
            InsectListObj.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    private void SetWhichInsectShow()
    {
        for (int i = 0; i < InsectListObj.transform.childCount; i++)
        {
            if (InsectListObj.transform.GetChild(i).tag.Equals(AimComtroller.instance.Target.tag))
            {
                InsectListObj.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    public void TurnOnUI(UIinGame selectedUI)
    {
        TurnOffAllUI();

        switch (selectedUI)
        {
            case UIinGame.bugCatchedUI:
                botAndButPosForUI.SetActive(true);
                bugCatchedUI.gameObject.SetActive(true);
                SetWhichInsectShow();
                break;
            case UIinGame.ConverseUI:
                botAndButPosForUI.SetActive(true);
                converseUI.gameObject.SetActive(true);
                SetWhichInsectShow();
                break;
            case UIinGame.EndUI:
                endUI.gameObject.SetActive(true);
                break;
        }

        PauseGame();
    }

    public void BackToGame()
    {
        HUD.gameObject.SetActive(true);
        botAndButPosForUI.SetActive(false);
        bugCatchedUI.gameObject.SetActive(false);
        converseUI.gameObject.SetActive(false);
        ContinueGame();
    }

    public void GoToScene(SceneAsset scene)
    {
        GameManager.instance.LoadScene(scene);
    }
}
