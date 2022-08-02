using Microsoft.AspNetCore.Mvc;
using Cards.API.Data;
using Microsoft.EntityFrameworkCore;
using Cards.API.Models;

namespace Cards.API.Controllers
{
    [ApiController]
    [Route("api/cards")]
    public class CardsController : Controller
    {
        private readonly CardsDbContext cardsDbContext;

       

        public CardsController(CardsDbContext cardsDbContext)
        {
            this.cardsDbContext = cardsDbContext;
        }

        //Get all cards
        [HttpGet]
        [Route("get-all")]
        public async Task<IActionResult> GetAllCards()
        {
            var cards = await cardsDbContext.Cards.ToListAsync();
            return Ok(cards);
        }

        //Get Single Card
        [HttpGet]
        [Route("{id:int}")]
        [ActionName("GetSingleCard")]
        public async Task<IActionResult> GetSingleCard([FromRoute] int id)
        {
            var card = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (card != null)
            {
                return Ok(card);
            }
            return NotFound("Card Not Found");
        }

        //Add card
        
        [HttpPost]        
        public async Task<IActionResult> AddCard([FromBody] Card card)
        {
            card.Id = new int();

            await cardsDbContext.Cards.AddAsync(card);
            await cardsDbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSingleCard), new { Id = card.Id }, card);
        }

        //update card
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateCard([FromRoute] int id, [FromBody] Card card)
        {
            var exisitngcard = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (exisitngcard != null)
            {
                exisitngcard.CardHolderName = card.CardHolderName;
                exisitngcard.CardNumber = card.CardNumber;
                exisitngcard.ExpiryMonth = card.ExpiryMonth;
                exisitngcard.ExpiryYear = card.ExpiryYear; 
                exisitngcard.CVC = card.CVC;
                await cardsDbContext.SaveChangesAsync();
                return Ok(exisitngcard);
            }
            return NotFound("Card Not Found");
        }

        //Delete card
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteCard([FromRoute] int id)
        {
            var exisitngcard = await cardsDbContext.Cards.FirstOrDefaultAsync(x => x.Id == id);
            if (exisitngcard != null)
            {
                cardsDbContext.Remove(exisitngcard);                
                await cardsDbContext.SaveChangesAsync();
                return Ok(exisitngcard);
            }
            return NotFound("Card Not Found");
        }
    }
}
