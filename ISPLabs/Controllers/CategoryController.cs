using Microsoft.AspNetCore.Mvc;
using ISPLabs.Services;
using Oracle.ManagedDataAccess.Client;

namespace ISPLabs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private OracleConnection _conn;

        public CategoryController()
        {
            _conn = OracleHelper.GetDBConnection();
            _conn.Open();
        }

        //[HttpGet]
        //public ActionResult<ISet<CategoryAPIModel>> GetAll() {
            
        //}

        //[HttpGet("{id}", Name = "GetCategory")]
        //public ActionResult<CategoryAPIModel> GetById(int id) => categories.GetById(id);

        //[Authorize(Roles = "admin")]
        //[HttpPost]
        //public ActionResult Create(CategoryAPIModel cat)
        //{
        //    var newCategory = categories.Append(cat);
        //    if (newCategory == null)
        //        return StatusCode(403);
        //    return CreatedAtRoute("GetCategory", new { id = newCategory.Id }, newCategory);

        //}
        //[Authorize(Roles = "admin")]
        //[HttpPut("{id}")]
        //public IActionResult Update(int id, CategoryAPIModel cat)
        //{
        //    cat.Id = id;
        //    if(categories.Update(cat))
        //        return NoContent();
        //    return BadRequest();
        //}
        //[Authorize(Roles = "admin")]
        //[HttpDelete("{id}")]
        //public IActionResult Delete(int id)
        //{
        //    if (categories.Remove(id))
        //        return NoContent();
        //    return BadRequest();
        //}
        ~CategoryController()
        {
            _conn.Close();
            _conn.Dispose();
        }
    }
}