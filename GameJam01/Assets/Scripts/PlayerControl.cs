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

    private Weapon leftWeapon, rightWeapon;
    private float deltaTimeFire;

    //Synchronized variables


    // Use this for initialization
    void Start() {

        leftWeapon = gunLeft.GetComponent<GunController>().GetAttachedWeapon().GetComponent<Weapon>();
        rightWeapon = gunRight.GetComponent<GunController>().GetAttachedWeapon().GetComponent<Weapon>();

        transform.position = new Vector3(0F, 0F, -5F);
        //body = transform.Find("Body").gameObject;
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
        LookAtMouse();
        Move();
        //if (Input.GetKeyDown(KeyCode.Mouse0)) {
            if (isLocalPlayer) {
                CmdFire();
            }
        //}
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
        //GameObject bullet1 = Instantiate(projectile, gunLeft.transform.position, Quaternion.identity) as GameObject;
        //bullet1.GetComponent<Projectiles>().Fire(body.transform.rotation);
        //if (isLocalPlayer) {
        //    bullet1.GetComponent<Projectiles>().isFromMyPlayer = true;
        //}

        //GameObject bullet2 = Instantiate(projectile, gunRight.transform.position, Quaternion.identity) as GameObject;
        //bullet2.GetComponent<Projectiles>().Fire(body.transform.rotation);
        //if (isLocalPlayer) {
        //    bullet2.GetComponent<Projectiles>().isFromMyPlayer = true;
        //}

        //NetworkServer.Spawn(bullet1);
        //NetworkServer.Spawn(bullet2);

        if (Input.GetButton("Fire1") && this.deltaTimeFire >= 1 / leftWeapon.fireRate) {
            leftWeapon.FireProjectile(gunLeft);
            rightWeapon.FireProjectile(gunRight);
            this.deltaTimeFire = 0;
        }
        this.deltaTimeFire += Time.deltaTime;
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

    public void Die() {
        Destroy(gameObject);
    }
}
