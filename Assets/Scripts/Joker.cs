using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Joker : MonoBehaviour
{
    private float rotateAngle;
    private float angleBound = 15f;
    private float rotateSpeed = 3f;
    private float upSpeed = 2f;

    public static bool isCilginMode = false;
    public static bool isSlowMode = false;

    [SerializeField] float cilginModSure = 20f; // Joker aktiflik süreleri
    [SerializeField] float slowModeSure = 20f;

    private GameSession gameSession;

    // Start is called before the first frame update
    void Start()
    {
        rotateAngle = Random.Range(-angleBound,angleBound);
        gameSession = GameObject.Find("Game Session").GetComponent<GameSession>();
    }

    private void OnDisable() {
        isCilginMode = false;
        StopAllCoroutines();
        
    }


    private void OnMouseDown() 
    {
        if(gameSession.isGameActive)
        {
             if(gameObject.CompareTag("cilginMod"))
                {
                Debug.Log(gameObject.tag);
                CilginMod();
                DisableObject();
                }

            else if (gameObject.CompareTag("slowMode"))
            {
                Debug.Log(gameObject.tag);
                SlowMode();
                DisableObject();
            //  Destroy(this.gameObject);

            }
        }
       
        
    }

    public void DisableObject()
    {
         this.GetComponent<SpriteRenderer>().enabled = false;
         this.GetComponent<BoxCollider2D>().enabled = false;
    }

     void CilginMod()
    {
        StartCoroutine(CilginModeAktifSure());
        isCilginMode = true;
        GameScore.kelimeSkor *= 2;
        //gameSession.StopCoroutine(gameSession.SpawnCilginJokerCoroutine());
        //gameSession.levelIcinSkor *= 2; // Çılgın Mod boyunca skor 2x
        

    }

      IEnumerator CilginModeAktifSure()
        {
             yield return new WaitForSeconds(cilginModSure);
             isCilginMode = false;
             GameScore.kelimeSkor /= 2;
            // gameSession.StartCoroutine(gameSession.SpawnCilginJokerCoroutine());
            //gameSession.levelIcinSkor /= 2;
        
       
        }

      void SlowMode()
        {
            StartCoroutine(SlowModeAktifSure());
            isSlowMode = true;
            Time.timeScale = 0.5f;        

        }

     IEnumerator SlowModeAktifSure()
        {
            yield return new WaitForSeconds(slowModeSure);
            isSlowMode = false ;
            Time.timeScale = 1;
        }

    

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * upSpeed * Time.deltaTime);
        //transform.Rotate(Vector3.forward, rotateAngle * Time.deltaTime * rotateSpeed);
        
    }
}
