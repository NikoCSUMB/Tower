using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
  public Path route;
  private Waypoint[] myPath;

  public int coinWorth;
  public Image healthBar;
  public float maxHealth;
  private float health;
  public float speed = .25f;

  private int index = 0;
  private Vector3 nextWaypoint;
  private bool stop = false;

  public GameObject particle;
  public UnityEvent deathEvent;

  void Start()
  {
    health = maxHealth;
    myPath = route.path;
    transform.position = myPath[index].transform.position;
    Recalculate();
  }

  void Update()
  {
    if (!stop)
    {
      if ((transform.position - myPath[index + 1].transform.position).magnitude < .1f)
      {
        index = index + 1;
        Recalculate();
      }
      Vector3 moveThisFrame = nextWaypoint * Time.deltaTime * speed;
      transform.Translate(moveThisFrame);
    }

  }

  void Recalculate()
  {
    if (index < myPath.Length -1)
    {
      nextWaypoint = (myPath[index + 1].transform.position - myPath[index].transform.position).normalized;
    }
    else
    {
      stop = true;
    }
  }
  public void takeDamage(float damage){  
    health -= damage;
    healthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(50 * (health / maxHealth), 10);
    if (health <= 0){
      GameObject newParticle = GameObject.Instantiate(particle, this.transform.position, Quaternion.identity);
      Destroy(newParticle, 1f);
      deathEvent.Invoke();
      deathEvent.RemoveAllListeners();
      GameManager.master.addCoins(coinWorth);
      //health = maxHealth;
       Destroy(this.gameObject);
    }
  }

  void OnDestroy()
  {
    GameManager.master.CheckForEnemies();
  }
}
