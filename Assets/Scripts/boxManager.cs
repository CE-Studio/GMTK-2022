using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class boxManager {

    public static List<boxControler> boxes = new List<boxControler>();
    public static boxControler lastheld;

    public static bool getAt(Vector2Int pos, out boxControler result) {
        foreach (boxControler i in boxes) {
            if (Vector2Int.FloorToInt(i.transform.localPosition) == pos) {
                result = i;
                return !i.beingHeld;
            }
        }
        result = null;
        return false;
    }

    public static bool grab(Vector2Int pos) {
        boxControler i;
        if (getAt(pos, out i)) {
            lastheld = i;
            lastheld.beingHeld = true;
            return true;
        }
        return false;
    }
}
