using EscrowAPI.Contracts.V1;
using EscrowAPI.Contracts.V1.Requests;
using EscrowAPI.Contracts.V1.Responses;
using EscrowAPI.Domain;
using EscrowAPI.Extensions;
using EscrowAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EscrowAPI.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TransactionController : Controller
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet(ApiRoutes.Transaction.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _transactionService.GetTransactionsAsync());
        }

        [HttpGet(ApiRoutes.Transaction.Get)]
        public async Task<IActionResult> Get([FromRoute]Guid transactionId)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(transactionId);
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }

        [HttpPut(ApiRoutes.Transaction.Update)]
        public async Task<IActionResult> Update([FromRoute]Guid transactionId, [FromBody] UpdateTransactionRequest request)
        {
            
            var userOwnsTransaction = await _transactionService.UserOwnsTransactionAsync(transactionId, HttpContext.GetUserId());

            var buyerInTransaction = await _transactionService.UserBuyingTransactionAsync(transactionId, HttpContext.GetUserEmail());

            if (userOwnsTransaction || buyerInTransaction)
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(transactionId);
                transaction.Date = request.Date;
                transaction.Email = request.Email;
                transaction.MobilePhone = request.MobilePhone;
                transaction.Name = request.Name;
                transaction.Price = request.Price;

                var updated = await _transactionService.UpdateTransactionAsync(transaction);
                if (updated)
                    return Ok(transaction);
                return NotFound();
                
            }

            return BadRequest(error: new { error = "You cannot access this transaction" });

        }

        [HttpPost(ApiRoutes.Transaction.Create)]
        public async Task<IActionResult> Create([FromBody] CreateTransactionRequest transactionRequest)
        {
            var transaction = new Transaction
            {
                UserId = HttpContext.GetUserId(),
                Date = transactionRequest.Date,
                Email = transactionRequest.Email,
                MobilePhone = transactionRequest.MobilePhone,
                Name = transactionRequest.Name,
                Price = transactionRequest.Price
            };

            await _transactionService.CreateTransactionAsync(transaction);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.Transaction.Get.Replace("{transactionId}", transaction.Id.ToString());

            var response = new TransactionResponse
            {
                Id = transaction.Id,
                Price = transaction.Price,
                Name = transaction.Name,
                MobilePhone = transaction.MobilePhone,
                Email = transaction.Email,
                Date = transaction.Date
            };
            return Created(locationUrl, response);
        }

        [HttpDelete(ApiRoutes.Transaction.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid transactionId)
        {
            var userOwnsTransaction = await _transactionService.UserOwnsTransactionAsync(transactionId, HttpContext.GetUserId());
            if (!userOwnsTransaction)
            {
                return BadRequest(error: new { error = "You do not own this transaction" });
            }

            var deleted = await _transactionService.DeleteTransactionAsync(transactionId);

            if (deleted)
                return NoContent();
            return NotFound();
        }

    }
}
