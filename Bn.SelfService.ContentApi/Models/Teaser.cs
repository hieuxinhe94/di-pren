namespace Bn.SelfService.ContentApi.Models
{
    public class Teaser
    {
        public Teaser(string title, string text, string imageUrl, string buttonText, string buttonLinkUrl)
        {
            Title = title;
            Text = text;
            ImageUrl = imageUrl;
            ButtonText = buttonText;
            ButtonLinkUrl = buttonLinkUrl;
        }
        public string Title { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        public string ButtonText { get; set; }
        public string ButtonLinkUrl { get; set; }
    }
}