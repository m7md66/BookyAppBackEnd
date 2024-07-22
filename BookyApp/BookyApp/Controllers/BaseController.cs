using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookyApp.Controllers
{



    [Route("api/[controller]")]
    [ApiController]



    public class BaseController : ControllerBase
    {
        
        public BaseController() {
            
        }

        public string currentUserId;

        public string currentUserName
        { 
            get => currentUserId;
          set { currentUserId = User.Claims.FirstOrDefault(a => a.Type.Contains("nameidentifier")).Value; }
        }
    }



   
}
