using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Geta.NotFoundHandler.Admin.Pages.Geta.NotFoundHandler.Admin.Models;
using Geta.NotFoundHandler.Core.Redirects;
using Geta.NotFoundHandler.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Geta.NotFoundHandler.Admin.Pages.Geta.NotFoundHandler.Admin
{
    public class IndexModel : PageModel
    {
        private readonly IRedirectsService _redirectService;

        public IndexModel(IRedirectsService redirectService)
        {
            _redirectService = redirectService;
        }

        public string Message { get; set; }
        public IEnumerable<CustomRedirect> Items { get; set; } = new List<CustomRedirect>();

        [BindProperty]
        public CustomRedirectModel CustomRedirect { get; set; }

        public void OnGet()
        {
            var redirects = _redirectService.GetSaved().ToList();

            Message = $"There are currently stored {redirects.Count} custom redirects.";
            Items = redirects;
        }

        public IActionResult OnPostCreate()
        {
            if (!ModelState.IsValid) return RedirectToPage();

            var customRedirect = new CustomRedirect(CustomRedirect.OldUrl,
                                                    CustomRedirect.NewUrl,
                                                    CustomRedirect.WildCardSkipAppend,
                                                    CustomRedirect.RedirectType);

            _redirectService.AddOrUpdate(customRedirect);

            return RedirectToPage();
        }

        public IActionResult OnPostDeleteAsync(Guid id)
        {
            _redirectService.Delete(id);
            return RedirectToPage();
        }
    }
}
