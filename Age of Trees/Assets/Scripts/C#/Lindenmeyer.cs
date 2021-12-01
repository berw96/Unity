#define LINDENMEYER
#if (UNITY_2019_3_OR_NEWER && LINDENMEYER)

using System.Collections.Generic;

/// <summary>
/// Contains class definitions for common Lindenmeyer systems (L-Systems)
/// derived from a base class providing the user with the option to compose
/// their own grammars.
/// </summary>
namespace Lindenmeyer {

    public enum MODE {
        DETERMINISTIC = 1,
        STOCHASTIC = 2,
        CONTEXT_SENSITIVE = 3
    }

    /// <summary>
    /// The base class for all L-System presets and user-defined grammars.
    /// This class inherits from <c>MonoBehaviour</c> to enable access 
    /// to Unity's API and debugging features.
    /// </summary>
    public abstract class LindenmeyerSystem {

        protected MODE mode;

        /*
         * Mutate the system's current state using a provided
         * rule set and concactenate the results to a new string.
         */
        public delegate void RuleSet(int iterations);
        private RuleSet rule_set;

        protected string variables;
        protected string constants;
        protected string axiom;
        protected string current_state;

        /*
         * List for containing the results produced by an L-System
         * at each iteration where its rule set is applied.
         * 
         * This list is later queried by the interpretor which maps
         * each result to corresponding instructions used for invoking
         * responses (e.g., Turtle Graphics).
         */
        protected List<string> results = new List<string>();

        public MODE Mode {
            get { return this.mode; }
            set { this.mode = value; }
        }

        public RuleSet Rule_set {
            get { return this.rule_set; }
            set { this.rule_set = value; }
        }

        public string Variables {
            get { return this.variables; }
            set { this.variables = value; }
        }

        public string Constants {
            get { return this.constants; }
            set { this.constants = value; }
        }

        public string Axiom {
            get { return this.axiom; }
            set { this.axiom = value; }
        }

        public string Current_state {
            get { return this.current_state; }
            set { this.current_state = value; }
        }

        public List<string> Results {
            get { return this.results; }
            set { this.results = value; }
        }

        // default constructor for subclasses and custom L-Systems
        public LindenmeyerSystem() {
            variables = "";
            constants = "";
            axiom = "";
        }

        #region CUSTOM_LSYSTEM_CONSTRUCTORS
        public LindenmeyerSystem(string variables) {
            this.variables = variables;
            constants = "";
            axiom = "";
        }
        
        public LindenmeyerSystem(
            string variables,
            string constants) {
            this.variables = variables;
            this.constants = constants;
            axiom = "";
        }
        
        public LindenmeyerSystem(
            string variables, 
            string constants,
            string axiom) {
            this.variables = variables;
            this.constants = constants;
            this.axiom = axiom;
        }

        public LindenmeyerSystem(
            string variables,
            string constants,
            string axiom,
            MODE mode) {
            this.variables = variables;
            this.constants = constants;
            this.axiom = axiom;
            this.mode = mode;
        }

        public LindenmeyerSystem(
            string variables,
            string constants,
            string axiom,
            MODE mode,
            RuleSet rule_set) {
            this.variables = variables;
            this.constants = constants;
            this.axiom = axiom;
            this.mode = mode;
            this.rule_set = rule_set;
        }
        #endregion

        #region RULES
        /// <summary>
        /// Takes the system's current state and applies
        /// a predefined set of mutations on each character
        /// constituting the grammar.
        /// </summary>
        public virtual void ApplyRules(int iterations) {
            // If the system's current state is null, initialize it before applying ruleset.
            if (string.IsNullOrEmpty(this.current_state))
                this.current_state = this.axiom;

            // Invokes method assigned to the rule_set delegate.
            Rule_set.Invoke(iterations); 
        }
        #endregion
    }

    #region PRESETS
    public sealed class SierpinskiTriangle : LindenmeyerSystem {
        private readonly string sierpinski_triangle_variables = "AB";
        private readonly string sierpinski_triangle_constants = "+-";

        public SierpinskiTriangle(string axiom) : base() {
            this.variables = sierpinski_triangle_variables;
            this.constants = sierpinski_triangle_constants;
            this.axiom = axiom;
            this.current_state = this.axiom;
            this.Rule_set = ApplyRules;
        }

        public override void ApplyRules(int iterations) {
            for (int i = 0; i < iterations; i++) {
                string new_state = "";
                for (int j = 0; j < this.current_state.Length; j++) {
                    string symbol = this.current_state[j].ToString();
                    switch (symbol) {
                        case "A":
                            new_state += "B-A-B";
                            break;
                        case "B":
                            new_state += "A+B+A";
                            break;
                        case "+":
                            new_state += "+";
                            break;
                        case "-":
                            new_state += "-";
                            break;
                        default:
                            break;
                    }
                }
                this.current_state = new_state;
                this.results.Add(this.current_state);
            }
        }
    }

    public sealed class KochCurve : LindenmeyerSystem {
        private readonly string KochCurveVariables = "F";
        private readonly string KochCurveConstants = "+-";

        public KochCurve() {
            this.variables = KochCurveVariables;
            this.constants = KochCurveConstants;
            this.axiom = "F";
            this.current_state = this.axiom;
            this.Rule_set = ApplyRules;
        }

        public override void ApplyRules(int iterations) {
            for (int i = 0; i < iterations; i++) {
                string new_state = "";
                for (int j = 0; j < this.current_state.Length; j++) {
                    string symbol = this.current_state[j].ToString();
                    switch (symbol) {
                        case "F":
                            new_state += "F+F-F-F+F";
                            break;
                        case "+":
                            new_state += "+";
                            break;
                        case "-":
                            new_state += "-";
                            break;
                        default:
                            break;
                    }
                }
                this.current_state = new_state;
                this.results.Add(this.current_state);
            }
        }
    }

    public sealed class KochSnowflake : LindenmeyerSystem {
        /* 
         * Koch Snowflake applies identical symbols to basic Koch Curve
         * although inheritance is avoided here due to these systems
         * obiding by different instructions for Turtle Graphics.
         */
        private readonly string KochSnowflakeVariables = "F";
        private readonly string KochSnowflakeConstants = "+-";

        public KochSnowflake() : base() {
            this.variables = KochSnowflakeVariables;
            this.constants = KochSnowflakeConstants;
            this.axiom = "F++F++F";
            this.current_state = this.axiom;
            this.Rule_set = ApplyRules;
        }

        public override void ApplyRules(int iterations) {
            for (int i = 0; i < iterations; i++) {
                string new_state = "";
                for (int j = 0; j < this.current_state.Length; j++) {
                    string symbol = this.current_state[j].ToString();
                    switch (symbol) {
                        case "F":
                            new_state += "F-F++F-F";
                            break;
                        case "+":
                            new_state += "+";
                            break;
                        case "-":
                            new_state += "-";
                            break;
                        default:
                            break;
                    }
                }
                this.current_state = new_state;
                this.results.Add(this.current_state);
            }
        }
    }

    public sealed class SimplePlant : LindenmeyerSystem {
        private readonly string SimplePlantVariables = "AF";
        private readonly string SimplePlantConstants = "+-[]";

        public SimplePlant(string axiom) {
            this.variables = SimplePlantVariables;
            this.constants = SimplePlantConstants;
            this.axiom = axiom;
            this.current_state = this.axiom;
            this.Rule_set = ApplyRules;
        }

        public override void ApplyRules(int iterations) {
            for (int i = 0; i < iterations; i++) {
                string new_state = "";
                for (int j = 0; j < this.current_state.Length; j++) {
                    string symbol = this.current_state[j].ToString();
                    switch (symbol) {
                        case "A":
                            new_state += "F-[[A]+A]+F[+FA]-A";
                            break;
                        case "F":
                            new_state += "FF";
                            break;
                        case "+":
                            new_state += "+";
                            break;
                        case "-":
                            new_state += "-";
                            break;
                        case "[":
                            new_state += "[";
                            break;
                        case "]":
                            new_state += "]";
                            break;
                        default:
                            break;
                    }
                }
                this.current_state = new_state;
                this.results.Add(this.current_state);
            }
        }
    }

    public sealed class DragonCurve : LindenmeyerSystem {
        private readonly string DragonCurveVariables = "AB";
        private readonly string DragonCurveConstants = "F+-";

        public DragonCurve(string axiom) {
            this.variables = DragonCurveVariables;
            this.constants = DragonCurveConstants;
            this.axiom = axiom;
            this.current_state = this.axiom;
            this.Rule_set = ApplyRules;
        }

        public override void ApplyRules(int iterations) {
            for (int i = 0; i < iterations; i++) {
                string new_state = "";
                for (int j = 0; j < this.current_state.Length; j++) {
                    string symbol = this.current_state[j].ToString();
                    switch (symbol) {
                        case "A":
                            new_state += "A+BF";
                            break;
                        case "B":
                            new_state += "FA-B";
                            break;
                        case "F":
                            new_state += "F";
                            break;
                        case "+":
                            new_state += "+";
                            break;
                        case "-":
                            new_state += "-";
                            break;
                        default:
                            break;
                    }
                }
                this.current_state = new_state;
                this.results.Add(this.current_state);
            }
        }
    }
    #endregion
}
#endif
