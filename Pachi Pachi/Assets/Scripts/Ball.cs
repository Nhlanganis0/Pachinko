using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum rigStatus{
    random = 0,
    positive = 1,
    negative = 2
}
public class Ball : MonoBehaviour
{
    [SerializeField] float pushForce = 5f;
    Vector3 cacheVelocity;
    float cacheAngVelocity;
    int collCount;
    bool shot;
    Rigidbody2D rb;
    Collision2D pinColl;

    Sprite sprite;
    Transform lastPin;
    rigStatus rigStat = rigStatus.random;
    [SerializeField] float collModifier = 1f;
    [SerializeField] float maxPower = 10f;
    [SerializeField] float minPower =10f;
    [SerializeField] AudioClip pinKnock;
    [SerializeField] AudioClip hit;
    [SerializeField] AudioClip miss;
    float powerDiff;
    bool knockPlayed;
    AudioSource audioSource;
    float iniVelocity;

    // Start is called before the first frame update
    void Start()
    {
        
        audioSource = GetComponent<AudioSource>();

        powerDiff = maxPower - minPower;
        
        shot = false;
        collCount = 0;


        pinColl = null;
        rb = GetComponent<Rigidbody2D>();
        
    }
    
    private void OnEnable() {
        knockPlayed = false;
        //GameManager.instance.SetActiveBall(this.GetComponent<Ball>());
        CalculateRigged();
        //rigStat = rigStatus.positive;
    }
    private void OnDestroy() {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>

    public void Shoot(float power, Vector3 direction){
        float rand = Random.Range(0,10);
        
        if(rand == 2){
            switch((int) rigStat){
                case 0:
                if(!shot){
                rb.velocity = power * direction;
                shot = true;
                iniVelocity = power;
                }
                break;
                case 1:
                List<float> nums1 = GameManager.instance.riggedPowerPosiitive;
                int num1 = Random.Range(0, nums1.Count);
                if(!shot){
                    rb.velocity = nums1[num1] * direction;
                    shot = true;
                }
                break;
                case 2:
                List<float> nums = GameManager.instance.riggedPowerNegative;
                int num = Random.Range(0, nums.Count);
                if(!shot){
                    rb.velocity = nums[num] * direction;
                    shot = true;
                } 
                break;
            }
        }
        else{
            if(!shot){
            rb.velocity = power * direction;
            shot = true;
            iniVelocity = power;
        }
        }
        
        
        
    }
    public void RandomFallDirection(){
        
            int dir = Random.Range(0, 2);
            //Debug.Log(dir);
            Vector3 vel = rb.velocity;
            float ang = rb.angularVelocity;
            vel.x = 0;
            rb.velocity = vel;
            switch(dir){
                case 0:
                rb.angularVelocity = 0;
                rb.angularVelocity = -pushForce;
                rb.velocity = vel;
                break;

                case 1:
                
                rb.angularVelocity = 0;
                rb.angularVelocity = pushForce;
                rb.velocity = vel;
                break;
            }
    }
    public void FallLeft(){
        rb.angularVelocity = 0;
        rb.angularVelocity = pushForce;
    }
    public void FallRight(){
        rb.angularVelocity = 0;
        rb.angularVelocity = -pushForce;
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.tag == "spawn point"){
            GameManager.instance.inSpawnRegion = false;
        }

    }
    private void OnTriggerStay2D(Collider2D other) {
        if(other.tag == "spawn point"){
            GameManager.instance.inSpawnRegion = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "snap"  && other.transform.GetComponentInParent<Pin>().ramp == false){
            
            //transform.position = other.gameObject.transform.GetComponentInParent<Transform>().position + (posOffset *Vector3.up);
            setPositionX(other.transform.position.x);
            PlayPinKnockSound();
            setVelocityX(0);
            Pin pin = other.transform.GetComponentInParent<Pin>();
            switch((int)rigStat){
                case 0:
                    RandomFallDirection();
                break;
                
                case 1:
                if(pin.LeftResult() == 0){
                    FallLeft();
                }
                else if(pin.RightResult() == 0){
                    FallRight();
                }
                else{
                    RandomFallDirection();
                }
                break;
                case 2:
                    if(pin.LeftResult() == 2){
                    FallLeft();
                }
                else if(pin.RightResult() == 2){
                    FallRight();
                }
                else{
                    RandomFallDirection();
                }
                break;
            }
        }
        else if(other.tag == "triangle"){
            collCount = 0;
        }
        
        else if(other.tag == "pocket"){
            //PauseBall();
            Pocket pocket = other.gameObject.transform.GetComponentInParent<Pocket>();
            GameManager.instance.AddBalance(pocket.value);
            
            if(pocket.jackpot){
                Invoke("PauseBall", 0.2f);
                //play jackpot sound 
                GameManager.instance.IncrementJackpots();
                Debug.Log(iniVelocity + " jackpot velocity");
                SoundManager.instance.JackPotSound();
                GameManager.instance.PauseAllBalls();
                MinigameManager.instance.inMinigame = true;
                StartCoroutine(PhaseOut(0.5f));
                
            }
            else{
                Invoke("PauseBall", 0.2f);
                GameManager.instance.IncrementHits();
                //play pocket sound
                Debug.Log(iniVelocity + " pocket velocity");
                PlayPocketSound();
                StartCoroutine(PhaseOut(0.5f));
                
            }
        }
        else if(other.tag == "despawn"){
            Invoke("PauseBall", 0.2f);
            PlayMissSound();
            Debug.Log(iniVelocity + " miss velocity");
            StartCoroutine(PhaseOut(0.5f));
            GameManager.instance.IncrementMisses();
            
        }
    }
    
    public void PlayPocketSound(){
        audioSource.clip = hit;
        audioSource.Play();
    }
    public void PlayMissSound(){
        audioSource.clip = miss;
        audioSource.Play();
    }
    public void PauseBall(){
        
        cacheAngVelocity = rb.angularVelocity;
        cacheVelocity = rb.velocity;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    public void Resume(){
        rb.constraints = RigidbodyConstraints2D.None;
        rb.velocity = cacheVelocity;
        rb.angularVelocity = cacheAngVelocity;
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.collider.tag == "pin" && !knockPlayed){
            //PlayPinKnockSound();
            knockPlayed = true;
        }
        if(other.collider.tag == "ball"){
            Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());
        }
        //rb.constraints = RigidbodyConstraints2D.FreezeAll;
        // pin collision modification, downward velocity increases when ball hits pins
        if(other.collider.tag == "pin" && pinColl == null)
        {

            collCount++;
            Vector3 velocity = rb.velocity;
            velocity.y *= collModifier;
            rb.velocity = velocity;
            pinColl = other;
            //RandomFallDirection();
            
        }
    }
    /// <summary>
    /// Sent when a collider on another object stops touching this
    /// object's collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionExit2D(Collision2D other)
    {
        if(other == pinColl){
            pinColl = null;
            
        }
        switch(other.collider.tag){
            case "pin":
                StartCoroutine(soundDelay());
                setVelocityX(0);
            break;
        }

    }
    
    IEnumerator soundDelay(){
        knockPlayed = false;
        yield return new WaitForSeconds(1f);
    }
    public int getRiggedStatus(){
        return (int) rigStat;
    }
    public bool getShot(){
        return shot;
    }

    IEnumerator DestroDelay(float sec){
        Destroy(this.gameObject);
        yield return new WaitForSeconds(sec);
    }

    public void setPositionX(float x){
        Vector3 newPos = transform.position;
        newPos.x = x;
        transform.position = newPos;
    }
    public void setVelocityX(float velocity){
        Vector3 vel = rb.velocity;
        vel.x = velocity;
        rb.velocity = vel;

    }
    
    public void PlayPinKnockSound(){
        audioSource.clip = pinKnock;
        if(!audioSource.isPlaying){
            audioSource.Play();
        }
        
    }

    public void CalculateRigged(){
        float rand;
        int machine = GameManager.instance.getRigged();
        switch(machine){
            
            case 0:
            rand = Random.Range(0, 25);
            if(rand == 0){
                rigStat = rigStatus.negative;
            }
            else if (rand == 1){
                rigStat = rigStatus.positive;
            }
            break;

            case 1:
            rand = Random.Range(1, 11);
            if(rand <= GameManager.instance.getRiggedForChance()){
                rigStat = rigStatus.positive;
            }
            else{
                rigStat = rigStatus.random;
            }
            
            break;

            case 2:
            rand = Random.Range(1, 11);
            if(rand <= GameManager.instance.getRiggedAgainstChance()){
                rigStat = rigStatus.negative;
            }
            else{
                rigStat = rigStatus.random;
            }
            break;
        }
    }

    public IEnumerator PhaseOut(float t){
        float time = 0;
        float first = t * 0.333f;
        float second = t * 0.666f;
        
        Vector3 scale = transform.localScale;
        Vector3 f = 0.5f * scale;
        Vector3 s = 0.25f * scale;
        while(true){
            time+= Time.deltaTime;

            if(time >= t){
                Destroy(this.gameObject);
                break;
            }
            else{
                if(time >= first && time < second){
                    transform.localScale = f;
                }
                else if(time >= second){
                    transform.localScale = s;
                }
                
                yield return null;
            }
        }
    }

    
}
