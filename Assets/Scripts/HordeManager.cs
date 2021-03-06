using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class HordeManager : MonoBehaviour
{

  public Wave enemyWave;
  public Path enemyPath;
  public bool[] finished;


  IEnumerator Start()
  {
    finished = new bool[2];
    StartCoroutine(SpawnEnemies(enemyWave.coolDownBetweenSmallWave, 0));
    StartCoroutine(SpawnEnemies(enemyWave.coolDownBetweenLargeWave, 1));

    yield break;

  }

  IEnumerator SpawnEnemies(float cooldownBetweenWaves, int type)
  {
    finished[type] = new bool();
    for (int i = 0; i < enemyWave.groups.Length; i++)
    {
      if (enemyWave.groups[i].enemyTypes.Length > type) StartCoroutine(SpawnEnemy(enemyWave.groups[i].enemyTypes[type]));


      yield return new WaitForSeconds(cooldownBetweenWaves); // cooldown between groups
    }

    finished[type] = true;
    Debug.Log("done with wave");
    GameManager.master.CheckForEnemies();
  }

  IEnumerator SpawnEnemy(Type type)
  {
    for (int j = 0; j < type.number; j++)
      {
        Enemy spawnedEnemy = Instantiate(type.enemy).GetComponent<Enemy>();
        spawnedEnemy.route = enemyPath;
        yield return new WaitForSeconds(type.cooldown);

      }
  }





[Serializable]
public struct Type
{
  public GameObject enemy;
  public int number;
  public float cooldown;
}

[Serializable]
public struct Group
{
  public Type[] enemyTypes;
}

[Serializable]
public struct Wave
{
  public Group[] groups;
  public float coolDownBetweenSmallWave;
  public float coolDownBetweenLargeWave;
}


}
