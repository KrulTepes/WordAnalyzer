namespace AngleSharp.Html.Dom
{
    using AngleSharp.Dom;
    using System;

    /// <summary>
    /// The strong HTML element.
    /// </summary>
    sealed class HtmlStrongElement : HtmlElement
    {
        public HtmlStrongElement(Document owner, String? prefix = null)
            : base(owner, TagNames.Strong, prefix, NodeFlags.HtmlFormatting)
        {
        }
    }
}
