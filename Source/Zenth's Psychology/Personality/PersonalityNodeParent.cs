
namespace ZPsych
{
    /// <summary>
    /// Wonderful.
    /// Could this be a struct? Does it matter?
    /// </summary>
    public class PersonalityNodeParent
    {
        //The node this node descends from.
        public PersonalityNodeDef node;
        /* How that node modifies the node.
         * -1f = Half as influential.
         * 0f = Normal.
         * 1f = Inverse.
         * 2f = Inverse, half as influential.
         * Yes, this is the opposite of what you would expect it to be.
         */
        public float modifier;
    }
}
