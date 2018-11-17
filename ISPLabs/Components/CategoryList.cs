using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ISPLabs.Models;
using ISPLabs.Services;
using NHibernate;
using System.Linq;

namespace ISPLabs.Components
{
    public class CategoryList : ViewComponent
    {
        private NHibernateHelper nHibernateHelper;
        public CategoryList(NHibernateHelper nHibernateHelper)
        {
            this.nHibernateHelper = nHibernateHelper;
        }
        public IViewComponentResult Invoke(int partitionId)
        {
            using(ISession session = nHibernateHelper.OpenSession())
            {
                var cats = session.Query<Category>().Where(x => x.Partition.Id == partitionId).ToList();
                return View(cats);
            }
        }
    }
}
