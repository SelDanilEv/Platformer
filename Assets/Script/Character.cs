﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Unit
{
    [SerializeField]
    private int lives = 5;    //жизни
    [SerializeField]
    private float speed = 3.0F;   // скорость
    [SerializeField]
    private float jumpForce = 15.0F;   //сила прыжка

    private bool isGrounded = false;

    private Bullet bullet;         //шарик для урона

    private CharState State
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }

    new private Rigidbody2D rigidbody;
    private Animator animator; //Анимация 
    private SpriteRenderer sprite; // Поворот 


    private void Awake()     //принимает компоненты
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        bullet = Resources.Load<Bullet>("Bullet");
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()             //реакция на нажатие
    {
        if(isGrounded) State = CharState.Idle;

        if (Input.GetButtonDown("Fire1")) Shoot();
        if (Input.GetButton("Horizontal")) Run();
        if (/*isGrounded && */Input.GetButtonDown("Jump")) Jump();   // только если на земле
    }
        
    private void Run()          //бег
    {
        Vector3 direction = transform.right * Input.GetAxis("Horizontal"); //Направление движения 

        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

        sprite.flipX = direction.x < 0.0F;

       if(isGrounded) State = CharState.Run;
    }

    private void Jump()              //прыжок
    { 
        rigidbody.AddForce(transform.up * jumpForce,ForceMode2D.Impulse);  //вверх
    }

    private void Shoot()     
    {
        Vector3 position = transform.position; position.y += 1.0F;
         Bullet newBullet =  Instantiate(bullet, position, transform.rotation) as Bullet;
        newBullet.Parent = gameObject;
        newBullet.Direction = newBullet.transform.right * (sprite.flipX ? -1.0F : 1.0F);
    }

    public override void ReceiveDamage()
    {
        lives--;
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(transform.up * 8.0F, ForceMode2D.Impulse);
        Debug.Log(lives);
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position,0.3F);

        isGrounded = colliders.Length > 1;

        if (!isGrounded) State = CharState.Jump;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {

        Bullet bullet = collider.gameObject.GetComponent<Bullet>();
        if (bullet && bullet.Parent != gameObject)
            ReceiveDamage();

    }

}

public enum CharState
{
    Idle,
    Run,
    Jump
}