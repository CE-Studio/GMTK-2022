using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class interactableBase : MonoBehaviour {

    public Sprite offsprite;
    public Sprite onsprite;
    public SpriteRenderer sr;
    public Vector2Int[] outputs;

    public bool state = false;

    public static readonly Dictionary<string, Tile> replacedict = new Dictionary<string, Tile>{ {"door1_open",  Resources.Load<Tile>("Tiles/door1_closed")},
                                                                                                {"door2_open",  Resources.Load<Tile>("Tiles/door2_closed")},
                                                                                                {"door3_open",  Resources.Load<Tile>("Tiles/door3_closed")},
                                                                                                {"door4_open",  Resources.Load<Tile>("Tiles/door4_closed")},
                                                                                                {"door5_open",  Resources.Load<Tile>("Tiles/door5_closed")},
                                                                                                {"door6_open",  Resources.Load<Tile>("Tiles/door6_closed")},
                                                                                                {"door7_open",  Resources.Load<Tile>("Tiles/door7_closed")},
                                                                                                {"door8_open",  Resources.Load<Tile>("Tiles/door8_closed")},
                                                                                                {"door9_open",  Resources.Load<Tile>("Tiles/door9_closed")},
                                                                                                {"door10_open", Resources.Load<Tile>("Tiles/door10_closed")},
                                                                                                {"door11_open", Resources.Load<Tile>("Tiles/door11_closed")},
                                                                                                {"door12_open", Resources.Load<Tile>("Tiles/door12_closed")},
                                                                                                {"door13_open", Resources.Load<Tile>("Tiles/door13_closed")},
                                                                                                {"door14_open", Resources.Load<Tile>("Tiles/door14_closed")},
                                                                                                {"door15_open", Resources.Load<Tile>("Tiles/door15_closed")},
                                                                                                {"door16_open", Resources.Load<Tile>("Tiles/door16_closed")},
                                                                                                {"door1_closed",  Resources.Load<Tile>("Tiles/door1_open")},
                                                                                                {"door2_closed",  Resources.Load<Tile>("Tiles/door2_open")},
                                                                                                {"door3_closed",  Resources.Load<Tile>("Tiles/door3_open")},
                                                                                                {"door4_closed",  Resources.Load<Tile>("Tiles/door4_open")},
                                                                                                {"door5_closed",  Resources.Load<Tile>("Tiles/door5_open")},
                                                                                                {"door6_closed",  Resources.Load<Tile>("Tiles/door6_open")},
                                                                                                {"door7_closed",  Resources.Load<Tile>("Tiles/door7_open")},
                                                                                                {"door8_closed",  Resources.Load<Tile>("Tiles/door8_open")},
                                                                                                {"door9_closed",  Resources.Load<Tile>("Tiles/door9_open")},
                                                                                                {"door10_closed", Resources.Load<Tile>("Tiles/door10_open")},
                                                                                                {"door11_closed", Resources.Load<Tile>("Tiles/door11_open")},
                                                                                                {"door12_closed", Resources.Load<Tile>("Tiles/door12_open")},
                                                                                                {"door13_closed", Resources.Load<Tile>("Tiles/door13_open")},
                                                                                                {"door14_closed", Resources.Load<Tile>("Tiles/door14_open")},
                                                                                                {"door15_closed", Resources.Load<Tile>("Tiles/door15_open")},
                                                                                                {"door16_closed", Resources.Load<Tile>("Tiles/door16_open")},
                                                                                                };

    public void toggle() {
        state = !state;
        if (state) {
            sr.sprite = onsprite;
        } else {
            sr.sprite = offsprite;
        }
    }
}
