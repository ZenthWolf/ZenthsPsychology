using System.Collections.Generic;
using Verse;
using UnityEngine;

namespace ZPsych
{
    public class PersonalityNodeDef : Def
    {
        /* Being a woman has an 80% chance to modify this node by this amount, reduced by how gay she is.
         * This models the cultural impact traditional gender roles have on their personality. (Lesbians/bisexuals, obviously, tend to subvert them.)
         * Even in 55XX, the patriarchy has not been vanquished. /s
           - From Source
              -ZWR
         */
        public float femaleModifier;
        //A list of the DefNames of the parents of this node.
        public List<PersonalityNodeParent> parents;
        //What pawns talk about when they talk about this node.
        public List<string> conversationTopics;
        //What pawns with a high rating in this node use as a platform issue.
        public string platformIssueHigh;
        //What pawns with a low rating in this node use as a platform issue.
        public string platformIssueLow;
        //A list of the skills that modify this node.
        public List<PersonalityNodeSkillModifier> skillModifiers;
        //A list of the traits that modify this node.
        public List<PersonalityNodeTraitModifier> traitModifiers;
        //A list of the work types that being incapable of modify this node.
        public List<PersonalityNodeIncapableModifier> incapableModifiers;
        //How much a difference (or similarity) in this node affects what pawns think of each other after a conversation.
        public float controversiality;
        //The hours of the day that people with a high rating in this node will prefer to go on dates.
        public List<int> preferredDateHours;
        //A list of the actual parent Defs of this node.
        [Unsaved]
        private Dictionary<PersonalityNodeDef, PersonalityNodeParent> parentDict;

        public void ReloadParents()
        {
            parentDict.Clear();
        }

        /// <summary>
        /// This weirdness is exposited on in PersonalityNodeParent
        /// Consider making a enum (or will that cost?)
        /// </summary>
        /// <param name="def"></param>
        /// <returns></returns>
        public float GetModifier(PersonalityNodeDef def)
        {
            PersonalityNodeParent parent = ParentNodes[def];
            return (parent.modifier > 0 ? -1 / parent.modifier : 1 / Mathf.Abs(parent.modifier - 1));
        }

        /// <summary>
        /// Really confused abt this. Why does each NodeDef keep a dict of all other parents?
        /// smthng about the actual nodes being different as parental nodes?
        /// </summary>
        public Dictionary<PersonalityNodeDef, PersonalityNodeParent> ParentNodes
        {
            get
            {
                if (this.parentDict == null)
                {
                    this.parentDict = new Dictionary<PersonalityNodeDef, PersonalityNodeParent>();
                    if (this.parents != null && this.parents.Count > 0)
                    {
                        foreach (PersonalityNodeParent parent in this.parents)
                        {
                            this.parentDict.Add(parent.node, parent);
                        }
                    }
                }
                return this.parentDict;
            }
        }

        public override int GetHashCode()
        {
            return this.defName.GetHashCode();
        }
    }
}
