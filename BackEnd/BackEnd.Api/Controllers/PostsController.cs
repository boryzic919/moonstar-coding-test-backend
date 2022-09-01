using BackEnd.Api.Dtos;
using BackEnd.Api.Models;
using BackEnd.Api.Repository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository postRepository;

        public PostsController(IPostRepository _postRepository)
        {
            postRepository = _postRepository;
        }

        /// <summary>
        /// get all post
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var posts = (await postRepository.GetPostsAsync()).Select(a => a.AsDto());


            if (posts is null)
            {
                return NotFound();
            }

            return Ok(posts);
        }
        /// <summary>
        /// Get post based on id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var post = await postRepository
                .GetPostAsync(id);

            if (post is null)
            {
                return NotFound();
            }

            return Ok(post);
        }
        /// <summary>
        /// create a new post based on photo and content
        /// </summary>
        /// <param name="createPostDto">create post dto</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePostDto createPostDto)
        {
            if (string.IsNullOrWhiteSpace(createPostDto.Content) && string.IsNullOrWhiteSpace(createPostDto.PhotoUrl))
                return BadRequest();

            var newPost = new Post
            {
                Content = createPostDto.Content,
                PhotoUrl = createPostDto.PhotoUrl
            };

            await postRepository.CreatePostAsync(newPost);

            if (newPost.Id == default)
            {
                return Problem();
            }

            return CreatedAtAction(nameof(GetById), new { newPost.Id }, new { newPost.Id });
        }

        /// <summary>
        /// update post based on id and data
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="item">postdto</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdatePostDto item)
        {
            var existingItem = await postRepository.GetPostAsync(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            existingItem.PhotoUrl = item.PhotoUrl;
            existingItem.Content = item.Content;

            await postRepository.UpdatePostAsync(existingItem);

            return NoContent();
        }
        /// <summary>
        /// Delete post based on id
        /// </summary>
        /// <param name="id">id </param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var existingItem = await postRepository.GetPostAsync(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            await postRepository.DeletePostAsync(id);

            return NoContent();
        }

        /// <summary>
        /// Update/replace post based on id and value
        /// </summary>
        /// <param name="id"> id</param>
        /// <param name="item"> post</param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdatePatch([FromRoute] int id, [FromBody] JsonPatchDocument item)
        {
            var existingItem = await postRepository.GetPostAsync(id);

            if (existingItem is null)
            {
                return NotFound();
            }

            item.ApplyTo(existingItem);
            await postRepository.UpdatePostAsync(existingItem);
            return NoContent();
        }

    }
}