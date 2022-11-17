using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Rigidbody playerRB;
    public bool isKinematic = true;
    public bool moveLeft = false;
    public bool moveRight = false;


    void Update()
    {
        if (isKinematic == false)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime, Space.Self);
        }
    }

    void FixedUpdate()
    {
        if(moveLeft)
        {
            moveRight = false;
            transform.Translate(Vector3.left * speed * Time.deltaTime, Space.Self);
        }

        if (moveRight)
        {
            moveLeft = false;
            transform.Translate(Vector3.right * speed * Time.deltaTime, Space.Self);
        }

    }

}
