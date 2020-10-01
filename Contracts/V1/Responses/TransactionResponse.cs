using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EscrowAPI.Contracts.V1.Responses
{
    public class TransactionResponse
    {
        public Guid Id { get; set; }
        public long Price { get; set; }
        public string Name { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
        public DateTime Date { get; set; }
    }
}
