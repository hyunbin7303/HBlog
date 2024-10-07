using HBlog.Contract.DTOs;
using HBlog.WebClient.Services;

namespace HBlog.WebClient.States
{
    public class PostDashboardState
    {
        IPostService _postService;
        public PostDashboardState(IPostService postService)
        {
            _postService = postService;
        }
        public bool ShowingConfigureDialog { get; private set; }
        public List<TagDto>? SearchTags { get; private set; } = new();
        public IEnumerable<PostDisplayDto>? Posts;
        public int? selectCategoryId = 0;
        public void ShowConfigureTagSelectDialog()
        {
            ShowingConfigureDialog = true;
        }
        public void CancelConfigureTagDialog()
        {
            SearchTags = null;
            ShowingConfigureDialog = false;
        }

        public async Task ConfirmConfigureTagDialog()
        {
            Posts = await _postService.GetPostDisplayByFilters((int)selectCategoryId, SearchTags);
            ShowingConfigureDialog = false;
        }
    }
}
