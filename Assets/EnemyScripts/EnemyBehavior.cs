using System;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public LootBehavior lootBehavior;
    public CharacterController characterController;
    public AnimationClip attackAnimationClip;
    public AnimationClip moveAnimationClip;
    public AnimationClip standAnimationClip;
    public AnimationClip deathAnimationClip;
    public float speed;
    public float attackRange;
    public float maxHealth;
    public float damage = 1f;
    public float procTimePercent = 0.5f;

    public GameObject playerCharacter;
    private GameObject targetAtAttackTime;

    private float RotationalVelocity;
    private float procTimeAbsolute;
    private float attackStartTime; // TODO: sooo, this will overflow eventually...
    private bool hasAttackProced;

    private float currentHealth;
    private bool isDead;

    public void TakeHit(float damage)
    {
        this.currentHealth = Math.Max(this.currentHealth - damage, 0f);
        Debug.Log($"Took hit, now HP={this.currentHealth}");
    }

    // Start is called before the first frame update
    void Start()
    {
        this.RotationalVelocity = Time.deltaTime * 10f;

        this.attackStartTime= -1f;
        this.currentHealth = this.maxHealth;
        this.isDead = this.currentHealth <= 0f;
        this.hasAttackProced = false;
        this.procTimeAbsolute = this.attackAnimationClip.length * this.procTimePercent;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.isDead)
        {
            return;
        }

        if (this.currentHealth <= 0f)
        {
            this.Die();
            return;
        }

        this.LookAtPlayer();
        if (!this.WithinRangeOfPlayer())
        {
            this.MoveTowardPlayer();
        }
        else if (!this.IsAttacking())
        {
            this.Attack();
        }

        if (this.IsAttacking() && this.HasSurpassedProcTime() && !this.hasAttackProced)
        {
            this.HitTarget();
        }
    }

    void OnMouseOver()
    {
        var playerClickToAttack = this.playerCharacter.GetComponent<ClickToAttack>();
        playerClickToAttack.SetTarget(this);
    }

    void OnMouseExit()
    {
        var playerClickToAttack = this.playerCharacter.GetComponent<ClickToAttack>();
        playerClickToAttack.SetTarget(null); // TODO: do I really want to accept using null patterns? Optional maybe?
    }

    public float GetCurrentHealth()
    {
        return this.currentHealth;
    }

    public float GetMaxHealth()
    {
        return this.maxHealth;
    }

    private bool HasSurpassedProcTime()
    {
        var animation = this.characterController.GetComponent<Animation>();
        var attackAnimation = animation[this.attackAnimationClip.name];
        return attackAnimation.time > this.procTimeAbsolute;
    }

    private void Die()
    {
        var animation = this.characterController.GetComponent<Animation>();
        animation.CrossFade(this.deathAnimationClip.name);
        this.isDead = true;
        this.lootBehavior.DropLoot(this.transform.position);
        Destroy(this.characterController);
    }

    private void Attack()
    {
        this.attackStartTime = Time.time;
        this.hasAttackProced = false;
        this.targetAtAttackTime = this.playerCharacter;
        var animation = this.characterController.GetComponent<Animation>();
        animation.CrossFade(this.attackAnimationClip.name);
        this.characterController.SimpleMove(this.transform.forward * 0);
    }

    private bool IsAttacking()
    {
        return this.attackStartTime > 0f && (Time.time - this.attackStartTime) < this.attackAnimationClip.length;
    }

    private void HitTarget()
    {
        if (this.targetAtAttackTime == null)
        {
            throw new ApplicationException("HitTarget() called with targetAtAttackTime==null");
        }
        this.hasAttackProced = true;
        var playerBehavior = this.targetAtAttackTime.GetComponent<PlayerBehavior>();
        playerBehavior.TakeHit(this.damage);
        this.targetAtAttackTime = null;
    }

    private void LookAtPlayer()
    {
        var trueRotationToLocation = Quaternion.LookRotation(playerCharacter.transform.position - transform.position);
        var terminalRotation = RemoveVerticalRotation(trueRotationToLocation);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, terminalRotation, this.RotationalVelocity);
    }

    // TODO: Figure out how to have util classes in Unity lol. Presumably can just have vanilla C# static class?
    private static Quaternion RemoveVerticalRotation(Quaternion quaternion)
    {
        quaternion.x = 0f;
        quaternion.z = 0f;
        return quaternion;
    }

    private void MoveTowardPlayer()
    {
        var animation = this.characterController.GetComponent<Animation>();
        animation.CrossFade(this.moveAnimationClip.name);
        this.characterController.SimpleMove(this.transform.forward * this.speed);
    }

    private bool WithinRangeOfPlayer()
    {
        return Vector3.Distance(this.transform.position, this.playerCharacter.transform.position) <= attackRange;
    }
}
