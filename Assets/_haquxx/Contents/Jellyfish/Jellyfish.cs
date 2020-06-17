using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jellyfish : MonoBehaviour
{
    [SerializeField]
    private float _maxHeight = 2.0f;

    [SerializeField]
    private float _minHeight = 0.5f;

    [SerializeField]
    private float _highSpeed = 0.5f;
    [SerializeField]
    private float _slowSpeed = 0.1f;

    [SerializeField]
    private Vector2 _spawnRange;

    private float _interval = 2.0f;
    private float _time;
    private bool _enableBoost = false;

    private void Update()
    {
        if(_time >= _interval)
        {
            _time = 0f;
            _enableBoost = !_enableBoost;
            _interval = Random.Range(1.0f, 3.0f);
        }

        _time += Time.deltaTime;

        var pos = transform.position;
        pos.y += Time.deltaTime * (_enableBoost ? _highSpeed : _slowSpeed);
        if(pos.y > _maxHeight)
        {
            pos.y = _minHeight;
            pos.x = Random.Range(-_spawnRange.x / 2f, _spawnRange.x / 2f);
            pos.z = Random.Range(0f, _spawnRange.y);
        }
        transform.position = pos;
    }
}
