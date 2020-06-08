namespace Bn.CompanyService.ContentApi.Models
{
    public class Message
    {
        public Message(string id, string type, string title, string text)
        {
            Id = id;
            Type = type;
            Title = title;
            Text = text;
        }

        public string Id { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}
