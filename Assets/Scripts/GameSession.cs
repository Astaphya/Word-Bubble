using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Text;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] GameObject[] balloons;
    [SerializeField] GameObject cilginModePrefab;
    [SerializeField] GameObject slowModePrefab;

   //  string[] alfabe = {"A","B","C","Ç","D","E","F","G","Ğ","H","I","İ","J","K","L","M","N","O","Ö","P","R","S","Ş","T","U","Ü","V","Y","Z","A","E","İ","K","R","L"} ;
    private string[] alfabe = {"A","B","C","Ç","D","E","F","G","Ğ","H","I","İ","J","K","L","M","N","O","Ö","P","R","S","Ş","T","U","Ü","V","Y","Z",
    "A","A","A","A","A","A","A","A","A","A","A","B","B","D","D","D","D","E","E","E","E","E","E","E","E","I","I","I","I","İ","İ","İ","İ",
    "İ","İ","İ","K","K","K","K","L","L","L","L","L","M","M","M","N","N","N","N","O","O","R","R","R","R","R","R","S","S","Ş","T","T","U","U","Ü","Y","Y","Z"};
    //[SerializeField] GameObject[] spawnNoktaları;
    private GameObject[] karakterSlots; 
    private int karakterSlotNumbers;
    GameObject sonKarakterSlot;
    //GameObject scoreGameObject;
    //GameObject hedefScoreGameObject;

    [SerializeField] int levelIcinSkor = 10; // Her level için sabit kelimeSkor gibi.
    public int toplamSkorInLevel = 0; // Level içinde toplanan skor
    
    //[SerializeField] int kelimeSkor = 10; // Her level için sabit   kelimeSkor*kelime.length o level için kelimeskorunu belirtecek
    [SerializeField] Sprite dogruSprite;
    [SerializeField] Sprite yanlisSprite;
    [SerializeField] Sprite orijinalSprite;
    [SerializeField] Sprite kelimeVarSprite;
    [SerializeField] AudioClip dogruSound;
    [SerializeField] AudioClip  yanlisSound;
    
    
    [SerializeField] AudioClip  kelimeVar;
    [SerializeField] Image kelimeAnimasyon;
    string olusturulanSonKelime;
    Color kelimeImageColor;
    Color SonKelimeColor;
    List<string> kullanilanKelimeler = new List<string>();

    SceneLoader sceneLoader;
    GameScore gameScore;


    string[] karakterTexts ={} ;
    string kelimeSorgu = null;
   
    
    //string scoreText;
    //string hedefScoreText;
    float second = 0.4f; // Ienumarator Sprite değişim süresi

    [SerializeField] private float balonDelay;
    [SerializeField] private float cilginBalonDelay;
    [SerializeField] private float slowModeDelay;
    private Vector3 jokerSpawnPoint = new Vector3(10f,0.5f,0);
    public bool isGameActive = false;

    public bool isCilginActive;
    public bool isSlowActive;
    [SerializeField] float cilginJokerSpawnDelay;

    [SerializeField] GameObject cilginJokerImage;
    [SerializeField] GameObject slowJokerImage;
    [SerializeField] GameObject LevelPaneli;



    void Start()
    {
        Debug.Log(alfabe.Length);
              // OyunHiziniDegistir(1f); // Oyunun başlaması için timeScale'i 1 yapıyoruz.
             // InvokeRepeating("BalonOlusturVeTextAta",0.1f,1f); // Oyun başladıktan 0.5 sn sonra ve her 0.2sn'de bir bu fonksiyonu çalıştır.
            StartGame();
            //CharacterSlot tagına sahip GameObject'leri bul ve GameObject dizisine ekle.
            karakterSlots = GameObject.FindGameObjectsWithTag("CharacterSlot").OrderBy(g=>g.transform.GetSiblingIndex()).ToArray(); //Sahnedeki yukarıdan-aşağıya sıraya göre ekle.
            karakterSlotNumbers = karakterSlots.Length; //CharacterSlot tagına sahip child sayısı içinde text bulunanlar.
            gameScore = GameObject.FindGameObjectWithTag("GameScore").GetComponent<GameScore>();
            //scoreGameObject = GameObject.FindGameObjectWithTag("ScoreText");
            //hedefScoreGameObject = GameObject.FindGameObjectWithTag("HedefScore");
            sceneLoader = FindObjectOfType<SceneLoader>();
            //hedefScoreText = hedefScoreGameObject.GetComponent<Text>().text;   
            kelimeImageColor=kelimeAnimasyon.GetComponent<Image>().color; // Image color
            SonKelimeColor = kelimeAnimasyon.GetComponentInChildren<Text>().color; //Text color
            olusturulanSonKelime = kelimeAnimasyon.GetComponentInChildren<Text>().text;
                  
           
    }

    IEnumerator BalonOlustur()
    {
        while(isGameActive)
        {
            if(Joker.isCilginMode)
            {
               yield return new WaitForSeconds(cilginBalonDelay);

            }
            
            else if(Joker.isSlowMode)
            {
                yield return new WaitForSeconds(slowModeDelay);
                
            }

            else{
                 yield return new WaitForSeconds(balonDelay);

            }


            var gelenHarf = RastgeleHarfUret(alfabe);
            GameObject rastgeleBalon = balloons[Random.Range(0, balloons.Length)]; //Tanımlı balonlardan birini seç
            rastgeleBalon.GetComponentInChildren<Text>().text = gelenHarf;
            // Instantiate(rastgeleBalon,pos,transform.rotation);
            Instantiate(rastgeleBalon);
        }
    }

    
    private void Update()
    {
        
        KelimeKontrol(); // Apiden Kelimeyi kontrol eden fonksiyon.
        JokerModeControl();
      

      

    }

    public void JokerModeControl()
    {
        if(Joker.isCilginMode)
        {
           cilginJokerImage.gameObject.SetActive(true);


        }

        else if (Joker.isSlowMode)
        {
            slowJokerImage.gameObject.SetActive(true);
        }

        else{
            cilginJokerImage.gameObject.SetActive(false);
            slowJokerImage.gameObject.SetActive(false);
        }
    }


    

    private void KelimeKontrol()
    {
        
         sonKarakterSlot = karakterSlots[karakterSlotNumbers-1];
         kelimeSorgu=sonKarakterSlot.GetComponentInChildren<Text>().text;
         if(!string.IsNullOrEmpty(kelimeSorgu))
         {
            string kelimem=KarakterleriStringeCevir();
             if(!kullanilanKelimeler.Contains(kelimem) && ApidenKelimeOku(kelimem))
             {
                 kullanilanKelimeler.Add(kelimem);
                 Debug.Log("Kelime bulundu");
                 StartCoroutine(SpriteDegistir(dogruSprite));
                 KelimeSonucSound(dogruSound); //Sesler sıkıntılı daha sonra bak

                 gameScore.SkorEkle(kelimem);
                // toplamSkorInLevel += levelIcinSkor * kelimem.Length;
                  Debug.Log(toplamSkorInLevel.ToString());
                 //SkorEkle(kelimem);

                 olusturulanSonKelime = kelimem;
                 kelimeAnimasyon.GetComponentInChildren<Text>().text = olusturulanSonKelime;
                 StartCoroutine(SonKelimeFadeOut());

             }

             else if (kullanilanKelimeler.Contains(kelimem))
             {
                 Debug.Log("Kelimeyi daha önce oluşturdunuz!");
                 StartCoroutine(SpriteDegistir(kelimeVarSprite));
                 KelimeSonucSound(kelimeVar);
                 
             }
             else
             {
                 Debug.Log("Kelime yok!");
                 StartCoroutine(SpriteDegistir(yanlisSprite));    
                 KelimeSonucSound(yanlisSound);
                
             }
            
              
             TümHarfleriSil();
           
         }
    }

    public void OyunHiziniDegistir(float oyunHizi)
    {
        Time.timeScale = oyunHizi;
    }
    
    
    private string RastgeleHarfUret(string[] harfDizisi)
    {
        var randomHarf = harfDizisi[Random.Range(0,29)]; // Alfabe içerisinden rastgele harf çek
        return randomHarf;
    }

    private void BalonOlusturVeTextAta()
    {
        var gelenHarf = RastgeleHarfUret(alfabe);
        GameObject rastgeleBalon = balloons[Random.Range(0,4)]; //Tanımlı balonlardan birini seç
       // float rastgeleSpawnPointX = Random.Range(1.5f,15.5f); //Oyun alanı sınırları içersindeki balonun oluşacağı pozisyon aralığı
       // float y = 0f;
       // float z =0f;
       // Vector3 pos = new Vector3(rastgeleSpawnPointX,y,z); // Balonun pozisyonu Ballon scripitinde ayarlanmaktadır.
        rastgeleBalon.GetComponentInChildren<Text>().text =gelenHarf;
        // Instantiate(rastgeleBalon,pos,transform.rotation);
        Instantiate(rastgeleBalon);
      
        

    }

    public void KarakterSlotaTextAtama(string harf)
    {
      
           for(int k=0; k<karakterSlotNumbers;k++)
           {
               kelimeSorgu =  karakterSlots[k].GetComponentInChildren<Text>().text; //İlk çocuktan başlayarak text değerleri sırayla kelimeSorgu değişkenine atanıyor.
               if(string.IsNullOrEmpty(kelimeSorgu))
               {
                   //İlk çocuktan itibaren sırayla kelimeSorgu null veya Empty ise balondaki text'i characterSlot'taki texte atıyoruz.
                   //Debug.Log(harf);
                   karakterSlots[k].GetComponentInChildren<Text>().text = harf;    
                   break;
               }
             
               
           }   
          
       }

       public void SonEkleneHarfiSil()
       {
           if(isGameActive)
           {
                //Sahnedeki  RemoveLastChar butonu tıklandığında bu method çalışacak ve en eklenen harfi silecek.
                Debug.Log(karakterSlotNumbers.ToString());
                for(int i = karakterSlotNumbers-1;i>=0;i--)
                {

               kelimeSorgu = karakterSlots[i].GetComponentInChildren<Text>().text;
               if(!string.IsNullOrEmpty(kelimeSorgu))
                    {
                   karakterSlots[i].GetComponentInChildren<Text>().text = null;
                   break;
                    }
                }

           }
          
       }

       

       public void TümHarfleriSil()
       {
           if(isGameActive)
           {
                //Sahnedeki RemoveAllChars  butonu tıklandığında bu metot ile karakterSlotların içi null yapılacak.
           
                 for(int i=0;i<karakterSlotNumbers;i++)
                    {
                        karakterSlots[i].GetComponentInChildren<Text>().text = null;

                    }
           }
       
       }
    

       public string KarakterleriStringeCevir()
       {
           //Karakter slottaki karakterleri string değişkene toplayarak ekliyoruz ve bu değişkeni döndürüyoruz.
           StringBuilder kelimem= new StringBuilder();
             for(int i=0;i<karakterSlotNumbers;i++)
           {
               kelimem.Append(karakterSlots[i].GetComponentInChildren<Text>().text) ; 

           }
           Debug.Log(kelimem);
           return kelimem.ToString();
       }  

       public IEnumerator SpriteDegistir(Sprite dogruYanlisSprite)
       {
           //Tüm slotları dogruVeyaYanlis sprite'ını atayacak 0.4 saniye bekledikten sonra ikinci for ile orijinal spriteları atama işlemi gerçekleşecek.
           for(int i=0;i<karakterSlotNumbers;i++)
           {
              
               karakterSlots[i].GetComponent<Image>().sprite = dogruYanlisSprite;
                //characterSlotChilds[i].GetComponent<SpriteRenderer>().sprite = orijinalSprite;

           } yield return new WaitForSeconds(second);   
            //yield return new WaitForSeconds(0.4f);

            for(int i=0;i<karakterSlotNumbers;i++)
           {
               //Orijinal spriteların geri atanması
              
                karakterSlots[i].GetComponent<Image>().sprite = orijinalSprite;

           }
           

       }

       public void KelimeSonucSound(AudioClip kelimeSound)
       {    
           AudioSource.PlayClipAtPoint(kelimeSound,Camera.main.transform.position);

       }

        private string KelimeyiDüzenle(string word)
    {
        word = word.Trim().ToLower();
        word = word.Replace("ş", "s");
        word = word.Replace("ı", "i");
        word = word.Replace("ö", "o");
        word = word.Replace("ü", "u");
        word = word.Replace("ç", "c");
        word = word.Replace("ğ", "g");
        return word;
    }

       public bool ApidenKelimeOku(string kelime)
    {
        bool durum = false;
        string baslangic_harf = kelime.Substring(0, 1).ToLower(); // Kelimenin ilk harfini al
        kelime = "+" + KelimeyiDüzenle(kelime) + "+";
        string path = "KelimeApi/" + baslangic_harf; ///" + baslangic_harf + ".txt" //Resources dosyasının içerisinden baş harfine göre txt dosyasını git
        TextAsset level = Resources.Load<TextAsset>(path); //
        if (level != null)
        {
            using (StreamReader sr = new StreamReader(new MemoryStream(level.bytes)))
            {
                int sayi = sr.ReadToEnd().ToString().IndexOf(kelime);
                if (sayi != -1)
                {
                    durum = true;
                }
            }
        }
        else
        {
            Debug.Log("path yolu bulunamadı");
        }
        return durum;
    }

    public IEnumerator SonKelimeFadeOut( )
    {   
       
        for(float f=1f;f>=0f;f-=0.01f)
        {
             kelimeImageColor.a = f;
             SonKelimeColor.a = f;
             kelimeAnimasyon.GetComponent<Image>().color = kelimeImageColor;
             kelimeAnimasyon.GetComponentInChildren<Text>().color = SonKelimeColor;
             yield return new WaitForSeconds(0.01f);


        }

    }

    public IEnumerator LeveliEkranaYaz()
    {
        LevelPaneli.GetComponentInChildren<TextMeshProUGUI>().text = SceneManager.GetActiveScene().name;
        for(float i = 1 ; i >=0 ; i-= 0.01f)
        {
            Debug.Log("Çalışıyor.");
            yield return new WaitForSeconds(0.01f);
            LevelPaneli.GetComponent<CanvasGroup>().alpha = i;
        }
        LevelPaneli.gameObject.SetActive(false);
        //int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        
    }

    public void StartGame()
    {
        OyunHiziniDegistir(1);
        isGameActive = true;
        StartCoroutine(BalonOlustur());
        StartCoroutine(SpawnCilginJokerCoroutine());
       // StartCoroutine(SpawnSlowJokerCoroutine());
        StartCoroutine(LeveliEkranaYaz());
        
    }

    public IEnumerator SpawnCilginJokerCoroutine()
    {
 
        while(isGameActive)
        {
                yield return new WaitForSeconds(cilginJokerSpawnDelay);

                if(!Joker.isCilginMode && !Joker.isSlowMode)
                {
                    Instantiate(cilginModePrefab , jokerSpawnPoint,cilginModePrefab.transform.rotation);
                }

                else
                    yield return null;
               
            
       }
       
    }

    public IEnumerator SpawnSlowJokerCoroutine()
    {
            while(isGameActive)
            {
                    yield return new WaitForSeconds(12f);

                    if(!Joker.isSlowMode && !Joker.isCilginMode)
                    {   
                        Instantiate(slowModePrefab , jokerSpawnPoint,slowModePrefab.transform.rotation);
                    }

                    else
                        yield return null;
                    
                
            }
        
       
       
    }

    }

    