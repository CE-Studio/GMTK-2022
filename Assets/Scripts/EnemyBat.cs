using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBat : Enemy
{
    public override void Move() {
        List<Vector2Int> potentialPos = new List<Vector2Int>();
        if (playerControler.thePlayer.transform.localPosition.x < intendedPos.x &&
            playerControler.thePlayer.transform.localPosition.y < intendedPos.y &&
            isTraversable(intendedPos + Vector2Int.down + Vector2Int.left) && lastPos != (intendedPos + Vector2Int.down + Vector2Int.left)) {
            potentialPos.Add(Vector2Int.down + Vector2Int.left);
        } else if (playerControler.thePlayer.transform.localPosition.x < intendedPos.x &&
            playerControler.thePlayer.transform.localPosition.y > intendedPos.y + 1 &&
            isTraversable(intendedPos + Vector2Int.up + Vector2Int.left) && lastPos != (intendedPos + Vector2Int.up + Vector2Int.left)) {
            potentialPos.Add(Vector2Int.up + Vector2Int.left);
        } else if (playerControler.thePlayer.transform.localPosition.x > intendedPos.x + 1 &&
            playerControler.thePlayer.transform.localPosition.y < intendedPos.y &&
            isTraversable(intendedPos + Vector2Int.down + Vector2Int.right) && lastPos != (intendedPos + Vector2Int.down + Vector2Int.right)) {
            potentialPos.Add(Vector2Int.down + Vector2Int.right);
        } else if (playerControler.thePlayer.transform.localPosition.x > intendedPos.x + 1 &&
            playerControler.thePlayer.transform.localPosition.y > intendedPos.y + 1 &&
            isTraversable(intendedPos + Vector2Int.up + Vector2Int.right) && lastPos != (intendedPos + Vector2Int.up + Vector2Int.right)) {
            potentialPos.Add(Vector2Int.up + Vector2Int.right);
        } else {
            if (playerControler.thePlayer.transform.localPosition.y < intendedPos.y &&
                isTraversable(intendedPos + Vector2Int.down) && lastPos != (intendedPos + Vector2Int.down)) {
                potentialPos.Add(Vector2Int.down);
            } else if (playerControler.thePlayer.transform.localPosition.y > intendedPos.y + 1 &&
                isTraversable(intendedPos + Vector2Int.up) && lastPos != (intendedPos + Vector2Int.up)) {
                potentialPos.Add(Vector2Int.up);
            } else {
                if (playerControler.thePlayer.transform.localPosition.x < intendedPos.x &&
                    isTraversable(intendedPos + Vector2Int.left) && lastPos != (intendedPos + Vector2Int.left)) {
                    potentialPos.Add(Vector2Int.left);
                } else if (playerControler.thePlayer.transform.localPosition.x > intendedPos.x + 1 &&
                    isTraversable(intendedPos + Vector2Int.right) && lastPos != (intendedPos + Vector2Int.right)) {
                    potentialPos.Add(Vector2Int.right);
                }
            }
        }

        lastPos = intendedPos;
        int checkID = Random.Range(0, potentialPos.Count - 1);
        Vector2Int chosenPos = potentialPos.Count > 0 ? potentialPos[checkID] : Vector2Int.zero;
        Vector2Int normalizedDir = new Vector2Int(chosenPos.x > 0 ? 1 : (chosenPos.x < 0 ? -1 : 0), chosenPos.y > 0 ? 1 : (chosenPos.y < 0 ? -1 : 0));
        for (int steps = chosenPos.x == 0 ? Mathf.Abs(chosenPos.y) : Mathf.Abs(chosenPos.x); steps > 0; steps--) {
            if (isLocallyTraversable(normalizedDir))
                moveLocally(normalizedDir);
        }
        sprite.flipX = intendedPos.x > transform.localPosition.x;
    }
}
