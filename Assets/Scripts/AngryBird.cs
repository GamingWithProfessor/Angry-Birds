using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryBird : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _Rigidbody;
    [SerializeField] private CircleCollider2D _CircleCollider;

    private bool _HasBeenLaunched;
    private bool _ShouldFaceVelocityDirection;

    void Awake()
    {
        _Rigidbody = GetComponent<Rigidbody2D>();
        _CircleCollider = GetComponent<CircleCollider2D>();       
    }

    void Start()
    {
        _Rigidbody.isKinematic = true;
        _CircleCollider.enabled = false;
    }

    private void FixedUpdate()
    {
        if (_HasBeenLaunched && _ShouldFaceVelocityDirection)
        {
            transform.right = _Rigidbody.velocity;
        }
    }

    public void LaunchBird(Vector2 direction, float force)
    {
        _Rigidbody.isKinematic = false;
        _CircleCollider.enabled = true;

        _Rigidbody.AddForce(direction * force, ForceMode2D.Impulse);

        _HasBeenLaunched = true;
        _ShouldFaceVelocityDirection = true;

        Time.timeScale = 1f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _ShouldFaceVelocityDirection = false;
    }
}
