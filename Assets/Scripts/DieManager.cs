using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DieManager:MonoBehaviour {
    GameObject queueBar;
    Image queueImg;
    RectTransform queueRect;

    public int maxRolls = 3;
    public List<Vector2> currentQueue = new List<Vector2>();

    public Sprite[] spriteLib;

    public void Start() {
        queueBar = transform.GetChild(0).gameObject;
        queueImg = queueBar.GetComponent<Image>();
        queueRect = queueBar.GetComponent<RectTransform>();

        spriteLib = Resources.LoadAll<Sprite>("Images/Dice");
    }

    public void Update() {
        
    }

    private Sprite GetSprite(string name) {
        return spriteLib[name.ToLower().Replace(" ", "") switch {
            "move1" => 0,
            "m1" => 0,
            "move2" => 1,
            "m2" => 1,
            "move3" => 2,
            "m3" => 2,
            "move4" => 3,
            "m4" => 3,
            "move5" => 4,
            "m5" => 4,
            "move6" => 5,
            "m6" => 5,
            "turncw" => 6,
            "cw" => 6,
            "turnccw" => 7,
            "ccw" => 7,
            "action" => 8,
            _ => 0
        }];
    }
}
