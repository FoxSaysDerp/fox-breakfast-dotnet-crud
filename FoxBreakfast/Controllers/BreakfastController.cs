using ErrorOr;
using FoxBreakfast.Contracts.Breakfast;
using FoxBreakfast.Models;
using FoxBreakfast.Services.Breakfast;
using Microsoft.AspNetCore.Mvc;

namespace FoxBreakfast.Controllers;

[ApiController]
[Route("[controller]")]
public class BreakfastController : ApiController
{
   private readonly IBreakfastService _breakfastService;
   public BreakfastController(IBreakfastService breakfastService)
   {
      _breakfastService = breakfastService;
   }

   [HttpGet("{id:guid}")]
   public IActionResult GetBreakfast(Guid id)
   {
      ErrorOr<Breakfast> getBreakfastResult = _breakfastService.GetBreakfast(id);
      
      return getBreakfastResult.Match(
         breakfast => Ok(MapBreakfastResponse(breakfast)),
         errors => Problem(errors)
      );
   }

   private static BreakfastResponse MapBreakfastResponse(Breakfast breakfast)
   {
      var response = new BreakfastResponse(
         breakfast.Id,
         breakfast.Name,
         breakfast.Description,
         breakfast.StartDateTime,
         breakfast.EndDateTime,
         breakfast.LastModifiedDateTime,
         breakfast.Savory,
         breakfast.Sweet
      );
      return response;
   }

   [HttpPost]
   public IActionResult CreateBreakfast(CreateBreakfastRequest request)
   {
      var breakfast = new Breakfast(
         Guid.NewGuid(),
         request.Name,
         request.Description,
         request.StartDateTime,
         request.EndDateTime,
         DateTime.Now,
         request.Savory,
         request.Sweet
      );

      ErrorOr<Created> createBreakfastResult = _breakfastService.CreateBreakfast(breakfast);


      if (createBreakfastResult.IsError)
      {
         return Problem(createBreakfastResult.Errors);
      }
      
      return CreatedAtAction(
         nameof(GetBreakfast),
         new { id = breakfast.Id},
         MapBreakfastResponse(breakfast));
   }

   [HttpPut("{id:guid}")]
   public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
   {
      var breakfast = new Breakfast(
         id,
         request.Name,
         request.Description,
         request.StartDateTime,
         request.EndDateTime,
         DateTime.UtcNow,
         request.Savory,
         request.Sweet
      );

      _breakfastService.UpsertBreakfast(breakfast);

      // TODO: return 201 if a new breakfast was created
      return NoContent();
   }

   [HttpDelete("{id:guid}")]
   public IActionResult DeleteBreakfast(Guid id)
   {
      ErrorOr<Deleted> deletedResult = _breakfastService.DeleteBreakfast(id);

      return deletedResult.Match(
         deleted => NoContent(),
         errors => Problem(errors));
   }
}