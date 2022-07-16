using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// #   #  #### #####    #      ###   ####  ###  #        ####   ###   ####
// #   # #     #        #     #   # #     #   # #        #   # #   # #    
// #   #  ###  ###      #     #   # #     ##### #        #   # #   #  ### 
// #   #     # #        #     #   # #     #   # #        ####  #   #     #
//  ###  ####  #####    #####  ###   #### #   # #####    #      ###  #### 

public class EnemyBlob : Enemy
{
    public override void Move() {
        List<Vector2Int> potentialPos = new List<Vector2Int>();
        if (playerControler.thePlayer.transform.localPosition.x < transform.localPosition.x - 0.5f) {
            potentialPos.Add(Vector2Int.left);
            potentialPos.Add(Vector2Int.left * 2);
        }
        else if (playerControler.thePlayer.transform.localPosition.x > transform.localPosition.x + 0.5f) {
            potentialPos.Add(Vector2Int.right);
            potentialPos.Add(Vector2Int.right * 2);
        }
        if (playerControler.thePlayer.transform.localPosition.y < transform.localPosition.y - 0.5f) {
            potentialPos.Add(Vector2Int.down);
            potentialPos.Add(Vector2Int.down * 2);
        } else if (playerControler.thePlayer.transform.localPosition.y > transform.localPosition.y + 0.5f) {
            potentialPos.Add(Vector2Int.up);
            potentialPos.Add(Vector2Int.up * 2);
        }

        int checkID = Random.Range(0, potentialPos.Count - 1);
        Vector2Int chosenPos = potentialPos.Count > 0 ? potentialPos[checkID] : Vector2Int.zero;
        while (!CheckAllTilesBetween(chosenPos) && potentialPos.Count > 0) {
            potentialPos.RemoveAt(checkID);
            if (potentialPos.Count > 0) {
                checkID = Random.Range(0, potentialPos.Count - 1);
                chosenPos = potentialPos[checkID];
            } else
                chosenPos = Vector2Int.zero;
        }
        intendedPos = Vector2Int.FloorToInt(transform.localPosition) + chosenPos;
        sprite.flipX = intendedPos.x > transform.localPosition.x;
    }
}
