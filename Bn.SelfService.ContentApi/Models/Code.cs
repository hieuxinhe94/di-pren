namespace Bn.SelfService.ContentApi.Models
{
    public class Code
    {
        public Code(string codeListId, string heading, string image, string instructionText, string buttonText, string termsText)
        {
            CodeListId = codeListId;
            Heading = heading;
            ImageUrl = image;
            InstructionText = instructionText;
            ButtonText = buttonText;
            TermsText = termsText;
        }

        public string CodeListId { get; set; }
        public string Heading { get; set; }
        public string ImageUrl { get; set; }
        public string InstructionText { get; set; }
        public string ButtonText { get; set; }
        public string TermsText { get; set; }
    }
}
