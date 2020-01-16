using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Text;

public class GameSession : MonoBehaviour
{
    [SerializeField] GameObject[] balloons;
     string[] alfabe = {"A","B","C","Ç","D","E","F","G","Ğ","H","I","İ","J","K","L","M","N","O","Ö","P","R","S","Ş","T","U","Ü","V","Y","Z","A","E","İ","K","R","L"} ;
    //[SerializeField] GameObject[] spawnNoktaları;
    GameObject[] karakterSlots; 
    private int karakterSlotNumbers;
    GameObject sonKarakterSlot;
    //GameObject scoreGameObject;
    //GameObject hedefScoreGameObject;

    [SerializeField] int levelIcinSkor=10; // Her level için sabit kelimeSkor gibi.
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
    
 
    void Start()
    {
            OyunHiziniDegistir(1f); // Oyunun başlaması için timeScale'i 1 yapıyoruz.
            InvokeRepeating("BalonOlusturVeTextAta",0.1f,0.4f); // Oyun başladıktan 0.5 sn sonra ve her 0.2sn'de bir bu fonksiyonu çalıştır.
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
    
   

    private void Update()
    {
         sonKarakterSlot = karakterSlots[karakterSlotNumbers-1];
         kelimeSorgu=sonKarakterSlot.GetComponentInChildren<Text>().text;
         if(!string.IsNullOrEmpty(kelimeSorgu))
         {
            string kelimem=KarakterleriStringeCevir();
             if(ApidenKelimeOku(kelimem) && !kullanilanKelimeler.Contains(kelimem))
             {
                 kullanilanKelimeler.Add(kelimem);
                 Debug.Log("Kelime bulundu");
                 StartCoroutine(SpriteDegistir(dogruSprite));
                 KelimeSonucSound(dogruSound); //Sesler sıkıntılı daha sonra bak
                 gameScore.SkorEkle(kelimem);
                 toplamSkorInLevel+=levelIcinSkor * kelimem.Length;
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

             

             //Debug.Log(KarakterleriStringeCevir());
             //Debug.Log(kelimeSorgu);
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

    /*
    private void TextiBalonaAta()
    {
        string gelenHarf = RastgeleHarfUret(alfabe); //Daha sonrasında ingilizce versiyonu yapılabileceğinden parametre olarak veriyoruz.
        GameObject balon = RastgeleSpawnNoktasındaOlustur();
        balon.GetComponentInChildren<Text>().text = gelenHarf;
    }
    */
    

    private void BalonOlusturVeTextAta()
    {
        var gelenHarf = RastgeleHarfUret(alfabe);
        GameObject rastgeleBalon = balloons[Random.Range(0,4)]; //Tanımlı balonlardan birini seç
        float rastgeleSpawnPointX = Random.Range(1.5f,15.5f); //Oyun alanı sınırları içersindeki balonun oluşacağı pozisyon aralığı
        float y = 14.54f;
        float z =0f;
        Vector3 pos = new Vector3(rastgeleSpawnPointX,y,z);
        rastgeleBalon.GetComponentInChildren<Text>().text =gelenHarf;
        Instantiate(rastgeleBalon,pos,transform.rotation);
        

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

       public void SonEklenenHarfiSil()
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

       

       public void TümHarfleriSil()
       {
           //Sahnedeki RemoveAllChars  butonu tıklandığında bu metot ile karakterSlotların içi null yapılacak.
           for(int i=0;i<karakterSlotNumbers;i++)
           {
               karakterSlots[i].GetComponentInChildren<Text>().text = null;

           }
       }

       public IEnumerator TümHarfleriGecikmeliSil()
       {    
           yield return null;
           TümHarfleriSil();
           
           
           
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
       
        for(float f=1f;f>=0f;f-=0.05f)
        {
             kelimeImageColor.a = f;
             SonKelimeColor.a = f;
             kelimeAnimasyon.GetComponent<Image>().color = kelimeImageColor;
             kelimeAnimasyon.GetComponentInChildren<Text>().color = SonKelimeColor;
             yield return new WaitForSeconds(0.2f);


        }

    }


    }