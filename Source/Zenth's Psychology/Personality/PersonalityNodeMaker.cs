using Verse;

namespace ZPsych
{
    public static class PersonalityNodeMaker
    {
        public static PersonalityNode MakeNode(PersonalityNodeDef def, Pawn pawn)
        {
            PersonalityNode node = new PersonalityNode(pawn);
            node.def = def;
            node.Initialize();
            return node;
        }
    }
}
