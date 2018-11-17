using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ISPLabs.Services;
using ISPLabs.Models;
using ISPLabs.Models.API;
using NHibernate;
using Microsoft.AspNetCore.Authorization;

namespace ISPLabs.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private NHibernateHelper nHibernateHelper;
        public CategoryController(NHibernateHelper nHibernateHelper)
        {
            this.nHibernateHelper = nHibernateHelper;
        }
        [HttpGet]
        public ActionResult<ISet<CategoryAPIModel>> GetAll()
        {
            using (NHibernate.ISession session = nHibernateHelper.OpenSession())
            {
                return session.Query<Category>().Select(x => new CategoryAPIModel(x)).ToHashSet();
            }
        }
        [HttpGet("{id}", Name = "GetCategory")]
        public ActionResult<CategoryAPIModel> GetById(int id)
        {
            using (NHibernate.ISession session = nHibernateHelper.OpenSession())
            {
                var cat = session.Query<Category>().Single(x => x.Id == id);
                return new CategoryAPIModel(cat);
            }
        }
        [HttpPost]
        public ActionResult Create(CategoryAPIModel cat)
        {
            using (NHibernate.ISession session = nHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var addedCat = new Category
                    {
                        Partition = session.Query<Partition>().Single(x => x.Id == cat.PartitionId),
                        Name = cat.Name
                    };
                    session.SaveOrUpdate(addedCat);
                    transaction.Commit();
                    var dbCat = session.Query<Category>().FirstOrDefault(x => x.Name == addedCat.Name);
                    return CreatedAtRoute("GetCategory", new { id = dbCat.Id }, new CategoryAPIModel(dbCat));
                }
            }
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, CategoryAPIModel cat)
        {
            using (NHibernate.ISession session = nHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var addedCat = session.Query<Category>().Single(x => x.Id == id);
                    addedCat.Name = cat.Name;
                    session.Update(addedCat);
                    transaction.Commit();
                    return NoContent();
                }
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using (NHibernate.ISession session = nHibernateHelper.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    var cat = session.Query<Category>().FirstOrDefault(x => x.Id == id);
                    if (cat == null)
                        return NotFound();
                    session.Delete(cat);
                    transaction.Commit();
                    return NoContent();
                }
            }
        }
    }
}