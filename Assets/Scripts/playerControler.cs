using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System;

public class playerControler : MonoBehaviour {

    public SpriteRenderer sr;
    public Tilemap playarea;
    public string[] traversable;
    public DieManager manager;
    public GameObject pathArrowPrefab;
    public GameObject weapon;
    public Sprite[] weaponSprites;

    public static playerControler thePlayer;

    public List<Vector2Int> endLevelTiles = new List<Vector2Int>();
    private float deathTimer = 1;
    public Scene thisLevel;

    [Serializable]
    public struct spritelist {
        public Sprite[] sprites;
    }

    public spritelist[] sprites;
    public int dir = 0;  // 0 = down, 1 = left, 2 = up, 3 = right
    public int mode = 0; // 1 = normal, 2 = carrying box, 3 = attacking
    private int tempMode = 0;

    void Start() {
        transform.localPosition = new Vector3(Mathf.Floor(transform.localPosition.x) + 0.5f, Mathf.Floor(transform.localPosition.y) + 0.5f, transform.localPosition.z);
        thePlayer = this;
        thisLevel = SceneManager.GetActiveScene();
    }

    void Update() {
        Enemy temp;
        if (EnemyManager.getAt(Vector2Int.FloorToInt(transform.localPosition), out temp) && deathTimer == 1) {
            GetComponent<SpriteRenderer>().enabled = false;
            manager.setButtonState(false);
            deathTimer -= Time.deltaTime;
        }
        if (deathTimer != 1)
            deathTimer -= Time.deltaTime;
        if (deathTimer <= 0)
           loadLevel(thisLevel.name);

        if (endLevelTiles.Contains(Vector2Int.FloorToInt(transform.position))) {
            // End level code here
            Debug.Log("ay you beat it yay");
        }
    }

    public void loadLevel(string levelName) {
        manager.StopAllCoroutines();
        StopAllCoroutines();
        boxManager.boxes.Clear();
        EnemyManager.enemies.Clear();
        interactableBase.switches.Clear();
        SceneManager.LoadScene(levelName);
    }

    bool isLocallyTraversable(Vector2Int pos) {
        Vector3Int h = Vector3Int.FloorToInt(transform.localPosition);
        return (isTraversable(new Vector2Int(h.x + pos.x, h.y + pos.y)));
    }

    bool isTraversable(Vector2Int pos) {
        boxControler i;
        if (boxManager.getAt(pos, out i))
            return false;
        Enemy h;
        if (EnemyManager.getAt(pos, out h))
            return false;
        Tile tile = playarea.GetTile<Tile>(new Vector3Int(pos.x, pos.y, 0));
        if (tile == null) return true;
        print(tile.name);
        return (Array.IndexOf(traversable, tile.name) > -1);
    }

    void updateSprite() {
        sr.sprite = sprites[dir].sprites[mode];
    }

    public void startMove(DieManager.Die[] instructions) {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        StartCoroutine(move(instructions));
    }

    IEnumerator move(DieManager.Die[] instructions) {
        foreach (DieManager.Die i in instructions) {
            switch (i.dieData.x) {
                default:
                    int h = i.dieData.y;
                    while (h > 0) {
                        walk();
                        h -= 1;
                        yield return new WaitForSeconds(0.25f);
                    }
                    break;
                case 1:
                    turn(i.dieData.y == 2);
                    yield return new WaitForSeconds(0.25f);
                    break;
                case 2:
                    action(i.dieData.y);
                    yield return new WaitForSeconds(0.25f);
                    break;
            }
            manager.RemoveFrontDie();
        }
        manager.endTurn();
    }

    void walk() {
        switch (dir) {
            case 0:
                if (isLocallyTraversable(Vector2Int.down))
                    moveLocally(Vector2Int.down);
                break;
            case 1:
                if (isLocallyTraversable(Vector2Int.left))
                    moveLocally(Vector2Int.left);
                break;
            case 2:
                if (isLocallyTraversable(Vector2Int.up))
                    moveLocally(Vector2Int.up);
                break;
            case 3:
                if (isLocallyTraversable(Vector2Int.right))
                    moveLocally(Vector2Int.right);
                break;
        }
    }

    void moveLocally(Vector2Int pos) {
        transform.localPosition = new Vector3(Mathf.Floor(transform.localPosition.x) + 0.5f + pos.x, Mathf.Floor(transform.localPosition.y) + 0.5f + pos.y, transform.localPosition.z);
    }

    void turn(bool cw) {
        if (cw) {
            dir += 1;
        } else {
            dir -= 1;
        }
        if (dir > 3) {
            dir = 0;
        }
        if (dir < 0) {
            dir = 3;
        }
        updateSprite();
    }

    void action(int actmode) {
        switch (actmode) {
            case 1:
                stabOne();
                break;
            case 2:
                stabLine();
                break;
            case 3:
                pickup();
                break;
            case 4:
                drop();
                break;
            case 5:
                stabSwoosh();
                break;
            case 6:
                interact();
                break;
        }
        updateSprite();
    }

    void interact() {
        interactableBase i;
        if (interactableBase.getAt(Vector2Int.FloorToInt(transform.localPosition), out i)) i.press();
        switch (dir) {
            case 0:
                if (interactableBase.getAt(Vector2Int.FloorToInt(transform.localPosition) + Vector2Int.down, out i))
                    print("Pressing!");
                    i.press();
                break;
            case 1:
                if (interactableBase.getAt(Vector2Int.FloorToInt(transform.localPosition) + Vector2Int.left, out i))
                    i.press();
                break;
            case 2:
                if (interactableBase.getAt(Vector2Int.FloorToInt(transform.localPosition) + Vector2Int.up, out i))
                    i.press();
                break;
            case 3:
                if (interactableBase.getAt(Vector2Int.FloorToInt(transform.localPosition) + Vector2Int.right, out i))
                    i.press();
                break;
        }
    }

    IEnumerator stabOneAnim() {
        mode = 2;
        updateSprite();
        Vector2 currentDir = dir switch { 0 => Vector2.down, 1 => Vector2.left, 2 => Vector2.up, _ => Vector2.right };
        GameObject sword = Instantiate(weapon, transform);
        sword.transform.Rotate(new Vector3(0, 0, dir * -90));
        sword.transform.localPosition = Vector2.zero + (currentDir * 0.5f);
        float animTimer = 0;
        while (animTimer < 0.25f) {
            sword.transform.localPosition = Vector2.zero + (currentDir * Mathf.Sin(animTimer * 10f));
            animTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(sword);
        mode = tempMode;
        updateSprite();
    }

    IEnumerator stabLineAnim() {
        mode = 2;
        updateSprite();
        Vector2 currentDir = dir switch { 0 => Vector2.down, 1 => Vector2.left, 2 => Vector2.up, _ => Vector2.right };
        GameObject bow = Instantiate(weapon, transform);
        GameObject arrow = Instantiate(weapon, transform);
        SpriteRenderer bowSprite = bow.GetComponent<SpriteRenderer>();
        SpriteRenderer arrowSprite = arrow.GetComponent<SpriteRenderer>();
        bowSprite.sprite = weaponSprites[2];
        bowSprite.sortingOrder = 2;
        arrowSprite.sprite = weaponSprites[1];
        arrowSprite.sortingOrder = 1;
        bow.transform.Rotate(new Vector3(0, 0, dir * -90));
        arrow.transform.Rotate(new Vector3(0, 0, dir * -90));
        bow.transform.localPosition = Vector2.zero + currentDir * 0.5f;
        arrow.transform.localPosition = Vector2.zero + currentDir * 0.5f;
        float animTimer = 0;
        while (animTimer < 0.25f) {
            arrow.transform.localPosition = Vector2.zero + currentDir * Mathf.Lerp(0.5f, 5f, animTimer * 4);
            animTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(bow);
        Destroy(arrow);
        mode = tempMode;
        updateSprite();
    }

    IEnumerator stabSwooshAnim() {
        mode = 2;
        updateSprite();
        List<GameObject> swords = new List<GameObject> { Instantiate(weapon, transform), Instantiate(weapon, transform), Instantiate(weapon, transform),
            Instantiate(weapon, transform), Instantiate(weapon, transform), Instantiate(weapon, transform), Instantiate(weapon, transform), Instantiate(weapon, transform), };
        List<Vector2> dirs = new List<Vector2> { Vector2.down, (Vector2.down + Vector2.left).normalized, Vector2.left, (Vector2.left + Vector2.up).normalized,
            Vector2.up, (Vector2.up + Vector2.right).normalized, Vector2.right, (Vector2.right + Vector2.down).normalized};

        for (int i = 0; i < 8; i++) {
            swords[i].transform.localPosition = Vector2.zero + dirs[i];
            swords[i].transform.Rotate(new Vector3(0, 0, i * -45));
        }
        float animTimer = 0;
        while (animTimer < 0.25) {
            for (int i = 0; i < 8; i++)
                swords[i].transform.localPosition = Vector2.zero + (dirs[i] * Mathf.Sin(animTimer * 10f));
            animTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        for (int i = 7; i >= 0; i--)
            Destroy(swords[i]);
        mode = tempMode;
        updateSprite();
    }

    void pickup() {
        if (mode != 1) {
            switch (dir) {
                case 0:
                    if (boxManager.grab(Vector2Int.FloorToInt(transform.localPosition) + Vector2Int.down)) {
                        mode = 1;
                        tempMode = 1;
                    }
                    break;
                case 1:
                    if (boxManager.grab(Vector2Int.FloorToInt(transform.localPosition) + Vector2Int.left)) {
                        mode = 1;
                        tempMode = 1;
                    }
                    break;
                case 2:
                    if (boxManager.grab(Vector2Int.FloorToInt(transform.localPosition) + Vector2Int.up)) {
                        mode = 1;
                        tempMode = 1;
                    }
                    break;
                case 3:
                    if (boxManager.grab(Vector2Int.FloorToInt(transform.localPosition) + Vector2Int.right)) {
                        mode = 1;
                        tempMode = 1;
                    }
                    break;
            }
        }
    }

    void drop() {
        if (mode == 1) {
            switch (dir) {
                case 0:
                    if (isLocallyTraversable(Vector2Int.down)) {
                        boxManager.lastheld.beingHeld = false;
                        Vector2 pos = Vector2Int.FloorToInt(transform.localPosition) + Vector2Int.down + new Vector2(0.5f, 0.5f);
                        boxManager.lastheld.transform.localPosition = pos;
                        mode = 0;
                        tempMode = 0;
                    }
                    break;
                case 1:
                    if (isLocallyTraversable(Vector2Int.left)) {
                        boxManager.lastheld.beingHeld = false;
                        Vector2 pos = Vector2Int.FloorToInt(transform.localPosition) + Vector2Int.left + new Vector2(0.5f, 0.5f);
                        boxManager.lastheld.transform.localPosition = pos;
                        mode = 0;
                        tempMode = 0;
                    }
                    break;
                case 2:
                    if (isLocallyTraversable(Vector2Int.up)) {
                        boxManager.lastheld.beingHeld = false;
                        Vector2 pos = Vector2Int.FloorToInt(transform.localPosition) + Vector2Int.up + new Vector2(0.5f, 0.5f);
                        boxManager.lastheld.transform.localPosition = pos;
                        mode = 0;
                        tempMode = 0;
                    }
                    break;
                case 3:
                    if (isLocallyTraversable(Vector2Int.right)) {
                        boxManager.lastheld.beingHeld = false;
                        Vector2 pos = Vector2Int.FloorToInt(transform.localPosition) + Vector2Int.right + new Vector2(0.5f, 0.5f);
                        boxManager.lastheld.transform.localPosition = pos;
                        mode = 0;
                        tempMode = 0;
                    }
                    break;
            }
        }
    }

    public void visualizePath(DieManager.Die[] instructions) {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        int px = 0;
        int py = 0;
        int pr = dir;
        GameObject na = Instantiate(pathArrowPrefab, transform, false);
        na.transform.localPosition = new Vector3(px, py, 0);
        na.transform.localEulerAngles = new Vector3(0, 0, pr * -90);
        foreach (DieManager.Die i in instructions) {
            switch (i.dieData.x) {
                default:
                    int h = i.dieData.y;
                    while (h > 0) {
                        switch (pr) {
                            case 0:
                                if (isLocallyTraversable(new Vector2Int(px, py - 1))) {
                                    py -= 1;
                                    na = Instantiate(pathArrowPrefab, transform, false);
                                    na.transform.localPosition = new Vector3(px, py, 0);
                                    na.transform.localEulerAngles = new Vector3(0, 0, pr * -90);
                                }
                                break;
                            case 1:
                                if (isLocallyTraversable(new Vector2Int(px - 1, py))) {
                                    px -= 1;
                                    na = Instantiate(pathArrowPrefab, transform, false);
                                    na.transform.localPosition = new Vector3(px, py, 0);
                                    na.transform.localEulerAngles = new Vector3(0, 0, pr * -90);
                                }
                                break;
                            case 2:
                                if (isLocallyTraversable(new Vector2Int(px, py + 1))) {
                                    py += 1;
                                    na = Instantiate(pathArrowPrefab, transform, false);
                                    na.transform.localPosition = new Vector3(px, py, 0);
                                    na.transform.localEulerAngles = new Vector3(0, 0, pr * -90);
                                }
                                break;
                            case 3:
                                if (isLocallyTraversable(new Vector2Int(px + 1, py))) {
                                    px += 1;
                                    na = Instantiate(pathArrowPrefab, transform, false);
                                    na.transform.localPosition = new Vector3(px, py, 0);
                                    na.transform.localEulerAngles = new Vector3(0, 0, pr * -90);
                                }
                                break;
                        }
                        h -= 1;
                    }
                    break;
                case 1:
                    bool cw = i.dieData.y == 2;
                    if (cw) {
                        pr += 1;
                    } else {
                        pr -= 1;
                    }
                    if (pr > 3) {
                        pr = 0;
                    }
                    if (pr < 0) {
                        pr = 3;
                    }
                    na.transform.localEulerAngles = new Vector3(0, 0, pr * -90);
                    break;
                case 2:
                    break;
            }
        }
    }

    void stabOne() {
        StartCoroutine(stabOneAnim());
        switch (dir) {
            case 0:
                stabLocally(Vector2Int.down);
                break;
            case 1:
                stabLocally(Vector2Int.left);
                break;
            case 2:
                stabLocally(Vector2Int.up);
                break;
            case 3:
                stabLocally(Vector2Int.right);
                break;
        }
    }

    void stabLine() {
        StartCoroutine(stabLineAnim());
        for (int i = 1; i < 6; i++) {
            switch (dir) {
                case 0:
                    stabLocally(new Vector2Int(0, -i));
                    break;
                case 1:
                    stabLocally(new Vector2Int(-i, 0));
                    break;
                case 2:
                    stabLocally(new Vector2Int(0, i));
                    break;
                case 3:
                    stabLocally(new Vector2Int(i, 0));
                    break;
            }
        }
    }

    void stabSwoosh() {
        StartCoroutine(stabSwooshAnim());
        stabLocally(new Vector2Int(0, -1));
        stabLocally(new Vector2Int(-1, -1));
        stabLocally(new Vector2Int(-1, 0));
        stabLocally(new Vector2Int(-1, 1));
        stabLocally(new Vector2Int(0, 1));
        stabLocally(new Vector2Int(1, 1));
        stabLocally(new Vector2Int(1, 0));
        stabLocally(new Vector2Int(1, -1));
    }

    void stabLocally(Vector2Int pos) {
        Enemy hit;
        if (EnemyManager.getAt(Vector2Int.FloorToInt(transform.localPosition) + pos, out hit)) {
            hit.kill();
        }
    }
}
