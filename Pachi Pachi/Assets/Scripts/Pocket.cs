using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Pocket : MonoBehaviour
{
    public int value = 2;
    public bool jackpot = false;
    [SerializeField] TextMeshPro valueText;
    // Start is called before the first frame update
    void Start()
    {
        valueText.text = value.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
