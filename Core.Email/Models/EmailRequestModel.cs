namespace Core.Email.Models 
{
    public class EmailRequestModel
    {
        public string From { get; set; }

        public string[] To { get; set; }

        public string[] CC { get; set; }

        public string[] BCC { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public bool? IsHtmlBody { get; set; }

        public string HtmlContent { get; set; }

        public bool? IsAttachment { get; set; }

        public string AttachedFilePath { get; set; }
    }


}