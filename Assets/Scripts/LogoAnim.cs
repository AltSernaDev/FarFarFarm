using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LogoAnim : MonoBehaviour
{
    public Image im1,im2,im3,im4;
    public Color alphaCol,col;

    private void Awake()
    {
        StartCoroutine(introAnimation());
    }

    IEnumerator introAnimation()
    {
        im1.gameObject.SetActive(true);
        im2.gameObject.SetActive(true);
        im3.gameObject.SetActive(true);
        im4.gameObject.SetActive(true);

        im1.DOColor(col, 3);
        im2.DOColor(col, 3);
        im3.DOColor(col, 3);
        im4.DOColor(col, 3);

        yield return new WaitForSeconds(3.4f);

        //Apagar
        im1.DOColor(alphaCol, 3);
        im2.DOColor(alphaCol, 3);
        im3.DOColor(alphaCol, 3);
        im4.DOColor(alphaCol, 3);

        yield return new WaitForSeconds(2.5f);
        im1.gameObject.SetActive(false);
        im2.gameObject.SetActive(false);
        im3.gameObject.SetActive(false);
        im4.gameObject.SetActive(false);
    }
}
