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
}
