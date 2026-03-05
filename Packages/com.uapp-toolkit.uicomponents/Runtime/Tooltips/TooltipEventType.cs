namespace UAppToolKit.UIComponents.Tooltips
{
    public enum TooltipEventType
    {
        None, //works only for manual ShowTooltip call
        Click, //works with all types of raycast targets
        LongPress, //works only with LongPressButton
        NotInteractableClick //works with all types of Selectables
    }
}