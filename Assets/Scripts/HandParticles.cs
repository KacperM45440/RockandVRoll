using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandParticles : MonoBehaviour
{
    [SerializeField] private InputActionReference shootTrail;

    //public ParticleSystem trailSystem;
    public ParticleSystem impactSystem;
    public Transform tracerSpawnPoint;
    public TrailRenderer tracerTrail;
    public LayerMask maskRef;
    public Animator animatorRef;

    private void OnEnable()
    {
        shootTrail.action.performed += Shoot;
        shootTrail.action.Enable();
    }
    private void OnDisable()
    {
        shootTrail.action.performed -= Shoot;
        shootTrail.action.Disable();
    }
    public void Shoot(InputAction.CallbackContext obj)
    {
        animatorRef.SetBool("isShooting", true);
        //trailSystem.Play();
        Vector3 direction = GetDirection();

        if(Physics.Raycast(tracerSpawnPoint.position, direction, out RaycastHit hit, float.MaxValue, maskRef))
        {
            TrailRenderer trail = Instantiate(tracerTrail, tracerSpawnPoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, hit));
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = transform.forward;
        return direction;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }

        animatorRef.SetBool("isShooting", false);
        trail.transform.position = hit.point;
        Instantiate(impactSystem, hit.point, Quaternion.LookRotation(hit.normal));

        Destroy(trail.gameObject, trail.time);
    }
}
