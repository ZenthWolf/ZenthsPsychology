using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Verse;
using UnityEngine;

namespace ZPsych
{
    /// <summary>
    /// TODO:
    /// Make sure Personality Nodes make it into the database
    /// 
    /// Later:
    /// Opinions: add method TotalThoughtOpinion
    ///                      OpinionCacheDirty
    ///                      DisagreementCacheDirty
    /// Conversions: add method GetConversationTopicWeight
    /// Figure out LogPerformance Attribute
    /// </summary>
    class Pawn_PsychTracker : IExposable
    {
        public int upbringing;
        public int lastDateTick = 0;
        private Pawn pawn;
        private HashSet<PersonalityNode> nodes;
        private Dictionary<PersonalityNodeDef, PersonalityNode> nodeDict = new Dictionary<PersonalityNodeDef, PersonalityNode>();
        private Dictionary<string, float> cachedOpinions = new Dictionary<string, float>();
        private Dictionary<string, bool> recalcCachedOpinions = new Dictionary<string, bool>();
        private Dictionary<Pair<string, string>, float> cachedDisagreementWeights = new Dictionary<Pair<string, string>, float>();
        private Dictionary<Pair<string, string>, bool> recalcNodeDisagreement = new Dictionary<Pair<string, string>, bool>();
        public const int PersonalityCategories = 16; //ZW Check: This is that Meyer's Briggs type shit- Dig into this

        public Pawn_PsychTracker(Pawn pawn)
        {
            this.pawn = pawn;
        }

        //[LogPerformance]
        public void Initialize()
        {
            //ZW Check: This is just entirely random??
            this.upbringing = Mathf.RoundToInt(Rand.ValueSeeded(this.pawn.HashOffset()) * (PersonalityCategories - 1)) + 1;
            this.nodes = new HashSet<PersonalityNode>();
            foreach (PersonalityNodeDef def in DefDatabase<PersonalityNodeDef>.AllDefsListForReading)
            {
                nodes.Add(PersonalityNodeMaker.MakeNode(def, this.pawn));
            }
            foreach (PersonalityNode n in this.nodes)
            {
                //ZW Check: ????
                nodeDict[n.def] = n;
            }
        }

        /// <summary>
        /// Required Implementations
        /// Commented out in exploration phase
        /// </summary>
        public void ExposeData()
        {
            /*
            Scribe_Values.Look(ref this.upbringing, "upbringing", 0, false);
            Scribe_Values.Look(ref this.lastDateTick, "lastDateTick", 0, false);
            PsycheHelper.Look(ref this.nodes, "nodes", LookMode.Deep, new object[] { this.pawn });
            foreach (PersonalityNode n in this.nodes)
            {
                nodeDict[n.def] = n;
            }

            */
        }

        //[LogPerformance]
        /// <summary>
        /// Why.?
        /// </summary>
        /// <param name="def"></param>
        /// <returns></returns>
        public float GetPersonalityRating(PersonalityNodeDef def)
        {
            return GetPersonalityNodeOfDef(def).AdjustedRating;
        }

        public PersonalityNode GetPersonalityNodeOfDef(PersonalityNodeDef def)
        {
            return nodeDict[def];
        }

        public HashSet<PersonalityNode> PersonalityNodes
        {
            get
            {
                return nodes;
            }
            set
            {
                nodes = value;
            }
        }
    }
}
