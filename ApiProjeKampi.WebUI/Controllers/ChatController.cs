﻿using Microsoft.AspNetCore.Mvc;

namespace ApiProjeKampi.WebUI.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult SendChatWithGemini()
        {
            return View();
        }
    }
}
