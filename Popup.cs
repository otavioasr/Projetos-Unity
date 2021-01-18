using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Popup : MonoBehaviour
{
    const float DISAPPEAR_TIMER_MAX = .5f;

    TextMeshPro textMesh;
    float       disappearTimer = .5f; 
    Color       textColor;

    private void Awake ()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public static Popup Create(Vector3 position, string text, bool isCriticalHit, Color color)
    {
        Transform damagePopupTranform = Instantiate(GameAssets.Instance.pfPopup, position, Quaternion.identity);
        Popup damagePopup = damagePopupTranform.GetComponent<Popup>();
        damagePopup.Setup(text, isCriticalHit, color);

        return damagePopup;
    }

    public void Setup (string text, bool isCriticalHit, Color color)
    {
        textMesh.SetText(text);
        textMesh.color = color;

        if (isCriticalHit)
        {
            textMesh.fontSize = 10;
            textMesh.color = Color.red;
        }
            
        textColor = textMesh.color;
        disappearTimer = DISAPPEAR_TIMER_MAX;
    }

    void Update()
    {
        float moveYSpeed = .12f;
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        if (disappearTimer < DISAPPEAR_TIMER_MAX * .5f)
        {
            transform.localScale += Vector3.one * 0.1f * Time.deltaTime;
        }

        disappearTimer -= Time.deltaTime;

        if (disappearTimer < 0)
        {
            textColor.a -= 3f * Time.deltaTime;
            textMesh.color = textColor;

            if (textColor.a < 0)
                Destroy(gameObject);
        }
    }


}
