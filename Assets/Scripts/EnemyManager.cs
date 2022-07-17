using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyManager
{
    public static List<Enemy> enemies = new List<Enemy>();
    public static playerControler playerScript = GameObject.Find("Grid/player").GetComponent<playerControler>();

    public static AudioClip openBlueDoors = Resources.Load<AudioClip>("Sounds/LevelDoorOpen");
    public static AudioClip killEnemy = Resources.Load<AudioClip>("Sounds/KillEnemy");

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
        playerControler.thePlayer.aplay(killEnemy);
    }

    public static void removeLevelDoors() {
        Tilemap elementMap = GameObject.Find("Grid/Elements").GetComponent<Tilemap>();
        foreach (Vector3Int position in elementMap.cellBounds.allPositionsWithin) {
            if (!elementMap.HasTile(position))
                continue;
            Tile thisTile = elementMap.GetTile<Tile>(position);
            if (thisTile.name.Contains("LevelDoor")) {
                elementMap.SetTile(position, null);
            }
        }
        playerControler.thePlayer.aplay(openBlueDoors);
    }
}
