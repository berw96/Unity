/*-----------------------------------------------------------
    THE ROOM (2022)
    
    COPYRIGHT ELLIOT WALKER [3368 6408]
    and HAN XUE [SN: 3367 5676]
-----------------------------------------------------------*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public sealed class Player : MonoBehaviour {

    private readonly Color _CROSSHAIR_STANDARD_COLOR = new Color(1.0f, 1.0f, 1.0f, 4.0f/255.0f);
    private readonly Color _CROSSHAIR_HOVER_COLOR = Color.red;

    [Header("Player Stats")]
    [SerializeField] float _walking_speed = 2.0f;
    [SerializeField] float _sprinting_speed = 6.0f;
    [SerializeField] float _item_rotation_rate = 3.0f;
    [SerializeField] float _rotation_speed = 90.0f;
    [SerializeField] float _vertical_limit = 80.0f;

    [Header("Key Mappings")]
    [SerializeField] KeyCode _item_look_key;
    [SerializeField] KeyCode _crouch_key;
    [SerializeField] KeyCode _sprint_key;
    [SerializeField] KeyCode _zoom_key;
    [SerializeField] KeyCode _cancel_key;
    private const int _LMB = 0;

    [Header("Player Components")]
    [SerializeField] CharacterController _character_controller;
    [SerializeField] AudioSource _audio_source;
    [SerializeField] List<AudioClip> _audio_clips = new List<AudioClip>();
    [SerializeField] Image _crosshair;

    [Header("Environment")]
    [SerializeField] GameObject _drawer;
    [SerializeField] GameObject _radio;
    [SerializeField] GameObject _lamp;
    [SerializeField] GameObject _light_switch;
    [SerializeField] GameObject _shutter_cord;
    [SerializeField] GameObject _exit;
    [SerializeField] List<GameObject> _shutter_panels;
    private float _drawer_lerp_progress = 0.0f;
    private float _shutter_slerp_progress = 0.0f;
    [SerializeField] float _drawer_lerp_rate = 0.05f;
    [SerializeField] float _shutter_slerp_rate = 0.005f;
    private const float _drawer_movement_offset = 0.6f;
    private Vector3 _init_drawer_position;
    private Vector3 _res_drawer_position;
    private Vector3 _closed_drawer_position;
    private Vector3 _opened_drawer_position;
    private Quaternion _init_shutter_orientation;
    private Quaternion _res_shutter_orientation;
    private Quaternion _closed_shutter_orientation;
    private Quaternion _opened_shutter_orientation;

    private float _player_yaw = 0.0f;
    private float _player_pitch = 0.0f;
    private float _player_height = 3.0f;
    private float _item_yaw = 0.0f;
    private float _item_pitch = 0.0f;
    private const float _standing_height = 3.5f;
    private const float _crouching_height = 2.0f;
    private const float _crouching_rate = 0.1f;
    private const float _player_reach = 3.0f;
    private const float _default_FOV = 60.0f;
    private const float _zoomed_FOV = 20.0f;
    private const float _zoom_rate = 2.0f;
    private bool _is_opening_drawer = false;
    private bool _is_opening_shutters = false;

    private Quaternion _init_body_orientation;
    private Quaternion _init_head_orientation;
    private Vector3 _init_move_direction = Vector3.zero;

    private Transform _head;

    [Header("Items")]
    [SerializeField] List<GameObject> _memory = new List<GameObject>();
    public GameObject _held_item = null;
    public bool _is_holding_item = false;

    private void Start(){
        _head = GetComponentInChildren<Camera>().transform;
        
        _init_body_orientation = transform.localRotation;
        _init_head_orientation = _head.transform.localRotation;

        _init_drawer_position = _drawer.transform.position;
        _closed_drawer_position = _init_drawer_position;
        _opened_drawer_position = new Vector3(
            _drawer.transform.position.x,
            _drawer.transform.position.y,
            _drawer.transform.position.z + _drawer_movement_offset
            );
        _init_shutter_orientation = Quaternion.Euler(70.0f, 0.0f, 0.0f);
        _closed_shutter_orientation = _init_shutter_orientation;
        _opened_shutter_orientation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate(){
        if (GameManager._instance._game_overlay.activeSelf) {
            if (_held_item == null) {
                RotateCamera();
                ZoomCamera();
                Move();
                Crouch();

                _item_pitch = 0.0f;
                _item_yaw = 0.0f;
            } else
                RotateHeldObject();

            if (_is_opening_drawer)
                MoveDrawer();

            if (_is_opening_shutters)
                RotateShutters();

            ScanEnvironment();
            CheckProximityToItem();
        }
        if (Input.GetKeyDown(_cancel_key)) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            GameManager._instance._menu_overlay.SetActive(false);
            GameManager._instance._game_overlay.SetActive(true);
        }
    }

    /// <summary>
    /// Queries the mouse cursor’s xy-position onscreen and applies
    /// the values to set the rotation of the player camera.
    /// </summary>
    private void RotateCamera(){
        float _current_horizontal = Input.GetAxis("Mouse X") * Time.deltaTime * _rotation_speed;
        float _current_vertical = Input.GetAxis("Mouse Y") * Time.deltaTime * _rotation_speed;

        _player_yaw += _current_horizontal;
        _player_pitch -= _current_vertical;

        _player_pitch = Mathf.Clamp(_player_pitch, -_vertical_limit, _vertical_limit);

        Quaternion _res_body_rotation = Quaternion.AngleAxis(_player_yaw, Vector3.up);
        Quaternion _res_head_rotation = Quaternion.AngleAxis(_player_pitch, Vector3.right);

        transform.localRotation = _res_body_rotation * _init_body_orientation;
        _head.localRotation = _res_head_rotation * _init_head_orientation;
    }

    /// <summary>
    /// Queries input events from the WASD keys and applies their normalized 
    /// values to increment the player’s planar position in the game world.
    /// The left shift key may be held down to make the character sprint.
    /// </summary>
    private void Move(){
        Vector3 _input = new Vector3(
            Input.GetAxis("Horizontal"), 
            0.0f, 
            Input.GetAxis("Vertical")); ;

        if (Input.GetKey(_sprint_key))
            _input *= _sprinting_speed; 
        else
            _input *= _walking_speed;

        _input = transform.TransformDirection(_input);

        _init_move_direction = _input;

        _character_controller.Move(_init_move_direction * Time.deltaTime);
    }

    /// <summary>
    /// Increments and decrements the player camera’s field of view (FOV) between
    /// upper and lower bounds to give the player a closeup view.
    /// </summary>
    private void ZoomCamera() {
        if (Input.GetKey(_zoom_key))
            if (Camera.main.fieldOfView > _zoomed_FOV)
                Camera.main.fieldOfView -= _zoom_rate;
            else
                Camera.main.fieldOfView = _zoomed_FOV;
        else
            if (Camera.main.fieldOfView < _default_FOV)
                Camera.main.fieldOfView += _zoom_rate;
            else
                Camera.main.fieldOfView = _default_FOV;
    }

    /// <summary>
    /// Like the camera zoom function, this function increments and
    /// decrements the player camera’s height between upper (standing height)
    /// and lower (crouching height) bounds.
    /// </summary>
    private void Crouch(){
        if (Input.GetKey(_crouch_key))
            if (_player_height > _crouching_height)
                _player_height -= _crouching_rate;
            else
                _player_height = _crouching_height;
        else
            if (_player_height < _standing_height)
            _player_height += _crouching_rate;
        else
            _player_height = _standing_height;

        _head.position = new Vector3(
                    _head.position.x,
                    _player_height,
                    _head.position.z
                    );
    }

    /// <summary>
    /// Detect game objects across predefined layers using raycasting to facilitate
    /// the player’s interactions with their environment. Query game objects which
    /// are formally registered with the game manager and compared them to objects
    /// which the player has detected.
    /// </summary>
    private void ScanEnvironment() {
        Ray _ray = new Ray(_head.position, _head.forward);
        RaycastHit _hit;
        GameObject _obj;
        Physics.Raycast(_ray, out _hit, _player_reach, LayerMask.GetMask("Items", "Doors", "Switches"));
        Debug.DrawRay(_head.position, _head.forward * _player_reach, Color.red);

        // If hovering over something and not holding anything.
        if (_hit.collider != null && _held_item == null) {
            Debug.Log($"Hovering over: {_hit.collider.gameObject}");
            
            // Get the GameObject which this collider belongs to.
            _obj = GameObject.Find(_hit.collider.name);
            // Highlight it
            _crosshair.color = _CROSSHAIR_HOVER_COLOR;

            if (_obj.CompareTag("Triggerable")) {
                GameManager._instance._interact_prompt_text.gameObject.SetActive(true);
                if (Input.GetMouseButtonDown(_LMB)) {
                    if (_obj == _drawer) {
                        // open or close drawer
                        Debug.Log("Drawer detected");
                        if (!_is_opening_drawer) {
                            if (_drawer.transform.position.z >= _opened_drawer_position.z) {
                                _init_drawer_position = _opened_drawer_position;
                                _res_drawer_position = _closed_drawer_position;
                            } else if (_drawer.transform.position.z <= _closed_drawer_position.z) {
                                _init_drawer_position = _closed_drawer_position;
                                _res_drawer_position = _opened_drawer_position;
                            }

                            _drawer_lerp_progress = 0.0f;
                            _is_opening_drawer = true;
                        }
                        _drawer.GetComponent<AudioSource>().Play();
                    }
                    if (_obj == _radio) {
                        // toggle radio
                        Debug.Log("Radio detected");
                        if (_radio.GetComponent<AudioSource>().mute != true) {
                            _radio.GetComponent<AudioSource>().mute = true;
                            _radio.GetComponentInChildren<Light>().intensity = 0.0f;
                        } else if (_radio.GetComponent<AudioSource>().mute != false) {
                            _radio.GetComponent<AudioSource>().mute = false;
                            _radio.GetComponentInChildren<Light>().intensity = 10.0f;
                        }
                    }
                    if (_obj == _lamp) {
                        // toggle lamp
                        Debug.Log("Lamp detected");
                        if (_lamp.GetComponentInChildren<Light>().intensity > 0.0f) {
                            _lamp.GetComponentInChildren<Light>().intensity = 0.0f;
                        } else {
                            _lamp.GetComponentInChildren<Light>().intensity = 1.1f;
                        }
                        _lamp.GetComponent<AudioSource>().Play();
                    }
                    if (_obj == _light_switch) {
                        // toggle light switch
                        Debug.Log("Light Switch detected");
                        if (_light_switch.GetComponentInChildren<Light>().intensity > 0.0f) {
                            _light_switch.GetComponentInChildren<Light>().intensity = 0.0f;
                        } else {
                            _light_switch.GetComponentInChildren<Light>().intensity = 2.0f;
                        }
                        _light_switch.GetComponent<AudioSource>().Play();
                    }
                    if (_obj == _shutter_cord) {
                        // open or close shutters
                        Debug.Log("Shutter Cord detected");
                        if (!_is_opening_shutters) {
                            if (_shutter_panels[0].transform.rotation.eulerAngles.x >= _closed_shutter_orientation.eulerAngles.x) {
                                _init_shutter_orientation = _closed_shutter_orientation;
                                _res_shutter_orientation = _opened_shutter_orientation;
                            } else if (_shutter_panels[0].transform.rotation.eulerAngles.x <= _opened_shutter_orientation.eulerAngles.x) {
                                _init_shutter_orientation = _opened_shutter_orientation;
                                _res_shutter_orientation = _closed_shutter_orientation;
                            }

                            _shutter_slerp_progress = 0.0f;
                            _is_opening_shutters = true;
                        }
                        _shutter_cord.GetComponent<AudioSource>().Play();
                    }
                    if (_obj == _exit) {
                        Debug.Log("Exit detected");
                        _exit.GetComponent<AudioSource>().Play();
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                        GameManager._instance._menu_overlay.SetActive(true);
                        GameManager._instance._game_overlay.SetActive(false);
                    }
                    return;
                }
            }
            // Only enable observation with objects that are registered.
            if (GameManager._instance._registered_items.Contains(_obj)) {
                GameManager._instance._look_prompt_text.gameObject.SetActive(true);

                if (Input.GetKeyDown(_item_look_key)) {
                    if (_held_item == null) {
                        // hold it
                        _held_item = _obj;

                        GameManager._instance.UpdateCanvasMode(this);
                        return;
                    }
                }
            }
        } else {
            _crosshair.color = _CROSSHAIR_STANDARD_COLOR;
            GameManager._instance._look_prompt_text.gameObject.SetActive(false);
            GameManager._instance._interact_prompt_text.gameObject.SetActive(false);
        }

        if(Input.GetKeyDown(_item_look_key))
            if (_held_item != null) {
                // drop it
                if (!GameManager._instance._hidden_items.Contains(_held_item)) {
                    Debug.Log($"{_held_item} is not what you are looking for...");
                } else {
                    Debug.Log($"Found {_held_item}");
                    _memory.Add(_held_item);
                    GameManager._instance._item_proximity_icons.RemoveAt(GameManager._instance._hidden_items.IndexOf(_held_item));
                    GameManager._instance._hidden_items.Remove(_held_item);
                    _audio_source.clip = _audio_clips[_memory.Count - 1];
                    _audio_source.Play();
                }

                _held_item = null;

                GameManager._instance.UpdateCanvasMode(this);
                return;
            }
    }

    /// <summary>
    /// Calculates the xz-planar range between the player and each hidden object
    /// using Pythagorean theorem.
    /// </summary>
    private void CheckProximityToItem() {
        for (int i = 0; i < GameManager._instance._hidden_items.Count; i++) {
            float _resultant_distance;
            float _dx;
            float _dz;

            _dx = GameManager._instance._hidden_items[i].transform.position.x - transform.position.x;
            _dz = GameManager._instance._hidden_items[i].transform.position.z - transform.position.z;

            _resultant_distance = Mathf.Sqrt(Mathf.Pow(_dx, 2) + Mathf.Pow(_dz, 2));

            GameManager._instance.UpdateProximityReadings(_resultant_distance, i);

            Debug.LogWarning($"Resultant distance between player and " +
                $"{GameManager._instance._hidden_items[i]} is {_resultant_distance}");
        }
    }

    /// <summary>
    /// Rotate an object which is registered as being currently observed by
    /// the player about its x-axis (pitch) and y-axis (yaw) using the WASD keys.
    /// </summary>
    private void RotateHeldObject() {
        if (Input.GetKey(KeyCode.W))
            _item_pitch += _item_rotation_rate;
        else if (Input.GetKey(KeyCode.S))
            _item_pitch -= _item_rotation_rate;
        if (Input.GetKey(KeyCode.A))
            _item_yaw -= _item_rotation_rate;
        else if(Input.GetKey(KeyCode.D))
            _item_yaw += _item_rotation_rate;

        GameManager._instance._item_observed.transform.localRotation = Quaternion.Euler(
            _item_pitch,
            _item_yaw,
            0.0f
            );
    }

    /// <summary>
    /// Linearly interpolate (LERP) the drawer object in the room between closed
    /// and open positions. Triggered by left-clicking on the drawer object.
    /// </summary>
    private void MoveDrawer() {
        if (_drawer_lerp_progress < 1.0f)
            _drawer.transform.position = Vector3.Lerp(
                   _init_drawer_position,
                   _res_drawer_position,
                   _drawer_lerp_progress += _drawer_lerp_rate
                   );
        else {
            _is_opening_drawer = false;
            _drawer.transform.position = _res_drawer_position;
        }
    }

    /// <summary>
    /// Spherically interpolate (SLERP) each panel constituting the window shutters
    /// in the room between open and closed orientations. Triggered by left-clicking
    /// on the shutter cord object.
    /// </summary>
    private void RotateShutters() {
        if (_shutter_slerp_progress < 1.0f)
            foreach (GameObject _shutter_panel in _shutter_panels) {
                _shutter_panel.transform.rotation = Quaternion.Slerp(
                    _init_shutter_orientation,
                    _res_shutter_orientation,
                    _shutter_slerp_progress += _shutter_slerp_rate
                    );
            }
        else {
            _is_opening_shutters = false;
            foreach (GameObject _shutter_panel in _shutter_panels) {
                _shutter_panel.transform.rotation = _res_shutter_orientation;
            }
        }
    }
}
