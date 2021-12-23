using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour {

    private const float ball_movement_rate_x_collision_multiplier = 1.1f;
    private const float ball_movement_rate_y_collision_multiplier = 1.1f;

    private const float ball_max_movement_rate_x = 4.0f;
    private const float ball_min_movement_rate_x = 2.0f;
    private const float ball_max_movement_rate_y = 3.0f;
    private const float ball_min_movement_rate_y = 1.0f;

    private const float ball_max_movement_scale = 1.0f;
    private const float ball_min_movement_scale = 0.0001f;
    
    private float ball_position_x;
    private float ball_position_y;

    private int score = 0;

    [Range(ball_min_movement_scale, ball_max_movement_scale)]
    [SerializeField] float ball_movement_scale;

    [Range(ball_min_movement_rate_x, ball_max_movement_rate_x)]
    [SerializeField] float ball_movement_rate_x;

    [Range(ball_min_movement_rate_y, ball_max_movement_rate_y)]
    [SerializeField] float ball_movement_rate_y;

    [SerializeField] GameObject bat_object;
    [SerializeField] Text score_text;
    [SerializeField] List<GameObject> boundaries = new List<GameObject>(4);

    private void Start() {
        score_text.text = "";
    }

    private void OnEnable() {
        Debug.Log("Ball is now active!");
        score_text.text = "Score: ";
    }

    private void FixedUpdate() {
        ball_position_x += (Time.deltaTime * ball_movement_scale * ball_movement_rate_x);
        ball_position_y += (Time.deltaTime * ball_movement_scale * ball_movement_rate_y);

        gameObject.transform.position = new Vector3(ball_position_x, ball_position_y, 0.0f);

        score_text.text = "Score: " + score;
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        Debug.LogWarning($"{gameObject.name} collided with {collision.gameObject.name}!");

        if(Mathf.Abs(ball_movement_rate_x) < ball_max_movement_rate_x)
            ball_movement_rate_x *= ball_movement_rate_x_collision_multiplier;
        if(Mathf.Abs(ball_movement_rate_y) < ball_max_movement_rate_y)
            ball_movement_rate_y *= ball_movement_rate_y_collision_multiplier;

        if (collision.gameObject == bat_object)
            ball_movement_rate_y *= -1;

        if (boundaries.Contains(collision.gameObject))
            switch (collision.gameObject.name) {
                case "TOP":
                    ball_movement_rate_y *= -1;
                    score += 1;
                    score_text.text = "Score: " + score;
                    break;
                case "BOTTOM":
                    ball_movement_rate_y *= -1;
                    break;
                case "LEFT":
                    ball_movement_rate_x *= -1;
                    break;
                case "RIGHT":
                    ball_movement_rate_x *= -1;
                    break;
                default:
                    Debug.LogWarning("Collision boundary not found.");
                    break;
            }
    }
}
