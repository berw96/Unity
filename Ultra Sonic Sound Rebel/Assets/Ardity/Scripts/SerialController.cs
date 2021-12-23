/**
 * Ardity (Serial Communication for Arduino + Unity)
 * Author: Daniel Wilches <dwilches@gmail.com>
 *
 * This work is released under the Creative Commons Attributions license.
 * https://creativecommons.org/licenses/by/2.0/
 * 
 * Aspects of this source file have been modified
 * by me (Elliot Walker <ewalk008@gold.ac.uk>) to enable
 * the SerialController object to control the
 * PlayerObject based on input from the Ultrasonic
 * controller. Such aspects are encased with my
 * Goldsmiths Student Number (SN: 3368 6408).
 */

using System;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

/**
 * This class allows a Unity program to continually check for messages from a
 * serial device.
 *
 * It creates a Thread that communicates with the serial port and continually
 * polls the messages on the wire.
 * That Thread puts all the messages inside a Queue, and this SerialController
 * class polls that queue by means of invoking SerialThread.GetSerialMessage().
 *
 * The serial device must send its messages separated by a newline character.
 * Neither the SerialController nor the SerialThread perform any validation
 * on the integrity of the message. It's up to the one that makes sense of the
 * data.
 */
public class SerialController : MonoBehaviour
{
    [Tooltip("Port name with which the SerialPort object will be created.")]
    public string portName = "COM3";

    [Tooltip("Baud rate that the serial device is using to transmit data.")]
    public int baudRate = 9600;

    [Tooltip("Reference to an scene object that will receive the events of connection, " +
             "disconnection and the messages from the serial device.")]
    public GameObject messageListener;

    [Tooltip("After an error in the serial communication, or an unsuccessful " +
             "connect, how many milliseconds we should wait.")]
    public int reconnectionDelay = 1000;

    [Tooltip("Maximum number of unread data messages in the queue. " +
             "New messages will be discarded.")]
    public int maxUnreadMessages = 1;

    // Constants used to mark the start and end of a connection. There is no
    // way you can generate clashing messages from your serial device, as I
    // compare the references of these strings, no their contents. So if you
    // send these same strings from the serial device, upon reconstruction they
    // will have different reference ids.
    public const string SERIAL_DEVICE_CONNECTED = "__Connected__";
    public const string SERIAL_DEVICE_DISCONNECTED = "__Disconnected__";

    // Internal reference to the Thread and the object that runs in it.
    protected Thread thread;
    protected SerialThreadLines serialThread;

    // SN: 3368 6408
    private bool trigger = false;
    private float bat_movement_rate_x;

    private float timer = 3.0f;

    private const float bat_max_movement_scale = 0.3f;
    private const float bat_min_movement_scale = 0.0001f;

    [Header("Extensions")]
    [Space(100)]
    [Range(bat_min_movement_scale, bat_max_movement_scale)]

    [SerializeField] GameObject bat_object;
    [SerializeField] GameObject ball_object;

    [SerializeField] GameObject main_text;
    [SerializeField] Text secondary_text;
    [SerializeField] Text telemetry_text;
    // SN: 3368 6408

    // ------------------------------------------------------------------------
    // Invoked whenever the SerialController gameobject is activated.
    // It creates a new thread that tries to connect to the serial device
    // and start reading from it.
    // ------------------------------------------------------------------------
    void OnEnable()
    {
        serialThread = new SerialThreadLines(portName, 
                                             baudRate, 
                                             reconnectionDelay,
                                             maxUnreadMessages);
        thread = new Thread(new ThreadStart(serialThread.RunForever));
        thread.Start();

        // SN: 3368 6408
        bat_movement_rate_x = 0;
        ball_object.SetActive(false);
        // SN: 3368 6408
    }

    // ------------------------------------------------------------------------
    // Invoked whenever the SerialController gameobject is deactivated.
    // It stops and destroys the thread that was reading from the serial device.
    // ------------------------------------------------------------------------
    void OnDisable()
    {
        // If there is a user-defined tear-down function, execute it before
        // closing the underlying COM port.
        if (userDefinedTearDownFunction != null)
            userDefinedTearDownFunction();

        // The serialThread reference should never be null at this point,
        // unless an Exception happened in the OnEnable(), in which case I've
        // no idea what face Unity will make.
        if (serialThread != null)
        {
            serialThread.RequestStop();
            serialThread = null;
        }

        // This reference shouldn't be null at this point anyway.
        if (thread != null)
        {
            thread.Join();
            thread = null;
        }
    }

    // ------------------------------------------------------------------------
    // Polls messages from the queue that the SerialThread object keeps. Once a
    // message has been polled it is removed from the queue. There are some
    // special messages that mark the start/end of the communication with the
    // device.
    // ------------------------------------------------------------------------
    void Update()
    {
        // If the user prefers to poll the messages instead of receiving them
        // via SendMessage, then the message listener should be null.
        if (messageListener == null)
            return;

        // Read the next message from the queue
        string message = (string)serialThread.ReadMessage();
        if (message == null)
            return;

        // Check if the message is plain data or a connect/disconnect event.
        if (ReferenceEquals(message, SERIAL_DEVICE_CONNECTED))
            messageListener.SendMessage("OnConnectionEvent", true);
        else if (ReferenceEquals(message, SERIAL_DEVICE_DISCONNECTED))
            messageListener.SendMessage("OnConnectionEvent", false);
        else
            messageListener.SendMessage("OnMessageArrived", message);

        // SN: 3368 6408
        if (message.Contains("TRIGGER")) {
            Debug.Log("This is an action command...");
            trigger = true;
        } else {
            Debug.Log("This is a movement command...");
            try {
                bat_movement_rate_x = float.Parse(message);
                Debug.Log($"Movement rate = {bat_movement_rate_x}");
                if (bat_movement_rate_x != 0.0f)
                    bat_object.GetComponent<Rigidbody2D>().AddForce(new Vector2(bat_movement_rate_x, 0.0f));
                else
                    bat_object.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);
            } catch (FormatException) {
                Debug.LogWarning("The data acquired from the Arduino Serial Stream " +
                    $"cannot be parsed from {message.GetType()} to {bat_movement_rate_x.GetType()}.");
            }
        }

        if (trigger) {
            if (timer > 0.0f) {
                timer -= Time.deltaTime * 2;
                secondary_text.text = ((int)timer + 1).ToString();
            } else {
                secondary_text.text = "";
                telemetry_text.text = "Bat Speed: " + Mathf.Round(bat_object.GetComponent<Rigidbody2D>().velocity.x);
                ball_object.SetActive(trigger);
            }
            main_text.SetActive(!trigger);
        } 
        // SN: 3368 6408
    }

    // ------------------------------------------------------------------------
    // Returns a new unread message from the serial device. You only need to
    // call this if you don't provide a message listener.
    // ------------------------------------------------------------------------
    public string ReadSerialMessage()
    {
        // Read the next message from the queue
        return (string)serialThread.ReadMessage();
    }

    // ------------------------------------------------------------------------
    // Puts a message in the outgoing queue. The thread object will send the
    // message to the serial device when it considers it's appropriate.
    // ------------------------------------------------------------------------
    public void SendSerialMessage(string message)
    {
        serialThread.SendMessage(message);
    }

    // ------------------------------------------------------------------------
    // Executes a user-defined function before Unity closes the COM port, so
    // the user can send some tear-down message to the hardware reliably.
    // ------------------------------------------------------------------------
    public delegate void TearDownFunction();
    private TearDownFunction userDefinedTearDownFunction;
    public void SetTearDownFunction(TearDownFunction userFunction)
    {
        this.userDefinedTearDownFunction = userFunction;
    }
}
