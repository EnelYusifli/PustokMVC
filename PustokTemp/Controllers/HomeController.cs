using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PustokTemp.DAL;
using PustokTemp.Models;
using PustokTemp.ViewModels;

namespace PustokTemp.Controllers;

public class HomeController : Controller
{
    private readonly PustokDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public HomeController(PustokDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    public async Task<IActionResult> Index()
    {
        HomeViewModel homeViewModel = new HomeViewModel()
        {
            Sliders = await _context.Sliders.ToListAsync(),
            Books = await _context.Books.Include(x => x.Author).Include(x => x.Genre).Include(x => x.BookImages).ToListAsync()
        };
        return View(homeViewModel);
    }
    public async Task<IActionResult> Detail(int id)
    {
        Book? book = await _context.Books
            .Include(x => x.Author)
            .Include(x => x.Genre)
            .Include(x => x.BookImages)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (book is null) throw new Exception();
        return View(book);
    }

    public async Task<IActionResult> AddToBasket(int bookId)
    {
        if (!await _context.Books.AnyAsync(x => x.Id == bookId)) return NotFound();

        List<BasketItemViewModel> basketItems = new List<BasketItemViewModel>();
        BasketItemViewModel basketItem = null;
        BasketItem userBasketItem = null;
        var basketItemsStr = HttpContext.Request.Cookies["BasketItems"];

        AppUser appUser = null;

        if (HttpContext.User.Identity.IsAuthenticated)
        {
            appUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        }

        if (appUser is null)
        {
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
        }
        else
        {
            userBasketItem = await _context.BasketItems.FirstOrDefaultAsync(bi => bi.AppUserId == appUser.Id && bi.BookId == bookId);

            if (userBasketItem is not null && !userBasketItem.IsDeactive)
            {
                userBasketItem.Count++;
            }
            else
            {
                userBasketItem = new BasketItem()
                {
                    BookId = bookId,
                    Count = 1,
                    AppUserId = appUser.Id,
                    IsDeactive = false,
                    CreatedDate = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow,
                };

                await _context.BasketItems.AddAsync(userBasketItem);
            }
            await _context.SaveChangesAsync();
        }

        basketItemsStr = JsonConvert.SerializeObject(basketItems);

        HttpContext.Response.Cookies.Append("BasketItems", basketItemsStr);

        return Ok();
    }

    public async Task<IActionResult> RemoveItemFromBasket(int bookId)
    {
        if (!await _context.Books.AnyAsync(x => x.Id == bookId)) return NotFound();

        List<BasketItemViewModel> basketItems = new List<BasketItemViewModel>();
        BasketItemViewModel basketItem = null;
        BasketItem userBasketItem = null;
        List<BasketItem> userBasketItems = new List<BasketItem>();
        AppUser user = null;

        if (HttpContext.User.Identity.IsAuthenticated)
        {
            user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        }
        if (user is null)
        {
            var basketItemsStr = HttpContext.Request.Cookies["BasketItems"];
            if (basketItemsStr is not null)
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketItemsStr);

                basketItem = basketItems.FirstOrDefault(x => x.BookId == bookId);

                if (basketItem is not null)
                {
                    if (basketItem.Count > 1) basketItem.Count--;
                    else basketItems.Remove(basketItem);

                }
                else return NotFound();

            }
            else return NotFound();
            basketItemsStr = JsonConvert.SerializeObject(basketItems);

            HttpContext.Response.Cookies.Append("BasketItems", basketItemsStr);

        }
        else
        {
            userBasketItems = await _context.BasketItems.Where(bi => bi.AppUserId == user.Id && bi.IsDeactive == false).ToListAsync();
            userBasketItem = userBasketItems.FirstOrDefault(x => x.BookId == bookId);

            if (userBasketItem is not null)
            {
                if (userBasketItem.Count > 1) userBasketItem.Count--;
                else _context.BasketItems.Remove(userBasketItem);
                await _context.SaveChangesAsync();
            }
            else return NotFound();
        }


        return RedirectToAction("Basket");
    }

    public async Task<IActionResult> GetBasketItems()
    {
        List<BasketItemViewModel> basketItems = new List<BasketItemViewModel>();
        List<BasketItem> userBasketItems = new List<BasketItem>();
        AppUser user = null;

        if (HttpContext.User.Identity.IsAuthenticated)
        {
            user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        }

        if (user is not null)
        {
            userBasketItems = await _context.BasketItems.Where(bi => bi.AppUserId == user.Id && bi.IsDeactive == false).ToListAsync();
            basketItems = userBasketItems.Select(ubi => new BasketItemViewModel() { BookId = ubi.BookId, Count = ubi.Count }).ToList();
        }
        else
        {
            var basketItemsStr = HttpContext.Request.Cookies["BasketItems"];

            if (basketItemsStr is not null)
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketItemsStr);
            }
        }
        return Ok(basketItems);
    }

    public async Task<IActionResult> Basket()
    {
        List<BasketItemViewModel> basketItems = new List<BasketItemViewModel>();
        List<BasketItem> userBasketItems = new List<BasketItem>();
        AppUser user = null;

        if (HttpContext.User.Identity.IsAuthenticated)
        {
            user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        }

        if (user is not null)
        {
            userBasketItems = await _context.BasketItems.Where(bi => bi.AppUserId == user.Id && bi.IsDeactive == false).ToListAsync();
            basketItems = userBasketItems.Select(ubi => new BasketItemViewModel() { BookId = ubi.BookId, Count = ubi.Count }).ToList();
        }
        else
        {
            var basketItemsStr = HttpContext.Request.Cookies["BasketItems"];

            if (basketItemsStr is not null)
            {
                basketItems = JsonConvert.DeserializeObject<List<BasketItemViewModel>>(basketItemsStr);
            }
        }

        ViewBag.Books = _context.Books.Include(x => x.BookImages).ToList();
        return View(basketItems);
    }
    public async Task<IActionResult> Checkout()
    {
        List<CheckoutBook> checkoutBooks = new();
        List<BasketItem> userBasketItems = new();
        AppUser user = null;

        if (HttpContext.User.Identity.IsAuthenticated)
        {
            user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
        }
        else return RedirectToAction("Login", "Acc");

        if (user is not null)
        {
            userBasketItems = await _context.BasketItems.Where(bi => bi.AppUserId == user.Id && bi.IsDeactive == false).ToListAsync();
            checkoutBooks = userBasketItems.Select(ubi => new CheckoutBook() { BookId = ubi.BookId, Count = ubi.Count }).ToList();
        }
        else return NotFound();
        ViewBag.CheckoutBooks= checkoutBooks;
        ViewBag.Books= _context.Books.ToList();
		return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(Checkout checkout)
    {
        if (!ModelState.IsValid) return View(checkout);

        try
        {
            foreach (var item in ViewBag.CheckoutBooks)
            {
                CheckoutBook checkoutBook = new();
                checkoutBook = item;
                await _context.CheckoutBooks.AddAsync(checkoutBook);
            }
            await _context.Checkouts.AddAsync(checkout);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(checkout);
        }

        return RedirectToAction("Index");
    }
}
