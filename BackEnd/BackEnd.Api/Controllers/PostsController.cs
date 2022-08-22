using System.Threading.Tasks;
using BackEnd.Api.Data;
using BackEnd.Api.Dtos;
using BackEnd.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.EntityFrameworkCore;

namespace BackEnd.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly PostDbContext _postDbDbContext;

        public PostsController(PostDbContext postDbDbContext)
        {
            _postDbDbContext = postDbDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var posts = await _postDbDbContext
                .Posts
                .ToListAsync();

            if (posts is null)
            {
                return NotFound();
            }

            return Ok(posts);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute]int id)
        {
            var post = await _postDbDbContext
                .Posts
                .SingleOrDefaultAsync(post => post.Id.Equals(id));

            if (post is null)
            {
                return NotFound();
            }

            return Ok(post);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreatePostDto createPostDto)
        {
            if (string.IsNullOrWhiteSpace(createPostDto.Content))
                return BadRequest();
            if (string.IsNullOrWhiteSpace(createPostDto.PhotoUrl))
                return BadRequest();

            var newPost = new Post
            {
                Content = createPostDto.Content,
                PhotoUrl = createPostDto.PhotoUrl
            };

            _postDbDbContext.Posts
                .Add(newPost);

            await _postDbDbContext.SaveChangesAsync();

            if (newPost.Id == default)
            {
                return Problem();
            }

            return CreatedAtAction(nameof(GetById), new { newPost.Id }, new { newPost.Id });
        }
    }
}