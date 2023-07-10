using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class TimerControll : MonoBehaviour
{
    [SerializeField] GameObject PauseMenu;
    [SerializeField] Image timerBar;
    [SerializeField] float maxTime = 30f;
    float timeLeft;

    [SerializeField] GameObject gameOverMenu;
    [SerializeField] AudioClip gameOverSound;
    [SerializeField] GameObject LevelSuccessPanel;
    [SerializeField] GameObject tebriklerText;
    [SerializeField] ParticleSystem confetti;
    [SerializeField] ParticleSystem confettiBurst;
 
    string hedefSkor;
    string myScore;
    SceneLoader sceneLoader;
    GameSession gameSession;

    private int sayac;
    [SerializeField] Button pauseButton;


   // public bool isGameActive = true;


    void Start()
    {
       // pauseBaslangicImage = GetComponentInChildren<Image>().sprite;
        timeLeft = maxTime;
        hedefSkor = GameObject.FindGameObjectWithTag("HedefScore").GetComponent<Text>().text;
        sceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<SceneLoader>();
        gameSession = GameObject.Find("Game Session").GetComponent<GameSession>();
        
    }

    void Update()
    {
        myScore = GameObject.FindGameObjectWithTag("ScoreText").GetComponent<Text>().text;
        TimeBarControl();
        


    }

   
    public void TimeBarControl()
    {
        if (timeLeft > 0 && !Joker.isCilginMode)
            {

                timeLeft -= Time.deltaTime;
                timerBar.fillAmount = timeLeft / maxTime;
            }
    /*
        else if (Joker.isCilginMode)
            {
                Debug.Log("Cilgin mod aktif");
            }
    */

        else if (timeLeft <=0)
            {
                //Kalan süre 0 veya 0'dan küçük ise balonların collider'ını kapat. Oyun hızını da 0 yap.
                gameSession.isGameActive = false;
                //gameSession.OyunHiziniDegistir(0);

                int mScore;
                int hScore;

            if(Int32.TryParse(myScore , out mScore) && Int32.TryParse(hedefSkor , out hScore))
            {
                if (mScore <  hScore)
                    {

                        gameOverMenu.gameObject.SetActive(true);
                        gameSession.TümHarfleriSil();
                        gameSession.OyunHiziniDegistir(0);

                        //gameSession.KelimeSonucSound(gameOverSound);

                    }

                else
                    {
                        if(sayac == 0)
                        {
                            LevelCompleted();
                        }

                        else
                            return;
                        
                        //tebriklerText.transform.DOShakePosition( 1f,0.5f,5,20,true);
                        //MyScore hedefSkor'a eşit ya da daha büyük ise yeni ekran yüklenecek. Öncesinde tebrikler ekranı eklenebilir.
                      //  sceneLoader.LoadNextScene();

                    }
            }
            

        }
    }

    public void LevelCompleted()
    {
        StartCoroutine(OyunuDelaylıDurdur());
        LevelSuccessPanel.gameObject.SetActive(true);
        ParticleSystem conf = Instantiate(confetti ,new Vector2(10.69f,6.07f),confetti.transform.rotation);
        ParticleSystem confBurs = Instantiate(confettiBurst ,new Vector2(10.69f,6.07f),confettiBurst.transform.rotation);
       // conf.transform.SetParent(LevelSuccessPanel.transform);
       // confBurs.transform.SetParent(LevelSuccessPanel.transform);
       sayac++;


        //Konfeti animasyonları oynatılsın.
        
    }

    public IEnumerator OyunuDelaylıDurdur()
    {
        yield return new WaitForSeconds(3f);
        gameSession.OyunHiziniDegistir(0);
    }

    public IEnumerator PauseDelay(int TimeScale)
    {
        yield return new WaitForSeconds(0.35f);
        Time.timeScale = TimeScale;
    }

    public void PauseResumeControl()
    {
        
        
        if (Time.timeScale != 0 )
        {
            pauseButton.interactable = false;
           // PauseMenu.SetActive(true);
            PauseMenu.transform.DOLocalMoveX(0f,0.3f).SetEase(Ease.InOutElastic).SetUpdate(true);
            gameSession.isGameActive = false;
           // StartCoroutine(PauseDelay(0));
            Time.timeScale = 0;
            //GetComponentInChildren<Image>().sprite = pauseButtonImage;
        }

        else
        {
            if (timeLeft > 0)
            {
                //timeScale 0 a eşit ve daha süremiz bitmemiş ise pause menusu çalışsın.
                PauseMenu.transform.DOLocalMoveX(-1933.8f,0.1f).SetEase(Ease.InOutElastic).SetUpdate(true); // Timescale'den bağımsız animasyon oynatımı.
                gameSession.isGameActive = true;
                pauseButton.interactable = true;

                if(Joker.isSlowMode)
                    Time.timeScale = 0.5f;
                else
                    Time.timeScale = 1f;
                    
                //PauseMenu.SetActive(false);
               // GetComponentInChildren<Image>().sprite = pauseBaslangicImage;
            }


        }
    }

    
}
