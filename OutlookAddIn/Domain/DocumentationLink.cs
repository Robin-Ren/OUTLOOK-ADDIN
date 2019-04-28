using System.Configuration;
using System.Windows.Controls;
using System.Windows.Input;

namespace OutlookAddIn.Domain
{
    public class DocumentationLink
    {
        public DocumentationLink(DocumentationLinkType type, string url) : this(type, url, null)
        {
        }

        public DocumentationLink(DocumentationLinkType type, string url, string label)
        {
            Label = label ?? type.ToString();
            Url = url;
            Type = type;
            Open = new RelayCommand(Execute);
        }

        public string Label { get; }

        public string Url { get; }

        public DocumentationLinkType Type { get; }        

        public ICommand Open { get; }

        private void Execute(object o)
        {
            System.Diagnostics.Process.Start(Url);
        }
    }
}