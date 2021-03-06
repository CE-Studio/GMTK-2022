using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySnake : Enemy
{
    public override void Move() {
        List<Vector2Int> potentialPos = new List<Vector2Int>();
        if (playerControler.thePlayer.transform.localPosition.y < intendedPos.y &&
            isTraversable(intendedPos + Vector2Int.down) && lastPos != (intendedPos + Vector2Int.down * 2)) {
            potentialPos.Add(Vector2Int.down * 2);
        } else if (playerControler.thePlayer.transform.localPosition.y > intendedPos.y + 1 &&
            isTraversable(intendedPos + Vector2Int.up) && lastPos != (intendedPos + Vector2Int.up * 2)) {
            potentialPos.Add(Vector2Int.up * 2);
        } else {
            if (playerControler.thePlayer.transform.localPosition.x < intendedPos.x &&
                isTraversable(intendedPos + Vector2Int.left) && lastPos != (intendedPos + Vector2Int.left * 2)) {
                potentialPos.Add(Vector2Int.left * 2);
            } else if (playerControler.thePlayer.transform.localPosition.x > intendedPos.x + 1 &&
                isTraversable(intendedPos + Vector2Int.right) && lastPos != (intendedPos + Vector2Int.right * 2)) {
                potentialPos.Add(Vector2Int.right * 2);
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
