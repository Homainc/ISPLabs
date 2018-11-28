using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISPLabs.Models;
using ISPLabs.Services;
using NHibernate;
using ISPLabs.ViewModels;

namespace ISPLabs.Components
{
    public class SendMessageForm : ViewComponent
    {
        private NHibernateHelper nHibernateHelper;
        public SendMessageForm(NHibernateHelper nHibernateHelper)
        {
            this.nHibernateHelper = nHibernateHelper;
        }
        public IViewComponentResult Invoke(string email)
        {
            using(ISession session = nHibernateHelper.OpenSession())
            {
                var user = session.Query<User>().Single(x => x.Email == email);
                ViewBag.CurrentLogin = user.Login;
                return View();
            }
        }
    }
}
