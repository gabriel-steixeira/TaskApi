using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TaskApi.Data;
using TaskApi.Data.Entities;

namespace TaskApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TaskController : ControllerBase
    {
        private readonly TaskContext _context;

        public TaskController(TaskContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Search Task by ID
        /// </summary>
        /// <param name="id">ID Task</param>
        /// <returns>Task object</returns>
        /// <remarks>
        /// Sample request:
        ///     
        ///     GET /SearchTaskByID
        ///     {
        ///         "id": 1
        ///     }
        /// </remarks>
        /// <response code="200">Return Task</response>
        /// <response code="404">Task Not Found</response>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Search Task by ID")]
        public IActionResult SearchTaskByID(int id)
        {
            TaskClass task = _context.Tasks.Find(id);

            if (task == null)
                return NotFound(
                    new
                    {
                        Message = "Task Not Found. Try againg later...",
                        Error = true
                    });

            return Ok(
                new
                {
                    Message = "The task was found!",
                    Task = task
                });
        }

        /// <summary>
        /// Update Task by ID
        /// </summary>
        /// <param name="id">ID Task</param>
        /// <param name="taskWillUpdate">Task</param>
        /// <returns>Task updated</returns>
        /// <remarks>
        /// Sample request:
        ///     
        ///     PUT /UpdateTask
        ///     {
        ///         "id": int,
        ///         "task": TaskClass
        ///     }
        /// </remarks>
        /// <response code="200">Return TaskUpdated</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Task Not Found</response>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Search Task by ID")]
        public IActionResult UpdateTask(int id, TaskClass taskWillUpdate)
        {
            TaskClass task = _context.Tasks.Find(id);

            if (task == null)
                return NotFound(
                    new
                    {
                        Message = "Task Not Found. Try againg later...",
                        Error = true
                    });
            //else if (taskWillUpdate.STATUS > 1)
            //    return BadRequest(
            //        new
            //        {
            //            Message = "The number of STATUS is incorrect. The number must between 0 and 1",
            //            Error = true,
            //            Status = taskWillUpdate.STATUS
            //        });

            task.TITULO = taskWillUpdate.TITULO;
            task.DESCRICAO = taskWillUpdate.DESCRICAO;
            task.DATA = taskWillUpdate.DATA;
            task.STATUS = taskWillUpdate.STATUS;

            _context.Tasks.Update(task);
            _context.SaveChanges();

            return Ok(
                new
                {
                    Message = "The task was updated!",
                    Task = task
                });
        }

        /// <summary>
        /// Delete Task
        /// </summary>
        /// <param name="id">ID Task</param>
        /// <returns>Success or Error</returns>
        /// <remarks>
        /// Sample request:
        ///     
        ///     DELETE /DeleteTask
        ///     {
        ///         "id": 1
        ///     }
        /// </remarks>
        /// <response code="200">Return Success</response>
        /// <response code="404">Task Not Found</response>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete Task for ID")]
        public IActionResult DeleteTask(int id)
        {
            TaskClass task = _context.Tasks.Find(id);

            if (task == null)
                return NotFound(
                    new
                    {
                        Message = "Task Not Found. Try againg later...",
                        Error = true
                    });

            _context.Tasks.Remove(task);
            _context.SaveChanges();

            return Ok(
                new
                {
                    Success = true,
                    Message = "Task was deleted with success"
                });
        }

        /// <summary>
        /// Search All Tasks
        /// </summary>
        /// <returns>Task list</returns>
        /// <remarks>
        /// Sample request:
        ///     
        ///     GET /SearchAllTasks
        /// </remarks>
        /// <response code="200">Return Task list</response>
        /// <response code="404">No tasks</response>
        [HttpGet]
        [SwaggerOperation(Summary = "Search All Tasks")]
        public IActionResult SearchAllTasks()
        {
            List<TaskClass> tasks = _context.Tasks.ToList();

            if (tasks == null)
                return NotFound(
                    new
                    {
                        Message = "No tasks. Try againg later...",
                        Error = true
                    });

            foreach (var task in tasks)
            {
                if (task.STATUS == 0) {
                    task.STATUS = TaskStatusEnum.Pendente;
                }
                else {
                    task.STATUS = TaskStatusEnum.Finalizado;
                }
            }

            return Ok(
                new
                {
                    Message = "The tasks were found!",
                    ListTasks = tasks
                });
        }

        /// <summary>
        /// Search Tasks by title
        /// </summary>
        /// <param name="title">Title Task</param>
        /// <returns>List Task</returns>
        /// <remarks>
        /// Sample request:
        ///     
        ///     GET /SearchTasksByTitle {
        ///         "title": "title task"
        ///     }
        /// </remarks>
        /// <response code="200">Return List Task</response>
        /// <response code="404">No tasks</response>
        [HttpGet("{title}")]
        [SwaggerOperation(Summary = "Search Tasks by Title")]
        public IActionResult SearchTasksByTitle(string title)
        {
            List<TaskClass> tasks = _context.Tasks.Where(x => x.TITULO.Contains(title)).ToList();

            if (tasks == null)
                return NotFound(
                    new
                    {
                        Message = "Tasks weren't found. Try againg later...",
                        Error = true
                    });

            return Ok(
                new
                {
                    Message = "Some tasks were found!",
                    ListTasks = tasks
                });
        }

        /// <summary>
        /// Search Task by Date
        /// </summary>
        /// <param name="dateTask">Date Task</param>
        /// <returns>List Task</returns>
        /// <remarks>
        /// Sample request:
        ///     
        ///     GET /SearchTasksByDate {
        ///         "dateTask": "2023-11-09"
        ///     }
        /// </remarks>
        /// <response code="200">Return List Task</response>
        /// <response code="404">No tasks</response>
        [HttpGet("{dateTask}")]
        [SwaggerOperation(Summary = "Search Tasks by Date")]
        public IActionResult SearchTasksByDate(string dateTask)
        {
            DateTime date = DateTime.Now;
            DateTime.TryParse(dateTask, out date);
            List<TaskClass> tasks = _context.Tasks.Where(x => x.DATA == date).ToList();

            if (tasks == null)
                return NotFound(
                    new
                    {
                        Message = "Tasks weren't found. Try againg later...",
                        Error = true
                    });

            return Ok(
                new
                {
                    Message = "Some tasks were found!",
                    ListTasks = tasks
                });
        }

        /// <summary>
        /// Search Task by Status
        /// </summary>
        /// <param name="statusTask">Status Task</param>
        /// <returns>List Task</returns>
        /// <remarks>
        /// Sample request:
        ///     
        ///     GET /SearchTasksByStatus{
        ///         "statusTask": 0 or 1
        ///     }
        /// </remarks>
        /// <response code="200">Return List Task</response>
        /// <response code="404">No tasks</response>
        [HttpGet("{statusTask}")]
        [SwaggerOperation(Summary = "Search Tasks by Status")]
        public IActionResult SearchTasksByStatus(TaskStatusEnum statusTask)
        {
            List<TaskClass> tasks = _context.Tasks.Where(x => x.STATUS == statusTask).ToList();

            if (tasks == null)
                return NotFound(
                    new
                    {
                        Message = "Tasks weren't found. Try againg later...",
                        Error = true
                    });

            return Ok(
                new
                {
                    Message = "Some tasks were found!",
                    ListTasks = tasks
                });
        }

        /// <summary>
        /// Register Task
        /// </summary>
        /// <param name="taskRegister">Task</param>
        /// <returns>Task</returns>
        /// <remarks>
        /// Sample request:
        ///     
        ///     POST /RegisterTask{
        ///         "task": TaskClass
        ///     }
        /// </remarks>
        /// <response code="200">Return Task</response>
        /// <response code="400">Task Null</response>
        [HttpPost]
        [SwaggerOperation(Summary = "Register Task")]
        public IActionResult RegisterTask(TaskClass taskRegister)
        {
            if (taskRegister == null)
                return BadRequest(
                    new
                    {
                        Message = "Task is can't null. Try againg later...",
                        Error = true
                    });

            _context.Tasks.Add(taskRegister);
            _context.SaveChanges();

            return Ok(
                new
                {
                    Message = "Task registered with success!",
                    Task = taskRegister
                });
        }
    }
}
