using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxButton : interactableBase {



    void Update() {
        Enemy i;
        boxControler h;
        if (state != (isStoodOn() || EnemyManager.getAt(Vector2Int.FloorToInt(transform.localPosition), out i) || boxManager.getAt(Vector2Int.FloorToInt(transform.localPosition), out h))) {
            toggle();
        }
    }
}
