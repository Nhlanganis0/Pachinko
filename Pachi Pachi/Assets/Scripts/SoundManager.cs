using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager>
{
    AudioSource src;
    AudioSource kickSource;
    [SerializeField] Text jackpot;
    [SerializeField] AudioClip jackpotSound;
    [SerializeField] AudioClip minigameMusic;
    [SerializeField] AudioClip startupSound;
    [SerializeField] AudioClip countdownSound;
    [SerializeField] AudioSource background;
    // Start is called before the first frame update
    void Start()
    {
        src = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMiniGameMusic(){
        src.clip = minigameMusic;
        src.Play();
    }
    public void JackPotSound(){
        StartCoroutine(JackPotSequence());
    }

    public void StartupSound(){
        StartCoroutine(OpenGameSound());
    }
    public IEnumerator OpenGameSound(){
        src.clip = startupSound;
        src.Play();
        while(true){
            if(!src.isPlaying){
                GameManager.instance.inGame = true;
                break;
            }
            else{
                yield return null;
            }
        }
    }
    public IEnumerator JackPotSequence(){
        background.Stop();
        jackpot.gameObject.SetActive(true);
        src.clip = jackpotSound;
        int count = 0;
        Color red = Color.red;
        Color white = Color.white;
        src.Play();
        while(true){
            count++;
            if(!src.isPlaying){
                MinigameManager.instance.OpenMinigame();
                //PlayMiniGameMusic();
                jackpot.gameObject.SetActive(false);
                break;
            }
            else{
                if(count%2 == 0){
                    jackpot.color = red;
                }
                else{
                    jackpot.color = white;
                }
                yield return new WaitForSeconds(0.1f);
            }


        }
    }
}
