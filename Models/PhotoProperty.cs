
using ReactiveUI;

namespace PhZx.Models
{
    public class PhotoProperty : ReactiveObject
    {
        public bool Resize { get; set; }
        public string Suffix { get; set; }
        public int Quality { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
    }
}