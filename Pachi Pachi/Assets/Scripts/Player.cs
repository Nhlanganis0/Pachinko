using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float PlayerSpeed;
    public Rigidbody2D Ply;

    void Update()
    {
        Movement();
    }

    public void Movement()
    {
        float MoveX = Input.GetAxis("Horizontal");
        float MoveY = Input.GetAxis("Vertical");

        Vector2 Movement = Ply.velocity;
        Ply.velocity = new Vector2(MoveX * PlayerSpeed, MoveY * PlayerSpeed);
    }
}
