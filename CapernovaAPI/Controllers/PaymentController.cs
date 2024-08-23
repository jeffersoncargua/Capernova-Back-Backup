using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe.Checkout;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using User.Managment.Data.Models;
using User.Managment.Data.Models.PaypalOrder;
using User.Managment.Data.Models.Transaction;
using Amount = User.Managment.Data.Models.PaypalOrder.Amount;
using PurchaseUnit = User.Managment.Data.Models.PaypalOrder.PurchaseUnit;

namespace CapernovaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        protected ApiResponse _response; //Para dar respuesta a una solicitud que se haya realizado
        public PaymentController()
        {
            this._response = new();
        }

        [HttpPost]
        [Route("paypal")]
        public async Task<ActionResult<ApiResponse>> Paypal([FromBody] Order model )
        {
            try
            {

                using (var client = new HttpClient())
                {
                    //Es el Client ID donde se va a recibir el pago con Paypal
                    var userName = "AZiCGcfCQdlpvwPzJGoyLURcKroZ-zSI8B-HX-2Gu7LZVcoiTCADxSMY8h7-SY1EnGwaYJ3b--4kpfGP";
                    //Es la contraseña del Client que va a recibir el pago con Paypal
                    var secret = "EJjnPCRIkeP_orIjGkToI9v9NU5AjdoXqNP5lOgzE9T8ttn_IC3StJu_ef1JeKxF47P7DxhgTDk_n0a1";

                    //Es la direccion URL basica que vamos a emplear para poder efectuar los pagos con Paypal
                    client.BaseAddress = new Uri("https://api-m.sandbox.paypal.com"); //Esta solo es la url de prueba luego se cambia por una real

                    //Se debe codificar el username y el secrte para enviarlo a la url de PAYPAL
                    var authToken = Encoding.ASCII.GetBytes($"{userName}:{secret}");
                    //Se agrega la autenticacion para validar las credenciales de la empresa que va a recibir el pago
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                    //Permite deserializar el carrito de compras enviado desde el front-end
                    var shoppingCart = JsonConvert.DeserializeObject<List<OrderDto>>(model.Productos);

                    List<Item> listItems = new List<Item>();

                    foreach (var shop in shoppingCart)
                    {
                        string precio = Convert.ToString(shop.Precio).Replace(",", ".");
                        listItems.Add(new Item()
                        {
                            name = shop.Titulo,
                            quantity = "1",
                            unit_amount = new UnitAmount()
                            {
                                currency_code = "USD",
                                value = precio,
                            }
                        });
                    }


                    PurchaseUnit purchaseUnit = new PurchaseUnit()
                    {
                        amount = new Amount
                        {
                            currency_code = "USD",
                            value = model.Total.ToString(), //Aqui va el valor total de la venta
                            breakdown = new Breakdown()
                            {
                                item_total = new ItemTotal
                                {
                                    currency_code = "USD",
                                    value = model.Total.ToString(), //Aqui va el valor total de la venta
                                }
                            }
                        },
                        description = "Compra de cursos-productos de capernova",
                        items = listItems
                    };

                    //Se genera la orden para poder empezar con la operacion del pago con paypal
                    var order = new PaypalOrder()
                    {
                        intent = "CAPTURE",
                        purchase_units = new List<PurchaseUnit>() { purchaseUnit },


                        //purchase_units = new List<PurchaseUnit>()
                        //{
                        //    new PurchaseUnit()
                        //    {
                        //        amount = new Amount()
                        //        {
                        //            currency_code = "USD",
                        //            value = "1.75", //model.Total, //Aqui va el precio del valor a pagar por la compra,
                        //        },
                        //        description = "Primera venta de capernova",
                        //    }

                        //},

                        application_context = new ApplicationContext()
                        {
                            brand_name = "Capernova",
                            landing_page = "NO_PREFERENCE", //muestra la pagina para ingresar las credenciales si se tiene una cuenta paypal            
                            user_action = "PAY_NOW", //Accion para que paypal muestre el monto
                            return_url = "https://localhost:3000/confirmPay", //pagina para confirmar el pago                            
                            cancel_url = "https://localhost:3000/cancelPay", //pagina para cancelar el pago
                        }
                    };

                    //var json = JsonConvert.SerializeObject(order);
                    var json = JsonConvert.SerializeObject(order);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("/v2/checkout/orders", data);

                    if (response.IsSuccessStatusCode)
                    {
                        _response.isSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Message = "La solicitud de pago se ha generado";
                        _response.Result = response.Content.ReadAsStringAsync().Result;
                        return Ok(_response);
                    }

                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "La solicitud de pago no se ha generado";
                    _response.Errors = new List<string>() { response.ReasonPhrase.ToString() };
                    return BadRequest(_response);

                }
            }
            catch (Exception ex)
            {

                _response.isSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
            
        }


        [HttpPost]
        [Route("paypalCard")]
        public async Task<ActionResult<ApiResponse>> PaypalCard([FromBody] Order model)
        {
            try
            {

                using (var client = new HttpClient())
                {
                    //Es el Client ID donde se va a recibir el pago con Paypal
                    var userName = "AZiCGcfCQdlpvwPzJGoyLURcKroZ-zSI8B-HX-2Gu7LZVcoiTCADxSMY8h7-SY1EnGwaYJ3b--4kpfGP";
                    //Es la contraseña del Client que va a recibir el pago con Paypal
                    var secret = "EJjnPCRIkeP_orIjGkToI9v9NU5AjdoXqNP5lOgzE9T8ttn_IC3StJu_ef1JeKxF47P7DxhgTDk_n0a1";


                    //Es la direccion URL basica que vamos a emplear para poder efectuar los pagos con Paypal
                    client.BaseAddress = new Uri("https://api-m.sandbox.paypal.com"); //Esta solo es la url de prueba luego se cambia por una real

                    //Se debe codificar el username y el secrte para enviarlo a la url de PAYPAL
                    var authToken = Encoding.ASCII.GetBytes($"{userName}:{secret}");
                    //Se agrega la autenticacion para validar las credenciales de la empresa que va a recibir el pago
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                    //Permite deserializar el carrito de compras enviado desde el front-end
                    var shoppingCart = JsonConvert.DeserializeObject<List<OrderDto>>(model.Productos);

                    List<Item> listItems = new List<Item>();

                    foreach (var shop in shoppingCart)
                    {
                        string precio = Convert.ToString(shop.Precio).Replace(",", ".");
                        listItems.Add(new Item()
                        {
                            name = shop.Titulo,
                            quantity = shop.Cantidad,
                            sku = shop.Codigo,
                            unit_amount = new UnitAmount()
                            {
                                currency_code = "USD",
                                value = precio,
                            }
                        });
                    }


                    PurchaseUnit purchaseUnit = new PurchaseUnit()
                    {
                        amount = new Amount
                        {
                            currency_code = "USD",
                            value = model.Total.ToString(), //Aqui va el valor total de la venta
                            breakdown = new Breakdown()
                            {
                                item_total = new ItemTotal
                                {
                                    currency_code = "USD",
                                    value = model.Total.ToString(), //Aqui va el valor total de la venta
                                }
                            }
                        },
                        description = "Compra de cursos-productos de capernova",
                        items = listItems
                    };

                    //Se genera la orden para poder empezar con la operacion del pago con paypal
                    var order = new PaypalOrder()
                    {
                        intent = "CAPTURE",
                        purchase_units = new List<PurchaseUnit>() { purchaseUnit },


                        //purchase_units = new List<PurchaseUnit>()
                        //{
                        //    new PurchaseUnit()
                        //    {
                        //        amount = new Amount()
                        //        {
                        //            currency_code = "USD",
                        //            value = "1.75", //model.Total, //Aqui va el precio del valor a pagar por la compra,
                        //        },
                        //        description = "Primera venta de capernova",
                        //    }

                        //},

                        application_context = new ApplicationContext()
                        {
                            brand_name = "Capernova",
                            payment_method_preference= "IMMEDIATE_PAYMENT_REQUIRED",
                            payment_method_selected = "PAYPAL",
                            locale = "es-EC",
                            landing_page = "NO_PREFERENCE",
                            //landing_page = "BILLING", //muestra la pagina para ingresar los datos de la tarjeta si no se tiene una cuenta paypal
                            user_action = "PAY_NOW", //Accion para que paypal muestre el monto
                            //user_action = "COMMIT", //Accion para que paypal muestre el monto
                            return_url = "https://localhost:3000/confirmPay", //pagina para confirmar el pago                            
                            cancel_url = "https://localhost:3000/cancelPay", //pagina para cancelar el pago
                        }
                    };

                    //var json = JsonConvert.SerializeObject(order);
                    var json = JsonConvert.SerializeObject(order);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync("/v2/checkout/orders", data);

                    if (response.IsSuccessStatusCode)
                    {

                        _response.isSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        _response.Message = "La solicitud de pago se ha generado";
                        _response.Result = response.Content.ReadAsStringAsync().Result;
                        return Ok(_response);
                    }

                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "La solicitud de pago no se ha generado";
                    _response.Errors = new List<string>() { response.ReasonPhrase.ToString() };
                    return BadRequest(_response);

                }
            }
            catch (Exception ex)
            {

                _response.isSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;

        }


        [HttpGet]
        [Route("confirmPaypal")]
        public async Task<ActionResult<ApiResponse>> ConfirmPaypal([FromQuery] string token)
        {
            try
            {


                using (var client = new HttpClient())
                {
                    //Es el Client ID donde se va a recibir el pago con Paypal
                    var userName = "AZiCGcfCQdlpvwPzJGoyLURcKroZ-zSI8B-HX-2Gu7LZVcoiTCADxSMY8h7-SY1EnGwaYJ3b--4kpfGP";
                    //Es la contraseña del Client que va a recibir el pago con Paypal
                    var secret = "EJjnPCRIkeP_orIjGkToI9v9NU5AjdoXqNP5lOgzE9T8ttn_IC3StJu_ef1JeKxF47P7DxhgTDk_n0a1";

                    //Es la direccion URL basica que vamos a emplear para poder efectuar los pagos con Paypal
                    client.BaseAddress = new Uri("https://api-m.sandbox.paypal.com"); //Esta solo es la url de prueba luego se cambia por una real

                    //Se debe codificar el username y el secrte para enviarlo a la url de PAYPAL
                    var authToken = Encoding.ASCII.GetBytes($"{userName}:{secret}");
                    //Se agrega la autenticacion para validar las credenciales de la empresa que va a recibir el pago
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

                    //permite enviar un objeto vacio a la url de confirmacion de pago de Paypal que es un objeto vacio
                    var data = new StringContent("{}", Encoding.UTF8, "application/json");

                   //permite recibir la respuesta a la peticion hacia la url de paypal para confirmar la transaccion
                    HttpResponseMessage response = await client.PostAsync($"/v2/checkout/orders/{token}/capture", data);

                    //se averigua se se completo la transaccion, si es afirmativo se procede a informar al front-end que se ha confirmado y que el pago se ha realizado
                    if (response.IsSuccessStatusCode)
                    {
                        //Aqui va lo que se va a almacenar en la base de datos para poder asiganarle el rol de estudiante y los cursos que compro

                        //se envia la respuesta al front-end
                        _response.isSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        var jsonResponse = response.Content.ReadAsStringAsync().Result;
                        PaypalTransaction objeto = JsonConvert.DeserializeObject<PaypalTransaction>(jsonResponse);
                        _response.Message = "La transacción se ha realizado correctamente";
                        //Se envia el id de la transaccion que se realizo
                        _response.Result = objeto.purchase_units[0].payments.captures[0].id;
                        return Ok(_response);
                    }

                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "La solicitud de pago no se ha generado";
                    return BadRequest(_response);

                }
            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        }


        [HttpPost]
        [Route("paymentCard")]
        public async Task<ActionResult<ApiResponse>> PaymentCard() //Aqui falta agregar el carrito de compras
        {
            try
            {
                //se establece la url base
                var domain = "https://localhost:3000";
                //Se agrega las configuraciones del stripe
                var options = new SessionCreateOptions
                {
                    LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmount = 100,
                                Currency = "usd",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = "Cocina Capernova",
                                },
                            },
                            Quantity = 1,
                        },
                    },
                    Mode = "payment",
                    SuccessUrl = domain +"/confirmPay?status=true", //Aqui falata agregar el id de la compra para poder consultar despues con la plataforma
                    CancelUrl = domain + "/cancelPay?status=false",

                };

                var service = new SessionService();
                Session session = await service.CreateAsync(options);

                //return Json(new { clientSecret = session.RawJObject["client_secret"] });
                _response.isSuccess = true;
                _response.Message = "Se ha generado su pedido";
                _response.StatusCode = HttpStatusCode.RedirectMethod;
                //_response.Result = session.Id;
                //Response.Headers.Add("Location", session.Url);
                _response.Result = session.Url;
                return Ok(_response);
            }
            catch (Exception e)
            {
                _response.isSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.Message = e.Message.ToString();
                return BadRequest(_response);
                
            }
            
        }

        /// <summary>
        /// En esta funcion se debe agregar el id para poder enviar a la pagina web el ID de la compra para su verificacion
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("orderConfirm")]
        public async Task<ActionResult<ApiResponse>> OrderConfirm(string status)
        {
            if(status == "true")
            {
                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "La solicitud de pago se ha generado";
                _response.Result = "qwertyuiop1234567890";
                return Ok(_response);
            }
            _response.isSuccess = false;
            _response.StatusCode = HttpStatusCode.BadRequest;
            _response.Message = "La solicitud de pago no se ha generado";
            return BadRequest(_response);
        }
        

    }
}
