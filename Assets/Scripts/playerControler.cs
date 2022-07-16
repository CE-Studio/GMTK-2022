using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class playerControler : MonoBehaviour {

    [Serializable]
    public struct spritelist {
        public Sprite[] sprites;
    }

    public spritelist[] sprites;
    public int dir = 0;
    public int mode = 0;

    void Start() {
        
    }

    void Update() {
        
    }

    bool isTraversable(Vector2Int pos) {

    }
}
