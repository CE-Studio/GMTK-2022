using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class playerControler : MonoBehaviour {

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
    }

    void Update() {
        
    }

    bool isTraversable(Vector2Int pos) {
        Tile tile = playarea.GetTile<Tile>(new Vector3Int(pos.x, pos.y, 0));
        print(tile.name);
        return (Array.IndexOf(traversable, tile.name) > -1);
    }
}
