using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TimerControll : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject[] sahnedekiBalonlar;
    [SerializeField] Image timerBar;
   [SerializeField] float maxTime =30f;
   float timeLeft;

   [SerializeField] GameObject gameOverMenu;
   [SerializeField] AudioClip gameOverSound;
    string hedefSkor;
    string myScore;
   SceneLoader sceneLoader;
   GameSession gameSession;
  

     void Start()
    {

        timeLeft=maxTime;
        hedefSkor = GameObject.FindGameObjectWithTag("HedefScore").GetComponent<Text>().text;
        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();
        gameSession = FindObjectOfType<GameSession>();
    }

      void Update()
    {
        sahnedekiBalonlar=GameObject.FindGameObjectsWithTag("Balloon"); // Sahnedeki Balloon tagına sahip tüm objeleri bul ve diziye ata
         myScore = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>().text;
        if(timeLeft>0)
        {
            
            timeLeft -= Time.deltaTime;
            timerBar.fillAmount = timeLeft/maxTime;
        }

        else {
            //Kalan süre 0 veya 0'dan küçük ise balonların collider'ını kapat. Oyun hızını da 0 yap.
             BalonColliderAyarlar(false);
             gameSession.OyunHiziniDegistir(0);
             

            if(System.Convert.ToInt32(myScore) < System.Convert.ToInt32(hedefSkor))
            {
            
            gameOverMenu.SetActive(true);
            gameSession.TümHarfleriSil();
           
            //gameSession.KelimeSonucSound(gameOverSound);

            }

            else {
                //MyScore hedefSkor'a eşit ya da daha büyük ise yeni ekran yüklenecek. Öncesinde tebrikler ekranı eklenebilir.
                sceneLoader.LoadNextScene();

            }
           
            
        }
        
        
    }

    
   public void PauseResumeControl()
   {
       if(Time.timeScale != 0)
       {
           
           PauseMenu.SetActive(true);
           BalonColliderAyarlar(false);
           Time.timeScale = 0;
       }

       else
       {
           if(timeLeft>0)
           {
               //timeScale 0 a eşit ve daha süremiz bitmemiş ise pause menusu çalışsın.
            Time.timeScale = 1;
            PauseMenu.SetActive(false);
            BalonColliderAyarlar(true);
           }
            
                    
       }
   }

   public void BalonColliderAyarlar(bool durum)
   {
       //Pause Paneli yüklendikten sonra sahnedeki balonların tıklanmasını kapatyoruz.
       for(int i =0 ;i< sahnedekiBalonlar.Length;i++)
           {
               sahnedekiBalonlar[i].GetComponent<CircleCollider2D>().enabled = durum ;
           }

   }

}
