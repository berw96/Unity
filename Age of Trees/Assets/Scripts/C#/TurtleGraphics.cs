#define TURTLE_GRAPHICS
#if (UNITY_2019_3_OR_NEWER && TURTLE_GRAPHICS)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lindenmeyer;

namespace TurtleGraphics {

    /// <summary>
    /// Defines common behaviours for Turtle Graphics.
    /// </summary>
    public interface ITurtleGraphics {
        void Grow();
        void TurnRight(float degrees);
        void TurnLeft(float degrees);
    }

    /// <summary>
    /// This class instantiates game objects which represent branches produced
    /// by the provided L-System results.
    /// </summary>
    public class TutrtleGraphicsManager : MonoBehaviour, ITurtleGraphics {
        private Stack<TransformData> transform_data = new Stack<TransformData>();

        public void ApplyTurtleGraphics(LindenmeyerSystem lm, GameObject obj, int iteration) {
            Debug.Log($"Produced: {lm.Results[iteration]}");
            if (lm is SierpinskiTriangle) {
                foreach (char symbol in lm.Results[iteration]) {
                    if (symbol == 'A' || symbol == 'B') {
                        Debug.Log("I should GROW");
                        Debug.Log("I should PUSH transform to stack");

                        TransformData current_transform = new TransformData();
                        current_transform.position = obj.transform.position;
                        current_transform.rotation = obj.transform.rotation.eulerAngles;
                        transform_data.Push(current_transform);

                        Vector3 init_position = obj.transform.position;
                        obj.transform.Translate(Vector3.up);
                        Debug.DrawLine(init_position, obj.transform.position, Color.white, 10000f, false);
                    } else if (symbol == '+') {
                        Debug.Log("I should turn LEFT 60 degrees");
                        obj.transform.Rotate(Vector3.forward * 60);
                    } else if (symbol == '-') {
                        Debug.Log("I should turn RIGHT 60 degrees");
                        obj.transform.Rotate(Vector3.forward * -60);
                    }
                }
            }
            if (lm is KochCurve || lm is DragonCurve) {
                foreach (char symbol in lm.Results[iteration]) {
                    if(symbol == 'F') {
                        Debug.Log("I should GROW");
                        Debug.Log("I should PUSH transform to stack");
                        TransformData current_transform = new TransformData();
                        current_transform.position = obj.transform.position;
                        current_transform.rotation = obj.transform.rotation.eulerAngles;
                        transform_data.Push(current_transform);

                        Vector3 init_position = obj.transform.position;
                        obj.transform.Translate(Vector3.up);
                        Debug.DrawLine(init_position, obj.transform.position, Color.white, 10000f, false);

                    } else if (symbol == '+') {
                        Debug.Log("I should turn RIGHT 90 degrees");
                        obj.transform.Rotate(Vector3.forward * 90);
                    } else if (symbol == '-') {
                        Debug.Log("I should turn LEFT 90 degrees");
                        obj.transform.Rotate(Vector3.forward * -90);
                    }
                }
            }
            if (lm is KochSnowflake) {
                foreach (char symbol in lm.Results[iteration]) {
                    if (symbol == 'F') {
                        Debug.Log("I should GROW");
                        Debug.Log("I should PUSH transform to stack");
                        TransformData current_transform = new TransformData();
                        current_transform.position = obj.transform.position;
                        current_transform.rotation = obj.transform.rotation.eulerAngles;
                        transform_data.Push(current_transform);

                        Vector3 init_position = obj.transform.position;
                        obj.transform.Translate(Vector3.up);
                        Debug.DrawLine(init_position, obj.transform.position, Color.white, 10000f, false);
                    } else if (symbol == '+') {
                        Debug.Log("I should turn RIGHT 60 degrees");
                        obj.transform.Rotate(Vector3.forward * 60);
                    } else if (symbol == '-') {
                        Debug.Log("I should turn LEFT 60 degrees");
                        obj.transform.Rotate(Vector3.forward * -60);
                    }
                }
            }
            if (lm is SimplePlant) {
                Debug.Log("Simple Plant Detected");
                foreach (char symbol in lm.Results[iteration]) {
                    if (symbol == 'F') {
                        Debug.Log("I should GROW");
                        Vector3 init_position = obj.transform.position;
                        obj.transform.Translate(Vector3.up);
                        Debug.DrawLine(init_position, obj.transform.position, Color.white, 10000f, false);
                    } else if (symbol == '+') {
                        Debug.Log("I should turn RIGHT 25 degrees");
                        obj.transform.Rotate(Vector3.forward * 25);
                    } else if (symbol == '-') {
                        Debug.Log("I should turn LEFT 25 degrees");
                        obj.transform.Rotate(Vector3.forward * -25);
                    } else if (symbol == '[') {
                        Debug.Log("I should PUSH transform to stack");
                        TransformData current_transform = new TransformData();
                        current_transform.position = obj.transform.position;
                        current_transform.rotation = obj.transform.rotation.eulerAngles;
                        transform_data.Push(current_transform);
                    } else if (symbol == ']') {
                        Debug.Log("I should POP transform from stack");
                        TransformData current_transform = transform_data.Pop();
                        obj.transform.position = current_transform.position;
                        obj.transform.rotation = Quaternion.Euler(current_transform.rotation);
                    }
                }
            }
        }

        public void Grow() {

        }

        public void TurnRight(float degrees) {

        }

        public void TurnLeft(float degrees) {

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