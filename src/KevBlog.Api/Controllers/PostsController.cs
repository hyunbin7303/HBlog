using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;
using System.Security.Claims;
using KevBlog.Domain.Entities;

namespace API.Controllers
{
    [Authorize]
    public class PostsController : BaseApiController
    {
        private readonly IPostRepository _postRepository;

        public PostsController(IPostRepository postRepository)
        {
            this._postRepository = postRepository;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Post>>> GetPosts()
        {
            return Ok(await _postRepository.GetPostsAsync());
        }

        [AllowAnonymous]
        [HttpGet("users/{username}")]
        public async Task<ActionResult<Post>> GetPostByUsername(string username)
        {
            return await _postRepository.GetPostByUsername(username);
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPostById(int id)
        {
            return await _postRepository.GetPostById(id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Post post)
        {
            _postRepository.Update(post);
            return Ok(_postRepository.SaveAllAsync());
        }
        [HttpPut("{id}/status")]
        public async Task<IActionResult> SetPostStatus(int id, int statusId)
        {
            var post = await _postRepository.GetPostById(id);
            // post.Status
            return Ok();
        }
        
        [HttpPost]
        public async Task<ActionResult<User>> Post(Post post)
        {
            _postRepository.Update(post);
            return Ok(await _postRepository.SaveAllAsync());
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _postRepository.Remove(id);
            return Ok(await _postRepository.SaveAllAsync());
        }
    }
}
