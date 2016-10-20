﻿using AspenWiki.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspenWiki.DB
{
    public class SysSettingDAO : GenericDAO<SysSetting>
    {
        public SysSettingDAO():base("SysSettings","SettingName"){}
    }
}