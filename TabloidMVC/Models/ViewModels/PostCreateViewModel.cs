using System.Collections.Generic;

namespace TabloidMVC.Models.ViewModels
{
    public class PostCreateViewModel
    {
        public List<Category> CategoryOptions { get; set; }

        public Post Post { get; set; }

    }
}
