using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyCollider : MonoBehaviour
{
    [SerializeField] float balonTrasperent =0.3f;
    private void OnTriggerEnter2D(Collider2D balloon)  
    { 
        BalonuYokEt(balloon); // OnTriggerEnter2d collider'a temas edince çalışacak fonksiyon.
        
    }

    private void BalonuYokEt(Collider2D balloon)
    {
        Color color = balloon.GetComponent<SpriteRenderer>().color ;
        color.a = balonTrasperent;
        balloon.GetComponent<SpriteRenderer>().color = color;
        balloon.GetComponent<Button>().interactable = false;
        balloon.GetComponent<CircleCollider2D>().enabled = false;
       //Balonun transperentını düşürüp sonrasında 0.2f saniyede yok ediyoruz.
        Destroy(balloon.gameObject,0.2f);

    }
}
