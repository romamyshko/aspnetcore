using Microsoft.AspNetCore.Mvc;

namespace BankSolution.Controllers
{
    public class BankController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return Content("Welcome to the Best Bank");
        }

        [Route("/account-details")]
        public IActionResult AccountDetails()
        {
            var bankAccount = new { accountNumber = 1001, accountHolderName = "Example Name", currentBalance = 5000 };

            return Json(bankAccount);
        }

        [Route("/account-statement")]
        public IActionResult AccountStatement()
        {
            return File("~/statement.pdf", "application/pdf");
        }

        [Route("/get-current-balance/{accountNumber:int?}")]
        public IActionResult GetCurrentBalance()
        {
            object accountNumberObj;
            if (HttpContext.Request.RouteValues.TryGetValue("accountNumber", out accountNumberObj) && accountNumberObj is string accountNumber)
            {
                if (string.IsNullOrEmpty(accountNumber))
                {
                    return NotFound("Account Number should be supplied");
                }

                if (int.TryParse(accountNumber, out int accountNumberInt))
                {
                    var bankAccount = new { accountNumber = 1001, accountHolderName = "Example Name", currentBalance = 5000 };

                    if (accountNumberInt == 1001)
                    {
                        return Content(bankAccount.currentBalance.ToString());
                    }
                    else
                    {
                        return BadRequest("Account Number should be 1001");
                    }
                }
                else
                {
                    return BadRequest("Invalid Account Number format");
                }
            }
            else
            {
                return NotFound("Account Number should be supplied");
            }
        }
    }
}
