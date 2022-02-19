using System.Collections.Generic;
using Verse;

namespace ZPsych
{
    /// <summary>
    /// TODO:
    /// ... And dole comps out to Pawns (who are intelligent)
    /// 
    /// Later:
    /// Figure out LogPerformance Attribute
    /// Implement data scribing
    /// </summary>
    class PsychHelper
    {
        /// <summary>
        /// Identifies that given pawn exists and has psychology initiated.
        /// </summary>
        /// <param name="pawn"></param>
        /// <returns></returns>
        public static bool PsychologyEnabled(Pawn pawn)
        {
            return pawn != null && pawn.GetComp<CompPsychology>() != null && pawn.GetComp<CompPsychology>().isPsychologyPawn;
        }

        /// <summary>
        /// Gets pawn's psychology component
        /// Of note: Does NOT check if component exists (possibly intentional
        /// Of note: Why is this call useful???
        /// </summary>
        /// <param name="pawn"></param>
        /// <returns></returns>
        //[LogPerformance]
        public static CompPsychology Comp(Pawn pawn)
        {
            return pawn.GetComp<CompPsychology>();
        }
    }
}
