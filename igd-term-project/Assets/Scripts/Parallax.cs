using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)] float _lagAmount = 0f;
    Vector3 _previousCameraPosition;
    Transform _camera;
    Vector3 _targetPosition;
    public float _parallaxAmount;
    
    private float ParallaxAmount => 1f - _lagAmount;
    
    private void Awake()
    {
        _camera = Camera.main.transform;
        _previousCameraPosition = _camera.position;
    }

    private void LateUpdate()
    {
        Vector3 cameraMovement = GetCameraMovement();

        if (cameraMovement == Vector3.zero) return;

        _targetPosition = new Vector3(transform.position.x + cameraMovement.x * ParallaxAmount, transform.position.y, transform.position.z);
        transform.position = _targetPosition;
    }

    private Vector3 GetCameraMovement()
    {
        Vector3 movement = _camera.position - _previousCameraPosition;
        _previousCameraPosition = _camera.position;
        _parallaxAmount = ParallaxAmount;
        return movement;
    }
}