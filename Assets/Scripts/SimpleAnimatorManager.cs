using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAnimatorManager : MonoBehaviour
{
    [SerializeField]Animator myAnimator;
    [SerializeField]string targetParameter;
    public void SetAnimatorBool_True(string parameter){ myAnimator.SetBool(parameter, true);}
    public void SetAnimatorBool_False(string parameter){ myAnimator.SetBool(parameter, false);}
    public void SetTargetParameter(string value){ targetParameter = value;}
    public void SetAnimatorFloat(float value){ myAnimator.SetFloat(targetParameter, value);}
    
    void Awake()
    {
        if(myAnimator == null) myAnimator = this.GetComponent<Animator>();   
    }
}
