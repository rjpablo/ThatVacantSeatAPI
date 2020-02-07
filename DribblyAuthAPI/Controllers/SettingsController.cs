﻿using DribblyAuthAPI.Models;
using DribblyAuthAPI.Models.Courts;
using DribblyAuthAPI.Services;
using System.Collections.Generic;
using System.Web.Http;

namespace DribblyAuthAPI.Controllers
{
    [RoutePrefix("api/Settings")]
    public class SettingsController : BaseController
    {
        private ISettingsService _service = null;

        public SettingsController() : base()
        {
            _service = new SettingsService(new AuthContext());
        }

        //GETs
        [HttpGet]
        [Route("GetInitialSettings")]
        public IEnumerable<SettingModel> GetInitialSettings()
        {
            return _service.GetInitialSettings();
        }

        [HttpGet]
        [Route("GetValue/{key}")]
        public string GetValue(string key)
        {
            return _service.GetValue(key);
        }

    }
}
