using CrystalArena.Triggers;

namespace CrystalArena.Events
{
    public class AttachmentDetachedEvent : ITriggerMessage
    {
        public readonly Card AttachedTo;
        public readonly Card Attachment;

        public AttachmentDetachedEvent(Card attachment, Card attachedTo)
        {
            Attachment = attachment;
            AttachedTo = attachedTo;
        }
    }
}
