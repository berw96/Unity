/**
 * Ardity (Serial Communication for Arduino + Unity)
 * Author: Daniel Wilches <dwilches@gmail.com>
 *
 * This work is released under the Creative Commons Attributions license.
 * https://creativecommons.org/licenses/by/2.0/
 * 
 * Aspects of this source file have been modified
 * by Elliot Walker <ewalk008@gold.ac.uk> to enable
 * the SampleMessageListener object to control the
 * PlayerObject based on input from the Ultrasonic
 * controller. Such aspects are encased with their
 * Goldsmiths Student Number (SN: 3368 6408).
 */

using UnityEngine;
using System.Collections;
using System;

/**
 * When creating your message listeners you need to implement these two methods:
 *  - OnMessageArrived
 *  - OnConnectionEvent
 */
public class SampleMessageListener : MonoBehaviour {

    // SN: 3368 6408
    private bool trigger = false;
    private float movement_rate_x;
    private float position_x;

    private const float max_movement_scale = 0.3f;
    private const float min_movement_scale = 0.0001f;

    [Range(min_movement_scale, max_movement_scale)]
    [SerializeField] float movement_scale;

    [SerializeField] GameObject player_object;
    [SerializeField] GameObject ball_object;
    // SN: 3368 6408

    // Invoked when a line of data is received from the serial device.
    private void OnMessageArrived(string msg) {
        Debug.Log("Message Arrived: " + msg);

        // SN: 3368 6408
        if (msg.Contains("TRIGGER")) {
            Debug.Log("This is an action command...");
            trigger = true;
        } else {
            Debug.Log("This is a movement command...");
            try {
                movement_rate_x = float.Parse(msg);
                Debug.Log($"Movement rate = {movement_rate_x}");
            } catch (FormatException) {
                Debug.LogWarning("The data acquired from the Arduino Serial Stream " +
                    $"cannot be parsed from {msg.GetType()} to {movement_rate_x.GetType()}.");
            }
        }
        // SN: 3368 6408
    }

    // Invoked when a connect/disconnect event occurs. The parameter 'success'
    // will be 'true' upon connection, and 'false' upon disconnection or
    // failure to connect.
    private void OnConnectionEvent(bool success) {
        if (success)
            Debug.Log("Connection established");
        else
            Debug.Log("Connection attempt failed or disconnection detected");
    }

    // SN: 3368 6408
    private void Start() {
        movement_rate_x = 0;
    }

    private void FixedUpdate() {
        position_x += (Time.deltaTime * movement_scale * movement_rate_x);
        player_object.transform.position = new Vector3(position_x, player_object.transform.position.y, 0.0f);

        ball_object.SetActive(trigger);

        CheckBoundaries();
    }

    private void CheckBoundaries() {
        if (player_object.transform.position.x < (-7.5f))
            player_object.transform.position = new Vector3(-7.5f, player_object.transform.position.y, 0.0f);

        if (player_object.transform.position.x > (7.5f))
            player_object.transform.position = new Vector3(7.5f, player_object.transform.position.y, 0.0f);

    }
    // SN: 3368 6408
}
