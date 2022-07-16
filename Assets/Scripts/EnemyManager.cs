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
}
