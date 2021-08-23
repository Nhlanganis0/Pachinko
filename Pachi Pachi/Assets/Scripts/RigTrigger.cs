using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigTrigger : MonoBehaviour
{
    [SerializeField] float force = 10f;
    public bool left;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "ball"){
            Ball ball = other.transform.GetComponent<Ball>();
            int stat = ball.getRiggedStatus();
            switch(stat){
                case 1:
                    if(left){
                        ball.setVelocityX(force);
                    }
                    else{
                        ball.setVelocityX(-force);
                    }
                break;

                case 2: 
                    if(left){
                        ball.setVelocityX(-force);
                    }
                    else{
                        ball.setVelocityX(force);
                    }
                break;
            }

        }
    }
}
