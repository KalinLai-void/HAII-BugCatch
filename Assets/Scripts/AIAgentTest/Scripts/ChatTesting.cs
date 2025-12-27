using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChatTesting : MonoBehaviour
{
    public LLM m_ChatModel;
    
    public string userText = "";
    private string stackText = "";

    public UnityEvent onResponse;
    public string response;

    // // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    //void OnGUI(){
    //    userText = GUI.TextField(new Rect(10, Screen.height - 10 - 35, 275, 30), userText);

    //    if (GUI.Button(new Rect(10+275+10, Screen.height - 10 - 35, 75, 30), "SEND\n")){
    //        SendData(userText);
    //        stackText = $"> {userText}\n{stackText}";
    //        userText = "";
    //    }

    //    GUI.Label(new Rect(10, Screen.height - 10 - 35 - 10 - 350, 350, 350), stackText);
    //}

    public void SendData(string _postWord){
        if (_postWord.Equals(""))
            return;
        
        Debug.Log("Sended message：" + _postWord);
        string _msg = _postWord;

        m_ChatModel.PostMsg(_msg, CallBack);
    }

    private void CallBack(string _response)
    {
        _response = _response.Trim();
        _response = _response.Replace("*", "");

        response = _response;
        onResponse.Invoke();

        Debug.Log("Reponse from AI：" + _response);
        stackText = $"Gemini> {_response}\n{stackText}";
    }
}
