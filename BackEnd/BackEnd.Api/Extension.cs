using BackEnd.Api.Dtos;
using BackEnd.Api.Models;

namespace BackEnd.Api
{
    public static class Extensions
    {
        public static CreatePostDto AsDto(this Post item)
        {
            return new CreatePostDto() { PhotoUrl = item.PhotoUrl, Content = item.Content };
        }
    }
}
