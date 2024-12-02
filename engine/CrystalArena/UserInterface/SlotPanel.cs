namespace CrystalArena.UserInterface
{
  using Avalonia;
  using System;
  using Avalonia.Controls;
using Avalonia.Markup.Xaml;

  public class SlotPanel : Panel
  {
    /*
    public int ChildHorizontalOffset { get { return (int) GetValue(ChildHorizontalOffsetProperty); } set { SetValue(ChildHorizontalOffsetProperty, value); } }

    public int ChildVerticalOffset { get { return (int) GetValue(ChildVerticalOffsetProperty); } set { SetValue(ChildVerticalOffsetProperty, value); } }

    protected override Size ArrangeOverride(Size finalSize)
    {
      for (var i = 0; i < Children.Count; i++)
      {
        var child = Children[i];
        child.Arrange(new Rect(i*ChildHorizontalOffset, i*ChildVerticalOffset,
          child.DesiredSize.Width, child.DesiredSize.Height));
      }

      return finalSize;
    }

    protected override Size MeasureOverride(Size availableSize)
    {
      double resultHeight = 0;
      double resultWidth = 0;

      for (var i = 0; i < Children.Count; i++)
      {
        var child = Children[i];
        child.Measure(availableSize);

        resultHeight = Math.Max(resultHeight, child.DesiredSize.Height + i*ChildVerticalOffset);
        resultWidth = Math.Max(resultWidth, child.DesiredSize.Width + i*ChildHorizontalOffset);
      }

      resultWidth = double.IsPositiveInfinity(availableSize.Width)
        ? resultWidth
        : availableSize.Width;

      resultHeight = double.IsPositiveInfinity(availableSize.Height)
        ? resultHeight
        : availableSize.Height;

      return new Size(resultWidth, resultHeight);
    }
    
    */
  }
}