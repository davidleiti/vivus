namespace Vivus.Core.DataModels
{
    /// <summary>
    /// Specifies the display state of an element.
    /// </summary>
    public enum Visibility
    {
        /// <summary>
        /// Do not display the element, and do not reserve space for it in layout.
        /// </summary>
        Collapsed,

        /// <summary>
        /// Do not display the element, but reserve space for the element in layout.
        /// </summary>
        Hidden,

        /// <summary>
        /// Display the element.
        /// </summary>
        Visible
    }
}
