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
        void Grow(GameObject obj);
        void Split(GameObject obj);
        void TurnRight(GameObject obj, float degrees);
        void TurnLeft(GameObject obj, float degrees);
    }

    public class TutrtleGraphicsManager : ITurtleGraphics {
        public void ApplyTurtleGraphics(GameObject obj, LindenmeyerSystem lm, int iteration) {
            if (lm is SierpinskiTriangle)
                foreach (char symbol in lm.Results[iteration]) {
                    if (symbol == 'A' ||
                        symbol == 'B')
                        Debug.Log("I should GROW");
                    else if (symbol == '+')
                        Debug.Log("I should turn LEFT 120 degrees");
                    else if (symbol == '-')
                        Debug.Log("I should turn RIGHT 120 degrees");
                }

            if (lm is KochCurve)
                Debug.Log("Koch Curve Detected");
            if (lm is KochSnowflake)
                Debug.Log("Koch Snowflake Detected");
            if (lm is SimplePlant)
                Debug.Log("Simple Plant Detected");
            if (lm is DragonCurve)
                Debug.Log("Dragon Curve Detected");
        }

        public void Grow(GameObject obj) {
            Object.Instantiate(obj);
        }

        public void Split(GameObject obj) {

        }

        public void TurnRight(GameObject obj, float degrees) {
            obj.transform.rotation = Quaternion.Euler(degrees, 0.0f, 0.0f);
        }

        public void TurnLeft(GameObject obj, float degrees) {
            obj.transform.rotation = Quaternion.Euler(-degrees, 0.0f, 0.0f);
        }
    }
}
#endif
