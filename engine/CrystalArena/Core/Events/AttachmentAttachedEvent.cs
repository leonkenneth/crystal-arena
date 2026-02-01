using CrystalArena.Triggers;

namespace CrystalArena.Events
{
    public class AttachmentAttachedEvent : ITriggerMessage
    {
        public readonly Card Attachment;

        public AttachmentAttachedEvent(Card attachment)
        {
            Attachment = attachment;
        }

        public Card AttachedTo
        {
            get { return Attachment.AttachedTo; }
        }
    }
}
