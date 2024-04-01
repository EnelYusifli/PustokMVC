using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PustokTemp.DAL;
using PustokTemp.Models;
using PustokTemp.ViewModels;
using System.Diagnostics;

namespace PustokTemp.Controllers;

public class HomeController : Controller
{
    private readonly PustokDbContext _context;

    public HomeController(PustokDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        HomeViewModel homeViewModel = new HomeViewModel()
        {
            Sliders = await _context.Sliders.ToListAsync(),
            Books = await _context.Books.Include(x=>x.Author).Include(x=>x.Genre).Include(x=>x.BookImages).ToListAsync()
        };
        return View(homeViewModel);
    }
    public async Task<IActionResult> Detail(int id)
    {
        Book? book=await _context.Books
            .Include(x=>x.Author)
            .Include(x=>x.Genre)
            .Include(x=>x.BookImages)
            .FirstOrDefaultAsync(x=>x.Id==id);
        if (book is null) throw new Exception();
        return View(book);
    }

    public async Task<IActionResult> AddToBasket(int bookId)
    {
        if (!await _context.Books.AnyAsync(x => x.Id == bookId)) return NotFound(); // 404

        List<BasketItemViewModel> basketItems = new List<BasketItemViewModel>();
        BasketItemViewModel basketItem = null;
        var basketItemsStr = HttpContext.Request.Cookies["BasketItems"];

        if (basketItemsStr is not null)
        {
            basketItems = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketItemsStr);

            basketItem = basketItems.FirstOrDefault(x => x.BookId == bookId);

            if (basketItem is not null)
            {
                basketItem.Count++;
            }
            else
            {
                basketItem = new BasketItemViewModel()
                {
                    BookId = bookId,
                    Count = 1
                };

                basketItems.Add(basketItem);
            }
        }
        else
        {
            basketItem = new BasketItemViewModel()
            {
                BookId = bookId,
                Count = 1
            };

            basketItems.Add(basketItem);
        }

        basketItemsStr = JsonConvert.SerializeObject(basketItems);

        HttpContext.Response.Cookies.Append("BasketItems", basketItemsStr);

        return Ok();
    }

    public async Task<IActionResult> Basket()
    {
        List<BasketItemViewModel> basketItems = new();

        var basketItemsStr = HttpContext.Request.Cookies["BasketItems"];

        if (basketItemsStr is not null)
        {
            basketItems = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketItemsStr);
        }

		ViewBag.Books = _context.Books.Include(x => x.BookImages).ToList();
		
        return View(basketItems);
    }
}
