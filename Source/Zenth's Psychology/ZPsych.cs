using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using Verse;
using HugsLib.Settings;
using HugsLib;
using UnityEngine;
using HarmonyLib;

namespace ZPsych
{
    class ZPsych : ModBase
    {
        public override string ModIdentifier
        {
            get
            {
                return "ZPsych";
            }
        }
        public override void DefsLoaded()
        {
            if (ModIsActive)
            {
                /* Find all things which are intelligent, but with a select few exclusions */
                var zombieThinkTree = DefDatabase<ThinkTreeDef>.GetNamedSilentFail("Zombie");

                // Fancy!
                IEnumerable<ThingDef> things = (
                    from def in DefDatabase<ThingDef>.AllDefs
                    where def.race?.intelligence == Intelligence.Humanlike
                    && !def.defName.Contains("AIPawn") && !def.defName.Contains("Robot")
                    && !def.defName.Contains("ChjDroid") && !def.defName.Contains("ChjBattleDroid")
                    && (zombieThinkTree == null || def.race.thinkTreeMain != zombieThinkTree)
                    select def
                );

                //Register all intelligent things, and then...
                List<string> registered = new List<string>();
                foreach (ThingDef t in things)
                {
                    // ... add the Psych tab (this probably happens before the list is populated, which is why it's first...
                    if (t.inspectorTabsResolved == null)
                    {
                        t.inspectorTabsResolved = new List<InspectTabBase>(1);
                    }
                    t.inspectorTabsResolved.Add(InspectTabManager.GetSharedInstance(typeof(ITab_Pawn_ZPsych)));

                    /* Probably anxiety related?
                    if (t.recipes == null)
                    {
                        t.recipes = new List<RecipeDef>(6);
                    }
                    */

                    // ... give the intelligent thing the Psychology component...
                    if (t.comps == null)
                    {
                        t.comps = new List<CompProperties>(1);
                    }
                    t.comps.Add(new CompProperties_Psychology());

                    /* New hediffs- mental breaks?
                    if (!t.race.hediffGiverSets.NullOrEmpty())
                    {
                        if (t.race.hediffGiverSets.Contains(DefDatabase<HediffGiverSetDef>.GetNamed("OrganicStandard")))
                        {
                            t.race.hediffGiverSets.Add(DefDatabase<HediffGiverSetDef>.GetNamed("OrganicPsychology"));
                        }
                    }
                    */
                    registered.Add(t.defName);

                    Log.Message("[ZPsych] Temporary Verbose log!");
                    Log.Message("Psychology :: Registered " + string.Join(", ", registered.ToArray()));
                }
            }
        }
    }
}
