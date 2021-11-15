using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Contains class definitions for common Lindenmeyer systems (L-Systems)
/// derived from a base class providing the user with the option to compose
/// their own grammars.
/// </summary>
namespace Lindenmeyer {

    public enum MODE {
        DETERMINISTIC,
        STOCHASTIC,
        CONTEXT_SENSITIVE
    }

    public class LindenmeyerSystem {

        protected MODE mode;

        /*
         * Take a list of characters (symbols),
         * mutate them based on a specified ruleset, 
         * and assign the results to the system's current_state member.
         */
        public delegate void RuleSet(List<char> symbols);
        private RuleSet rule_set;

        protected List<char> variables;
        protected List<char> constants;
        protected List<char> axiom;
        protected string current_state;

        public MODE Mode {
            get { return this.mode; }
            set { this.mode = value; }
        }

        public RuleSet Rule_set {
            get { return this.rule_set; }
            set { this.rule_set = value; }
        }

        public List<char> Variables {
            get { return this.variables; }
            set { this.variables = value; }
        }

        public List<char> Constants {
            get { return this.constants; }
            set { this.constants = value; }
        }

        public List<char> Axiom {
            get { return this.axiom; }
            set { this.axiom = value; }
        }

        public string Current_state {
            get { return this.current_state; }
            set { this.current_state = value; }
        }

        // default constructor for subclasses and custom L-Systems
        public LindenmeyerSystem() {
            variables = new List<char>();
            constants = new List<char>();
            axiom = new List<char>();
        }

        #region CUSTOM_LSYSTEM_CONSTRUCTORS
        public LindenmeyerSystem(List<char> variables) {
            this.variables = variables;
            constants = new List<char>();
            axiom = new List<char>();
        }
        
        public LindenmeyerSystem(
            List<char> variables,
            List<char> constants) {
            this.variables = variables;
            this.constants = constants;
            axiom = new List<char>();
        }
        
        public LindenmeyerSystem(
            List<char> variables, 
            List<char> constants,
            List<char> axiom) {
            this.variables = variables;
            this.constants = constants;
            this.axiom = axiom;
        }

        public LindenmeyerSystem(
            List<char> variables,
            List<char> constants,
            List<char> axiom,
            MODE mode) {
            this.variables = variables;
            this.constants = constants;
            this.axiom = axiom;
            this.mode = mode;
        }

        public LindenmeyerSystem(
            List<char> variables,
            List<char> constants,
            List<char> axiom,
            MODE mode,
            RuleSet rule_set) {
            this.variables = variables;
            this.constants = constants;
            this.axiom = axiom;
            this.mode = mode;
            this.rule_set = rule_set;
        }
        #endregion

        public virtual void ApplyRules(List<char> symbols) { Rule_set.Invoke(symbols); }
    }

    #region PRESETS
    public sealed class SierpinskiTriangle : LindenmeyerSystem {
        private readonly List<char> sierpinski_triangle_variables = new List<char>() { 'A', 'B' };
        private readonly List<char> sierpinski_triangle_constants = new List<char>() { '+', '-' };

        public SierpinskiTriangle(char axiom) : base() {
            this.variables = sierpinski_triangle_variables;
            this.constants = sierpinski_triangle_constants;
            this.axiom.Add(axiom);
            this.current_state = this.axiom.ToString();
        }

        public override void ApplyRules(List<char> symbols) {
            foreach (char symbol in symbols) {
                if(this.variables.Contains(symbol) &&
                    this.constants.Contains(symbol)) {
                    return;
                } else {
                    switch (symbol) {
                        case 'A':
                            this.current_state.Replace(symbol.ToString(), "B-A-B");
                            break;
                        case 'B':
                            this.current_state.Replace(symbol.ToString(), "A+B+A");
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public class KochCurve : LindenmeyerSystem {
        private readonly List<char> KochCurveVariables = new List<char>() { 'F' };
        private readonly List<char> KochCurveConstants = new List<char>() { '+', '-' };

        public KochCurve() {
            this.variables = KochCurveVariables;
            this.constants = KochCurveConstants;
        }
    }

    public sealed class KochSnowflake : KochCurve {
        // Koch Snowflake applies identical symbols to basic Koch Curve
        public KochSnowflake() : base() {}
    }

    public sealed class SimplePlant : LindenmeyerSystem {
        private readonly List<char> SimplePlantVariables = new List<char>() { 'A', 'F' };
        private readonly List<char> SimplePlantConstants = new List<char>() { '+', '-', '[', ']' };

        public SimplePlant() {
            this.variables = SimplePlantVariables;
            this.constants = SimplePlantConstants;
        }
    }

    public sealed class DragonCurve : LindenmeyerSystem {
        private readonly List<char> DragonCurveVariables = new List<char>() { 'A', 'B' };
        private readonly List<char> DragonCurveConstants = new List<char>() { 'F', '+', '-' };

        public DragonCurve() {
            this.variables = DragonCurveVariables;
            this.constants = DragonCurveConstants;
        }
    }
    #endregion
}

