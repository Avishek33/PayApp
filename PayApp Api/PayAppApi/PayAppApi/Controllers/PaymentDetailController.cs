using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayAppApi.Data;
using PayAppApi.ViewModels;
using PayAppApi.ViewModels.PaymentDetail;

namespace PayAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentDetailController : ControllerBase
    {

        private readonly PayAppDbContext _context;
        public PaymentDetailController(PayAppDbContext payApp)
        {
            _context = payApp;
        }

        [HttpGet("get-payment-details-by/{matchId}")]
        public async Task<ActionResult<List<PaymentDetailVm>>> GetPaymentDetailsByMatchId([FromRoute] Guid matchId)
        {
            var paymentDetail = await _context.PaymentDetails.Where(x => x.MatchId == matchId).Select(x => new PaymentDetailVm
            {
                Id = x.Id,
                MatchId = x.MatchId,
                Amount = x.Amount,
                PaymentStatus = x.PaymentStatus,
                PaymentStatusInString = x.PaymentStatus.ToString(),
                UserId = x.UserId,
                Username = x.User.Name
            }).ToListAsync();
            return Ok(paymentDetail);
        }
        [HttpPut("set-as-Paid/{userId}/{matchId}")]

        public async Task<ActionResult<ApiResponse>> SetasPaid([FromRoute] Guid userId,Guid matchId)
        {
            var matchDetail = await _context.PaymentDetails.Where(x => x.MatchId == matchId && x.UserId == userId).FirstOrDefaultAsync();
            if (matchDetail is null)
                return BadRequest(new ApiResponse
                {
                    StatusCode = 1001,
                    Message = "Could not find any details."
                });

            if (matchDetail.PaymentStatus == Common.PaymentStatus.Paid)
                return Ok(new ApiResponse
                {
                    StatusCode = 1001,
                    Message = "Already Paid."
                });

            matchDetail.PaymentStatus = Common.PaymentStatus.Paid;

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return Ok(new ApiResponse
                {
                    StatusCode = 0,
                    Message = "Payment done successfully."
                });

            return BadRequest(new ApiResponse
            {
                StatusCode = 1001,
                Message = "Payment done failed."
            });

        }
    }
}
