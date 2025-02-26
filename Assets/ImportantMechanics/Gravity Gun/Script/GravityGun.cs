using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HealthbarGames
{
    public class GravityGun : MonoBehaviour
    {
        public WeaponInput weaponInput;
        public Camera aimCamera = null;

        [Header("Attraction Params")]
        public float attractionForce = 1.0f;
        public float attractionDistance = 20.0f;
        public AnimationCurve attractionCurve = AnimationCurve.Linear(0.0f, 1.0f, 1.0f, 0.0f);
        public LayerMask attractionLayerMask = -1;

        [Header("Holding Params")]
        public Transform holdingPoint = null;
        public float holdingRadius = 0.5f;

        [Header("Shooting Params")]
        public float shootingForce = 100.0f;
        public float shootingChargeTime = 1.0f;

        public LineRenderer beamLineRenderer;
        public float beamShowTime = 0.15f;

        [Space(10)]
        public float rayCastRadius = 0.1f;

        [Header("Audio Clips")]
        public AudioClip shootSound = null;
        public AudioClip rechargeSound = null;
        public AudioClip attractSound = null;

        private Transform m_Transform = null;
        private TargetObject m_TargetObject = null;

        // state info
        private bool m_IsShooting = false;
        private float m_NextShootTime = 0.0f;
        private bool m_IsAttracting = false;
        private AudioSource m_ShootAudioSource;
        private AudioSource m_AttractAudioSource;

        [ExecuteInEditMode]
        private void OnValidate()
        {
            attractionForce = Mathf.Clamp(attractionForce, 0.0f, Mathf.Infinity);
            attractionDistance = Mathf.Clamp(attractionDistance, 0.0f, Mathf.Infinity);
            holdingRadius = Mathf.Clamp(holdingRadius, 0.0f, Mathf.Infinity);
            shootingForce = Mathf.Clamp(shootingForce, 0.0f, Mathf.Infinity);
            shootingChargeTime = Mathf.Clamp(shootingChargeTime, 0.0f, Mathf.Infinity);
            beamShowTime = Mathf.Clamp(beamShowTime, 0.0f, Mathf.Infinity);
            rayCastRadius = Mathf.Clamp(rayCastRadius, Mathf.Epsilon, Mathf.Infinity);
            attractionForce = Mathf.Max(0, attractionForce);
        }

        private void Awake()
        {
            m_Transform = transform;

            if (aimCamera == null)
            {
                aimCamera = Camera.main;
            }

            if (holdingPoint == null)
            {
                GameObject autoHoldingPoint = new GameObject("Automatic Holding Point");
                holdingPoint = autoHoldingPoint.transform;
                holdingPoint.parent = m_Transform;
                holdingPoint.position = m_Transform.position + m_Transform.forward * holdingRadius;
            }

            m_ShootAudioSource = gameObject.AddComponent<AudioSource>();
            m_ShootAudioSource.clip = shootSound;

            m_AttractAudioSource = gameObject.AddComponent<AudioSource>();
            m_AttractAudioSource.clip = attractSound;
            m_AttractAudioSource.volume = 1.0f;

            if (beamLineRenderer != null)
            {
                beamLineRenderer.positionCount = 2;
                beamLineRenderer.SetPosition(0, Vector3.zero);
                beamLineRenderer.SetPosition(1, Vector3.zero);
                beamLineRenderer.enabled = false;
            }

            m_TargetObject = new TargetObject();

            if (weaponInput == null)
            {
                weaponInput = GetComponent<WeaponInput>();
            }
            if (weaponInput == null)
            {
                weaponInput = GetComponentInParent<WeaponInput>();
            }
        }

        private void Update()
        {
            m_IsShooting = weaponInput.GetActionState(WeaponInput.Action.Primary);

            if (m_IsShooting && CanShoot())
            {
                Shoot();
            }

            if (m_TargetObject.IsCaptured)
            {
                // Release when secondary action is triggered.
                if (weaponInput.GetActionActivate(WeaponInput.Action.Secondary))
                {
                    Rigidbody rb = m_TargetObject.ReleaseObject();
                    if (rb != null)
                    {
                        // Restore physics: non-kinematic so physics resumes.
                        rb.isKinematic = false;
                    }
                    m_AttractAudioSource.Stop();
                }
            }
            else
            {
                if (weaponInput.GetActionActivate(WeaponInput.Action.Secondary))
                {
                    m_IsAttracting = true;
                    if (!m_AttractAudioSource.isPlaying)
                        m_AttractAudioSource.Play();
                }
                else if (weaponInput.GetActionDeactivate(WeaponInput.Action.Secondary))
                {
                    m_IsAttracting = false;
                    m_TargetObject.ReleaseObject();
                    m_AttractAudioSource.Stop();
                }
            }
        }

        private void FixedUpdate()
        {
            if (m_TargetObject.IsCaptured)
            {
                // Because the object is now parented to the holding point, it follows automatically.
                // Calling HoldObject ensures its physics position remains in sync.
                m_TargetObject.HoldObject(holdingPoint.position);
            }
            else if (m_IsAttracting)
            {
                AttractObject();
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (holdingPoint != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(holdingPoint.position, holdingRadius);
            }
        }

        private bool CanShoot()
        {
            return Time.time >= m_NextShootTime;
        }

        private void Shoot()
        {
            Vector3 camAimPoint;
            Vector3 energyBeamEndPoint;
            RaycastHit rayHit;
            Ray ray = aimCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

            if (Physics.Raycast(ray, out rayHit, 10000.0f))
            {
                camAimPoint = rayHit.point;
            }
            else
            {
                camAimPoint = ray.GetPoint(10000.0f);
            }
            energyBeamEndPoint = camAimPoint;

            if (m_TargetObject.IsCaptured)
            {
                Rigidbody rigidbody = m_TargetObject.ReleaseObject();
                if (rigidbody != null)
                {
                    // Restore non-kinematic state and apply shooting force.
                    rigidbody.isKinematic = false;
                    rigidbody.AddForce(Vector3.Normalize(camAimPoint - holdingPoint.position) * shootingForce, ForceMode.Impulse);
                }
                energyBeamEndPoint = holdingPoint.position + holdingPoint.forward * 2.0f;
            }
            else
            {
                Vector3 rayOrigin = holdingPoint.position;
                Vector3 rayDirection = Vector3.Normalize(camAimPoint - rayOrigin);

                if (Physics.SphereCast(rayOrigin, rayCastRadius, rayDirection, out rayHit, attractionDistance))
                {
                    Rigidbody hitRigidbody = rayHit.rigidbody;
                    if (hitRigidbody != null)
                    {
                        hitRigidbody.AddForceAtPosition(rayDirection * shootingForce, rayHit.point, ForceMode.Impulse);
                    }
                }
            }

            if (beamLineRenderer != null)
            {
                StartCoroutine(ShowEnergyBeam(energyBeamEndPoint));
            }
            StartCoroutine(PlayShootSound());
            m_AttractAudioSource.Stop();

            m_NextShootTime = Time.time + shootingChargeTime;
        }

        private void AttractObject()
        {
            Vector3 camAimPoint;
            RaycastHit rayHit;
            Ray ray = aimCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            if (Physics.Raycast(ray, out rayHit, 10000.0f))
            {
                camAimPoint = rayHit.point;
            }
            else
            {
                camAimPoint = ray.GetPoint(10000.0f);
            }

            Vector3 rayOrigin = holdingPoint.position;
            Vector3 rayDirection = Vector3.Normalize(camAimPoint - holdingPoint.position);

            if (Physics.SphereCast(rayOrigin, rayCastRadius, rayDirection, out rayHit, attractionDistance, attractionLayerMask))
            {
                Rigidbody hitRigidbody = rayHit.rigidbody;
                if (hitRigidbody != null)
                {
                    // If we're not already targeting an object or a different one is hit, update our target.
                    if (m_TargetObject.IsNull)
                    {
                        m_TargetObject.StartAttractingObject(hitRigidbody);
                    }
                    else if (m_TargetObject.Rigidbody != hitRigidbody)
                    {
                        m_TargetObject.ReleaseObject();
                        m_TargetObject.StartAttractingObject(hitRigidbody);
                    }

                    // Check if the object is within the holding radius.
                    Collider[] colliders = Physics.OverlapSphere(holdingPoint.position, holdingRadius, attractionLayerMask);
                    for (int i = 0; i < colliders.Length; i++)
                    {
                        Rigidbody rb = colliders[i].GetComponent<Rigidbody>();
                        if (rb == hitRigidbody)
                        {
                            // **Set the object to kinematic and parent it so it follows the holding point.**
                            hitRigidbody.isKinematic = true;
                            m_TargetObject.CaptureObject(holdingPoint.position, holdingPoint);
                            m_IsAttracting = false;
                            break;
                        }
                    }

                    // If not yet captured, apply a force to pull the object closer.
                    if (!m_TargetObject.IsCaptured)
                    {
                        hitRigidbody.AddExplosionForce(
                            -attractionCurve.Evaluate(rayHit.distance / attractionDistance) * attractionForce,
                            holdingPoint.position,
                            attractionDistance,
                            0.0f);
                    }
                }
                else
                {
                    m_TargetObject.ReleaseObject();
                }
            }
            else
            {
                m_TargetObject.ReleaseObject();
            }
        }

        private IEnumerator PlayShootSound()
        {
            if (shootSound != null)
            {
                m_ShootAudioSource.PlayOneShot(shootSound);
            }
            yield return new WaitWhile(() => m_ShootAudioSource.isPlaying);
            if (rechargeSound != null)
            {
                m_ShootAudioSource.PlayOneShot(rechargeSound);
            }
        }

        private IEnumerator ShowEnergyBeam(Vector3 endPosition)
        {
            if (beamLineRenderer != null)
            {
                beamLineRenderer.useWorldSpace = false;
                beamLineRenderer.SetPosition(0, Vector3.zero);
                beamLineRenderer.SetPosition(1, beamLineRenderer.transform.InverseTransformPoint(endPosition));
                beamLineRenderer.enabled = true;
                yield return new WaitForSeconds(beamShowTime);
                beamLineRenderer.enabled = false;
            }
            yield return null;
        }
    }
}