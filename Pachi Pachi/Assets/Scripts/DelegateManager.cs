using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelegateManager : Singleton<DelegateManager>
{
    public delegate void OnGameOpenDelegate();
    public static event OnGameOpenDelegate GameOpenDelegate;
    public delegate void OnGameCloseDelegate();
    public static event OnGameOpenDelegate GameCloseDelegate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B)){
            OnGameOpen();
            //AddBalance(2);
        }
        if(Input.GetKeyDown(KeyCode.N)){
            OnGameClose();
            //AddBalance(2);
        }
    }

    public void OnGameOpen(){
        GameOpenDelegate();
    }

    public void OnGameClose(){
        GameCloseDelegate();
    }
}
