using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using storagemanagement3.Models;
using storagemanagement3.Services;
namespace storagemanagement3.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IAsyncDocumentSession _session;

        public ItemsController(RavenDbService ravenDbService)
        {
            _session = ravenDbService.DocumentStore.OpenAsyncSession();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _session.Query<Item>().ToListAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var item = await _session.LoadAsync<Item>(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Item item)
        {
            // Generate a custom ID (e.g., "ITEM-1000")
            var customIdPrefix = "ITEM-";
            var currentMaxId = await GetCurrentMaxId(customIdPrefix);
            item.Id = $"{customIdPrefix}{currentMaxId + 1}";

            await _session.StoreAsync(item);
            await _session.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        private async Task<int> GetCurrentMaxId(string prefix)
        {
            var items = await _session.Query<Item>()
                                      .Where(i => i.Id.StartsWith(prefix))
                                      .ToListAsync();

            if (items.Count == 0)
                return 0;

            return items
                .Select(i => int.TryParse(i.Id.Substring(prefix.Length), out var number) ? number : 0)
                .Max();
        }

        //public async Task<IActionResult> Create(Item item)
        //{
        //    await _session.StoreAsync(item);
        //    await _session.SaveChangesAsync();
        //    return NoContent();
        //    //CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        //}


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Item item)
        {
            var existingItem = await _session.LoadAsync<Item>(id);
            if (existingItem == null)
                return NotFound();

            existingItem.Name = item.Name;
            existingItem.Quantity = item.Quantity;
            existingItem.Location = item.Location; 
            await _session.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var item = await _session.LoadAsync<Item>(id);
            if (item == null)
                return NotFound();

            _session.Delete(item);
            await _session.SaveChangesAsync();
            return NoContent();
        }
    }

}
