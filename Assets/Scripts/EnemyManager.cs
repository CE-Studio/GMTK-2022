using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyManager
{
    public static List<Enemy> enemies = new List<Enemy>();
    public static playerControler playerScript = GameObject.Find("Grid/player").GetComponent<playerControler>();

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

    public static void removeEnemy(Enemy enemy) {
        enemies.Remove(enemy);
        if (enemies.Count == 0) {
            removeLevelDoors();
        }
    }

    public static void removeLevelDoors() {
        Debug.Log("Searching for blue doors");
        Tilemap elementMap = GameObject.Find("Grid/Elements").GetComponent<Tilemap>();
        foreach (Vector3Int position in elementMap.cellBounds.allPositionsWithin) {
            if (!elementMap.HasTile(position))
                continue;
            Tile thisTile = elementMap.GetTile<Tile>(position);
            if (thisTile.name.Contains("LevelDoor")) {
                elementMap.SetTile(position, null);
                playerScript.endLevelTiles.Add(new Vector2Int(position.x, position.y));
            }
        }
    }
}
