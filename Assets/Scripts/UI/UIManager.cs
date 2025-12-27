using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum UIinGame
{
    HUD, bugCatchedUI, ConverseUI, EndUI
}

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private Timer timer;

    [Header("UI")]
    [SerializeField] private Canvas HUD;
    [SerializeField] private GameObject botAndButPosForUI;
    [SerializeField] private GameObject InsectListObj;
    [SerializeField] private Canvas bugCatchedUI;
    [SerializeField] private Canvas converseUI;
    [SerializeField] private Canvas endUI;

    [Header("LLM")]
    public ChatTesting chatLLM;
    [SerializeField] private TextMeshProUGUI bugNameText;
    [SerializeField] private TextMeshProUGUI bugPersonalityText;
    [SerializeField] private TextMeshProUGUI bugIntroText;
    [SerializeField] private TextMeshProUGUI bugDialogText;

    public UIinGame nowActiveUI;

    private void Start()
    {
        if (instance == null) instance = this;
        nowActiveUI = UIinGame.HUD;
    }

    public void PauseGame()
    {
        timer.PauseCountdownSFX();
        GameManager.instance.PauseGame();
    }

    public void ContinueGame()
    {
        timer.PlayCountdownSFX();
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

    public void ParseInsectData(string rawText)
    {
        string[] tags = { "personality", "intro", "dialog" };

        foreach (string tag in tags)
        {
            // Regex 解釋：
            // <tag>           : 匹配開始標籤
            // (.*?)           : 捕獲組，匹配標籤內的所有內容（? 代表非貪婪匹配，遇到第一個結束標籤就停止）
            // </tag>          : 匹配結束標籤
            // RegexOptions.Singleline : 讓 . 可以匹配換行符號（非常重要，因為對話通常會換行）
            string pattern = $"<{tag}>(.*?)</{tag}>";
            Match match = Regex.Match(rawText, pattern, RegexOptions.Singleline);

            if (match.Success)
            {
                string content = match.Groups[1].Value.Trim();
                Debug.Log($"<color=yellow>[{tag}]</color> 提取結果: {content}");

                if (tag == "personality") bugPersonalityText.text = content;
                else if (tag == "intro") bugIntroText.text = content;
                else if (tag == "dialog") bugDialogText.text = content;
            }
            else
            {
                Debug.LogWarning($"找不到標籤: <{tag}>");
            }
        }
    }

    public void SetBugTextFromLLM()
    {
        TurnOnUI(UIinGame.bugCatchedUI);
        ParseInsectData(chatLLM.response);
    }

    public void SetBugTextToLLM(string bugTag)
    {
        string nameCh;

        switch (bugTag)
        {
            case "Spider": nameCh = "跳蛛"; break;
            case "Termite": nameCh = "飛蟻(大水蟻)"; break;
            case "Bee": nameCh = "蜜蜂"; break;
            case "Cockroach": nameCh = "蟑螂"; break;
            case "Mosquito": nameCh = "蚊子"; break;
            default: nameCh = "Unknown"; break;
        }
        chatLLM.SendData(nameCh);
        bugNameText.text = nameCh;
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
        nowActiveUI = selectedUI;

        PauseGame();
    }

    public void BackToGame()
    {
        nowActiveUI = UIinGame.HUD;
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
