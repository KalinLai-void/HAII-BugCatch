using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ChatAnimateTesting : MonoBehaviour
{
    public LLM m_ChatModel;
    public bool autoTrim = false;
    public int wordsPerLine = 10;

    [Header("Show Text GameObject")]
    public TextMeshProUGUI showText;
    public bool typoAnimated = false;
    public float delayPerTex = 0.01f;

    [Header("Unity Event")]
    public UnityEvent<string> onSendData = new UnityEvent<string>();
    public UnityEvent<string> onRecvData = new UnityEvent<string>();
    public UnityEvent onTextAnimatedEnd = new UnityEvent();

    private Coroutine animatedTextCoro = null;
    // Start is called before the first frame update
    void Start()
    {
        if (showText != null)
        {
            showText.text = "";
        }
    }

    // // Update is called once per frame
    // void Update()
    // {

    // }

    IEnumerator AnimatedText(string _response)
    {
        for (int i = 0; i < _response.Length; i++)
        {
            showText.text += _response[i];

            if(autoTrim)
            {
                if (i > 0 && i % wordsPerLine == 0)
                {
                    showText.text += "\n";
                }
            }
            yield return new WaitForSeconds(delayPerTex);
        }

        onTextAnimatedEnd.Invoke();
    }

    public void ShowText(string _response)
    {
        if (showText == null)
        {
            Debug.LogWarning("Please Assign Text Mesh");
            return;
        }

        showText.text = "";

        if (typoAnimated)
        {
            if (animatedTextCoro != null)
            {
                StopCoroutine(animatedTextCoro);
            }

            animatedTextCoro = StartCoroutine(AnimatedText(_response));
        }
        else
        {
            for (int i = 0; i < _response.Length; i++)
            {
                showText.text += _response[i];

                if (i > 0 && i % wordsPerLine == 0)
                {
                    showText.text += "\n";
                }
            }
        }
    }

    public void SendData(string _postWord)
    {
        if (_postWord.Equals(""))
            return;

        Debug.Log("Sended message: " + _postWord);
        string _msg = _postWord;

        onSendData.Invoke(_msg);
        m_ChatModel.PostMsg(_msg, CallBack);
    }

    private void CallBack(string _response)
    {
        _response = _response.Trim();
        _response = _response.Replace("*", "");

        Debug.Log("Gemini> " + _response);
        // stackText = $"Gemini> {_response}\n{stackText}";
        onRecvData.Invoke(_response);
        ShowText(_response);
    }
}
