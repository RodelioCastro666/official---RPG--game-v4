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

    protected override void Update()
    {
        if (IsAlive)
        {
            if (!IsAttacking)
            {
                MyAttackTime += Time.deltaTime;
            }

            currentState.Update();

           
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
   

    public override void TakeDamage(float damage, Transform source)
    {

        if (!(currentState is EvadeState))
        {
            SetTarget(source);
            base.TakeDamage(damage, source);

            OnHealthChanged(health.MyCurrentValue);
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
            lootTable.ShowLoot();
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
}
