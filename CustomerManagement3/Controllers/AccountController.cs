using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

public class AccountController : Controller
{
    private readonly IMemoryCache _memoryCache;

    public AccountController(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public IActionResult Index()
    {
        try
        {
            var accounts = _memoryCache.Get<List<Account>>("Accounts") ?? new List<Account>();
            return View(accounts);
        }
        catch (Exception ex)
        {
            return View("Error", ex.Message);
        }
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(Account account)
    {
        AddAccountToCache(account);
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login()
    {
        ViewData["Title"] = "Giriş";
        return View();
    }

    [HttpPost]
    public IActionResult Login(Account user)
    {
        if (IsAuthenticated(user))
        {
            var cachedAccounts = _memoryCache.Get<List<Account>>("Accounts");

            if (cachedAccounts != null)
            {
                var foundUser = cachedAccounts.FirstOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);

                if (foundUser != null)
                {
                    foundUser.IsOnline = true;
                    _memoryCache.Set("Accounts", cachedAccounts);
                }
            }

            TempData["IsAuthenticated"] = true;
            return RedirectToAction("Index", "Account");
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Geçersiz e-posta veya şifre");
            return View();
        }
    }

    [HttpPost]
    public IActionResult Logout()
    {
        var authenticatedEmail = User.Identity.Name;
        SetUserAsOffline(authenticatedEmail);

        return RedirectToAction("Hello", "Home");
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        try
        {
            var accounts = _memoryCache.Get<List<Account>>("Accounts") ?? new List<Account>();
            var account = accounts.FirstOrDefault(a => a.Id == id);

            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }
        catch (Exception ex)
        {
            return View("Error", ex.Message);
        }
    }

    [HttpPost]
    public IActionResult Edit(Account updatedAccount)
    {
        try
        {
            var accounts = _memoryCache.Get<List<Account>>("Accounts") ?? new List<Account>();
            var existingAccount = accounts.FirstOrDefault(a => a.Id == updatedAccount.Id);

            if (existingAccount == null)
            {
                return NotFound();
            }

            // Update the properties
            existingAccount.UserName = updatedAccount.UserName;
            existingAccount.Email = updatedAccount.Email;
            existingAccount.Password = updatedAccount.Password;
            existingAccount.BirthDate = updatedAccount.BirthDate;

            _memoryCache.Set("Accounts", accounts);

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            return View("Error", ex.Message);
        }
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        try
        {
            var accounts = _memoryCache.Get<List<Account>>("Accounts") ?? new List<Account>();
            var account = accounts.FirstOrDefault(a => a.Id == id);

            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }
        catch (Exception ex)
        {
            return View("Error", ex.Message);
        }
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        try
        {
            var accounts = _memoryCache.Get<List<Account>>("Accounts") ?? new List<Account>();
            var accountToRemove = accounts.FirstOrDefault(a => a.Id == id);

            if (accountToRemove == null)
            {
                return NotFound();
            }

            accounts.Remove(accountToRemove);
            _memoryCache.Set("Accounts", accounts);

            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            return View("Error", ex.Message);
        }
    }

    private void SetUserAsOffline(string email)
    {
        var cachedAccounts = _memoryCache.Get<List<Account>>("Accounts");

        if (cachedAccounts != null)
        {
            var foundUser = cachedAccounts.FirstOrDefault(u => u.Email == email);

            if (foundUser != null)
            {
                foundUser.IsOnline = false;
                _memoryCache.Set("Accounts", cachedAccounts);
            }
        }
    }

    private bool IsAuthenticated(Account user)
    {
        var cachedAccounts = _memoryCache.Get < List
            <Account>>("Accounts");

        if (cachedAccounts != null)
        {
            return cachedAccounts.Any(u => u.UserName == user.UserName && u.Password == user.Password);
        }

        return false;
    }

    private void AddAccountToCache(Account account)
    {
        var cachedAccounts = _memoryCache.Get<List<Account>>("Accounts") ?? new List<Account>();
        cachedAccounts.Add(account);
        _memoryCache.Set("Accounts", cachedAccounts);
    }
}
