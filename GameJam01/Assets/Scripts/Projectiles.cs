using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Projectiles : NetworkBehaviour
{

  private Rigidbody2D body;

  public Sprite iconSprite;
  public bool isFromMyPlayer = false;
  public float speed = 30f;
  public int damage = 10;

  // Use this for initialization
  void Awake()
  {
    transform.position = new Vector3(transform.position.x, transform.position.y, -5f);
  }

  private void FixedUpdate()
  {
    transform.Translate(new Vector3(0f, speed / 100, 0f));
  }

  public void Fire(Quaternion projectilRotation)
  {
    transform.rotation = projectilRotation;
  }

  private void OnTriggerEnter2D(Collider2D collision)
  {
    // Si le gameObject avec lequel le projectile entre en collision 
    // avec un gameObject ayant un component LifeManager alor on applique les dégats.
    LifeManager lifeManager = collision.gameObject.GetComponent<LifeManager>();
    if (lifeManager)
    {
      lifeManager.Hit(this.damage);
    }
    // Une fois l'application des dégat faite il faut destroy le gameObject.
    Destroy(gameObject);
  }
}
