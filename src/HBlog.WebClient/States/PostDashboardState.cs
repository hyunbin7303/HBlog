using HBlog.Contract.DTOs;

namespace HBlog.WebClient.States
{
    public class PostDashboardState
    {
        public bool ShowingConfigureDialog { get; set; }    
        public List<TagDto>? SearchTags { get; set; }
        public List<TagDto> Tags { get; set; }
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
            if (SearchTags is not null)
            {
                //SearchTags.Add(ConfiguringPizza);
                //ConfiguringPizza = null;
            }

            ShowingConfigureDialog = false;
        }
    }
}
