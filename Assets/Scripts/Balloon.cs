using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Balloon : MonoBehaviour
{
    [SerializeField] GameObject balonAnimasyonVFX; // Visual Effect
    [SerializeField] AudioClip balonPatmaSesi; //SFX Sound Effect
    [SerializeField] string balonText ; //Debug için
    GameSession gameSessions; // Script reference

    private void Start()
    {
        gameSessions = FindObjectOfType<GameSession>();
    }
      void OnMouseDown()
    { 
        // Mouse sol click veya mobilden dokulduğunda çalışacak fonksiyon.
        BalonYokOlmaEfekt();
        balonText = gameObject.transform.GetChild(0).GetComponentInChildren<Text>().text;
        //Debug.Log(balonText);
        gameSessions.KarakterSlotaTextAtama(balonText);
       
        
    }
    private void BalonYokOlmaEfekt()
    {
        BalonPatlamaSesi();
        Destroy(gameObject);
        BalonPatlamaAnimasyonu();

    }
    private void BalonPatlamaAnimasyonu()
    {
        GameObject patlamaAnimasyonu = Instantiate(balonAnimasyonVFX,transform.position,transform.rotation);
        Destroy(patlamaAnimasyonu,1f);

    }

    private void BalonPatlamaSesi()
    {
        AudioSource.PlayClipAtPoint(balonPatmaSesi,Camera.main.transform.position);
    }
}
