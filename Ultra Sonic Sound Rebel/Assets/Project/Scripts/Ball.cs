using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public float movement_rate_x;
    public float movement_rate_y;
    private float position_x;
    private float position_y;

    private const float max_movement_scale = 1.0f;
    private const float min_movement_scale = 0.0001f;

    [Range(min_movement_scale, max_movement_scale)]
    [SerializeField] float movement_scale;

    [SerializeField] GameObject player_object;
    [SerializeField] List<GameObject> boundaries = new List<GameObject>(4);

    void Start() {

    }

    void FixedUpdate() {
        position_x += (Time.deltaTime * movement_scale * movement_rate_x);
        position_y += (Time.deltaTime * movement_scale * movement_rate_y);

        gameObject.transform.position = new Vector3(position_x, position_y, 0.0f);
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        Debug.LogWarning($"{gameObject.name} collided with {collision.gameObject.name}!");

        if (collision.gameObject == player_object)
            movement_rate_y *= -1;

        if (boundaries.Contains(collision.gameObject))
            switch (collision.gameObject.name) {
                case "TOP":
                    movement_rate_y *= -1;
                    break;
                case "BOTTOM":
                    movement_rate_y *= -1;
                    break;
                case "LEFT":
                    movement_rate_x *= -1;
                    break;
                case "RIGHT":
                    movement_rate_x *= -1;
                    break;
                default:
                    Debug.LogWarning("Collision boundary not found.");
                    break;
            }
    }
}
