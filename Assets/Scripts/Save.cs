using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

 [System.Serializable]
public class Save : MonoBehaviour
{
    
    private Scene SavedLevel;
    private int SavedScore;
    private int sceneIndex;
    private string score;
    Scene scene;


   public void SaveLevel(int currentScene)
   {
          scene = SceneManager.GetActiveScene();
          if(scene.buildIndex !=0)
          {
          score = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>().text;
          //sceneIndex = SceneManager.GetActiveScene().buildIndex;
          PlayerPrefs.SetString("playerScore",score); // Kullanıcının skorunu kaydediyoruz.
          PlayerPrefs.SetInt("ActiveScene",currentScene); // Aktif sahneyi kaydet.
          PlayerPrefs.Save();

          }
         
   }

   public void LoadLevel()
   {
     
       int sahneIndex = PlayerPrefs.GetInt("ActiveScene");
       SceneManager.LoadScene(sahneIndex);

      
       
      
       
       
       
       /*
       if(scene.buildIndex ==sahneIndex)
       {
           Debug.Log("calisti");
            GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>().text = getScore;
       //ScoreText.GetComponent<Text>().text = getScore;
       }
       */
      


   }
}
