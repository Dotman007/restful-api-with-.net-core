using BuildingRestFulAPI.Services;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuildingRestFulAPI.Hubs
{
    public class TransactionHub : Hub
    {
        private IHubContext<TransactionHub> _hubContext;
        private IAccountTransaction _transactionService;
        public TransactionHub(IHubContext<TransactionHub> hubContext, IAccountTransaction transactionService)
        {
            _hubContext = hubContext;
            _transactionService = transactionService;
        }
        
        public async Task<int> GetDailyCreditTransaction(Guid customerId)
        {
            var transaction = _transactionService.DailyCreditTransaction(customerId);
            await Clients.All.SendAsync("GetDailyCreditTransaction", transaction);
            return await Task.FromResult<int>(transaction);
        }

       

    }

    
}
