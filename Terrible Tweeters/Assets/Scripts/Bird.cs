using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    [SerializeField] private float _launchForce = 1000;
    [SerializeField] private float _maxDrag = 3.5f;

    private Vector2 _startPosition;
    Rigidbody2D _rigidBody2D;
    SpriteRenderer _spriteRenderer;
    private Vector2 _previousPos;

    private float _boostForceMultiplier = 50;

    private void Awake()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _startPosition = _rigidBody2D.position;
        _rigidBody2D.isKinematic = true;
    }

    private void OnMouseDown()
    {
        _spriteRenderer.color = Color.red;
    }

    private void OnMouseUp()
    {

        _rigidBody2D.isKinematic = false;

        _previousPos = _rigidBody2D.position;

        Vector2 currentPosition = _rigidBody2D.position;
        Vector2 direction = _startPosition - currentPosition;
        direction.Normalize();

        _rigidBody2D.AddForce(direction * _launchForce);

        _spriteRenderer.color = Color.white;
    }

    private void OnMouseDrag()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 desiredPosition = mousePos;

        float distance = Vector2.Distance(desiredPosition, _startPosition);

        if (distance > _maxDrag)
        {
            Vector2 direction = desiredPosition - _startPosition;
            direction.Normalize();

            desiredPosition = _startPosition + direction * _maxDrag;
        }

        if (desiredPosition.x > _startPosition.x)
            desiredPosition.x = _startPosition.x;

        _rigidBody2D.position = desiredPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_rigidBody2D.isKinematic)
        {

            if (Input.GetMouseButtonDown(0))
            {
                Vector2 direction = _rigidBody2D.position - _previousPos;
                _rigidBody2D.AddForce(direction * _boostForceMultiplier);

            }

        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(ResetAfterDelay());

    }

    private IEnumerator ResetAfterDelay()
    {
        yield return new WaitForSeconds(3);
        _rigidBody2D.position = _startPosition;
        _rigidBody2D.isKinematic = true;
        _rigidBody2D.velocity = Vector2.zero;
    }
}
