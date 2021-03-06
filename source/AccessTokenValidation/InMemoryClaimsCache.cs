﻿/*
 * Copyright 2014 Dominick Baier, Brock Allen
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Runtime.Caching;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Thinktecture.IdentityServer.v3.AccessTokenValidation
{
    public class InMemoryClaimsCache : IClaimsCache
    {
        private readonly IdentityServerBearerTokenAuthenticationOptions _options;
        private readonly MemoryCache _cache;

        public InMemoryClaimsCache(IdentityServerBearerTokenAuthenticationOptions options)
        {
            _options = options;
            _cache = new MemoryCache("thinktecture.validationCache");
        }

        public Task AddAsync(string token, IEnumerable<Claim> claims)
        {
            _cache.Add(token, claims, DateTimeOffset.UtcNow.Add(_options.ClaimsCacheDuration));

            return Task.FromResult<object>(null);
        }

        public Task<IEnumerable<Claim>> GetAsync(string token)
        {
            var result = _cache.Get(token);

            if (result != null)
            {
                return Task.FromResult(result as IEnumerable<Claim>);
            }

            return Task.FromResult<IEnumerable<Claim>>(null);
        }
    }
}