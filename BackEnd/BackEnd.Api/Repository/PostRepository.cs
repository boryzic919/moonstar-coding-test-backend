using BackEnd.Api.Data;
using BackEnd.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEnd.Api.Repository
{
    public class PostRepository : IPostRepository
    {
        private readonly PostDbContext postDbDbContext;

        public PostRepository(PostDbContext _postDbDbContext)
        {
            postDbDbContext = _postDbDbContext;
        }
        /// <summary>
        /// get  post based on id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Post> GetPostAsync(int id)
        {
            var post = await postDbDbContext.Posts.SingleOrDefaultAsync(post => post.Id.Equals(id));

            return await Task.FromResult(post);
        }
        /// <summary>
        /// get all post
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Post>> GetPostsAsync()
        {
            return await postDbDbContext.Posts.ToListAsync();
        }
        /// <summary>
        /// create a post and update db
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task CreatePostAsync(Post item)
        {
            postDbDbContext.Posts.Add(item);
            await Task.CompletedTask;
            await postDbDbContext.SaveChangesAsync();
        }
        /// <summary>
        /// update a post in db
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task UpdatePostAsync(Post item)
        {
            var post = postDbDbContext.Posts.FindAsync(item.Id).Result;
            post.PhotoUrl = item.PhotoUrl;
            post.Content = item.Content;

            await Task.CompletedTask;
            await postDbDbContext.SaveChangesAsync();
        }
        /// <summary>
        /// delete a post from db
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeletePostAsync(int id)
        {
            var post = postDbDbContext.Posts.FindAsync(id).Result;
            postDbDbContext.Posts.Remove(post);
            await Task.CompletedTask;
            await postDbDbContext.SaveChangesAsync();
        }

    }
}
