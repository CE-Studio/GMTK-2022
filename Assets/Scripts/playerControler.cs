using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class playerControler : MonoBehaviour {

    public SpriteRenderer sr;
    public Tilemap playarea;
    public string[] traversable;
    public DieManager manager;
    public GameObject pathArrowPrefab;

    public static playerControler thePlayer;

    [Serializable]
    public struct spritelist {
        public Sprite[] sprites;
    }

    public spritelist[] sprites;
    public int dir = 0;
    public int mode = 0;

    void Start() {
        transform.localPosition = new Vector3(Mathf.Floor(transform.localPosition.x) + 0.5f, Mathf.Floor(transform.localPosition.y) + 0.5f, transform.localPosition.z);
        thePlayer = this;
    }

    void Update() {
        
    }

    bool isLocallyTraversable(Vector2Int pos) {
        Vector3Int h = Vector3Int.FloorToInt(transform.localPosition);
        return (isTraversable(new Vector2Int(h.x + pos.x, h.y + pos.y)));
    }

    bool isTraversable(Vector2Int pos) {
        boxControler i;
        if (boxManager.bm.getAt(pos, out i))
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
                break;
            case 2:
                break;
            case 3:
                pickup();
                break;
            case 4:
                drop();
                break;
            case 5:
                break;
            case 6:
                break;
        }
        updateSprite();
    }

    void pickup() {
        if (mode != 1) {
            switch (dir) {
                case 0:
                    if (boxManager.bm.grab(Vector2Int.FloorToInt(transform.localPosition) + Vector2Int.down))
                        mode = 1;
                    break;
                case 1:
                    if (boxManager.bm.grab(Vector2Int.FloorToInt(transform.localPosition) + Vector2Int.left))
                        mode = 1;
                    break;
                case 2:
                    if (boxManager.bm.grab(Vector2Int.FloorToInt(transform.localPosition) + Vector2Int.up))
                        mode = 1;
                    break;
                case 3:
                    if (boxManager.bm.grab(Vector2Int.FloorToInt(transform.localPosition) + Vector2Int.right))
                        mode = 1;
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
                    }
                    break;
                case 1:
                    if (isLocallyTraversable(Vector2Int.left)) {
                        boxManager.lastheld.beingHeld = false;
                        Vector2 pos = Vector2Int.FloorToInt(transform.localPosition) + Vector2Int.left + new Vector2(0.5f, 0.5f);
                        boxManager.lastheld.transform.localPosition = pos;
                        mode = 0;
                    }
                    break;
                case 2:
                    if (isLocallyTraversable(Vector2Int.up)) {
                        boxManager.lastheld.beingHeld = false;
                        Vector2 pos = Vector2Int.FloorToInt(transform.localPosition) + Vector2Int.up + new Vector2(0.5f, 0.5f);
                        boxManager.lastheld.transform.localPosition = pos;
                        mode = 0;
                    }
                    break;
                case 3:
                    if (isLocallyTraversable(Vector2Int.right)) {
                        boxManager.lastheld.beingHeld = false;
                        Vector2 pos = Vector2Int.FloorToInt(transform.localPosition) + Vector2Int.right + new Vector2(0.5f, 0.5f);
                        boxManager.lastheld.transform.localPosition = pos;
                        mode = 0;
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
}
