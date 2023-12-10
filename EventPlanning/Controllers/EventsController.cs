using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EventPlanning.Interfaces;
using EventPlanning.Models;
using EventPlanning.Models.DTO;
using EventPlanning.Models.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace EventPlanning.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class EventsController : BaseController
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateEvent([FromBody] EventCreateDto eventCreateDto)
        {
            var createdEvent = await _eventService.CreateEventAsync(eventCreateDto);
            if (createdEvent == null)
                return BadRequest("Не удалось создать событие");

            return Ok(createdEvent.ToDto());
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _eventService.GetAllEventsAsync();
            var eventsList = events.Select(x => x.ToDto());
            return Ok(eventsList);
        }

        [HttpGet("{eventId}")]
        public async Task<IActionResult> GetEventById(string eventId)
        {
            var eventItem = await _eventService.GetByIdAsync(new ObjectId(eventId));
            if (eventItem == null)
                return NotFound();
            return Ok(eventItem.ToDto());
        }


        //    //[HttpPost("{eventId}/register")]
        //    //public async Task<IActionResult> RegisterForEvent(ObjectId eventId)
        //    //{
        //    //    var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    //    ObjectId userId;

        //    //    if (!ObjectId.TryParse(userIdString, out userId))
        //    //    {
        //    //        return Unauthorized();
        //    //    }

        //    //    var (success, message) = await _eventService.RegisterForEventAsync(eventId, userId);

        //    //    if (!success)
        //    //    {
        //    //        return BadRequest(message);
        //    //    }

        //    //    return Ok(message);
        //    //}

        [HttpPost("{eventId}/requestParticipation")]
        public async Task<IActionResult> RequestParticipation(string eventId)
        {
            var userId = CurrentUserId;
            if (!userId.HasValue)
            {
                return Unauthorized();
            }

            var (result, message) = await _eventService.RequestParticipationAsync(new ObjectId(eventId), userId.Value);

            if (!result)
            {
                return BadRequest(message);
            }

            return Ok(message);
        }

        [HttpPost("{eventId}/confirmParticipation/{confirmationCode}")]
        public async Task<IActionResult> ConfirmParticipation(string eventId, string confirmationCode)
        {
            var userId = CurrentUserId;
            if (!userId.HasValue)
            {
                return Unauthorized();
            }

            var result = await _eventService.ConfirmParticipationAsync(new ObjectId(eventId), userId.Value, confirmationCode);

            if (!result)
            {
                return BadRequest("Ошибка при подтверждении участия.");
            }

            return Ok("Участие успешно подтверждено.");
        }

        [HttpGet("{eventId}/isUserParticipating")]
        public async Task<IActionResult> IsUserParticipating(string eventId)
        {
            var userId = CurrentUserId;
            if (!userId.HasValue)
            {
                return Unauthorized();
            }

            var isParticipating = await _eventService.IsUserParticipatingAsync(new ObjectId(eventId), userId.Value);

            return Ok(isParticipating);
        }

        [HttpPut("{eventId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEvent(string eventId, [FromBody] EventUpdateDto eventUpdateDto)
        {
            var success = await _eventService.UpdateEventAsync(new ObjectId(eventId), eventUpdateDto);
            if (!success)
            {
                return NotFound("Событие не найдено.");
            }

            return Ok("Событие успешно обновлено.");
        }

        [HttpDelete("{eventId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEvent(string eventId)
        {
            var success = await _eventService.DeleteEventAsync(new ObjectId(eventId));
            if (!success)
            {
                return NotFound("Событие не найдено.");
            }

            return Ok("Событие успешно удалено.");
        }

        [HttpGet("{eventId}/participants")]
        public async Task<IActionResult> GetEventParticipants(string eventId)
        {
            var participants = await _eventService.GetEventParticipantsAsync(new ObjectId(eventId));
            if (participants == null)
            {
                return NotFound("Событие не найдено.");
            }

            return Ok(participants);
        }

        [HttpPost("{eventId}/cancelParticipation")]
        public async Task<IActionResult> CancelParticipation(string eventId)
        {
            var userId = CurrentUserId;
            if (!userId.HasValue)
            {
                return Unauthorized();
            }

            var success = await _eventService.CancelParticipationAsync(new ObjectId(eventId), userId.Value);
            if (!success)
            {
                return BadRequest("Не удалось отменить участие.");
            }

            return Ok("Участие в событии отменено.");
        }


    }
}

