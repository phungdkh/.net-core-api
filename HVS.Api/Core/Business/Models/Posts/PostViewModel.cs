
using System;

namespace HVS.Api.Core.Business.Models.Posts
{
    public class PostViewModel
    {
        public PostViewModel()
        {

        }

        public PostViewModel(HVS.Api.Core.Entities.Post post) : this()
        {
            if (post != null)
            {
                Id = post.Id;
                UserId = post.UserId;
                Title = post.Title;
                Content = post.Content;
                Thumbnail = post.Thumbnail;
            }
        }

        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Thumbnail { get; set; }
    }
}
