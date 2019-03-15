using Microsoft.AspNetCore.Mvc;

namespace ISPLabs.Components
{
    public class SendMessageForm : ViewComponent
    {
        //private NHibernateHelper nHibernateHelper;
        //public SendMessageForm(NHibernateHelper nHibernateHelper)
        //{
        //    this.nHibernateHelper = nHibernateHelper;
        //}
        public IViewComponentResult Invoke(string email)
        {
            //using(ISession session = nHibernateHelper.OpenSession())
            //{
                //var user = session.Query<User>().Single(x => x.Email == email);
                //ViewBag.CurrentLogin = user.Login;
                return View();
            //}
        }
    }
}
