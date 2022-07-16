using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class boxManager : MonoBehaviour {

    public boxControler[] boxes;
    [NonSerialized]
    public boxControler lastheld;

    public bool getAt(Vector2Int pos, out boxControler result) {
        foreach (boxControler i in boxes) {
            if (Vector2Int.FloorToInt(i.transform.localPosition) == pos) {
                result = i;
                return true;
            }
        }
        result = null;
        return false;
    }
}
