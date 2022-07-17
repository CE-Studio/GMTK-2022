using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// #   #  #### #####    #      ###   ####  ###  #        ####   ###   ####
// #   # #     #        #     #   # #     #   # #        #   # #   # #    
// #   #  ###  ###      #     #   # #     ##### #        #   # #   #  ### 
// #   #     # #        #     #   # #     #   # #        ####  #   #     #
//  ###  ####  #####    #####  ###   #### #   # #####    #      ###  #### 


public class Enemy : MonoBehaviour
{
    public BoxCollider2D box;
    public SpriteRenderer sprite;

    public Tilemap map;
    public string[] traversable;

    public Sprite idle1;
    public Sprite idle2;
    private float animTimer = 0;
    private float timerMax = 0;

    public Vector2Int intendedPos = Vector2Int.zero;
    private const float LERP_VALUE = 10;
    public Vector2Int lastPos = new Vector2Int(999, 999);
    
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        intendedPos = Vector2Int.FloorToInt(transform.localPosition);
        map = GameObject.Find("Grid/Elements").GetComponent<Tilemap>();
        EnemyManager.enemies.Add(this);
        timerMax = UnityEngine.Random.Range(0.5f, 1.5f);
        animTimer = UnityEngine.Random.Range(0f, 1f);
        sprite.flipX = UnityEngine.Random.Range(0, 2) == 1;
    }

    void Update()
    {
        animTimer = (animTimer + Time.deltaTime) % timerMax;
        sprite.sprite = animTimer < (timerMax * 0.5f) ? idle1 : idle2;

        transform.localPosition = Vector2.Lerp(transform.localPosition, new Vector2(intendedPos.x + 0.5f, intendedPos.y + 0.5f), LERP_VALUE * Time.deltaTime);
        box.offset = new Vector2(transform.localPosition.x - (intendedPos.x + 0.5f), transform.localPosition.y - (intendedPos.y + 0.5f));
    }

    public virtual void Move() {

    }

    public bool isTraversable(Vector2Int pos) {
        Tile tile = map.GetTile<Tile>(new Vector3Int(pos.x, pos.y, 0));
        boxControler temp;
        Enemy temp2;
        return (tile == null ? true : Array.IndexOf(traversable, tile.name) > -1) && !boxManager.getAt(pos, out temp) && !EnemyManager.getAt(pos, out temp2);
    }
    public bool isLocallyTraversable(Vector2Int pos) {
        Vector2Int h = Vector2Int.FloorToInt(intendedPos) + pos;
        return isTraversable(h);
    }

    public void moveLocally(Vector2Int pos) {
        intendedPos += new Vector2Int(pos.x, pos.y);
    }

    public bool CheckAllTilesBetween(Vector2Int posRelative) {
        int iterationCount = posRelative.x == 0 ? Mathf.Abs(posRelative.y) : Mathf.Abs(posRelative.x);
        Vector2Int normalizedDir = new Vector2Int(posRelative.x > 0 ? 1 : (posRelative.x < 0 ? -1 : 0), posRelative.y > 0 ? 1 : (posRelative.y < 0 ? -1 : 0));
        Vector2Int checkPos = Vector2Int.FloorToInt(transform.localPosition) + normalizedDir;
        bool blocked = false;
        for (int i = 0; i < iterationCount; i++) {
            blocked = blocked == true || isTraversable(checkPos);
            checkPos += normalizedDir;
        }
        //Debug.Log("(" + posRelative.x + ", " + posRelative.y + "), " + iterationCount + ", " + blocked);
        return blocked;
    }

    public void kill() {
        EnemyManager.removeEnemy(this);
        Destroy(gameObject);
    }
}
