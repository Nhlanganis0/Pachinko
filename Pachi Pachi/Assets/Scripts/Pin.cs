using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum result{
    pocket = 0,
    neutral = 1,
    despawn = 2
}
public class Pin : MonoBehaviour
{
    [SerializeField] Transform topPoint;
    [SerializeField] result left = result.neutral;
    [SerializeField] result right = result.neutral;
    [SerializeField] bool pocket;

    public bool ramp = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Transform GetTopPoint(){
        return topPoint;
    }

    public int LeftResult(){
        return (int) left;
    }
    public int RightResult(){
        return (int) right;
    }

}
