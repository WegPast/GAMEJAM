using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControl : MonoBehaviour
{

    [Header("Player properties")]
    public float speed = 1.0f;

    [Header("Player guns")]
    public GameObject projectile;
    public GameObject gunLeft, gunRight;

    private GameObject body;

    //Synchronized variables


    // Use this for initialization
    void Start() {

            transform.position = new Vector3(0F, 0F, -5F);
            body = transform.Find("Body").gameObject;
            if (!gunLeft) {
                Debug.LogError("No Left Gun attached, ma couille!!!");
            }
            if (!gunRight) {
                Debug.LogError("No Right Gun attached, ma couille!!!");
            }
    }

    void Update() {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        LookAtMouse();
        Move();
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Fire();
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
    }

    public void Fire() {
        GameObject bullet1 = Instantiate(projectile, gunLeft.transform.position, Quaternion.identity) as GameObject;
        bullet1.GetComponent<Projectiles>().Fire(body.transform.rotation);

        GameObject bullet2 = Instantiate(projectile, gunRight.transform.position, Quaternion.identity) as GameObject;
        bullet2.GetComponent<Projectiles>().Fire(body.transform.rotation);
    }

    void LookAtMouse() {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        float rotation = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        body.transform.rotation = Quaternion.Euler(0f, 0f, rotation - 90);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.GetComponent<Ennemy>()) {
            Destroy(gameObject);
        }
    }
}
