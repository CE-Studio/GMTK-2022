using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager
{
    public static List<GameObject> enemies = new List<GameObject>();

    public static void MoveEnemies() {
        foreach (GameObject enemy in enemies) {
            switch (enemy.name) {
                default:
                case "Enemy blob":
                case "Enemy blob(Clone)":
                    enemy.GetComponent<EnemyBlob>().Move();
                    break;
            }
        }
    }
}
