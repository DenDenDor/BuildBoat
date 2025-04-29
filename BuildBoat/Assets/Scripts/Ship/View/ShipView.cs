using System;
using System.Collections.Generic;
using UnityEngine;

public class ShipView : MonoBehaviour
{
    [Header("References")]
    public Transform motorAttachmentPoint; // Точка крепления мотора
    public float playerMassCompensationForce = 10f; // Сила компенсации веса игрока
    public float buoyancyForce = 5f; // Сила плавучести
    
    [Header("Movement Settings")]
    [Range(0.1f, 50)] public float maxMotorPower = 25;
    [Range(1f, 30f)] public float rotationSpeed = 5f;
    [Range(5f, 90f)] public float maxRotationAngle = 30f;
    
    [Header("Damping")]
    [Range(0.9f, 0.99f)] public float movementDamping = 0.98f; // Увеличенное демпфирование
    [Range(0.9f, 0.99f)] public float rotationDamping = 0.95f;
    
    private Rigidbody shipRigidbody;
    private float normalizedOffset;
    private bool playerOnBoard;
    private Rigidbody playerRigidbody;

    
    void Start()
    {
        shipRigidbody = GetComponent<Rigidbody>();
        shipRigidbody.linearDamping = 1f; // Больше сопротивления воде
        shipRigidbody.angularDamping = 1f;
        CalculateMotorOffset();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnBoard = true;
            playerRigidbody = other.GetComponent<Rigidbody>();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerOnBoard = false;
            playerRigidbody = null;
        }
    }
    
    public void CalculateCenterOfMass(List<BlockView> blockViews)
    {
        Vector3 centerOfMass = Vector3.zero;
        int blockCount = 0;

        foreach (var block in blockViews)
        {
            centerOfMass += block.transform.position;
            blockCount++;
        }
        
        if (blockCount > 0)
        {
            centerOfMass /= blockCount;
            shipRigidbody.centerOfMass = transform.InverseTransformPoint(centerOfMass);
        }
    }
    
    void CalculateMotorOffset()
    {
        Transform[] blocks = Array.FindAll(GetComponentsInChildren<Transform>(), t => t.CompareTag("ShipBlock"));
        if (blocks.Length == 0) return;
        
        Array.Sort(blocks, (a, b) => a.localPosition.z.CompareTo(b.localPosition.z));
        int motorIndex = Array.IndexOf(blocks, motorAttachmentPoint.parent);
        normalizedOffset = (motorIndex / (float)(blocks.Length - 1)) * 2 - 1;
    }
    
    void Update()
    {
        float rotationInput = 0f;
        
        if (Input.GetKey(KeyCode.Q)) rotationInput = -1f;
        if (Input.GetKey(KeyCode.E)) rotationInput = 1f;
        
        if (rotationInput != 0)
        {
            float effectiveRotationSpeed = rotationSpeed * Mathf.Abs(normalizedOffset) * 0.5f; // Дополнительное уменьшение
            motorAttachmentPoint.Rotate(Vector3.up, rotationInput * effectiveRotationSpeed * Time.deltaTime);
            
            float currentAngle = Vector3.SignedAngle(transform.forward, motorAttachmentPoint.forward, Vector3.up);
            float maxAllowedAngle = maxRotationAngle * Mathf.Abs(normalizedOffset);
            currentAngle = Mathf.Clamp(currentAngle, -maxAllowedAngle, maxAllowedAngle);
            
            motorAttachmentPoint.rotation = Quaternion.AngleAxis(currentAngle, Vector3.up) * transform.rotation;
        }
    }
    
    void FixedUpdate()
    {
        if (motorAttachmentPoint == null)
        {
            return;
        }
        
        // Применяем силу мотора
        shipRigidbody.AddForce(Vector3.up * buoyancyForce, ForceMode.Force);

        // Компенсируем вес игрока, если он на борту
        if (playerOnBoard && playerRigidbody != null)
        {
            Vector3 compensationForce = Vector3.up * playerMassCompensationForce * playerRigidbody.mass;
            shipRigidbody.AddForce(compensationForce, ForceMode.Force);
        }

        // Управление мотором (оставляем как было)
        if (motorAttachmentPoint != null)
        {
            Vector3 currentMotorPosition = motorAttachmentPoint.position;
            Vector3 forceDirection = motorAttachmentPoint.forward;
            
            float distanceFactor = Mathf.Lerp(0.7f, 1f, Mathf.Abs(normalizedOffset));
            float effectivePower = maxMotorPower * distanceFactor * 0.1f;
            
            shipRigidbody.AddForceAtPosition(forceDirection * effectivePower, currentMotorPosition);
        }

        // Водное демпфирование
        shipRigidbody.linearVelocity *= movementDamping;
        shipRigidbody.angularVelocity *= rotationDamping;

        
        // Визуализация
        // Debug.DrawRay(currentMotorPosition, forceDirection * 2, Color.red);
        // Debug.DrawLine(shipRigidbody.worldCenterOfMass, currentMotorPosition, Color.blue);
    }
    
    void OnDrawGizmos()
    {
        if (!Application.isPlaying || shipRigidbody == null) return;
        
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(shipRigidbody.worldCenterOfMass, 0.1f);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(shipRigidbody.worldCenterOfMass, motorAttachmentPoint.position);
    }
}