using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Balloon : MonoBehaviour
{
    [SerializeField] AudioClip balonPatmaSesi; //SFX Sound Effect
    [SerializeField] string balonText ; //Debug için
    [SerializeField] Sprite[] baloonSprites;
    public float speed;
    public float cilginSpeed = 2f;

    private GameSession gameSessions; // Script reference
    private TimerControll timerControll;
    
    

    private void Start()
    {
        transform.position = RandomPos();
        speed = RandomSpeed();
        gameSessions = FindObjectOfType<GameSession>();
        timerControll = GameObject.Find("PauseControl").GetComponent<TimerControll>();
    }

    private void Update()
    {
        
        if (Joker.isCilginMode)
        {
            transform.Translate(Vector3.up * Time.deltaTime * cilginSpeed); // yukarı yönde

        }

        else{
             transform.Translate(Vector3.up * Time.deltaTime * speed); // yukarı yönde

        }


        

    }

 
    void OnMouseDown()
    {
        Debug.Log("Tıklandı.");
        if(gameSessions.isGameActive && transform.position.y > 3.3f)
        {
            //BalonYokOlmaEfekt();
            balonText = gameObject.transform.GetChild(0).GetComponentInChildren<Text>().text;
            gameSessions.KarakterSlotaTextAtama(balonText);
            StartCoroutine(BalonPatlamaAnimasyonu(baloonSprites));
        }
        // Mouse sol click veya mobilden dokulduğunda çalışacak fonksiyon.
        
       
        
    }

    private IEnumerator BalonPatlamaAnimasyonu(Sprite[] baloonSprites)
    {
        BalonPatlamaSesi();
        balonText = "";
        GetComponentInChildren<Text>().text = balonText;
        for (int i=0; i<baloonSprites.Length;i++)
        {
           yield return new WaitForSeconds(0.002f);
           GetComponent<SpriteRenderer>().sprite = baloonSprites[i];


        }
        Destroy(this.gameObject);


    }

    private Vector3 RandomPos()
    {
        float rastgeleSpawnPointX = Random.Range(9.5f, 12f); //Oyun alanı sınırları içersindeki balonun oluşacağı pozisyon aralığı
        float y = 0.25f;
        float z = 0f;
        Vector3 pos = new Vector3(rastgeleSpawnPointX, y, z);
        return pos;
    }

    private float RandomSpeed()
    {
        var hiz = Random.Range(0.9f,1f);
        return hiz;
    }
    /*
    private void BalonYokOlmaEfekt()
    {
        BalonPatlamaSesi();
        Destroy(gameObject);
        BalonPatlamaAnimasyonu();

    }
    */
    /*
    private void BalonPatlamaAnimasyonu()
    {
        GameObject patlamaAnimasyonu = Instantiate(balonAnimasyonVFX,transform.position,transform.rotation);
        Destroy(patlamaAnimasyonu,1f);

    }
    */

    private void BalonPatlamaSesi()
    {
        AudioSource.PlayClipAtPoint(balonPatmaSesi,Camera.main.transform.position);
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(this.gameObject);
    }
    */
}
