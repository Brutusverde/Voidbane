using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headbob : MonoBehaviour
{
    [Header("Configuration")]
    public bool enable = true;
    [SerializeField, Range(0, 0.1f)] private float Amplitude = 0.015f;
    [SerializeField, Range(0, 0.1f)] private float runAmplitude = 0.015f;
    [SerializeField, Range(0, 30)] private float frequency = 10.0f;
    [SerializeField, Range(0, 30)] private float runFrequency = 20.0f;
    [SerializeField] public Transform _camera;
    [SerializeField] public Transform cameraHolder;

    private float toggleSpeed = 1.0f;
    private float runSpeed = 4.5f;

    private Vector3 _startPos;
    public PlayerNetwork _controller;

    void Start()
    {
        _startPos = _camera.localPosition;
    }

    void Update()
    {
        if (!enable) return;
        CheckMotion();
        ResetPosition();
        _camera.LookAt(FocusTarget());
    }

    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency) * Amplitude;
        pos.x += Mathf.Cos(Time.time * frequency / 2) * Amplitude * 2;
        return pos;
    }

    private Vector3 RunStepMotion()
    {
        Vector3 pos2 = Vector3.zero;
        pos2.y += Mathf.Sin(Time.time * runFrequency) * runAmplitude;
        pos2.x += Mathf.Cos(Time.time * runFrequency / 2) * runAmplitude * 2;
        return pos2;
    }

    private void CheckMotion()
    {
        float speed = new Vector3(_controller.GetComponent<Rigidbody>().velocity.x, 0, _controller.GetComponent<Rigidbody>().velocity.z).magnitude;
        if (!_controller.grounded) return;
        if (speed > toggleSpeed && speed < runSpeed)
        {
            PlayMotion(FootStepMotion());
        }
        if (speed > toggleSpeed && speed > runSpeed)
        {
            PlayMotion(RunStepMotion());
        }
    }

    private void PlayMotion(Vector3 motion)
    {
        _camera.localPosition += motion;
    }

    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + cameraHolder.localPosition.y, transform.position.z);
        pos += cameraHolder.forward * 15.0f;
        return pos;
    }
    private void ResetPosition()
    {
        if (_camera.localPosition == _startPos) return;
        _camera.localPosition = Vector3.Lerp(_camera.localPosition, _startPos, 1 * Time.deltaTime);
    }
}