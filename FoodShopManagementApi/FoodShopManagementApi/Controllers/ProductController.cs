﻿using DTO;
using FoodShopManagementApi.DAO;
using FoodShopManagementApi.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodShopManagementApi.DTO
{
    [Route("api/FoodShopManagement")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IConfiguration _config;

        public ProductController(IConfiguration config)
        {
            _config = config;
        }

        public bool ValidateToken()
        {
            var header = HttpContext.Request.Headers;//doc header cua request
            header.TryGetValue("Authorization", out Microsoft.Extensions.Primitives.StringValues value);
            bool isValid = JwtUtil.ValidateJSONWebToken(value, _config);
            return isValid;
        }

        [HttpGet("loadProducts")]
        [Produces("application/json")]
        public IActionResult LoadProducts()
        {
            bool isValidToken = ValidateToken();
            if (isValidToken)
            {
                TblProductsDAO dao = new TblProductsDAO();
                try
                {
                    List<TblProductsDTO> listProduct = dao.findAll();
                    if (listProduct != null)
                    {
                        return Ok(listProduct);
                    }
                }
                catch (Exception)
                {
                    StatusCode(500);
                }
            }
            return Unauthorized();
        }
    }
}
