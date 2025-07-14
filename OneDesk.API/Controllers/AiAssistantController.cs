using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;

namespace YourAppNamespace.Controllers
{
    [ApiController]
    [Route("api/ai")]
    public class AiAssistantController : ControllerBase
    {
        public class AiRequest
        {
            public string Prompt { get; set; }
        }

        [HttpPost("parse")]
        public IActionResult ParsePrompt([FromBody] AiRequest request)
        {
            if (request?.Prompt == null)
                return BadRequest(new { error = "Prompt is required." });

            string prompt = request.Prompt.Trim().ToLowerInvariant();

            // 1) ADD: "add 5 phones at 700 each, sell at 1000"
            if (prompt.StartsWith("add"))
            {
                // match quantity, name, cost, selling price
                var m = Regex.Match(prompt, @"add\s+(\d+)\s+(\w+)\s+.*?(\d+)\s+each.*?(\d+)", RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    int qty = int.Parse(m.Groups[1].Value);
                    string name = CultureName(m.Groups[2].Value);
                    int cost = int.Parse(m.Groups[3].Value);
                    int sell = int.Parse(m.Groups[4].Value);

                    return Ok(new
                    {
                        action = "add",
                        data = new
                        {
                            name = name,
                            sku = name.Substring(0, Math.Min(3, name.Length)).ToUpper() + "-" + DateTime.UtcNow.Ticks % 10000,
                            description = $"Auto-added {name}",
                            category = "General",
                            supplier = "Default Supplier",
                            quantityInStock = qty,
                            costPrice = cost,
                            sellingPrice = sell,
                            createdAt = DateTime.UtcNow
                        }
                    });
                }
            }

            // 2) SALE: "sell 3 of item id 7 at 100"
            if (prompt.StartsWith("sell") || prompt.Contains("sale"))
            {
                var m = Regex.Match(prompt, @"(?:sell|sale).+?(\d+).+?id\s+(\d+)(?:.*?(\d+))?", RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    int qty = int.Parse(m.Groups[1].Value);
                    int id  = int.Parse(m.Groups[2].Value);
                    decimal price = m.Groups[3].Success ? decimal.Parse(m.Groups[3].Value) : 0m;

                    return Ok(new
                    {
                        action = "sale",
                        data = new
                        {
                            inventoryItemId = id,
                            quantitySold = qty,
                            sellingPriceAtTime = price
                        }
                    });
                }
            }

            // 3) DELETE: "delete item with id 11"
            if (prompt.StartsWith("delete"))
            {
                var m = Regex.Match(prompt, @"id\s+(\d+)", RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    int id = int.Parse(m.Groups[1].Value);
                    return Ok(new
                    {
                        action = "delete",
                        data = new { id }
                    });
                }
            }

            // 4) GET ALL: "get all" or "show all items"
            if (prompt.Contains("get all") || prompt.Contains("show all"))
            {
                return Ok(new
                {
                    action = "get",
                    data = new { type = "all" }
                });
            }

            // 5) GET BY ID: "get item 5" or "get id 5"
            var idMatch = Regex.Match(prompt, @"get.*?(\d+)");
            if (idMatch.Success)
            {
                int id = int.Parse(idMatch.Groups[1].Value);
                return Ok(new
                {
                    action = "get",
                    data = new { type = "id", id }
                });
            }

            // 6) GET BY NAME: "get phones" (fallback)
            var wordMatch = Regex.Match(prompt, @"get\s+(\w+)");
            if (wordMatch.Success)
            {
                string name = CultureName(wordMatch.Groups[1].Value);
                return Ok(new
                {
                    action = "get",
                    data = new { type = "name", name }
                });
            }

            // Unknown command
            return Ok(new
            {
                action = "unknown",
                message = "Sorry, I didn't understand that. Please specify add/get/sale/delete."
            });
        }

        private static string CultureName(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }
    }
}
