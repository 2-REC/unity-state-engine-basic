using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

// TODO: namespace?
namespace StateEngine.Runtime {

    //TODO: rename to "GameData" (?)
    public class DifficultyData {
        private const int DEFAULT_LIVES = 1;
        private const int DEFAULT_CONTINUES = 0;

        private readonly List<DifficultyValues> difficultyValues;

        public class DifficultyValues {
            public int lives;
            public int continues;
            public Dictionary<string, int> fields = null;

            // Default values if nothing is provided.
            public DifficultyValues() {
                lives = DEFAULT_LIVES;
                continues = DEFAULT_CONTINUES;
                fields = new Dictionary<string, int>();
            }

            public DifficultyValues(int lives, int continues, Dictionary<string, int> fields) {
                this.lives = lives;
                this.continues = continues;
                this.fields = (fields != null) ? fields : new Dictionary<string, int>();
            }
        }


        public DifficultyData(TextAsset xmlFile) {
            difficultyValues = LoadValues(xmlFile);
        }

        public DifficultyValues GetValues(int difficulty) {
            if (difficulty < 0 || difficulty >= difficultyValues.Count) {
                throw new ArgumentOutOfRangeException("difficulty", "Invalid difficulty index: " + difficulty);
            }

            return difficultyValues[difficulty];
        }

        public static Dictionary<string, int> LoadGlobalDefaults(TextAsset xmlFile) {
            Dictionary<string, int> defaults = new Dictionary<string, int>();
            if (xmlFile == null || string.IsNullOrEmpty(xmlFile.text)) {
                return defaults;
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlFile.text);

            XmlNodeList globals = xmlDoc.GetElementsByTagName("global");
            foreach (XmlNode node in globals) {
                ParseFields(node, defaults, false);
            }

            // TODO: why? aren't these game fields?
            if (defaults.Count == 0) {
                XmlNodeList commons = xmlDoc.GetElementsByTagName("common");
                foreach (XmlNode node in commons) {
                    ParseFields(node, defaults, false);
                }
            }

            return defaults;
        }

        private static List<DifficultyValues> LoadValues(TextAsset xmlFile) {
            List<DifficultyValues> values = ParseDifficultyValues(xmlFile);
            if (values.Count == 0) {
                values.Add(new DifficultyValues());
            }
            return values;
        }

        private static List<DifficultyValues> ParseDifficultyValues(TextAsset xmlFile) {
            ValidateXmlFile(xmlFile);

            List<DifficultyValues> values = new List<DifficultyValues>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlFile.text);

            // get common values (if any)
            Dictionary<string, int> commonFields = new Dictionary<string, int>();
            XmlNodeList commons = xmlDoc.GetElementsByTagName("common");
            foreach (XmlNode node in commons) {
                ParseFields(node, commonFields, true);
            }

            // get difficulty specific values
            XmlNodeList nodes = xmlDoc.GetElementsByTagName("difficulty");
            foreach (XmlNode node in nodes) {
                int lives = DEFAULT_LIVES;
                int continues = DEFAULT_CONTINUES;
                Dictionary<string, int> fields = new Dictionary<string, int>(commonFields);

                if (node.Attributes != null) {
                    foreach (XmlAttribute attribute in node.Attributes) {
                        if (attribute.Name.Equals("lives")) {
                            int.TryParse(attribute.Value, out lives);
                        } else if (attribute.Name.Equals("continues")) {
                            int.TryParse(attribute.Value, out continues);
                        }
                    }
                }

                ParseFields(node, fields, true);
                values.Add(new DifficultyValues(lives, continues, fields));
            }

            return values;
        }

        private static void ParseFields(XmlNode node, Dictionary<string, int> fields, bool validateReservedNames) {
            if (node == null || !node.HasChildNodes) {
                return;
            }

            foreach (XmlNode child in node.ChildNodes) {
                if (!child.Name.Equals("field")) {
                    continue;
                }

                string name = null;
                int value = 0;
                bool valueSet = false;

                if (child.Attributes != null) {
                    foreach (XmlAttribute attribute in child.Attributes) {
                        if (attribute.Name.Equals("name")) {
                            name = attribute.Value;
                        } else if (attribute.Name.Equals("value")) {
                            valueSet = int.TryParse(attribute.Value, out value);
                        }
                    }
                }

                if (string.IsNullOrEmpty(name) || !valueSet) {
                    throw new FormatException("Malformed <field> node in values XML.");
                }

                if (validateReservedNames) {
                    ValidateFieldName(name);
                }

                fields[name] = value;
            }
        }

        private static void ValidateXmlFile(TextAsset xmlFile) {
            if (xmlFile == null) {
                throw new ArgumentNullException("xmlFile", "XML TextAsset cannot be null.");
            }

            if (string.IsNullOrEmpty(xmlFile.text)) {
                throw new ArgumentException("XML TextAsset is empty.", "xmlFile");
            }
        }

        private static void ValidateFieldName(string name) {
            if (string.IsNullOrEmpty(name)) {
                throw new ArgumentException("Field name cannot be null or empty.", "name");
            }

            if (name.Equals("LEVEL")
                || name.Equals("DIFFICULTY")
                || name.Equals("LIVES")
                || name.Equals("CONTINUES")
                || name.StartsWith("INITIAL_", StringComparison.Ordinal)
                || name.StartsWith("LEVEL_", StringComparison.Ordinal)) {
                throw new ArgumentException("Reserved field name cannot be declared in values.xml: " + name, "name");
            }
        }
    }
}
