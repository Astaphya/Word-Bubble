using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameScore : MonoBehaviour
{
     static GameObject scoreGameObject;
     [SerializeField] int kelimeSkor = 10; // Her level için sabit   kelimeSkor*kelime.length o level için kelimeskorunu belirtecek
     string scoreText;
     
    private void Awake()
    {
        //Eğer sahnede GameScore var ise olanı kullan eğer yok ise ilk oluşturulanı çağır
        //Skorun diğer levellara aktarılmasını sağlamak amacıyla
        //Singleton pattern önceki lvllarda oluşturulan GameScore'u kullan.
        int gameScoreCount = FindObjectsOfType<GameScore>().Length;
        if (gameScoreCount > 1)
        {
            gameObject.SetActive(false); //OnDestroy komutu script ordering de en altta olduğu için bu obje en son yok edilecek bu yüzden öncesinde objenin aktifliğini kapatıp bu obje ile olan bağlantıyı kesiyoruz.
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ResetGame()
    {
        Destroy(gameObject); // Objeyi yok et. Skor sıfırlansın.
    }
    
    void Start()
    {
          scoreGameObject = GameObject.FindGameObjectWithTag("ScoreText");        
    }

    // Update is called once per frame
   
     public int SkorEkle(string Kelime)
       {
            scoreText = scoreGameObject.GetComponent<Text>().text;
            int MyScore=System.Convert.ToInt32(scoreText);
            MyScore += kelimeSkor*Kelime.Length; //kelimeSkor değişkeni sabit olduğundan oluşturduğumuz kelimenin uzunluğu ile çarparak her level için farklı bir skor değeri elde ediyoruz.
            scoreGameObject.GetComponent<Text>().text = MyScore.ToString();   
            return  MyScore;
            
       }
}
