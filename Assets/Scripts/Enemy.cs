using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void HealthChanged(float health);

public delegate void CharacterRemoved();

public class Enemy : Character, IInteractable
{
    public event HealthChanged healthChanged;

    public event CharacterRemoved characterRemoved;


    [SerializeField]
    private CanvasGroup healthGroup;

    [SerializeField]
    private float initAggroRange;

    [SerializeField]
    private LootTable lootTable;

    [SerializeField]
    private Astar astar;

    [SerializeField]
    private LayerMask losMask;

    [SerializeField]
    private int damage;

    private bool canDoDamage = true;

    public Vector3 MyStartPosition { get; set; }

    public float MyAggroRange { get; set;  }

    public bool InRange
    {
        get
        {
            return Vector2.Distance(transform.position, MyTarget.position) < MyAggroRange;
        }
    }
    

    private IState currentState;

    public float MyAttackRange { get; set; }

    public float MyAttackTime { get; set; }

    [SerializeField]
    private Sprite portrait;

    public Sprite MyPortrait { get => portrait; }


    public Astar MyAstar { get => astar;  }

    protected override void Update()
    {
        if (IsAlive)
        {
            if (!IsAttacking)
            {
                MyAttackTime += Time.deltaTime;
            }

            currentState.Update();

            if(MyTarget != null && !Player.MyInstance.IsAlive)
            {
                ChangeState(new EvadeState()); 
            }
           
        }
        base.Update();

    }

    public Transform Select()
    {
        healthGroup.alpha = 1;

        return hitBox;

    }

    public  void DeSelect()
    {
        healthGroup.alpha = 0;
        healthChanged -= new HealthChanged(UiManager.MyInstance.UpdateTargetFrame);
        characterRemoved -= new CharacterRemoved(UiManager.MyInstance.HideTargetFrame);

        
    }
   
    public void DoDamage()
    {
        if (canDoDamage)
        {
            Player.MyInstance.TakeDamage(damage, transform);
            canDoDamage = false;
        }
    }

    public void CanDoDamage()
    {
        canDoDamage = true;
    }

    public override void TakeDamage(float damage, Transform source)
    {

        if (!(currentState is EvadeState))
        {
            if (IsAlive)
            {
                SetTarget(source);
                base.TakeDamage(damage, source);

                OnHealthChanged(health.MyCurrentValue);

                if (!IsAlive)
                {
                    Player.MyInstance.MyAttackers.Remove(this);
                    Player.MyInstance.GainXP(Xpmanager.CalculateXP(this as Enemy));
                }
            }

          
        }
       
    }

    protected void Awake()
    {
        SpriteRenderer sr;
        sr = GetComponent<SpriteRenderer>();
        sr.enabled = true;
        health.Initialize(initHealth, initHealth);
        MyStartPosition = transform.position;
        MyAggroRange = initAggroRange;
        MyAttackRange = 0.5f;
        ChangeState(new IdleState());
    }


    public void ChangeState(IState newState)
    {
        if(currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;

        currentState.Enter(this);
    }

    public void SetTarget(Transform source)
    {
        if(MyTarget == null && !(currentState is EvadeState))
        {
            float distance = Vector2.Distance(transform.position, source.position);
            MyAggroRange = initAggroRange;
            MyAggroRange += distance;
            MyTarget = source;
        }
    }

    public void Reset()
    {
        this.MyTarget = null;
        this.MyAggroRange = initAggroRange;
        this.MyHealth.MyCurrentValue = this.MyHealth.MyMaxValue;
        OnHealthChanged(health.MyCurrentValue);
    }

    public  void Interact()
    {
        if (!IsAlive)
        {
            List<Drop> drops = new List<Drop>();

            foreach(IInteractable interactable in Player.MyInstance.MyInteractables)
            {
                if(interactable is Enemy && !(interactable as Enemy).IsAlive)
                {
                    drops.AddRange((interactable as Enemy).lootTable.GetLoot());
                }
            }

            LootWindow.MyInstance.CreatePages(drops);
        }

    }

    public  void StopInteract()
    {
        LootWindow.MyInstance.Close();
    }

    public void OnHealthChanged(float health)
    {
        if (healthChanged != null)
        {
            healthChanged(health);
        }
    }

    public void OnCharacterRemoved()
    {
        if (characterRemoved != null)
        {
            characterRemoved();
        }

        Destroy(gameObject);
    }

    public bool CanSeePlayer()
    {
        Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), losMask);

        if(hit.collider != null)
        {
            return false;
        }

        return true;
    }
}
