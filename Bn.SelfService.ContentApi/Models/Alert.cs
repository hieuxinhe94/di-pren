using System;

namespace Bn.SelfService.ContentApi.Models
{
    public class Alert
    {
        public Alert(string id, string title, string text)
        {
            Id = id;
            Title = title;
            Text = text;
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
