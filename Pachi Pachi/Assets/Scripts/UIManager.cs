using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] Image topCRT;
    [SerializeField] Image bottomCRT;

    [SerializeField] GameObject barsCRT;
    [SerializeField] float speedCRT;

    // Start is called before the first frame update
    void Start()
    {
    
    }
    private void OnEnable() {
        //DelegateManager.GameOpenDelegate += OnGameOpen;
        //DelegateManager.GameCloseDelegate += OnGameClose;
    }

    private void OnDisable() {
        //DelegateManager.GameOpenDelegate -= OnGameOpen;
        //DelegateManager.GameCloseDelegate -= OnGameClose;

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnGameOpen(){
        //barsCRT.SetActive(true);
        StartCoroutine(OpenCRT(speedCRT));
    }
    public void OnGameClose(){
        StartCoroutine(CloseCRT(speedCRT));
    }

    public IEnumerator OpenCRT(float spd){
        float time = 0;
        while(true){
            time += Time.deltaTime;
            float exp = 2-Mathf.Exp(time*spd);
            topCRT.fillAmount = exp;
            bottomCRT.fillAmount = exp;
            if(exp <= 0){
                break;
            }
            else{
                yield return null;
            }
        }
    }
    public IEnumerator CloseCRT(float spd){
        float time = 0;
        while(true){
            time += Time.deltaTime;
            float exp = Mathf.Exp(time*spd)-1;
            topCRT.fillAmount = exp;
            bottomCRT.fillAmount = exp;
            if(exp >= 1){
                //barsCRT.SetActive(false);
                break;
            }
            else{
                yield return null;
            }
        }
    }
}
