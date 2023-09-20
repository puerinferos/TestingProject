using System;
using UnityEngine;

public class CustomRigidbody : MonoBehaviour
{
    [SerializeField]private float zeroVelocityTime;
    [SerializeField]private float gravity = -9.81f;
    [SerializeField] private int maxRicochets;
    [SerializeField] private ParticleSystem destroyParticles;
    
    private Vector3 velocity = new Vector3(0, 0, 0);
    private float zeroVelocityTimer;
    private int currentRicochets;

    private void Start()
    {
        zeroVelocityTimer = zeroVelocityTime;
    }

    private void Death()
    {
        destroyParticles.transform.SetParent(null);
        destroyParticles.gameObject.SetActive(true);
        Destroy(destroyParticles.gameObject,destroyParticles.main.duration);
        
        Destroy(gameObject);
    }

    public void AddVelocity(Vector3 additionalVelocity)
    {
        this.velocity += additionalVelocity;
    }
    private void Update()
    {
        if (velocity.magnitude < .1f)
            zeroVelocityTimer -= Time.deltaTime;
        
        velocity.y += gravity * Time.deltaTime;
        Vector3 newPosition = transform.position + velocity * Time.deltaTime;
        BounceCheck(newPosition);
        transform.position = newPosition;
        
        if(zeroVelocityTimer < 0)
            Death();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,transform.position+velocity);
    }

    private void BounceCheck(Vector3 newPosition)
    {
        RaycastHit hit;
        Vector3 direction = (newPosition - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, newPosition);

        // Get the MeshCollider component
        MeshCollider meshCollider = GetComponent<MeshCollider>();

        // Calculate the bounds of the MeshCollider
        Bounds meshBounds = meshCollider.bounds;
        float maxBoundSize = Mathf.Max(meshBounds.size.x, meshBounds.size.y, meshBounds.size.z)/2;

        // Perform the raycast
        if (Physics.Raycast(transform.position, direction, out hit, distance + maxBoundSize))
        {
            if (hit.transform.GetComponent<DecalPainter>())
            {
                hit.transform.GetComponent<DecalPainter>().DrawDecal(hit.textureCoord);
            }
            
            // Calculate the reflection vector
            Vector3 reflection = Vector3.Reflect(velocity, hit.normal);

            // Adjust the velocity based on the reflection and a bounce factor
            float bounceFactor = 0.8f;
            velocity = reflection * bounceFactor;
            currentRicochets++;
            
            if(currentRicochets >=maxRicochets)
                Death();
        }
    }
}