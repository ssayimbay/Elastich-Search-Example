using ExampleElastichSearch.API.Elasticsearch.Model;
using ExampleElastichSearch.API.Elasticsearch.Repository.Elasticsearch;
using ExampleElastichSearch.API.Entity;
using ExampleElastichSearch.API.Entity.Employee;
using ExampleElastichSearch.API.Entity.Task;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System.Threading.Tasks;

namespace ExampleElastichSearch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EsController : ControllerBase
    {
        //private readonly IRepository<EmployeeListModel, string> _Employeerepository;
        private readonly IRepository<TaskIndexModel, string> _taskRepository;

        public EsController(IRepository<TaskIndexModel, string> taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpGet, Route("Create")]
        public async Task<IActionResult> Create()
        {
            await _taskRepository.CreateIndexAsync();
            return Ok();
        }

        [HttpPost, Route("AddOrUpdate")]
        public async Task<IActionResult> AddOrUpdate(TaskIndexModel task)
        {
            await _taskRepository.AddOrUpdate(task);
            return Ok();
        }

        [HttpPost, Route("Get")]
        public async Task<IActionResult> Get(SearchModel searchModel)
        {
            var result = await _taskRepository.SearchAsync( searchModel);
            return Ok(result);
        }

        [HttpDelete, Route("deleteIndex")]
        public async Task<IActionResult> DeleteIndex()
        {
            await _taskRepository.DeleteIndexAsync();
            return Ok();
        }
    }
}
