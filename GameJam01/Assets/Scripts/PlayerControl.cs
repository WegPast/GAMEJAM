using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour
{

  [Header("Player properties")]
  public float speed = 1.0f;

  [Header("Player guns")]
  public GameObject projectile;

  public GameObject gunLeft, gunRight;

  public GameObject body;
  private GameManager gameManager;

  private float deltaTimeFire;
  private float deltaTimeFire2;

  // Use this for initialization
  void Start() {

    if (isLocalPlayer) {
      GameObject.Find("Main Camera").GetComponent<CameraControl>().player = this.gameObject;
      GameObject.Find("Main Camera").transform.position = new Vector3(0f, 0f, -20f);
    }

    transform.position = new Vector3(0F, 0F, -5F);
    gameManager = FindObjectOfType<GameManager>();
    gameManager.isGameStarted = true;
    GameManager.nbEnnemiesKilled = 0;

    if (!gunLeft) {
      Debug.LogError("No Left Gun attached, ma couille!!!");
    }
    if (!gunRight) {
      Debug.LogError("No Right Gun attached, ma couille!!!");
    }

  }

  void Update() {
    if (isLocalPlayer) {
      if (Input.GetButton("Fire1")) { CmdFire(); }
      LookAtMouse();
      Move();
    }

    
  }

  void Move() {
    if (Input.GetKey(KeyCode.Z)) {
      transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
    if (Input.GetKey(KeyCode.S)) {
      transform.Translate(Vector2.down * speed * Time.deltaTime);
    }
    if (Input.GetKey(KeyCode.D)) {
      transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
    if (Input.GetKey(KeyCode.Q)) {
      transform.Translate(Vector2.left * speed * Time.deltaTime);
    }
    transform.position = new Vector3(transform.position.x, transform.position.y, -5f);
  }

  [Command]
  public void CmdFire() {

    GameObject projectile;

    projectile = this.gunLeft.GetComponent<GunController>().FireGun(isLocalPlayer);
    if (projectile != null) { NetworkServer.Spawn(projectile); }

    projectile = this.gunRight.GetComponent<GunController>().FireGun(isLocalPlayer);
    if (projectile != null) { NetworkServer.Spawn(projectile); }

  }

  void LookAtMouse() {
    Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    difference.Normalize();
    float rotation = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
    body.transform.rotation = Quaternion.Euler(0f, 0f, rotation - 90);
  }

  private void OnCollisionEnter2D(Collision2D collision) {
    if (collision.gameObject.GetComponent<Ennemy>()) {
      Die();
    }
  }

  public void ChangeGunWeapon(GameObject newWeapon) {
    this.gunRight.GetComponent<GunController>().ChangeWeapon(newWeapon);
    this.gunLeft.GetComponent<GunController>().ChangeWeapon(newWeapon);
  }

  public void Die() {
    Destroy(gameObject);
  }
}
