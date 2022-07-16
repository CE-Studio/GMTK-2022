using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyBlob : Enemy
{
    public override void Move() {
        List<Vector2Int> potentialPos = new List<Vector2Int> { Vector2Int.up, Vector2Int.up, Vector2Int.up * 2, Vector2Int.left, Vector2Int.left, Vector2Int.left * 2,
            Vector2Int.right, Vector2Int.right, Vector2Int.right * 2, Vector2Int.down, Vector2Int.down, Vector2Int.down * 2 };
        if (player.transform.position.x < transform.position.x - 0.5f) {
            potentialPos.Add(Vector2Int.left);
            potentialPos.Add(Vector2Int.left * 2);
        }
        else if (player.transform.position.x > transform.position.x + 0.5f) {
            potentialPos.Add(Vector2Int.right);
            potentialPos.Add(Vector2Int.right * 2);
        }
        if (player.transform.position.y < transform.position.y - 0.5f) {
            potentialPos.Add(Vector2Int.down);
            potentialPos.Add(Vector2Int.down * 2);
        } else if (player.transform.position.y > transform.position.y + 0.5f) {
            potentialPos.Add(Vector2Int.up);
            potentialPos.Add(Vector2Int.up * 2);
        }

        Vector2Int chosenPos = potentialPos[Random.Range(0, potentialPos.Count - 1)];
        while (!IsTraversible(Vector2Int.FloorToInt(transform.position) + chosenPos)) {
            chosenPos = potentialPos[Random.Range(0, potentialPos.Count - 1)];
        }
        intendedPos = chosenPos;
    }
}