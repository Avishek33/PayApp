using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayAppApi.Data;
using PayAppApi.Models;
using PayAppApi.ViewModels;
using PayAppApi.ViewModels.MatchDetail;
using PayAppApi.ViewModels.PaymentDetail;

namespace PayAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchDetailController : ControllerBase
    {
        private readonly PayAppDbContext _context;
        public MatchDetailController(PayAppDbContext payApp)
        {
            _context = payApp;
        }

        [HttpGet("match-detail")]
        public async Task<ActionResult<List<MatchDetailVm>>> GetMatchDetail()
        {
            var matchDetails = await _context.MatchDetails.Select(x => new MatchDetailVm
            {
                Id = x.Id,
                Amount = x.Amount,
                MatchDateInString = x.MatchDate.ToString("yyyy-MM-dd"),
                PayerId = x.PayerId,
                PayerName = x.User.Name,
                MatchDate = x.MatchDate
            }).ToListAsync();

            return Ok(matchDetails);
        }

        [HttpPost("create-match-detail")]
        public async Task<ActionResult<ApiResponse>> CreateMatchDetail([FromBody] CreateMatchDetail matchDetailObj)
        {

            if (matchDetailObj is null)
                return BadRequest(new ApiResponse
            {
                StatusCode = 1001,
                Message = "Match details is empty."
            });

            var matchDetail = new MatchDetail
            {
                Id = Guid.NewGuid(),
                MatchDate = matchDetailObj.MatchDate,
                Amount = matchDetailObj.Amount,
                PayerId = matchDetailObj.PayerId,
            };

            _context.MatchDetails.Add(matchDetail);

            var amountForEachPlayer = matchDetailObj.Amount / matchDetailObj.UserId.Count();

            foreach(var userId in matchDetailObj.UserId)
            {
                var paymentDetails = new PaymentDetail
                {
                    MatchId = matchDetail.Id,
                    UserId = userId,
                    PaymentStatus = Common.PaymentStatus.UnPaid,
                    Amount = amountForEachPlayer
                };
                _context.PaymentDetails.Add(paymentDetails);
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
                return Ok(new ApiResponse
                {
                    StatusCode =0,
                     Message = "Match details added successfully."
                });

            return BadRequest(new ApiResponse
            {
                StatusCode = 1001,
                Message = "Match details added failed."
            });
        }


    }
}
