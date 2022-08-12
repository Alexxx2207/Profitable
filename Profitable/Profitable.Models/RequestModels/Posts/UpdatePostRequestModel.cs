namespace Profitable.Models.RequestModels.Posts
{
    public class UpdatePostRequestModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string? Image { get; set; }

        public string? ImageFileName { get; set; }
    }
}
