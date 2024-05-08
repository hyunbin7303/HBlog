using HBlog.Contract.DTOs;

namespace HBlog.WebClient.States
{
    public class PostDashboardState
    {
        public bool ShowingConfigureDialog { get; private set; }
        public List<TagDto>? SearchTags { get; private set; } = new();
        public void ShowConfigureTagSelectDialog()
        {
            ShowingConfigureDialog = true;
        }
        public void CancelConfigureTagDialog()
        {
            SearchTags = null;
            ShowingConfigureDialog = false;
        }

        public void ConfirmConfigureTagDialog()
        {
            ShowingConfigureDialog = false;
        }
    }
}
