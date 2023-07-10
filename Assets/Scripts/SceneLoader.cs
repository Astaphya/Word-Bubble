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
    bool isMute;
    [SerializeField] Sprite musicOn;
    [SerializeField] Sprite musicOff;
    [SerializeField] GameObject voiceIcon;
    [SerializeField] CanvasGroup BölümHataPaneli;

   
    void Start()
    {
    
        save = GetComponent<Save>();
        scene = SceneManager.GetActiveScene();
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex; //Aktif sahnenin indexi
        
        if(scene.name != "Start Scene")
         gameScore = GameObject.FindGameObjectWithTag("GameScore").GetComponent<GameScore>();
        
         getScore = PlayerPrefs.GetString("playerScore");

         isMute = PlayerPrefs.GetInt("mute") == 1 ? true: false; 
         SesIconAtama();   
            

    }

    
	public void LoadNextScene()
    {
        
        SceneManager.LoadScene(currentSceneIndex + 1);
        //PlayerPrefs.DeleteAll();
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

            else {
                    Debug.Log("Kayıtlı level yok.");
                    BölümHataPaneli.gameObject.SetActive(true);
                    StartCoroutine(FadeOut());
                
            }

           
        
    }

    public IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(3f);
        for(float i =1 ; i>=0 ; i-= 0.1f)
        {
            yield return new WaitForSeconds(0.1f);
            BölümHataPaneli.alpha = i;
        }
        BölümHataPaneli.alpha = 1f;
        BölümHataPaneli.gameObject.SetActive(false);

    }

    public void SesIconAtama()
    {
        if(currentSceneIndex != 0)
        {            
            if(!isMute)
                {
                    voiceIcon.GetComponent<Image>().sprite = musicOn;
                }

            else    {
                    voiceIcon.GetComponent<Image>().sprite = musicOff;
                        }
    }
}

    public void SesKontrol()
    {
         
         isMute = !isMute;
        AudioListener.volume = isMute ? 0 : 1;
        PlayerPrefs.SetInt("mute" , isMute ? 1 :0);
        SesIconAtama();
    }

   
}


