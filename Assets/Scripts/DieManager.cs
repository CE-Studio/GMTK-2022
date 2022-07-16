using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DieManager:MonoBehaviour {
    public physdieController dieRoller;
    public playerControler player;

    public Button[] buttons;

    GameObject queueBar;
    Image queueImg;
    RectTransform queueRect;

    public int maxRolls = 3;
    GameObject dieObj;
    public struct Die {
        public GameObject dieObj;
        public Image dieImg;
        public RectTransform dieRect;
        public Vector2Int dieData; // X = die type (move, turn, action); Y = data (how far to go, where to turn, what to do)
    };
    public List<Die> dice = new List<Die>();

    public Sprite[] spriteLib;

    private Vector2 dieSize = Vector2.zero;
    private const float LERP_VALUE = 15;
    private const float SPACE_BUFFER = 32;

    private int managerState = 0; // 0 = waiting, 1 = rolling
    private int lastDieRolled = 0; // 0 = move, 1 = turn, 2 = action

    public void Start() {
        setButtonState(true);
        queueBar = transform.GetChild(0).gameObject;
        queueImg = queueBar.GetComponent<Image>();
        queueRect = queueBar.GetComponent<RectTransform>();
        queueRect.anchorMin = new Vector2(0.5f, 0);
        queueRect.anchorMax = new Vector2(0.5f, 0);

        dieObj = Resources.Load<GameObject>("Objects/Die");

        dieSize = dieObj.GetComponent<RectTransform>().sizeDelta;
    }

    public void Update() {
        queueRect.sizeDelta = Vector2.Lerp(queueRect.sizeDelta, new Vector2(dieSize.x * maxRolls + SPACE_BUFFER * (maxRolls + 1), dieSize.y), LERP_VALUE * Time.deltaTime);
        for (int i = 0; i < dice.Count; i++) {
            if (dieSize == Vector2.zero)
                dieSize = new Vector2(dice[i].dieRect.rect.width, dice[i].dieRect.rect.height);
            dice[i].dieRect.localPosition = Vector2.Lerp(dice[i].dieRect.localPosition, new Vector2(queueRect.localPosition.x - ((SPACE_BUFFER + dieSize.x) * dice.Count * 0.5f) +
                (SPACE_BUFFER * (i + 1)) + (dieSize.x * i) + (dieSize.x * 0.5f) - (SPACE_BUFFER * 0.5f), queueRect.localPosition.y), LERP_VALUE * Time.deltaTime);
        }

        if (managerState == 1 && !dieRoller.isRolling()) {
            int dieData = lastDieRolled == 1 ? dieRoller.curside switch { 1 => 1, 2 => 1, 3 => 2, 4 => 2, 5 => 2, _ => 1 } : dieRoller.curside;
            AddDie(new Vector2Int(lastDieRolled, dieData));
            managerState = 0;
            setButtonState(true);
        }
    }

    void setButtonState(bool clickable) {
        foreach (Button i in buttons) {
            i.interactable = clickable;
        }
    }

    public void Roll(int mode) {
        if (managerState == 0 && dice.Count < maxRolls) {
            dieRoller.roll(mode);
            managerState = 1;
            lastDieRolled = mode;
            setButtonState(false);
        }
    }

    public void AddDie(Vector2Int data) {
        GameObject newDieObj = Instantiate(dieObj, transform, false);
        Die newDie = new Die { dieObj = newDieObj, dieData = data };
        newDie.dieRect = newDie.dieObj.GetComponent<RectTransform>();
        newDie.dieImg = newDie.dieObj.GetComponent<Image>();
        newDie.dieImg.sprite = GetSprite(data);
        newDie.dieRect.localPosition = new Vector2(queueRect.localPosition.x, queueRect.localPosition.y + 512);
        dice.Add(newDie);
    }

    public void RemoveFrontDie() {
        if (dice.Count > 0) {
            Destroy(dice[0].dieObj);
            dice.RemoveAt(0);
        }
    }

    private Sprite GetSprite(Vector2Int data) {
        return spriteLib[data.x switch {
            0 => data.y switch { 1 => 0, 2 => 1, 3 => 2, 4 => 3, 5 => 4, _ => 5 },
            1 => data.y switch { 1 => 7, _ => 6 },
            _ => data.y switch { 1 => 9, 2 => 11, 3 => 12, 4 => 13, 5 => 10, _ => 8 }
        }];
    }

    public void beginTurn() {
        player.startMove(dice.ToArray());
        setButtonState(false);
    }

    public void endTurn() {
        setButtonState(true);
    }
}
