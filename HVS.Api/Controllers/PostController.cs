using Microsoft.AspNetCore.Mvc;
using HVS.Api.Core.Business.Services;
using System;
using Microsoft.AspNetCore.Cors;
using HVS.Api.Core.Business.Models.Posts;
using HVS.Api.Core.Business.Filters;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HVS.Api.Controllers
{
    [Route("api/posts")]
    public class PostController : Controller
    {

        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        
        [HttpGet]
        public async Task<IActionResult> Get(PostRequestListViewModel postRequestListViewModel)
        {
            var posts = await _postService.ListPostAsync(postRequestListViewModel);

            return Ok(posts);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostRequestCreateViewModel postRequestCreateViewModel)
        {
            var post = await _postService.CreatePostAsync(postRequestCreateViewModel);

            return Ok(post);
        }
    }
}
