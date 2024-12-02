namespace CrystalArena.Triggers
{
  using CrystalArena.Events;
  using CrystalArena.Infrastructure;

  public class OnAttachmentDetached : Trigger, IReceive<AttachmentDetachedEvent>
  {
    public void Receive(AttachmentDetachedEvent message)
    {
      if (message.AttachedTo == Ability.SourceCard)
        Set(message);
    }
  }
}