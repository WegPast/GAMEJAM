﻿using System.Collections;
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

  public Weapon WeaponLeft {
    get {
      return gunLeft.GetComponent<GunController>().GetAttachedWeapon().GetComponent<Weapon>();
    }
  }

  public Weapon WeaponRight {
    get {
      return gunRight.GetComponent<GunController>().GetAttachedWeapon().GetComponent<Weapon>();
    }
  }

  public GameObject body;
  private GameManager gameManager;

  private float deltaTimeFire;
  private float deltaTimeFire2;
  private LifeManager lifeManager;

  [SyncVar] bool isFiring;


  // Use this for initialization
  void Start()
  {
    if (isLocalPlayer)
    {
      GameObject.Find("Main Camera").GetComponent<CameraControl>().player = this.gameObject;
      GameObject.Find("Main Camera").transform.position = new Vector3(0f, 0f, -20f);

    }

      transform.position = new Vector3(0F, 0F, -5F);
    gameManager = FindObjectOfType<GameManager>();
    gameManager.IsGameStarted = true;
    GameManager.NbEnnemiesKilled = 0;

    if (!gunLeft)
    {
      Debug.LogError("No Left Gun attached, ma couille!!!");
    }
    if (!gunRight)
    {
      Debug.LogError("No Right Gun attached, ma couille!!!");
    }

    lifeManager = GetComponent<LifeManager>();

  }

  void Update()
  {
    if (isLocalPlayer)
    {
      if (Input.GetButton("Fire1")) { SendFiringState(true); }
      else { SendFiringState(false); }
      LookAtMouse();
      Move();
    }
    //Childs Animations
    HandleGunsAnimation();
    CheckPlayerLife();
  }

  private void HandleGunsAnimation()
  {
    if (isFiring)
    {
      this.gunLeft.GetComponent<GunController>().AnimationFiring();
      this.gunRight.GetComponent<GunController>().AnimationFiring();
    }
    this.gunLeft.GetComponent<GunController>().UpdateDeltaFiringTime(isFiring);
    this.gunRight.GetComponent<GunController>().UpdateDeltaFiringTime(isFiring);
  }

  void Move()
  {
    if (Input.GetKey(KeyCode.Z))
    {
      transform.Translate(Vector2.up * speed * Time.deltaTime);
    }
    if (Input.GetKey(KeyCode.S))
    {
      transform.Translate(Vector2.down * speed * Time.deltaTime);
    }
    if (Input.GetKey(KeyCode.D))
    {
      transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
    if (Input.GetKey(KeyCode.Q))
    {
      transform.Translate(Vector2.left * speed * Time.deltaTime);
    }
    transform.position = new Vector3(transform.position.x, transform.position.y, -5f);
  }

  void LookAtMouse()
  {
    Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    difference.Normalize();
    float rotation = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
    body.transform.rotation = Quaternion.Euler(0f, 0f, rotation - 90);
  }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    // Handle collision

  }

  public void ChangeGunWeapon(GameObject newWeapon)
  {
    this.gunRight.GetComponent<GunController>().ChangeWeapon(newWeapon);
    this.gunLeft.GetComponent<GunController>().ChangeWeapon(newWeapon);
  }

  //Client
  [Client]
  void SendFiringState(bool firingState)
  {
    CmdFiringState(firingState);
    if (firingState) CmdFire(); 
  }

  //Command
  [Command]
  void CmdFiringState(bool firingState)
  {
    isFiring = firingState;
  }

  [Command]
  public void CmdFire()
  {
    GameObject projectile;

    projectile = this.gunLeft.GetComponent<GunController>().FireGun(isLocalPlayer);
    if (projectile) NetworkServer.Spawn(projectile); 

    projectile = this.gunRight.GetComponent<GunController>().FireGun(isLocalPlayer);
    if (projectile) NetworkServer.Spawn(projectile); 
  }


  public void CheckPlayerLife() {
    if(lifeManager.lifeValue <= 0f) {
      Destroy(gameObject);
    }
  }

}
