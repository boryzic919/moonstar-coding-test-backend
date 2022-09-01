using BackEnd.Api.Models;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackEnd.Api.Repository
{
    public interface IPostRepository
    {
        Task<Post> GetPostAsync(int id);
        Task<IEnumerable<Post>> GetPostsAsync();
        Task CreatePostAsync(Post item);
        Task UpdatePostAsync(Post item);
        Task DeletePostAsync(int id);

    }
}
