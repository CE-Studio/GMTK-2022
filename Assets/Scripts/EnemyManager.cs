using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    public static List<Enemy> enemies = new List<Enemy>();

    public static void MoveEnemies() {
        foreach (Enemy enemy in enemies) {
            enemy.Move();
        }
    }

    public static bool getAt(Vector2Int pos, out Enemy result) {
        foreach (Enemy i in enemies) {
            if (Vector2Int.FloorToInt(i.intendedPos) == pos) {
                result = i;
                return true;
            }
        }
        result = null;
        return false;
    }
}
