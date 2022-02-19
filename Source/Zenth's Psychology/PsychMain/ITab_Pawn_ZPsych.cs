using RimWorld;
using Verse;
using UnityEngine;

namespace ZPsych
{
    /// <summary>
    /// 100% copied for now
    /// TODO
    /// Check where Psyche is keyed (if it is!)
    /// Actually make a key entry for TabPsyche (Does this need to be tagged with Z? This is a hard incompatibility...
    /// </summary>
    public class ITab_Pawn_ZPsych : ITab
    {
        /// <summary>
        /// ctor
        /// Expected use: Set size of Psyche tab
        /// Hold string key for tab name
        /// ??? Tutor bit seems not to be a key??
        /// </summary>
        public ITab_Pawn_ZPsych()
        {
            this.size = new Vector2(500f, 470f); ;
            this.labelKey = "TabPsyche";
            this.tutorTag = "Psyche";
        }

        /// <summary>
        /// required override- calls Psyche Card Utility to fill info in card
        /// </summary>
        protected override void FillTab()
        {
            PsychCardUtility.DrawPsycheCard(new Rect(0f, 0f, this.size.x, this.size.y), this.PawnToShowInfoAbout);
        }

        /// <summary>
        /// ???????
        /// </summary>
        public override bool IsVisible
        {
            get
            {
                return PsychHelper.PsychologyEnabled(this.PawnToShowInfoAbout);
            }
        }

        /// <summary>
        /// Identifies to Pawn selected (even if currently a corpse)
        /// </summary>
        private Pawn PawnToShowInfoAbout
        {
            get
            {
                Pawn pawn = null;
                if (base.SelPawn != null)
                {
                    pawn = base.SelPawn;
                }
                else
                {
                    Corpse corpse = base.SelThing as Corpse;
                    if (corpse != null)
                    {
                        pawn = corpse.InnerPawn;
                    }
                }
                if (pawn == null)
                {
                    Log.Error("Psyche tab found no selected pawn to display.");
                    return null;
                }
                return pawn;
            }
        }
    }
}