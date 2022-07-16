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
    public GameObject player;
    public playerControler playerScript;
    public string[] traversable;

    public Sprite idle1;
    public Sprite idle2;
    private float animTimer = 0;

    public Vector2Int intendedPos = Vector2Int.zero;
    private const float LERP_VALUE = 10;
    
    void Start()
    {
        box = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        intendedPos = new Vector2Int(Mathf.FloorToInt(transform.localPosition.x), Mathf.FloorToInt(transform.localPosition.y));
        map = GameObject.Find("Grid/Elements").GetComponent<Tilemap>();
        player = GameObject.Find("Grid/player");
        playerScript = player.GetComponent<playerControler>();
        EnemyManager.enemies.Add(gameObject);
    }

    void Update()
    {
        animTimer = (animTimer + Time.deltaTime) % 1;
        sprite.sprite = animTimer < 0.5f ? idle1 : idle2;

        transform.localPosition = Vector2.Lerp(transform.localPosition, new Vector2(intendedPos.x + 0.5f, intendedPos.y + 0.5f), LERP_VALUE * Time.deltaTime);
        box.offset = new Vector2(transform.localPosition.x - (intendedPos.x + 0.5f), transform.localPosition.y - (intendedPos.y + 0.5f));
    }

    public virtual void Move() {

    }

    public bool IsTraversible(Vector2Int pos) {
        Tile tile = map.GetTile<Tile>(new Vector3Int(pos.x, pos.y, 0));
        return tile == null || Array.IndexOf(traversable, tile.name) > -1;
    }
}
