using Verse;

namespace ZPsych
{
    /// <summary>
    /// TODO:
    /// 
    /// Later:
    /// Enable Scribing
    /// /// PostExposeData
    /// Activate Sexuality
    /// /// declare Pawn_SexualityTracker Sexuality
    /// /// LDRTick thing?
    /// </summary>
    
    class CompPsychology : ThingComp
    {
        //private Pawn_SexualityTracker sexuality;
        private Pawn_PsychTracker psyche;
        //Actually commented in source //public Pawn_TourMemories recruiting;
        private bool beenBuried = false;
        private int tickSinceLastSeenLover;

        public Pawn_PsychTracker Psyche
        {
            get
            {
                if (this.psyche == null)
                {
                    Pawn p = this.parent as Pawn;
                    if (p != null)
                    {
                        this.psyche = new Pawn_PsychTracker(p);
                        this.psyche.Initialize();
                        foreach (PersonalityNode node in this.psyche.PersonalityNodes)
                        {
                            if (node.rawRating < 0)
                            {
                                //ZW Check: Just reminding self to check into this node nonsense carefully
                                node.Initialize();
                            }
                        }
                    }
                    else
                    {
                        Log.Error("Psychology :: CompPsychology was added to " + this.parent.Label + " which cannot be cast to a Pawn.");
                    }
                }
                return this.psyche;
            }
            set
            {
                this.psyche = value;
            }
        }

        /// <summary>
        /// Transcribers note: wtf
        /// </summary>
        public bool AlreadyBuried
        {
            get
            {
                return this.beenBuried;
            }
            set
            {
                this.beenBuried = value;
            }
        }

        public bool isPsychologyPawn
        {
            get
            {
                return this.Psyche != null && true;
                //return this.Psyche != null && this.Sexuality != null;
            }
        }
    }
}
