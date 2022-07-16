using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class playerControler : MonoBehaviour {

    public SpriteRenderer sr;
    public Tilemap playarea;
    public string[] traversable;

    [Serializable]
    public struct spritelist {
        public Sprite[] sprites;
    }

    public spritelist[] sprites;
    public int dir = 0;
    public int mode = 0;

    void Start() {
        transform.localPosition = new Vector3(Mathf.Floor(transform.localPosition.x) + 0.5f, Mathf.Floor(transform.localPosition.y) + 0.5f, transform.localPosition.z);
        print(isTraversable(Vector2Int.zero));
    }

    void Update() {
        
    }

    bool isLocallyTraversable(Vector2Int pos) {
        Vector3Int h = Vector3Int.FloorToInt(transform.localPosition);
        return (isTraversable(new Vector2Int(h.x + pos.x, h.y + pos.y)));
    }

    bool isTraversable(Vector2Int pos) {
        Tile tile = playarea.GetTile<Tile>(new Vector3Int(pos.x, pos.y, 0));
        if (tile == null) return true;
        print(tile.name);
        return (Array.IndexOf(traversable, tile.name) > -1);
    }

    void updateSprite() {
        sr.sprite = sprites[dir].sprites[mode];
    }

    public void startMove(DieManager.Die[] instructions) {
        StartCoroutine(move(instructions));
    }

    IEnumerator move(DieManager.Die[] instructions) {
        foreach (DieManager.Die i in instructions) {
            switch (i.dieData.x) {
                default:
                    int h = i.dieData.y;
                    while (h > 1) {
                        walk();
                        h -= 1;
                        yield return new WaitForSeconds(1);
                    }
                    break;
                case 1:
                    turn(i.dieData.y == 2);
                    yield return new WaitForSeconds(1);
                    break;
                case 2:
                    action(i.dieData.y);
                    yield return new WaitForSeconds(1);
                    break;
            }
        }
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
    
    }
}
