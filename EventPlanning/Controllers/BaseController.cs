using System;
using EventPlanning.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Security.Claims;

namespace EventPlanning.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected ObjectId? CurrentUserId
        {
            get
            {
                var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (ObjectId.TryParse(userIdString, out ObjectId userId))
                {
                    return userId;
                }
                return null;
            }
        }
    }

}

