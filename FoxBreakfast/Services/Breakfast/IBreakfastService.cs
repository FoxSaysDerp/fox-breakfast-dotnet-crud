namespace FoxBreakfast.Services.Breakfast;

public interface IBreakfastService
{
   void CreateBreakfast(Models.Breakfast breakfast);
   void DeleteBreakfast(Guid id);
   Models.Breakfast GetBreakfast(Guid id);
   void UpsertBreakfast(Models.Breakfast breakfast);
}