using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public ParticleSystem explosion;
    public int bounceLimit = 3;
    
    private Rigidbody2D _rb;
    private Vector2 velocity;
    private bool exit = false;

    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _rb.velocity = transform.right * bulletSpeed;
        velocity = _rb.velocity;
        GameLogic.Ins.bullets.Add(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (exit)
        {
            var colided = col.gameObject;
            if (col.CompareTag("Wall"))
            {
                if (bounceLimit != 0)
                {
                    var bulletVelocity = _rb.velocity;
                    var normal = -colided.transform.right;
                    var reflection = Vector2.Reflect(bulletVelocity, normal).normalized * bulletSpeed;
                    transform.right = transform.position + (Vector3) reflection;
                    _rb.velocity = Vector2.Reflect(bulletVelocity, normal).normalized * bulletSpeed;
                    velocity = _rb.velocity;
                    bounceLimit--;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            if (col.CompareTag("Border"))
            {
                Destroy(gameObject);
            }

            if (col.CompareTag("Mouse"))
            {
                Instantiate(explosion,col.gameObject.transform.position,quaternion.identity);
                colided.SetActive(false);
                Destroy(gameObject);
                GameLogic.Ins.ControllerWin();
            }

            if (col.CompareTag("Controller"))
            {
                Instantiate(explosion,col.gameObject.transform.position,quaternion.identity);
                colided.SetActive(false);
                Destroy(gameObject);
                GameLogic.Ins.MouseWin();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        exit = true;
    }
}