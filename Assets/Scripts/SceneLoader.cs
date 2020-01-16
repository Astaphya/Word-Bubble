using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {
    int currentSceneIndex;
    GameScore gameScore;
    
    Scene scene ;
    Save save;
    string getScore;

   
    void Start()
    {
    
        save = GetComponent<Save>();
        scene = SceneManager.GetActiveScene();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex; //Aktif sahnenin indexi
        
        if(scene.name != "Start Scene")
         gameScore = GameObject.FindGameObjectWithTag("GameScore").GetComponent<GameScore>();
        
         getScore = PlayerPrefs.GetString("playerScore");
        
       
        
    }
	public void LoadNextScene()
    {
        
        SceneManager.LoadScene(currentSceneIndex + 1);
        PlayerPrefs.DeleteAll();
        save.SaveLevel(currentSceneIndex+1);
        
    }

    public void LoadStartScene()
    {
        
        SceneManager.LoadScene(0);
        gameScore.ResetGame(); // Skoru sıfırlıyoruz.
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartLevel()
    {  
        SceneManager.LoadScene(currentSceneIndex);
        if(scene.name =="Level 1")
        {
            gameScore.ResetGame();

        }

           GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>().text = getScore;


        
        
      

    }

    public void LoadSavedLevel()
    {
         int sahneIndex = PlayerPrefs.GetInt("ActiveScene");
        
            if(sahneIndex >= 2)
            {
            save.LoadLevel();
            Debug.Log("calisti");
            GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>().text = getScore;
            }

           
        
       
         

        
    }

   
}


