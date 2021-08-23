using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum rigged{
    non = 0,
    positive = 1, 
    negative = 2
}
public class GameManager : Singleton<GameManager>
{
    [SerializeField] rigged rig = (rigged) 0;
    [SerializeField] GameObject game;
    [SerializeField] int currBalance = 10;
    [SerializeField] Transform shootPt;
    [SerializeField] Image powerBar;
    [SerializeField] float maxPower = 160f;
    [SerializeField] float minPower = 75f;
    [SerializeField] float powerUpSpd = 0.01f;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] Transform ballSpawnPos;
    [SerializeField] Text balanceTxt;
    [SerializeField] GameObject arcade;
    [SerializeField] float riggedForChance = 6;
    [SerializeField] float riggedAgainstChance = 6;
    [SerializeField] GameObject gameUI;
    [SerializeField] AudioClip powerUpsound;
    [SerializeField] GuessScript guess;
    AudioSource src;
    public static int [] hits = new int [5];
    public static int [] misses = new int [5];
    public static int [] jackpots = new int [5];
    public List<float> riggedPowerPosiitive = new List<float>();
    public List<float> riggedPowerNegative = new List<float>();

    public List<float> riggedPowerJackpot = new List<float>();
    public bool inSpawnRegion;
    
    float powerDiff;
    Ball activeBall;
    Coroutine powerUpCo;
    Vector3 direction;

    public bool inGame;
    int currMachine = 0;


    public void setCurrMachine(int num){
        currMachine = num;
    }

    public void ResetRecords(){
        for(int loop = 0; loop < 5; loop++){
            hits[loop] = 0;
            misses[loop] = 0;
            jackpots[loop] = 0;
        }
    }

    public void IncrementHits(){
        hits[currMachine]++;
    }
    public void IncrementMisses(){
        misses[currMachine]++;
    }
    public void IncrementJackpots(){
        jackpots[currMachine]++;
    }
    // Start is called before the first frame update
    void Start()
    {
        src = GetComponent<AudioSource>();
        inGame = false;
        powerDiff = maxPower - minPower;
        ResetPowerBar();
        direction = shootPt.position - ballSpawnPos.position;
        OnGameClose();
        ResetRecords();
        
    }

    
    // Update is called once per frame
    void Update()
    {
        
        if(currBalance <= 0){
            guess.FailScreen();
        }
        if(Input.GetKeyDown("space") && powerUpCo == null && activeBall != null && !MinigameManager.instance.inMinigame){
            if(inGame && !activeBall.getShot() && !MinigameManager.instance.inMinigame){
                powerUpCo = StartCoroutine(PowerUp(powerUpSpd));
                
            }
            
            //StartCoroutine(PowerUp(0.0005f));

        }
        if(inGame && activeBall == null && !MinigameManager.instance.inMinigame && !inSpawnRegion){
            if(currBalance > 0){
                SpawnBall();
            }
        }
        
        
        
    }

    private void OnEnable() {
        DelegateManager.GameOpenDelegate += OnGameOpen;
        DelegateManager.GameCloseDelegate += OnGameClose;

    }
    private void OnDisable() {
        DelegateManager.GameOpenDelegate -= OnGameOpen;
        DelegateManager.GameCloseDelegate -= OnGameClose;
    }
    public void setRigged(int n){
        if(n < 3){
            rig = (rigged) n;
        }
    }

    public int getRigged(){
        return (int) rig;
    }
    public void PowerUpSound(){
        src.clip = powerUpsound;
        src.Play();
    }
    public void ResetPowerBar(){
        powerBar.fillAmount = 0;
    }
    // <summary 
    IEnumerator PowerUp(float rate){
        float power = 0;
        PowerUpSound();
        while(true){
            power += rate;
            powerBar.fillAmount = power/powerDiff;
            if(power >= powerDiff || !Input.GetKey("space")){
                if(power > powerDiff){
                    power = powerDiff;
                }
                src.Stop();
                activeBall.Shoot(minPower + power, direction);
                currBalance--;
                Invoke("NullActiveBall", 0.5f);
                UpdateBalanceTxt();
                ResetPowerBar();
                break;
            }
            else{
                yield return new WaitForSeconds(0.01f);
            }
        }
        //activeBall.Shoot(minPower + power);
        powerUpCo = null;
        yield return 0;
    }
    public void SetActiveBall(Ball ball){
        activeBall = ball;
    }
    public void SpawnBall(){

        GameObject newBall = Instantiate(ballPrefab, ballSpawnPos.position, Quaternion.identity, ballSpawnPos);
        activeBall = newBall.GetComponent<Ball>();
        Debug.Log(currBalance + " balanace");
        UpdateBalanceTxt();
    }
    public int getBalance(){
        return currBalance;
    }

    public void PauseAllBalls(){
        Ball [] balls = ballSpawnPos.GetComponentsInChildren<Ball>();
        for(int loop = 0; loop < balls.Length; loop++){
            balls[loop].PauseBall();
        }

    }
    public void ResumeAllBalls(){
        Ball [] balls = ballSpawnPos.GetComponentsInChildren<Ball>();
        for(int loop = 0; loop < balls.Length; loop++){
            balls[loop].Resume();
        }
    }
    public void OnGameOpen(){
        gameUI.SetActive(true);
        if(currBalance > 0 && activeBall == null){
            SpawnBall();
        }
        
        UIManager.instance.OnGameOpen();
        
        game.SetActive(true);
        arcade.SetActive(false);
        //inGame = true;
        SoundManager.instance.StartupSound();
        
    }
    
    public void OnGameClose(){
        DestroyBalls();
        UpdateBalanceTxt();
        game.SetActive(false);
        gameUI.SetActive(false);
        arcade.SetActive(true);
        inGame = false;
        StopAllCoroutines();
    }
        
    public void DestroyBalls(){
        Ball [] balls = ballSpawnPos.GetComponentsInChildren<Ball>();
        for(int loop = 0; loop < balls.Length; loop++){
            Destroy(balls[loop].gameObject);

        }
        AddBalance(balls.Length);
    }
    
    public void AddBalance(int value){
        currBalance+= value;
        UpdateBalanceTxt();
    }

    public Ball getActiveBall(){
        return activeBall;
    }
    public void UpdateBalanceTxt(){
        balanceTxt.text = "Tokens - " + currBalance.ToString();
    }
    public float getRiggedForChance(){
        return riggedForChance;
    }

    public float getRiggedAgainstChance(){
        return riggedAgainstChance;
    }

    public void NullActiveBall(){
        activeBall = null;
    }
    
    public void Bet(int Amount)
    {
        currBalance -= Amount;
    }


  
}
