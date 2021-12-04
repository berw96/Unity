#define TURTLE_GRAPHICS
#if (UNITY_2019_3_OR_NEWER && TURTLE_GRAPHICS)

using System;
using System.Collections.Generic;
using UnityEngine;
using Lindenmeyer;

namespace TurtleGraphics {
    /// <summary>
    /// Defines common behaviours for Turtle Graphics.
    /// </summary>
    public interface ITurtleGraphics {
        void Grow(GameObject obj);
        void Turn(GameObject obj, float degrees);
        void AddTransform(GameObject obj);
        void RemoveTransform(GameObject obj);
        void ResetGraphics(GameObject obj);
    }

    /// <summary>
    /// This class instantiates game objects which represent branches produced
    /// by the provided L-System results.
    /// </summary>
    public class TurtleGraphicsManager : ITurtleGraphics {
        private Stack<TransformData> transform_data = new Stack<TransformData>();
        private Stack<GameObject> branches = new Stack<GameObject>();

        // prefab provided by the GameManager
        private GameObject branch;

        public Stack<TransformData> Transform_data {
            get { return this.transform_data; }
        }

        public Stack<GameObject> Branches {
            get { return this.branches; }
        }

        public GameObject Branch {
            get { return this.branch; }
            set { this.branch = value; }
        }

        public void ApplyTurtleGraphics(LindenmeyerSystem lm, GameObject obj, int iteration) {
            // reset transform data
            ResetGraphics(obj);
            if (lm is SierpinskiTriangle) {
                foreach (char symbol in lm.Results[iteration]) {
                    if (symbol == 'A' || symbol == 'B') {
                        AddTransform(obj);
                        Grow(obj);
                    } else if (symbol == '+') {
                        Turn(obj, 60f);
                    } else if (symbol == '-') {
                        Turn(obj, -60f);
                    }
                }
            }
            if (lm is KochCurve || lm is DragonCurve) {
                foreach (char symbol in lm.Results[iteration]) {
                    if (symbol == 'F') {
                        AddTransform(obj);
                        Grow(obj);
                    } else if (symbol == '+') {
                        Turn(obj, 90f);
                    } else if (symbol == '-') {
                        Turn(obj, -90f);
                    }
                }
            }
            if (lm is KochSnowflake) {
                foreach (char symbol in lm.Results[iteration]) {
                    if (symbol == 'F') {
                        AddTransform(obj);
                        Grow(obj);
                    } else if (symbol == '+') {
                        Turn(obj, 60f);
                    } else if (symbol == '-') {
                        Turn(obj, -60f);
                    }
                }
            }
            if (lm is SimplePlantA || lm is SimplePlantE) {
                Debug.Log("Simple Plant A Detected");
                foreach (char symbol in lm.Results[iteration]) {
                    if (symbol == 'F') {
                        Grow(obj);
                    } else if (symbol == '+') {
                        Turn(obj, 25.7f);
                    } else if (symbol == '-') {
                        Turn(obj, -25.7f);
                    } else if (symbol == '[') {
                        AddTransform(obj);
                    } else if (symbol == ']') {
                        RemoveTransform(obj);
                    }
                }
            }
            if (lm is SimplePlantC) {
                Debug.Log("Simple Plant C Detected");
                foreach (char symbol in lm.Results[iteration]) {
                    if (symbol == 'F') {
                        Grow(obj);
                    } else if (symbol == '+') {
                        Turn(obj, 22.5f);
                    } else if (symbol == '-') {
                        Turn(obj, -22.5f);
                    } else if (symbol == '[') {
                        AddTransform(obj);
                    } else if (symbol == ']') {
                        RemoveTransform(obj);
                    }
                }
            }
            if (lm is SimplePlantB || lm is SimplePlantD) {
                Debug.Log("Simple Plant D Detected");
                foreach (char symbol in lm.Results[iteration]) {
                    if (symbol == 'F') {
                        Grow(obj);
                    } else if (symbol == '+') {
                        Turn(obj, 20f);
                    } else if (symbol == '-') {
                        Turn(obj, -20f);
                    } else if (symbol == '[') {
                        AddTransform(obj);
                    } else if (symbol == ']') {
                        RemoveTransform(obj);
                    }
                }
            }
            if (lm is SimplePlantF) {
                Debug.Log("Simple Plant F Detected");
                foreach (char symbol in lm.Results[iteration]) {
                    if (symbol == 'F') {
                        Grow(obj);
                    } else if (symbol == '+') {
                        Turn(obj, 25f);
                    } else if (symbol == '-') {
                        Turn(obj, -25f);
                    } else if (symbol == '[') {
                        AddTransform(obj);
                    } else if (symbol == ']') {
                        RemoveTransform(obj);
                    }
                }
            }
        }

        public void Grow(GameObject obj) {
            Vector3 init_position = obj.transform.position;
            obj.transform.Translate(Vector3.up);
            branches.Push(GameObject.Instantiate(branch));
            branches.Peek().transform.position = obj.transform.position;
            branches.Peek().transform.rotation = obj.transform.rotation;
        }

        public void Turn(GameObject obj, float degrees) {
            obj.transform.Rotate(Vector3.forward * degrees);
        }

        public void AddTransform(GameObject obj) {
            TransformData current_transform = new TransformData();
            current_transform.position = obj.transform.position;
            current_transform.rotation = obj.transform.rotation.eulerAngles;
            current_transform.scale = obj.transform.localScale;
            transform_data.Push(current_transform);
        }

        public void RemoveTransform(GameObject obj) {
            try {
                TransformData current_transform = transform_data.Pop();
                obj.transform.position = current_transform.position;
                obj.transform.rotation = Quaternion.Euler(current_transform.rotation);
                obj.transform.localScale = current_transform.scale;
            } catch (InvalidOperationException e) {
                Debug.LogWarning($"{e}");
            }
        }

        public void ResetGraphics(GameObject obj) {
            try {
                transform_data.Clear();
                foreach (GameObject branch in branches) {
                    GameObject.Destroy(branch);
                }
                branches.Clear();
                obj.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
                obj.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
            } catch (InvalidOperationException e) {
                Debug.LogWarning($"{e}");
            }
        }
    }

    /// <summary>
    /// An abstraction of fundamental transform components.
    /// </summary>
    public struct TransformData {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
    }
}
#endif
