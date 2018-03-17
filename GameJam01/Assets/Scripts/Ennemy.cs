using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ennemy : NetworkBehaviour {

	private CharaMono currentTarget;
	private float contactDist = 0F;
	[Header("Enemy characteristic")]
	public float movementSpeed = 1F;

    //Synchronized variables
    [SyncVar] Vector2 synchronizedPosition;
    [SyncVar] Quaternion synchronizedRotation;

    // Use this for initialization
    void Start () {
        transform.position = new Vector3(0F, 0F, -5F);
    }
	
	// Update is called once per frame
	void Update () {

        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        if (currentTarget != null) {
            move();
		} else {
            DefineTarget();
		}

        //Network synchronisation
        SendPosition();
    }

    void move()
    {
        if (Vector2.Distance(transform.position, currentTarget.transform.position) >= contactDist)
        {
            // Init mouvement guide line
            Vector2 axe = currentTarget.transform.position - gameObject.transform.position;
            axe.Normalize();
            gameObject.transform.Translate(axe * movementSpeed * Time.deltaTime);
        }
    }

    void DefineTarget()
    {
        CharaMono[] potentialTarget = GameObject.FindObjectsOfType<CharaMono>();
        if (potentialTarget != null && potentialTarget.Length > 0)
        {
            int luckyBastard = Random.Range(1, potentialTarget.Length + 1);
            currentTarget = potentialTarget[luckyBastard - 1];
        }
    }

    //Network function

    //Client
    [Client]
    void SendPosition()
    {
        CmdSendMyPositionToServer(transform.position);
        CmdSendMyRotationToServer(transform.Find("enemy_sprite").rotation);
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
