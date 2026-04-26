using HotelReservation.Business;
using Microsoft.AspNetCore.Components.Forms;

namespace HotelReservation.Web;

internal sealed class BusinessExceptionPopulator
{
    private readonly EditContext _context;
    private readonly ValidationMessageStore _store;

    public BusinessExceptionPopulator(EditContext context)
    {
        _context = context;
        _store = new ValidationMessageStore(context);
    }

    public void PopulateExceptionMessage(BusinessException exception)
    {
        ArgumentNullException.ThrowIfNull(exception);
        _store.Clear();
        
        FieldIdentifier identifier = _context.Field(exception.ErrorProperty);
        _store.Add(identifier, exception.Message);
        _context.NotifyValidationStateChanged();
    }

    public void Clear()
    {
        _context.NotifyValidationStateChanged();
    }
}
