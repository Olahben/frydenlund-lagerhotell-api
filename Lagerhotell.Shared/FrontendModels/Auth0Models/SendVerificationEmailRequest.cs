﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagerhotellAPI.Models.FrontendModels;

public record SendVerificationEmailRequest(string Auth0Id);
