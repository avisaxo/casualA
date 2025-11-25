using UnityEngine;

public class CameraFollowSmooth : MonoBehaviour
{
    public Transform target;

    [Range(0.01f, 1.0f)]
    public float smoothTime = 0.3f;

    private float _offsetX;
    private float _initialCameraY;
    private float _initialCameraZ;
    private Quaternion _initialCameraRotation;
    private float _currentXVelocity = 0.0f;

    void Awake()
    {
        _initialCameraY = transform.position.y;
        _initialCameraZ = transform.position.z;
        _initialCameraRotation = transform.rotation;
    }

    public void SetTarget(Transform playerTransform)
    {
        target = playerTransform;
        if (target == null)
        {
            Debug.LogWarning("SetTarget(null) llamado. La cámara no tiene objetivo.");
            return;
        }

        _offsetX = transform.position.x - target.position.x;
    }

    void LateUpdate()
    {
        if (target == null) return;

        var targetX = target.position.x + _offsetX;

        var newCameraX = Mathf.SmoothDamp(
            transform.position.x,
            targetX,              
            ref _currentXVelocity, 
            smoothTime
        );

        transform.position = new Vector3(newCameraX, _initialCameraY, _initialCameraZ);
        transform.rotation = _initialCameraRotation;
    }
}