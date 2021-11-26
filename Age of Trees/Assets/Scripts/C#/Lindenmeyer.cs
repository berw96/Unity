using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
         * Take a string of characters (symbols),
         * mutate them based on a specified ruleset, 
         * and concactenate the results to the system's
         * current_state member.
         */
        public delegate void RuleSet(string symbols);
        private RuleSet rule_set;

        protected string variables;
        protected string constants;
        protected string axiom;
        protected string current_state;

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

        /// <summary>
        /// Takes a defined list of characters and applies
        /// a predefined set of mutations on each character
        /// constituting the grammar.
        /// </summary>
        /// <param name="symbols"></param>
        public virtual void ApplyRules(string symbols) {
            // If the system has no axiom set, assign the value of the provided symbol(s).
            if (string.IsNullOrEmpty(this.axiom))
                this.axiom = symbols;

            // If the system's current state is null, initialize it before applying ruleset.
            if (string.IsNullOrEmpty(this.current_state))
                this.current_state = this.axiom;

            // Invokes method assigned to the rule_set delegate.
            Rule_set.Invoke(symbols); 
        }
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
        }

        public override void ApplyRules(string symbols) {
            string new_state = "";
            for (int i = 0; i < symbols.Length; i++) {
                string symbol = symbols[i].ToString();
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
        }
    }

    public class KochCurve : LindenmeyerSystem {
        private readonly string KochCurveVariables = "F";
        private readonly string KochCurveConstants = "+-";

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
        private readonly string SimplePlantVariables = "AF";
        private readonly string SimplePlantConstants = "+-[]";

        public SimplePlant() {
            this.variables = SimplePlantVariables;
            this.constants = SimplePlantConstants;
        }
    }

    public sealed class DragonCurve : LindenmeyerSystem {
        private readonly string DragonCurveVariables = "AB";
        private readonly string DragonCurveConstants = "F+-";

        public DragonCurve() {
            this.variables = DragonCurveVariables;
            this.constants = DragonCurveConstants;
        }
    }
    #endregion
}

