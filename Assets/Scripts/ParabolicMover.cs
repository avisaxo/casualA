using UnityEngine;
using System.Collections;

public class ParabolicMover : MonoBehaviour
{
    [Header("Parámetros de Lanzamiento")]
    public Transform targetPoint;
    public float maxHeight = 5f;
    public float duration = 2f;
    public Level0 label;
    private Vector3 startPosition;
    private Vector3 lastPosition;

    public void Launch()
    {
        startPosition = transform.position;
        lastPosition = startPosition;
        StopAllCoroutines(); 
        StartCoroutine(MoveAlongParabola());
    }

    private IEnumerator MoveAlongParabola()
    {
        float timeElapsed = 0f;
        Vector3 endPosition = targetPoint.position;

        while (timeElapsed < duration)
        {
            float t = timeElapsed / duration;

            Vector3 currentPos = Vector3.Lerp(startPosition, endPosition, t);
            float arcHeight = maxHeight * (4 * t - 4 * t * t);
            Vector3 newPosition = currentPos + Vector3.up * arcHeight;
            Vector3 moveDirection = newPosition - lastPosition;

            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
            transform.position = newPosition;
            lastPosition = newPosition;
            timeElapsed += Time.deltaTime;
            yield return null; 
        }
        transform.position = endPosition;
        label.CreateExplocionMissile(targetPoint);
        DestroidMissile();
    }
    
    private void DestroidMissile()
    {
        Destroy(this.gameObject);
    }
    
    public void SetRarguetPosition(Transform targuetPos)
    {
        targetPoint = targuetPos;
    }
}
