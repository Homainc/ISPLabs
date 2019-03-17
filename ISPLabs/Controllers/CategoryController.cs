using Microsoft.AspNetCore.Mvc;
using ISPLabs.Services;
using Oracle.ManagedDataAccess.Client;
using System.Threading.Tasks;
using ISPLabs.Models;
using ISPLabs.Manager;
using Microsoft.AspNetCore.Authorization;

namespace ISPLabs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private OracleConnection _conn;
        private CategoryManager _categories;

        public CategoryController()
        {
            _conn = OracleHelper.GetDBConnection();
            _conn.Open();
            _categories = new CategoryManager(_conn);
        }

        [HttpGet("{id}", Name = "GetCategory")]
        public async Task<Category> GetById(int id) => await _categories.GetByIdAsync(id);

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> Create(Category category)
        {
            if (await _categories.CreateAsync(category))
                return Ok(category);
            return BadRequest(_categories.LastError);
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> Update(Category category)
        {
            if(await _categories.UpdateAsync(category))
                return Ok(category);
            return BadRequest(_categories.LastError);
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _categories.DeleteAsync(id))
                return Ok(id);
            return BadRequest(_categories.LastError);
        }

        ~CategoryController()
        {
            _conn.Close();
            _conn.Dispose();
        }
    }
}