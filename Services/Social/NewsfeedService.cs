using AutoMapper;
using HRM.Data;
using HRM.Models;
using HRM.ViewModels.Social;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.Social
{
    public interface INewsfeedService
    {
        Task<List<PostVM>> GetFeedAsync();
        Task CreatePostAsync(PostVM postVM, int authorId);
        Task AddCommentAsync(int postId, string content, int authorId);
    }

    public class NewsfeedService : INewsfeedService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public NewsfeedService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddCommentAsync(int postId, string content, int authorId)
        {
            var comment = new Comment
            {
                PostId = postId,
                Content = content,
                AuthorId = authorId,
                CreatedAt = DateTime.UtcNow
            };
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }

        public async Task CreatePostAsync(PostVM postVM, int authorId)
        {
            var post = _mapper.Map<Post>(postVM);
            post.AuthorId = authorId;
            post.CreatedAt = DateTime.UtcNow;
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task<List<PostVM>> GetFeedAsync()
        {
            var posts = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.Author)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return _mapper.Map<List<PostVM>>(posts);
        }
    }
}
