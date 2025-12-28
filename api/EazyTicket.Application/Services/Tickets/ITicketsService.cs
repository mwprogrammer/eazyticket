using EazyTicket.Application.Services.Tickets.Models;
using EazyTicket.Application.Shared.Models;

namespace EazyTicket.Application.Services.Tickets
{
    public interface ITicketsService
    {
        public Task<BaseResponse<TicketDto>> CreateTicketAsync(TicketDto request);
        public Task<BaseResponse<TicketDto>> GetTicketByNumberAsync(string ticketNumber);
    }
}
