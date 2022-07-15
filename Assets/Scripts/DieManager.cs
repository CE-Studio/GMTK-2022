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
    GameObject dieObj;
    public struct Die {
        public GameObject dieObj;
        public Image dieImg;
        public RectTransform dieRect;
        public Vector2 dieData; // X = die type (move, turn, action); Y = data (how far to go, where to turn, what to do)
    };
    public List<Die> dice = new List<Die>();

    public Sprite[] spriteLib;

    private readonly Vector2 dieSize = new Vector2(128, 128);
    private const float LERP_VALUE = 15;
    private const float SPACE_BUFFER = 32;

    public void Start() {
        queueBar = transform.GetChild(0).gameObject;
        queueImg = queueBar.GetComponent<Image>();
        queueRect = queueBar.GetComponent<RectTransform>();
        queueRect.anchorMin = new Vector2(0.5f, 0);
        queueRect.anchorMax = new Vector2(0.5f, 0);

        dieObj = Resources.Load<GameObject>("Objects/Die");
        spriteLib = Resources.LoadAll<Sprite>("Images/Dice");
    }

    public void Update() {
        queueRect.sizeDelta = Vector2.Lerp(queueRect.sizeDelta, new Vector2(dieSize.x * maxRolls + SPACE_BUFFER * (maxRolls + 1),
            dieSize.y + SPACE_BUFFER * 2), LERP_VALUE * Time.deltaTime);
        for (int i = 0; i < dice.Count; i++) {
            //dice[i].dieRect.position = Vector2.Lerp(dice[i].dieRect.position, new Vector2(queueRect.position.x - (queueRect.sizeDelta.x * 0.5f) +
            //    (SPACE_BUFFER * i) + (dieSize.x * (i - 1)) + (dieSize.x * 0.5f), queueRect.position.y), LERP_VALUE * Time.deltaTime);
            dice[i].dieRect.position = Vector2.Lerp(dice[i].dieRect.position, new Vector2(queueRect.position.x - ((SPACE_BUFFER + dieSize.x) *(dice.Count - 1) * 0.5f) +
                (SPACE_BUFFER * i) + (dieSize.x * (i - 1)) + (dieSize.x * 0.5f), queueRect.position.y), LERP_VALUE * Time.deltaTime);
        }
    }

    public void AddDie(string name, Vector2 data) {
        GameObject newDieObj = Instantiate(dieObj);
        Die newDie = new Die { dieObj = newDieObj, dieData = data };
        newDie.dieRect = newDie.dieObj.GetComponent<RectTransform>();
        newDie.dieImg = newDie.dieObj.GetComponent<Image>();
        newDie.dieImg.sprite = GetSprite(name);
        newDie.dieRect.position = new Vector2(queueRect.position.x, queueRect.position.y - 256);
        dice.Add(newDie);
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
