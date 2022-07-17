using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeTutorialText : MonoBehaviour
{
    Image img;
    Text text;
    private float timer;
    Color textBlank;
    Color textWhite;
    Color imgBlank;
    Color imgFilled;

    private void Start() {
        img = GetComponent<Image>();
        text = transform.GetChild(0).GetComponent<Text>();
        textBlank = new Color(1, 1, 1, 0);
        textWhite = Color.white;
        imgBlank = new Color(0, 0, 0, 0);
        imgFilled = new Color(0, 0, 0, 0.5f);
    }

    private void Update() {
        timer += Time.deltaTime;
        if (timer < 1.5f) {
            text.color = textBlank;
            img.color = imgBlank;
        } else if (timer >= 1.5f && timer < 3) {
            text.color = Color.Lerp(textBlank, textWhite, (timer - 1.5f) * 0.6667f);
            img.color = Color.Lerp(imgBlank, imgFilled, (timer - 1.5f) * 0.6667f);
        } else if (timer >= 3 && timer < 13) {
            text.color = textWhite;
            img.color = imgFilled;
        } else if (timer >= 13 && timer < 15.5f) {
            text.color = Color.Lerp(textWhite, textBlank, (timer - 13) * 0.6667f);
            img.color = Color.Lerp(imgFilled, imgBlank, (timer - 13) * 0.6667f);
        } else
            Destroy(gameObject);
    }
}
