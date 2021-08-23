using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GuessScript : MonoBehaviour
{
    bool correct;
    public string WinScene,LoseScene;
    public GameObject BuyPanel, GuessPanel, BluePanel, RedPanel, BlackPanel, PurpPanel, GreenPanel, FailPanel, CorrectPanel;
    public KeyCode ActivatePanel;
    public int cost, PointAwarded, PointsToWin;

    void Start()
    {
        BuyPanel.SetActive(false);
        GuessPanel.SetActive(false);
        RedPanel.SetActive(false); 
        BluePanel.SetActive(false);
        BlackPanel.SetActive(false);
        PurpPanel.SetActive(false);
        GreenPanel.SetActive(false);
        FailPanel.SetActive(false);
        CorrectPanel.SetActive(false);
    }

    void Update()
    {
        Activate();
        Win();
    }

    public void Activate()
    {
        if (Input.GetKey(ActivatePanel))
        {
            PanelOneNav();
        }
    }

    public void AnswerBlue()
    {
        RedPanel.SetActive(false);
        BluePanel.SetActive(true);
        BlackPanel.SetActive(false);
        PurpPanel.SetActive(false);
        GreenPanel.SetActive(false);
        GuessPanel.SetActive(false);
        FailPanel.SetActive(false);
        BuyPanel.SetActive(false);
        CorrectPanel.SetActive(false);
    }

    public void AnswerRed()
    {
        RedPanel.SetActive(true);
        BluePanel.SetActive(false);
        BlackPanel.SetActive(false);
        PurpPanel.SetActive(false);
        GreenPanel.SetActive(false);
        GuessPanel.SetActive(false);
        FailPanel.SetActive(false);
        BuyPanel.SetActive(false);
        CorrectPanel.SetActive(false);
    }

    public void AnswerBlack()
    {
        RedPanel.SetActive(false);
        BluePanel.SetActive(false);
        BlackPanel.SetActive(true);
        PurpPanel.SetActive(false);
        GreenPanel.SetActive(false);
        GuessPanel.SetActive(false);
        FailPanel.SetActive(false);
        BuyPanel.SetActive(false);
        CorrectPanel.SetActive(false);
    }
    public void AnswerPurple()
    {
        RedPanel.SetActive(false);
        BluePanel.SetActive(false);
        BlackPanel.SetActive(false);
        PurpPanel.SetActive(true);
        GreenPanel.SetActive(false);
        GuessPanel.SetActive(false);
        FailPanel.SetActive(false);
        BuyPanel.SetActive(false);
        CorrectPanel.SetActive(false);
    }
    public void AnswerGreen()
    {
        RedPanel.SetActive(false);
        BluePanel.SetActive(false);
        BlackPanel.SetActive(false);
        PurpPanel.SetActive(false);
        GreenPanel.SetActive(true);
        GuessPanel.SetActive(false);
        FailPanel.SetActive(false);
        BuyPanel.SetActive(false);
        CorrectPanel.SetActive(false);
    }
    public void PanelOneNav()
    {
        BuyPanel.SetActive(true);
        GuessPanel.SetActive(false);
    }
    
    public void PanelTwoNav()
    {
        if (GameManager.instance.getBalance() >= cost)
        {
            GameManager.instance.Bet(cost);
            BuyPanel.SetActive(false);
            GuessPanel.SetActive(true); 
        }
    }

    public void Cancel()
    {
        BuyPanel.SetActive(false);
        GuessPanel.SetActive(false);
        FailPanel.SetActive(false);
        RedPanel.SetActive(false);
        BluePanel.SetActive(false);
        BlackPanel.SetActive(false);
        PurpPanel.SetActive(false);
        GreenPanel.SetActive(false);
        GuessPanel.SetActive(false);
        FailPanel.SetActive(false);
        BuyPanel.SetActive(false);
        CorrectPanel.SetActive(false);
        
    }
    public void ForAgainst()
    {
        PointAwarded++;
        CorrectPanel.SetActive(true);
    }

    public void FailScreen()
    {
        if(GameManager.instance.getBalance() == 0)
        {
            SceneManager.LoadScene(LoseScene);
        }
        else
        {
            FailPanel.SetActive(true);
        }
    }

    public void Win()
    {
        if (PointAwarded == PointsToWin)
        {
            SceneManager.LoadScene(WinScene);
        }
    }
}
