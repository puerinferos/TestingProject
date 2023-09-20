using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    public Action OnShoot;
    [Header("References")]
    [SerializeField] private CustomRigidbody projectilePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform cannonTransform;
    [SerializeField] private TrajectoryRenderer trajectoryRenderer;
    [Header("General Options")]
    [SerializeField] private float duration;
    [SerializeField] private float VerticalRotationRange;
    [SerializeField] private float HorizontalRotationRange;

    private Vector2 horizontalRange;
    private Vector2 verticalRange;
    
    private float force;
    private Vector3 targetRotation;
    private void Start()
    {
        trajectoryRenderer.ToggleOnTrajectory();

        horizontalRange.x = transform.rotation.eulerAngles.x-HorizontalRotationRange;
        horizontalRange.y = transform.rotation.eulerAngles.x+HorizontalRotationRange;
        
        verticalRange.x = cannonTransform.localRotation.eulerAngles.x-VerticalRotationRange;
        verticalRange.y = cannonTransform.localRotation.eulerAngles.x+VerticalRotationRange;
        
        targetRotation.x = transform.rotation.eulerAngles.y;
        targetRotation.y = cannonTransform.localRotation.eulerAngles.x;
        Overview.OnMove += OnMoveHandler;
        Overview.OnMoveEnded += OnMoveEnded;
    }

    private void OnMoveEnded()
    {
        SpawnProjectile();
        OnShoot?.Invoke();
    }

    public void ChangeForce(float force)
    {
        this.force = force; 
    }

    private void OnMoveHandler(Vector3 input)
    {
        targetRotation.x += input.x;
        targetRotation.y -= input.y;

        targetRotation.x = Mathf.Clamp (targetRotation.x, horizontalRange.x, horizontalRange.y);
        targetRotation.y = Mathf.Clamp (targetRotation.y, verticalRange.x, verticalRange.y);

        cannonTransform.localRotation = Quaternion.Euler(Vector3.right * targetRotation.y);
        transform.rotation = Quaternion.Euler(Vector3.up * targetRotation.x);
    }

    private IEnumerator AnimateShoot()
    {
        var startPos = cannonTransform.localPosition;
        for (float i = 0; i < 1; i+=Time.deltaTime/(duration/2))
        {
            cannonTransform.localPosition = Vector3.Lerp(startPos, startPos - cannonTransform.up / 2, i);
            yield return null;
        }
        
        for (float i = 0; i < 1; i+=Time.deltaTime/(duration/2))
        {
            cannonTransform.localPosition = Vector3.Lerp(startPos - cannonTransform.up / 2,startPos, i);
            yield return null;
        }

        cannonTransform.localPosition = startPos;
    }

    public void SpawnProjectile()
    {
        StartCoroutine(AnimateShoot());
        var clone = Instantiate(projectilePrefab);
        clone.transform.position = spawnPoint.position;
        clone.AddVelocity(cannonTransform.up * force);
    }

    private void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.F))
            SpawnProjectile();
        if (Input.GetKey(KeyCode.Q))
            force -= Time.deltaTime;
        if (Input.GetKey(KeyCode.E))
            force += Time.deltaTime;

        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");
        cannonTransform.rotation *= Quaternion.Euler(Vector3.right * -vertical);
        transform.rotation *= Quaternion.Euler(Vector3.up * horizontal);
        */
        trajectoryRenderer.ShowTrajectory(spawnPoint.position,cannonTransform.up * force);
    }
}