using System;
using UnityEngine;
[RequireComponent(typeof(Movement))]
public class Pacman : MonoBehaviour
{
    private InputSystem_Actions controls;
    public Movement movement { get; private set; }

    public AnimatedSprite movementSequence;
    public AnimatedSprite deathSequence;
    public SpriteRenderer spriteRenderer;
    public CircleCollider2D circleCollider;


    private void Awake()
    {
        controls = new InputSystem_Actions();
        this.movement = GetComponent<Movement>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();

        movementSequence.enabled = true;
        deathSequence.enabled = false;
    }
    private void OnEnable()
    {
        controls.Player.Move.performed += ctx =>
        {
            Vector2 input = ctx.ReadValue<Vector2>();

            if (input == Vector2.up) movement.SetDirection(Vector2.up);
            else if (input == Vector2.down) movement.SetDirection(Vector2.down);
            else if (input == Vector2.left) movement.SetDirection(Vector2.left);
            else if (input == Vector2.right) movement.SetDirection(Vector2.right);
        };

        controls.Player.Enable();


    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
    private void Update()
    {
        float angle = Mathf.Atan2(this.movement.direction.y, this.movement.direction.x);
        this.transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);

    }

    public void ResetState()
    {
        enabled = true;
        spriteRenderer.enabled = true;
        circleCollider.enabled = true;

        movementSequence.enabled = true;
        movementSequence.loop = true;
        deathSequence.enabled = false;

        this.movement.ResetState();
        this.gameObject.SetActive(true);
    }
    public void DeathSequence()
    {
        enabled = false;
        // spriteRenderer.enabled = false;
        circleCollider.enabled = false;
        movement.enabled = false;

        movementSequence.enabled = false;
        movementSequence.loop = false;
        deathSequence.enabled = true;
        deathSequence.Restart();
    }
}
