using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Rigged{
    non = 0,
    positive = 1,
    negative = 2
}
public class ActivateSlot : MonoBehaviour
{
    public int machineNum = 0;
    public GameObject Slot;
    public KeyCode InteractButton;
    [SerializeField] Rigged rig = Rigged.non;
    [SerializeField] TextMeshPro hits;
    [SerializeField] TextMeshPro misses;
    [SerializeField] TextMeshPro jackpots;
    void Start()
    {
        
    }

    private void OnEnable() {
        UpdateRecord();
    }
    public void UpdateRecord(){
        hits.text = GameManager.hits[machineNum].ToString();
        misses.text = GameManager.misses[machineNum].ToString();
        jackpots.text = GameManager.jackpots[machineNum].ToString();
    }
    private void OnTriggerStay2D(Collider2D Interact)
    {
        if (Input.GetKey(InteractButton) && Interact.gameObject.CompareTag("Player") && GameManager.instance.inGame == false)
        {
            //Slot.SetActive(true);
            GameManager.instance.OnGameOpen();
            GameManager.instance.setRigged((int) rig);
            GameManager.instance.setCurrMachine(machineNum);
        }
    }
}
