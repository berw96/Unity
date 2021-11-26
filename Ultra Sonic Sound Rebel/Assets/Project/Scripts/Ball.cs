using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    private float movement_rate_x;
    private float movement_rate_y;
    private float position_x;
    private float position_y;

    private const float max_movement_scale = 0.1f;
    private const float min_movement_scale = 0.0001f;

    [Range(min_movement_scale, max_movement_scale)]
    [SerializeField] float movement_scale;

    [SerializeField] GameObject player_object;

    void Start() {

    }

    void FixedUpdate() {
        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject == player_object) {
            Debug.LogWarning($"{gameObject.name} collided with {player_object.name}!");
        }
    }
}
