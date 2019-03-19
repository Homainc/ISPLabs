using Microsoft.AspNetCore.Mvc;

namespace ISPLabs.Components
{
    public class SendMessageForm : ViewComponent
    {
        public IViewComponentResult Invoke(string email) => View();
    }
}
