/*-----------------------------------------------------------
    THE ROOM (2022)
    
    COPYRIGHT ELLIOT WALKER [3368 6408]
    and HAN XUE [SN: 3367 5676]
-----------------------------------------------------------*/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public sealed class GameManager : MonoBehaviour {
    // GameManager instance for other scripts to access during gameplay.
    public static GameManager _instance;

    // constants for determining how proximity tags react to player's resultant distance from hidden items.
    public const float _resultant_distance_red_limit = 3.0f;
    public const float _resultant_distance_amber_limit = 2.0f;
    public const float _resultant_distance_green_limit = 1.5f;

    [Header("Environment")]
    public const int _number_of_hidden_items = 3;

    [SerializeField] GameObject _player_object;
    [SerializeField] Camera _player_head_camera;
    [SerializeField] Camera _item_display_canvas_camera;

    public List<GameObject> _registered_items = new List<GameObject>();
    public List<GameObject> _hidden_items = new List<GameObject>(_number_of_hidden_items);
    public List<Image> _item_proximity_icons = new List<Image>(_number_of_hidden_items);
    public List<Button> _menu_buttons = new List<Button>();

    [Header("User Interface")]
    [SerializeField] Canvas _canvas;
    public GameObject _menu_overlay;
    public GameObject _game_overlay;
    public GameObject _item_observed;
    
    public Text _look_prompt_text;
    public Text _interact_prompt_text;
    public Text _menu_prompt_text_A;
    public Text _menu_prompt_text_B;
    public Text _menu_confirmation_text;

    public Text _title_text;
    public Text _options_text;
    public Text _volume_text;
    public Text _language_text;
    public Text _play_text;
    public Text _back_text;
    public Text _loading_bar_prompt;
    public Text _loading_bar_advice;

    public Font _mandarin_font;
    public Font _korean_font;

    private int _loading_bar_advice_rng;

    private void Start(){
        _instance = this;
        switch (SceneManager.GetActiveScene().name) {
            case "Menu":
                break;
            case "Main":
                _hidden_items.Clear();
                for (int i = 0; i < _number_of_hidden_items; i++)
                    SetHiddenObjects();

                _loading_bar_advice_rng = Random.Range(1, 4);
                break;
        }
    }

    private void Update() {
        UpdateSettings();
    }

    /// <summary>
    /// Applies random number generation during initialization to
    /// choose which three of the registered objects the player will
    /// be tasked to find.
    /// </summary>
    private void SetHiddenObjects() {
        int _hidden_item_rng = Random.Range(0, _registered_items.Count - 1);
        if (_hidden_items.Contains(_registered_items[_hidden_item_rng])) {
            SetHiddenObjects();
        } else
            _hidden_items.Add(_registered_items[_hidden_item_rng]);
    }

    /// <summary>
    /// Toggles one of two possible canvas modes based on whether the
    /// player is holding(observing) an item or not.If the player is
    /// observing an item, its 3D mesh is rendered to a camera attached
    /// to the canvas.
    /// </summary>
    /// <param name="_player"></param>
    public void UpdateCanvasMode(Player _player) {
        if (_player._held_item != null) {
            _canvas.renderMode = RenderMode.ScreenSpaceCamera;
            // if the player is holding an item, get its mesh data and display it on the canvas.
            _item_observed.GetComponent<MeshFilter>().mesh = _player._held_item.GetComponent<MeshFilter>().mesh;
            _item_observed.GetComponent<MeshRenderer>().materials = _player._held_item.GetComponent<MeshRenderer>().materials;

            foreach (Image _icon in _item_proximity_icons) {
                _icon.gameObject.SetActive(false);
            }
        } else {
            _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            // if the player is NOT holding an item, remove the item's mesh from the canvas view.
            _item_observed.GetComponent<MeshFilter>().mesh = null;
            _item_observed.GetComponent<MeshRenderer>().material = null;

            foreach (Image _icon in _item_proximity_icons) {
                _icon.gameObject.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Queries the players current xz-planar distance from a hidden object
    /// and uses this to set boththe scale and the colour of the proximity
    /// icons located on the game overlay. This mapping is implicit.
    /// </summary>
    /// <param name="_resultant_distance"></param>
    /// <param name="_item_index"></param>
    public void UpdateProximityReadings(float _resultant_distance, int _item_index) {
        if (_resultant_distance > _resultant_distance_red_limit) {
            _item_proximity_icons[_item_index].color = Color.white;
            _item_proximity_icons[_item_index].rectTransform.localScale = new Vector3(0.25f, 0.25f, 1.0f);
            return;
        } else if (_resultant_distance <= _resultant_distance_red_limit &&
            _resultant_distance > _resultant_distance_amber_limit) {
            _item_proximity_icons[_item_index].color = Color.red;
            _item_proximity_icons[_item_index].rectTransform.localScale = new Vector3(0.50f, 0.50f, 1.0f);
            return;
        } else if (_resultant_distance <= _resultant_distance_amber_limit &&
             _resultant_distance > _resultant_distance_green_limit) {
            _item_proximity_icons[_item_index].color = Color.yellow;
            _item_proximity_icons[_item_index].rectTransform.localScale = new Vector3(0.75f, 0.75f, 1.0f);
            return;
        } else if (_resultant_distance <= _resultant_distance_green_limit) {
            _item_proximity_icons[_item_index].color = Color.green;
            _item_proximity_icons[_item_index].rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            return;
        }
    }

    /// <summary>
    /// Map the current volume and language settings to relevant components
    /// throughout the current scene.
    /// </summary>
    public void UpdateSettings() {
        foreach (AudioSource _audio_source in FindObjectsOfType<AudioSource>()) {
            _audio_source.volume = PlayerSettings._instance._volume_setting;
        }

        foreach (Text _text_field in FindObjectsOfType<Text>()) {

            // Update text displayed based on language setting
            if (_text_field == _title_text) {
                SetFont(_text_field);
                switch (PlayerSettings._instance._language_setting) {
                    case PlayerSettings.LANGUAGE.ENGLISH:
                        _text_field.text = "The Room";
                        break;
                    case PlayerSettings.LANGUAGE.MANDARIN:
                        _text_field.text = "房间";
                        break;
                    case PlayerSettings.LANGUAGE.KOREAN:
                        _text_field.text = "침실";
                        break;
                    case PlayerSettings.LANGUAGE.JAPANESE:
                        _text_field.text = "ベッドルーム";
                        break;
                }
            }
            if (_text_field == _volume_text) {
                SetFont(_text_field);
                switch (PlayerSettings._instance._language_setting) {
                    case PlayerSettings.LANGUAGE.ENGLISH:
                        _text_field.text = "Volume";
                        break;
                    case PlayerSettings.LANGUAGE.MANDARIN:
                        _text_field.text = "音量";
                        break;
                    case PlayerSettings.LANGUAGE.KOREAN:
                        _text_field.text = "용량";
                        break;
                    case PlayerSettings.LANGUAGE.JAPANESE:
                        _text_field.text = "音量";
                        break;
                }
            }
            if (_text_field == _language_text) {
                SetFont(_text_field);
                switch (PlayerSettings._instance._language_setting) {
                    case PlayerSettings.LANGUAGE.ENGLISH:
                        _text_field.text = "Language";
                        break;
                    case PlayerSettings.LANGUAGE.MANDARIN:
                        _text_field.text = "语言";
                        break;
                    case PlayerSettings.LANGUAGE.KOREAN:
                        _text_field.text = "언어";
                        break;
                    case PlayerSettings.LANGUAGE.JAPANESE:
                        _text_field.text = "言語";
                        break;
                }
            }
            if (_text_field == _back_text) {
                SetFont(_text_field);
                switch (PlayerSettings._instance._language_setting) {
                    case PlayerSettings.LANGUAGE.ENGLISH:
                        _text_field.text = "Back";
                        break;
                    case PlayerSettings.LANGUAGE.MANDARIN:
                        _text_field.text = "返回";
                        break;
                    case PlayerSettings.LANGUAGE.KOREAN:
                        _text_field.text = "반품";
                        break;
                    case PlayerSettings.LANGUAGE.JAPANESE:
                        _text_field.text = "戻る";
                        break;
                }
            }
            if (_text_field == _options_text) {
                SetFont(_text_field);
                switch (PlayerSettings._instance._language_setting) {
                    case PlayerSettings.LANGUAGE.ENGLISH:
                        _text_field.text = "Options";
                        break;
                    case PlayerSettings.LANGUAGE.MANDARIN:
                        _text_field.text = "设置";
                        break;
                    case PlayerSettings.LANGUAGE.KOREAN:
                        _text_field.text = "설정";
                        break;
                    case PlayerSettings.LANGUAGE.JAPANESE:
                        _text_field.text = "設定";
                        break;
                }
            }
            if (_text_field == _play_text) {
                SetFont(_text_field);
                switch (PlayerSettings._instance._language_setting) {
                    case PlayerSettings.LANGUAGE.ENGLISH:
                        _text_field.text = "Enter";
                        break;
                    case PlayerSettings.LANGUAGE.MANDARIN:
                        _text_field.text = "进入";
                        break;
                    case PlayerSettings.LANGUAGE.KOREAN:
                        _text_field.text = "들어가다";
                        break;
                    case PlayerSettings.LANGUAGE.JAPANESE:
                        _text_field.text = "入る";
                        break;
                }
            }
            if (_text_field == _look_prompt_text) {
                SetFont(_text_field);
                switch (PlayerSettings._instance._language_setting) {
                    case PlayerSettings.LANGUAGE.ENGLISH:
                        _text_field.text = "Look [E]";
                        break;
                    case PlayerSettings.LANGUAGE.MANDARIN:
                        _text_field.text = "查看 [E]";
                        break;
                    case PlayerSettings.LANGUAGE.KOREAN:
                        _text_field.text = "보다 [E]";
                        break;
                    case PlayerSettings.LANGUAGE.JAPANESE:
                        _text_field.text = "見て [E]";
                        break;
                }
            }
            if (_text_field == _interact_prompt_text) {
                SetFont(_text_field);
                switch (PlayerSettings._instance._language_setting) {
                    case PlayerSettings.LANGUAGE.ENGLISH:
                        _text_field.text = "Interact [LMB]";
                        break;
                    case PlayerSettings.LANGUAGE.MANDARIN:
                        _text_field.text = "互动 [LMB]";
                        break;
                    case PlayerSettings.LANGUAGE.KOREAN:
                        _text_field.text = "상호작용 [LMB]";
                        break;
                    case PlayerSettings.LANGUAGE.JAPANESE:
                        _text_field.text = "インタラクティブ [LMB]";
                        break;
                }
            }
            if (_text_field == _menu_prompt_text_A) {
                SetFont(_text_field);
                switch (PlayerSettings._instance._language_setting) {
                    case PlayerSettings.LANGUAGE.ENGLISH:
                        _text_field.text = "Leave the room?";
                        break;
                    case PlayerSettings.LANGUAGE.MANDARIN:
                        _text_field.text = "想离开房间吗?";
                        break;
                    case PlayerSettings.LANGUAGE.KOREAN:
                        _text_field.text = "떠나고 싶니?";
                        break;
                    case PlayerSettings.LANGUAGE.JAPANESE:
                        _text_field.text = "帰りたい?";
                        break;
                }
            }
            if (_text_field == _menu_prompt_text_B) {
                SetFont(_text_field);
                switch (PlayerSettings._instance._language_setting) {
                    case PlayerSettings.LANGUAGE.ENGLISH:
                        _text_field.text = "Or press Backspace to continue exploring...";
                        break;
                    case PlayerSettings.LANGUAGE.MANDARIN:
                        _text_field.text = "或按 删除键 继续探索...";
                        break;
                    case PlayerSettings.LANGUAGE.KOREAN:
                        _text_field.text = "또는 백스페이스 키를 눌러 탐색을 계속하십시오...";
                        break;
                    case PlayerSettings.LANGUAGE.JAPANESE:
                        _text_field.text = "または、Backspace キーを押して探索を続けます...";
                        break;
                }
            }
            if (_text_field == _menu_confirmation_text) {
                SetFont(_text_field);
                switch (PlayerSettings._instance._language_setting) {
                    case PlayerSettings.LANGUAGE.ENGLISH:
                        _text_field.text = "Yes";
                        break;
                    case PlayerSettings.LANGUAGE.MANDARIN:
                        _text_field.text = "是的";
                        break;
                    case PlayerSettings.LANGUAGE.KOREAN:
                        _text_field.text = "네";
                        break;
                    case PlayerSettings.LANGUAGE.JAPANESE:
                        _text_field.text = "はい";
                        break;
                }
            }
            if (_text_field == _loading_bar_prompt) {
                SetFont(_text_field);
                switch (PlayerSettings._instance._language_setting) {
                    case PlayerSettings.LANGUAGE.ENGLISH:
                        _text_field.text = "Press any button";
                        break;
                    case PlayerSettings.LANGUAGE.MANDARIN:
                        _text_field.text = "按任意按钮";
                        break;
                    case PlayerSettings.LANGUAGE.KOREAN:
                        _text_field.text = "아무 버튼이나 누르세요";
                        break;
                    case PlayerSettings.LANGUAGE.JAPANESE:
                        _text_field.text = "任意のボタンを押します";
                        break;
                }
            }
            if (_text_field == _loading_bar_advice) {
                SetFont(_text_field);
                switch (SceneManager.GetActiveScene().name) {
                    // Inform the player of their goal upon loading the main level
                    case "Menu":
                        switch (PlayerSettings._instance._language_setting) {
                            case PlayerSettings.LANGUAGE.ENGLISH:
                                _text_field.text = $"Find {_number_of_hidden_items} hidden items...";
                                break;
                            case PlayerSettings.LANGUAGE.MANDARIN:
                                _text_field.text = $"找到 {_number_of_hidden_items} 个隐藏物品...";
                                break;
                            case PlayerSettings.LANGUAGE.KOREAN:
                                _text_field.text = $"숨겨진 아이템 {_number_of_hidden_items} 개 찾기...";
                                break;
                            case PlayerSettings.LANGUAGE.JAPANESE:
                                _text_field.text = $"{_number_of_hidden_items} つの隠しアイテムを見つける...";
                                break;
                        }
                        break;
                    // Give the player advice on how to play the game
                    default:
                        switch (_loading_bar_advice_rng) {
                            case 1:
                                switch (PlayerSettings._instance._language_setting) {
                                    case PlayerSettings.LANGUAGE.ENGLISH:
                                        _text_field.text = "Hold \"C\" to crouch";
                                        break;
                                    case PlayerSettings.LANGUAGE.MANDARIN:
                                        _text_field.text = "按住“C”蹲下";
                                        break;
                                    case PlayerSettings.LANGUAGE.KOREAN:
                                        _text_field.text = "웅크리려면 \"C\"를 누르십시오";
                                        break;
                                    case PlayerSettings.LANGUAGE.JAPANESE:
                                        _text_field.text = "「C」を押し続けるとしゃがみます";
                                        break;
                                }
                                break;
                            case 2:
                                switch (PlayerSettings._instance._language_setting) {
                                    case PlayerSettings.LANGUAGE.ENGLISH:
                                        _text_field.text = "Hold \"Q\" to zoom in";
                                        break;
                                    case PlayerSettings.LANGUAGE.MANDARIN:
                                        _text_field.text = "按住 \"Q\" 增强视力";
                                        break;
                                    case PlayerSettings.LANGUAGE.KOREAN:
                                        _text_field.text = "시력을 향상시키려면 \"Q\"를 길게 누르십시오";
                                        break;
                                    case PlayerSettings.LANGUAGE.JAPANESE:
                                        _text_field.text = "「Q」を押し続けると、視力が向上します";
                                        break;
                                }
                                break;
                            case 3:
                                switch (PlayerSettings._instance._language_setting) {
                                    case PlayerSettings.LANGUAGE.ENGLISH:
                                        _text_field.text = "Use \"WASD\" to rotate items";
                                        break;
                                    case PlayerSettings.LANGUAGE.MANDARIN:
                                        _text_field.text = "使用 \"WASD\" 旋转项目";
                                        break;
                                    case PlayerSettings.LANGUAGE.KOREAN:
                                        _text_field.text = "항목을 회전하려면 \"WASD\"를 사용하십시오";
                                        break;
                                    case PlayerSettings.LANGUAGE.JAPANESE:
                                        _text_field.text = "「WASD」を使用してアイテムを回転させます";
                                        break;
                                }
                                break;
                            case 4:
                                switch (PlayerSettings._instance._language_setting) {
                                    case PlayerSettings.LANGUAGE.ENGLISH:
                                        _text_field.text = "Hold \"Shift\" to sprint";
                                        break;
                                    case PlayerSettings.LANGUAGE.MANDARIN:
                                        _text_field.text = "按住 \"Shift\" 冲刺";
                                        break;
                                    case PlayerSettings.LANGUAGE.KOREAN:
                                        _text_field.text = "전력 질주하려면 \"Shift\"를 누르고 있습니다";
                                        break;
                                    case PlayerSettings.LANGUAGE.JAPANESE:
                                        _text_field.text = "「Shift」を押したままスプリントします";
                                        break;
                                }
                                break;
                        }
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Different languages require different supporting fonts.
    /// In our game, two fonts are applied: one for Korean, and one
    /// for the other languages (English, Mandarin, and Japanese).
    /// The font applied to each Text component depends on the
    /// language currently selected.
    /// </summary>
    /// <param name="_text_field"></param>
    private void SetFont(Text _text_field) {
        // Update font depending on language setting
        switch (PlayerSettings._instance._language_setting) {
            case PlayerSettings.LANGUAGE.KOREAN:
                _text_field.font = _korean_font;
                _text_field.fontStyle = FontStyle.Bold;
                break;
            default:
                _text_field.font = _mandarin_font;
                _text_field.fontStyle = FontStyle.Normal;
                break;
        }
    }
}
