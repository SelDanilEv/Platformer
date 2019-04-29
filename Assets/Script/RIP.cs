using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RIP : MonoBehaviour
{
    public SpriteRenderer sprite;

    public void Awake()
    {
        RIP Rip = new RIP();
        sprite = GetComponentInChildren<SpriteRenderer>();
        Rip = Resources.Load<RIP>("RIP");
    }
    public void OnTriggerEnter2D(Collider2D collider)
    {
        RIP Rip = collider.GetComponent<RIP>();
    }
    public void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position,0);
    }
}
