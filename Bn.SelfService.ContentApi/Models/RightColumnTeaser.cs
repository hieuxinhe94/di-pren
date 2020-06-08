namespace Bn.SelfService.ContentApi.Models
{
    public class RightColumnTeaser
    {
        public RightColumnTeaser(string title, string text)
        {
            Title = title;
            Text = text;
        }

        public string Title { get; set; }
        public string Text { get; set; }
    }
}
