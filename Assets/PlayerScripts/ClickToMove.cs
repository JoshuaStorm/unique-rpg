using UnityEngine;

public class ClickToMove : MonoBehaviour
{
    public CharacterController characterController;
    public MouseOverGuiElementHandler mouseOverGuiElementHandler;
    public AnimationClip moveAnimationClip;
    public AnimationClip standAnimationClip;
    public ClickToAttack clickToAttack;
    public float speed;

    private float RotationalVelocity;
    private float TerminalPositionTolerance;
    private Vector3 terminalPosition;

    // Start is called before the first frame update
    void Start()
    {
        this.RotationalVelocity = Time.deltaTime * 10f;
        this.TerminalPositionTolerance = this.clickToAttack.GetAttackRange();
        this.terminalPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        this.LookAtTerminalPosition();
        if (Vector3.Distance(this.transform.position, this.terminalPosition) > this.TerminalPositionTolerance)
        {
            this.MoveTowardTerminalPosition();
        }
        else if (!this.clickToAttack.IsAttacking()) // Can I avoid needing to attack clickToAttack here?
        {
            this.Idle();
        } 
    }

    public void HandleLeftClick()
    {
        this.UpdateTerminalPosition();
    }

    private void Idle()
    {
        var animation = this.characterController.GetComponent<Animation>();
        animation.CrossFade(this.standAnimationClip.name);
    }

    private void LookAtTerminalPosition()
    {
        var targetLocation = this.terminalPosition - this.transform.position;
        if (!targetLocation.Equals(Vector3.zero))
        {
            var trueRotationToLocation = Quaternion.LookRotation(targetLocation);
            var terminalRotation = RemoveVerticalRotation(trueRotationToLocation);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, terminalRotation, this.RotationalVelocity);
        }
    }

    private static Quaternion RemoveVerticalRotation(Quaternion quaternion)
    {
        quaternion.x = 0f;
        quaternion.z = 0f;
        return quaternion;
    }

    private void MoveTowardTerminalPosition()
    {
        var animation = this.characterController.GetComponent<Animation>();
        animation.CrossFade(this.moveAnimationClip.name);
        this.characterController.SimpleMove(this.transform.forward * this.speed);
    }

    private void UpdateTerminalPosition()
    {
        const int RaycastLength = 1000; // Arbitrary number that is just sufficiently large to hit anything within screen
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // TODO: I gotta do something about this cus when the ray hits the player you just kinda don't move (or attack an enemy just beyond the player)
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, RaycastLength))
        {
            this.terminalPosition = new Vector3(x: hit.point.x, y: hit.point.y, z: hit.point.z);
        }
    }
}
