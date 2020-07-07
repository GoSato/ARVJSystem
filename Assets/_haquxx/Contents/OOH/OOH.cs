using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOH : MonoBehaviour
{
    [SerializeField]
    private float _minInterval = 0.5f;
    [SerializeField]
    private float _maxInterval = 1.0f;
    [SerializeField]
    private float _minZ = -5f;
    [SerializeField]
    private float _maxZ = 5f;
    [SerializeField]
    private float _minSpeed = 1.0f;
    [SerializeField]
    private float _maxSpeed = 2.0f;

    private float _interval = 0.0f;
    private float _time;
    private SpriteRenderer _spriteRenderer;
    private float _speed;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _speed = Random.Range(_minSpeed, _maxSpeed);
    }

    private void Update()
    {
        if(_time >= _interval)
        {
            _spriteRenderer.enabled = !_spriteRenderer.enabled;
            _interval = Random.Range(_minInterval, _maxInterval);
            _time = 0f;
        }
        _time += Time.deltaTime;

        var pos = transform.position;
        pos.z -= _speed * Time.deltaTime;
        if(pos.z < _minZ)
        {
            pos.z = _maxZ;
        }
        transform.position = pos;
    }
}
