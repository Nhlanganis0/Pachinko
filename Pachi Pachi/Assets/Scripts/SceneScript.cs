using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneScript : MonoBehaviour
{
   public void Quit()
    {
        Application.Quit();
    }

    public void LoadGame(){
        SceneManager.LoadScene("Game");
    }
    public void SceneNav(int Scene)
    {
        SceneManager.LoadScene(Scene);
    }
}
