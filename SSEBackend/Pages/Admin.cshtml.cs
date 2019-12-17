using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SSEBackend.Pages
{
    [ResponseCache(Duration = 3600)]
    public class AdminModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
