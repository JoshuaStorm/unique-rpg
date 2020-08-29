using System;
using UnityEngine;

public class ClickToAttack : MonoBehaviour
{
    public CharacterController characterController;
    public AnimationClip attackAnimationClip;
    public float attackRange;
    public float damage;
    public float procTimePercent = 0.5f;

    private EnemyBehavior target;
    private EnemyBehavior targetAtAttackTime;
    private float attackStartTime; // TODO: sooo, this will overflow eventually... I think practically it never will though, right?
    private bool hasAttackProced;
    private float procTimeAbsolute;

    // Start is called before the first frame update
    void Start()
    {
        this.attackStartTime = -1f;
        this.target = null;
        this.targetAtAttackTime = null;
        this.hasAttackProced = false;
        this.procTimeAbsolute = this.attackAnimationClip.length * this.procTimePercent;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.IsAttacking() && this.HasSurpassedProcTime() && !this.hasAttackProced)
        {
            this.HitTarget();
        }
    }

    public void HandleLeftClick()
    {
        if (this.HasTargetWithinRange() && !this.IsAttacking())
        {
            this.Attack();
        }
    }

    public void SetTarget(EnemyBehavior target)
    {
        this.target = target;
    }

    public EnemyBehavior GetTarget()
    {
        return this.target;
    }

    public bool HasTarget()
    {
        return this.target != null;
    }

    public bool IsAttacking()
    {
        return this.attackStartTime > 0f && (Time.time - this.attackStartTime) < this.attackAnimationClip.length;
    }

    public float GetAttackRange()
    {
        return this.attackRange;
    }

    private void Attack()
    {
        this.attackStartTime = Time.time;
        this.hasAttackProced = false;
        this.targetAtAttackTime = this.target;
        var animation = this.characterController.GetComponent<Animation>();
        animation.CrossFade(this.attackAnimationClip.name);
    }
    
    private void HitTarget()
    {
        if (this.targetAtAttackTime == null)
        {
            throw new ApplicationException("HitTarget() called with targetAtAttackTime==null");
        }
        this.hasAttackProced = true;
        var targetEnemyBehavior = this.targetAtAttackTime.GetComponent<EnemyBehavior>();
        targetEnemyBehavior.TakeHit(this.damage);
        this.targetAtAttackTime = null;
    }

    private bool HasSurpassedProcTime()
    {
        var animation = this.characterController.GetComponent<Animation>();
        var attackAnimation = animation[this.attackAnimationClip.name];
        return attackAnimation.time > this.procTimeAbsolute;
    }

    private bool HasTargetWithinRange()
    {
        return this.target != null 
            && Vector3.Distance(this.target.transform.position, this.transform.position) <= this.attackRange;
    }
}
