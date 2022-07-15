using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DieManager:MonoBehaviour {
    public GameObject tray;
    public Image trayImg;
    public RectTransform trayRect;
    public GameObject queueBar;
    public Image queueImg;
    public RectTransform queueRect;

    public struct Die {
        public GameObject dieObj;
        public Image dieSprite;
        public GameObject actionObj;
        public Image actionSprite;
    };
    public List<Die> dice = new List<Die>();

    public Dictionary<string, Sprite> imageLib = new Dictionary<string, Sprite>();

    List<Vector3Int> dieStates = new List<Vector3Int>();
    // The X value is the die's current value
    // The Y value is the die's position in the queue (zero if not in queue)
    // The Z value is the current action

    private const float LERP_VALUE = 15;

    void Start() {
        tray = transform.GetChild(0).gameObject;
        trayImg = tray.GetComponent<Image>();
        trayRect = tray.GetComponent<RectTransform>();
        queueBar = transform.GetChild(1).gameObject;
        queueImg = queueBar.GetComponent<Image>();
        trayRect = queueBar.GetComponent<RectTransform>();
        for (int i = 2; i < transform.childCount; i++) {
            Die newDie = new Die {
                dieObj = transform.GetChild(i).gameObject,
                dieSprite = transform.GetChild(i).GetComponent<Image>(),
                actionObj = transform.GetChild(i).GetChild(0).gameObject,
                actionSprite = transform.GetChild(i).GetChild(0).GetComponent<Image>()
            };
            dice.Add(newDie);
        }
        for (int i = 0; i < dice.Count; i++)
            dieStates.Add(Vector3Int.zero);

        imageLib = new Dictionary<string, Sprite>
        {
            { "one", Resources.Load<Sprite>("Images/Dice_one") },
            { "two", Resources.Load<Sprite>("Images/Dice_two") },
            { "three", Resources.Load<Sprite>("Images/Dice_three") },
            { "four", Resources.Load<Sprite>("Images/Dice_four") },
            { "five", Resources.Load<Sprite>("Images/Dice_five") },
            { "six", Resources.Load<Sprite>("Images/Dice_six") },
            { "move", Resources.Load<Sprite>("Images/Dice_move") },
            { "ccw", Resources.Load<Sprite>("Images/Dice_ccw") },
            { "cw", Resources.Load<Sprite>("Images/Dice_cw") },
            { "attack", Resources.Load<Sprite>("Images/Dice_attack") },
            { "use", Resources.Load<Sprite>("Images/Dice_use") },
            { "wait", Resources.Load<Sprite>("Images/Dice_wait") }
        };
    }

    void Update() {
        for (int i = 0; i < dice.Count; i++) {
            //dice[i].dieObj.transform.localPosition = Vector2.Lerp(dice[i].dieObj.transform.localPosition, dieStates[i].y == 0 ?
            //    new Vector2(tray.transform.localPosition.x + (trayRect.localScale.x / 3 * (i + 1))) : new Vector2(),
            //    LERP_VALUE * Time.deltaTime
            //    );
        }
    }
}
