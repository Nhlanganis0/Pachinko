using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MinigameManager : Singleton<MinigameManager>
{
    [SerializeField] Text buttonCount;
    [SerializeField] Text bonus;
    [SerializeField] Text timeCount;
    [SerializeField] Text countDown;
    [SerializeField] float maxTime = 10f;
    [SerializeField] Animator characterAnimator;
    [SerializeField] Image ken;
    [SerializeField] GameObject minigameWindow;
    [SerializeField] AudioSource background;
    public bool inMinigame;
    [SerializeField] AudioSource aud;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)){
            StartCoroutine(StartCountdown());
        }
    }

    public IEnumerator StartCountdown(){
        int number = 4;
        ken.gameObject.SetActive(false);
        while(true){
            
            number--;
            countDown.text = number.ToString();
            if(number <0){
                StartCoroutine(StartTimer(maxTime));
                ken.gameObject.SetActive(true);
                //kick sound goes here
                countDown.gameObject.SetActive(false);
                break;
            }
            else{

                yield return new WaitForSeconds(1);

            }

        }
    }

    public IEnumerator Flash(SpriteRenderer sr, float t, int times){
        int count = 0;
        while(true){
            count++;
            if(count >= times){
                break;
            }
            else{
                if(count % 2 == 0){
                    sr.enabled = true;
                }
                else{
                    sr.enabled = false;
                }
                yield return new WaitForSeconds(t);
            }
        }
    }
    public IEnumerator StartTimer(float maxTime){
        float time = maxTime;
        int count = 0;
        SoundManager.instance.PlayMiniGameMusic();
        while(true){
            time -= Time.deltaTime;
            int second = (int) time;
            int ms = (int)Mathf.Abs((time - second)*100);
            
            timeCount.text = second.ToString() + ":" + ms.ToString();
            if(time <= 0){
                count = (int) count/3;
                bonus.text = "You Earned " + (count + 20) + " tokens!";
                GameManager.instance.AddBalance(count);
                bonus.gameObject.SetActive(true);
                ken.gameObject.SetActive(false);
                Invoke("CloseMinigame", 1f);
                break;
            }
            else{
                if(Input.GetKeyDown("space")){
                    count++;
                    characterAnimator.Play("Kick");
                    buttonCount.text = count.ToString();
                }
                yield return null;
            }
        }
    }

    public void OpenMinigame(){
        minigameWindow.SetActive(true);
        GameManager.instance.inGame = false;
        StartCoroutine(StartCountdown());
        inMinigame = true;
    }

    public void CloseMinigame(){
        background.Play();
        minigameWindow.SetActive(false);
        inMinigame = false;
        GameManager.instance.inGame = true;
    }
}
