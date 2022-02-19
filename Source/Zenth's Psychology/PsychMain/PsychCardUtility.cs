using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using Verse;

namespace ZPsych
{
    /// <summary>
    /// TODO:
    /// Implement class PersonalityNode
    /// 
    /// Interim review:
    /// What and where is a nodestring?
    /// What is "Categories"? WHy is it 6f?
    /// 
    /// Later:
    /// Activate Sexuality? (Kinsey functionality seems to be a highlight to many)
    /// /// Draw in card
    /// /// DrawSexuality(Rect rect, Pawn pawn, bool notOnMenu)
    /// Figure out LogPerformance Attribute
    /// </summary>
    class PsychCardUtility
    {
        private static string[] NodeDescriptions = { "Not", "Slightly", "Less", "", "More", "Very", "Extremely" };
        private static Color[] NodeColors = { new Color(1f, 0.2f, 0.2f, 0.6f), new Color(1f, 0.4f, 0.4f, 0.4f), new Color(1f, 0.6f, 0.6f, 0.2f), Color.white, new Color(0.6f, 1f, 0.6f, 0.2f), new Color(0.4f, 1f, 0.4f, 0.4f), new Color(0.2f, 1f, 0.2f, 0.6f) };
        private static readonly Color HighlightColor = new Color(0.5f, 0.5f, 0.5f, 1f);
        private static float Categories = 6f;
        private static Vector2 nodeScrollPosition = Vector2.zero;
        private static List<Pair<string, int>> nodeStrings = new List<Pair<string, int>>();
        private const float RowLeftRightPadding = 5f;
        private const float RowTopPadding = 1f;

        //[LogPerformance]
        private static void DrawPersonalityNodes(Rect rect, Pawn pawn)
        {
            float width = rect.width - 26f - 3f;

            //ZW Check: "Orders nodes by rating in descending order"
            List<PersonalityNode> allNodes = (from n in PsychHelper.Comp(pawn).Psyche.PersonalityNodes
                                              orderby n.AdjustedRating descending, n.def.defName
                                              select n).ToList();
            //ZW Check: wtf dis ?
            PsychCardUtility.nodeStrings.Clear();
            float num = 0f;
            for (int i = 0; i < allNodes.Count(); i++)
            {
                float rating = allNodes[i].AdjustedRating;
                float yAxis = 0.5f - rating;
                float weight = Mathf.Sqrt(Mathf.Abs(rating - 0.5f)) * (1 / Mathf.Sqrt(0.5f));
                int category = Mathf.RoundToInt((Categories / 2) - (Categories * yAxis * weight));
                //ZW Check: Decide "intensity" name based on Node Strength
                if (/*!allNodes[i].Core && */category != 3)
                {
                    string text;
                    if (NodeDescriptions[category] == "")
                    {
                        text = "";
                    }
                    else
                    {
                        text = ("Psyche" + NodeDescriptions[category]).Translate();
                    }
                    PsychCardUtility.nodeStrings.Add(new Pair<string, int>(text, i));
                    num += Mathf.Max(26f, Text.CalcHeight(text, width));
                }
            }
            Rect viewRect = new Rect(0f, 0f, rect.width, num);
            viewRect.xMax *= 0.9f;
            Widgets.BeginScrollView(rect, ref PsychCardUtility.nodeScrollPosition, viewRect);
            float num3 = 0f;
            //ZW Check: Draws each line> nodeString relates to a line?
            for (int j = 0; j < PsychCardUtility.nodeStrings.Count; j++)
            {
                string first = PsychCardUtility.nodeStrings[j].First;
                PersonalityNode node = allNodes[PsychCardUtility.nodeStrings[j].Second];
                float num4 = Mathf.Max(26f, Text.CalcHeight(first, width));
                Rect rect2 = new Rect(10f, num3, width / 3, num4);
                Rect rect3 = new Rect(10f + width / 3, num3, (2 * width) / 3, num4);
                float rating = node.AdjustedRating;
                float yAxis = 0.5f - rating;
                float weight = Mathf.Sqrt(Mathf.Abs(rating - 0.5f)) * (1 / Mathf.Sqrt(0.5f));
                int category = Mathf.RoundToInt((Categories / 2) - (Categories * yAxis * weight));
                GUI.color = NodeColors[category];
                Widgets.Label(rect2, first.ToString());
                if (Prefs.DevMode && Prefs.LogVerbose)
                {
                    //ZW Check: ????
                    TooltipHandler.TipRegion(rect2, delegate { return (100f * node.AdjustedRating) + "%"; }, 693261 + j * 310);
                }
                GUI.color = Color.white;
                Widgets.DrawHighlightIfMouseover(rect3);
                Widgets.Label(rect3, node.def.label.CapitalizeFirst());
                Func<String> descriptionString = delegate
                {
                    if (node.def.conversationTopics != null)
                    {
                        return node.def.description + "\n\n" + "ConversationTooltip".Translate(string.Join(", ", node.def.conversationTopics.Take(node.def.conversationTopics.Count - 1).ToArray()) + "ConversationAnd".Translate() + node.def.conversationTopics.Last());
                    }
                    return node.def.description;
                };
                TooltipHandler.TipRegion(rect3, descriptionString, 613261 + j * 612);
                num3 += num4;
            }
            Widgets.EndScrollView();
        }

    /// <summary>
    /// Checks and calls if devmode edit psych is being requested
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="pawn"></param>
    public static void DrawDebugOptions(Rect rect, Pawn pawn)
    {
        GUI.BeginGroup(rect);
        if (Widgets.ButtonText(new Rect((rect.width * 0.6f) - 80f, 0f, 100f, 22f), "EditPsyche".Translate(), true, false, true))
        {
            Find.WindowStack.Add(new Dialog_EditPsych(pawn));
        }
        GUI.EndGroup();
    }

    /// <summary>
    /// I mean... it's below but slightly larger?
    /// Also has devmode option
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="pawn"></param>
    public static void DrawPsycheCard(Rect rect, Pawn pawn)
    {
        if(PsychHelper.PsychologyEnabled(pawn))
        {
            GUI.BeginGroup(rect);
            Text.Font = GameFont.Small;
            Rect rect2 = new Rect(20f, 20f, rect.width-20f, rect.height-20f);
            Rect rect3 = rect2.ContractedBy(10f);
            Rect rect4 = rect3;
            Rect rect5 = rect3;
            rect4.width *= 0.6f;
            rect5.x = rect4.xMax + 17f;
            rect5.xMax = rect3.xMax;
            GUI.color = new Color(1f, 1f, 1f, 0.5f);
            Widgets.DrawLineVertical(rect4.xMax, 0f, rect.height);
            GUI.color = Color.white;
            if (Prefs.DevMode)
            {
                Rect rect6 = new Rect(0f, 5f, rect3.width, 22f);
                PsychCardUtility.DrawDebugOptions(rect6, pawn);
            }
            PsychCardUtility.DrawPersonalityNodes(rect4, pawn);
            //PsycheCardUtility.DrawSexuality(rect5, pawn, true);
            GUI.EndGroup();
        }
    }

    /// <summary>
    /// Uhm.... ????
    /// Fills in personality node info..? maybe?
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="pawn"></param>
    public static void DrawPsychMenuCard(Rect rect, Pawn pawn)
    {
        if (PsychHelper.PsychologyEnabled(pawn))
        {
            GUI.BeginGroup(rect);
            Text.Font = GameFont.Small;
            Rect rect2 = new Rect(10f, 10f, rect.width - 10f, rect.height - 10f);
            Rect rect4 = rect2;
            Rect rect5 = rect2;
            rect4.width *= 0.6f;
            rect4.xMin -= 20f;
            rect5.x = rect4.xMax + 17f;
            rect5.xMax = rect.xMax;
            GUI.color = new Color(1f, 1f, 1f, 0.5f);
            Widgets.DrawLineVertical(rect4.xMax, 0f, rect.height);
            GUI.color = Color.white;
            PsychCardUtility.DrawPersonalityNodes(rect4, pawn);
            //PsycheCardUtility.DrawSexuality(rect5, pawn, false);
            GUI.EndGroup();
        }
    }
    


    }
}
