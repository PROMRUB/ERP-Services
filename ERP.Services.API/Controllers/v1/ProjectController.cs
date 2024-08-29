using ERP.Services.API.Handlers;
using ERP.Services.API.Interfaces;
using ERP.Services.API.Models.RequestModels.Project;
using ERP.Services.API.Models.ResponseModels.Project;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Services.API.Controllers.v1
{
    [ApiController]
    [Route("v{version:apiVersion}/api/[controller]")]
    [ApiVersion("1")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService projectService;
        public ProjectController(IProjectService projectService) {
            this.projectService = projectService;
        }

        [HttpGet]
        [Route("org/{id}/action/GetProjectList/{businessId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetProjectList(string id, Guid businessId)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = await projectService.GetProjectListByBusiness(id, businessId);
                return Ok(ResponseHandler.Response<List<ProjectResponse>>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        public record ProjectResourceParameter(string Keyword,int Page,int PageSize);
        
        [HttpGet]
        [Route("org/{id}/action/GetProjectListWithPaging/{businessId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetProjectListWithPaging(string id, Guid businessId
        ,[FromQuery] ProjectResourceParameter resourceParameter)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = await projectService.GetProjectListByBusiness(id, businessId,
                    resourceParameter.Keyword,resourceParameter.Page,resourceParameter.PageSize);
                return Ok(ResponseHandler.Response("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpGet]
        [Route("org/{id}/action/GetProjectInformation/{businessId}/{projectId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> GetProjectInformation(string id, Guid businessId, Guid projectId)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                var result = await projectService.GetProjectInformationById(id, businessId, projectId);
                return Ok(ResponseHandler.Response<ProjectResponse>("1000", null, result));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
        [HttpPost]
        [Route("org/{id}/action/CreateProject")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CreateProject(string id, [FromBody] ProjectRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                await projectService.CreateProject(id, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpPost]
        [Route("org/{id}/action/UpdateProject/{businessId}/{projectId}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> UpdateProject(string id, Guid businessId, Guid projectId, [FromBody] ProjectRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                await projectService.UpdateProject(id, businessId, projectId, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }

        [HttpPost]
        [Route("org/{id}/action/DeleteProject/{businessId}/{project}")]
        [MapToApiVersion("1")]
        public async Task<IActionResult> DeleteProject(string id, Guid businessId, Guid projectId, [FromBody] ProjectRequest request)
        {
            try
            {
                if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                    throw new ArgumentException("1101");
                await projectService.DeleteProject(id, businessId, projectId, request);
                return Ok(ResponseHandler.Response("1000", null));
            }
            catch (Exception ex)
            {
                return Ok(ResponseHandler.Response(ex.Message, null));
            }
        }
    }
}
