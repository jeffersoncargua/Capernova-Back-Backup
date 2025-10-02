// <copyright file="PaymentController.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using AutoMapper;
using Capernova.Utility;
using Microsoft.AspNetCore.Mvc;
using User.Managment.Data.Models.PaypalOrder;
using User.Managment.Repository.Repository.IRepository;

namespace CapernovaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _repoPayment;
        private readonly IMapper _mapper;
        protected ApiResponse _response; // Para dar respuesta a una solicitud que se haya realizado

        public PaymentController(IPaymentRepository repoPayment, IMapper mapper)
        {
            _repoPayment = repoPayment;
            _mapper = mapper;
            this._response = new();
        }

        /// <summary>
        /// Esta funcion permite recibir la peticion para poder empezar con el proceso de pago y compra a traves de Paypal.
        /// </summary>
        /// <param name="model">Son los parametros que contienen la información para empezar con la solictud de pago con paypal.</param>
        /// <returns>Si todo sale correctamente, se envia el id de transacción para continuar con el proceso de pago con paypal.</returns>
        [HttpPost]
        [Route("paypalCard")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> PaypalCard([FromBody] Order model)
        {
            var result = await _repoPayment.PaypalCardAsync(model);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpGet]
        [Route("confirmPaypal")]
        public async Task<ActionResult<ApiResponse>> ConfirmPaypal([FromQuery] string token)
        {
            var result = await _repoPayment.ConfirmPaypalAsync(token);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }

        [HttpPost]
        [Route("createOrder")]
        [ResponseCache(CacheProfileName = "Default30")]
        public async Task<ActionResult<ApiResponse>> CreateOrder([FromBody] ConfirmOrder confirmOrder)
        {
            var result = await _repoPayment.CreateOrderAsync(confirmOrder);

            return StatusCode((int)result.StatusCode, _mapper.Map<ApiResponse>(result));
        }
    }
}