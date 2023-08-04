using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour
{

    [SerializeField]
    private float speed;

    [SerializeField]
    private string type;

    private Vector2 direction;

    [SerializeField]
    private int level;

    private Rigidbody2D myRigidbody;

    public Transform MyTarget { get; set; }
    
    public Animator MyAnimator { get; set; }

    [SerializeField]
    protected float initHealth;

   

    public bool IsAttacking { get; set; }

    protected Coroutine actionRoutine;

    [SerializeField]
    protected Transform hitBox;

    [SerializeField]
    protected Stat health;

    public bool IsAlive
    {
        get
        {
            return health.MyCurrentValue > 0;
        }
    }

    public Stat MyHealth
    {
        get { return health;  }
    }

    public bool IsMoving
    {
        get
        {
            return Direction.x != 0 || Direction.y != 0;
        }
    }

    public Vector2 Direction { get => direction; set => direction = value; }

    public float Speed { get => speed; set => speed = value; }

    public string MyType { get => type;  }

    public int MyLevel { get => level; set => level = value; }

    protected virtual void Start()
    {
       

        myRigidbody = GetComponent<Rigidbody2D>();

        MyAnimator = GetComponent<Animator>();

        
    }

    
    protected virtual void Update()
    {
        HandleLayers();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        if (IsAlive)
        {
            myRigidbody.velocity = Direction.normalized * Speed;
        }

      
    }

    public void GetHealth(int health)
    {
        MyHealth.MyCurrentValue += health;
        CombatTextManager.MyInstance.CreateText(transform.position, health.ToString(), SCTTYPE.HEAL, true);
    }

    public void HandleLayers()
    {
        if (IsAlive)
        {
            if (IsMoving)
            {

                ActivateLayer("Walk");


                MyAnimator.SetFloat("x", Direction.x);
                MyAnimator.SetFloat("y", Direction.y);



            }
            else if (IsAttacking)
            {
                ActivateLayer("Attack");
            }
            else
            {
                ActivateLayer("Idle");
            }
           
        }
        else
        {
            ActivateLayer("Death");
        }


    }

    

    public void ActivateLayer(string layerName)
    {
        for (int i = 0; i < MyAnimator.layerCount; i++)
        {
            MyAnimator.SetLayerWeight(i, 0);
        }

        MyAnimator.SetLayerWeight(MyAnimator.GetLayerIndex(layerName), 1);
    }

   

    public virtual void TakeDamage(float damage,Transform source)
    {
        

        health.MyCurrentValue -= damage;
        CombatTextManager.MyInstance.CreateText(transform.position, damage.ToString(), SCTTYPE.DAMAGE, false); 
        if (health.MyCurrentValue <= 0)
        {

            myRigidbody.velocity = Direction;
            myRigidbody.velocity = Direction;
            GameManager.MyInstance.OnKillConfirmed(this);
            MyAnimator.SetTrigger("die");

           
        }
    }
}
