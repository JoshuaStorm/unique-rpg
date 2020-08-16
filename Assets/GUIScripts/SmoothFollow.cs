using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;
    public Transform cameraTransform;
    public Vector3 offset;
    public float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        this.offset = this.cameraTransform.position - this.target.position;
    }

    void LateUpdate()
    {
        var targetPosition = this.target.position + this.offset;
        cameraTransform.position = Vector3.SmoothDamp(this.transform.position, targetPosition, ref velocity, this.smoothTime);
        this.transform.LookAt(target);
    }
}
