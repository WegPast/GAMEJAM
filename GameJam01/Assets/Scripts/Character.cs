using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Character : NetworkBehaviour
{

    public float speed = 5.0F;

    //Synchronized variables
    [SyncVar] Vector2 synchronizedPosition;
    [SyncVar] Quaternion synchronizedRotation;


    // Use this for initialization
    void Start()
    {
        transform.position = new Vector3(0F, 0F, -5F);
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            transform.position = Vector2.Lerp(transform.position, synchronizedPosition, Time.deltaTime * 10);
            transform.Find("Spaceship_base1").rotation = synchronizedRotation;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            Rotate();
            move();
            attack();
            //Network synchronisation
            SendPosition();
        }
    }

    void move()
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
    }

    void attack()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {

        }
    }

    void Rotate()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        float rotation = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.Find("Spaceship_base1").rotation = Quaternion.Euler(0f, 0f, rotation - 90);
    }

    //Network function

    //Client
    [Client]
    void SendPosition()
    {
        CmdSendMyPositionToServer(transform.position);
        CmdSendMyRotationToServer(transform.Find("Spaceship_base1").rotation);
    }


    //Command
    [Command]
    void CmdSendMyPositionToServer(Vector2 position)
    {
        synchronizedPosition = position;
    }

    [Command]
    void CmdSendMyRotationToServer(Quaternion rotation)
    {
        synchronizedRotation = rotation;
    }
}
