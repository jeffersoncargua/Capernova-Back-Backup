using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using User.Managment.Data.Data;
using User.Managment.Data.Models;
using User.Managment.Data.Models.PaypalOrder;
using User.Managment.Data.Models.ProductosServicios;
using User.Managment.Data.Models.Student;
using User.Managment.Data.Models.Transaction;
using User.Managment.Data.Models.Ventas;
using User.Managment.Data.Models.Ventas.Dto;
using User.Managment.Repository.Models;
using User.Managment.Repository.Repository.IRepository;
using Amount = User.Managment.Data.Models.PaypalOrder.Amount;
using PurchaseUnit = User.Managment.Data.Models.PaypalOrder.PurchaseUnit;

namespace CapernovaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        protected ApiResponse _response; //Para dar respuesta a una solicitud que se haya realizado
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailRepository _emailRepository;
        
        public PaymentController(ApplicationDbContext db, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IEmailRepository emailRepository)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
            //_dbProduc = dbProduc;
            _emailRepository = emailRepository;
            this._response = new();
        }

        //[HttpPost]
        //[Route("paypal")]
        //public async Task<ActionResult<ApiResponse>> Paypal([FromBody] Order model )
        //{
        //    try
        //    {

        //        using (var client = new HttpClient())
        //        {
        //            //Es el Client ID donde se va a recibir el pago con Paypal
        //            var userName = "AZiCGcfCQdlpvwPzJGoyLURcKroZ-zSI8B-HX-2Gu7LZVcoiTCADxSMY8h7-SY1EnGwaYJ3b--4kpfGP";
        //            //Es la contraseña del Client que va a recibir el pago con Paypal
        //            var secret = "EJjnPCRIkeP_orIjGkToI9v9NU5AjdoXqNP5lOgzE9T8ttn_IC3StJu_ef1JeKxF47P7DxhgTDk_n0a1";

        //            //Es la direccion URL basica que vamos a emplear para poder efectuar los pagos con Paypal
        //            client.BaseAddress = new Uri("https://api-m.sandbox.paypal.com"); //Esta solo es la url de prueba luego se cambia por una real

        //            //Se debe codificar el username y el secrte para enviarlo a la url de PAYPAL
        //            var authToken = Encoding.ASCII.GetBytes($"{userName}:{secret}");
        //            //Se agrega la autenticacion para validar las credenciales de la empresa que va a recibir el pago
        //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToken));

        //            //Permite deserializar el carrito de compras enviado desde el front-end
        //            var shoppingCart = JsonConvert.DeserializeObject<List<OrderDto>>(model.Productos);

        //            List<Item> listItems = new List<Item>();

        //            foreach (var shop in shoppingCart)
        //            {
        //                string precio = Convert.ToString(shop.Precio).Replace(",", ".");
        //                listItems.Add(new Item()
        //                {
        //                    name = shop.Titulo,
        //                    quantity = "1",
        //                    unit_amount = new UnitAmount()
        //                    {
        //                        currency_code = "USD",
        //                        value = precio,
        //                    }
        //                });
        //            }


        //            PurchaseUnit purchaseUnit = new PurchaseUnit()
        //            {
        //                amount = new Amount
        //                {
        //                    currency_code = "USD",
        //                    value = model.Total.ToString(), //Aqui va el valor total de la venta
        //                    breakdown = new Breakdown()
        //                    {
        //                        item_total = new ItemTotal
        //                        {
        //                            currency_code = "USD",
        //                            value = model.Total.ToString(), //Aqui va el valor total de la venta
        //                        }
        //                    }
        //                },
        //                description = "Compra de cursos-productos de capernova",
        //                items = listItems
        //            };

        //            //Se genera la orden para poder empezar con la operacion del pago con paypal
        //            var order = new PaypalOrder()
        //            {
        //                intent = "CAPTURE",
        //                purchase_units = new List<PurchaseUnit>() { purchaseUnit },


        //                //purchase_units = new List<PurchaseUnit>()
        //                //{
        //                //    new PurchaseUnit()
        //                //    {
        //                //        amount = new Amount()
        //                //        {
        //                //            currency_code = "USD",
        //                //            value = "1.75", //model.Total, //Aqui va el precio del valor a pagar por la compra,
        //                //        },
        //                //        description = "Primera venta de capernova",
        //                //    }

        //                //},

        //                application_context = new ApplicationContext()
        //                {
        //                    brand_name = "Capernova",
        //                    landing_page = "NO_PREFERENCE", //muestra la pagina para ingresar las credenciales si se tiene una cuenta paypal            
        //                    user_action = "PAY_NOW", //Accion para que paypal muestre el monto
        //                    return_url = "https://localhost:3000/confirmPay", //pagina para confirmar el pago                            
        //                    cancel_url = "https://localhost:3000/cancelPay", //pagina para cancelar el pago
        //                }
        //            };

        //            //var json = JsonConvert.SerializeObject(order);
        //            var json = JsonConvert.SerializeObject(order);
        //            var data = new StringContent(json, Encoding.UTF8, "application/json");

        //            HttpResponseMessage response = await client.PostAsync("/v2/checkout/orders", data);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                _response.isSuccess = true;
        //                _response.StatusCode = HttpStatusCode.OK;
        //                _response.Message = "La solicitud de pago se ha generado";
        //                _response.Result = response.Content.ReadAsStringAsync().Result;
        //                return Ok(_response);
        //            }

        //            _response.isSuccess = false;
        //            _response.StatusCode = HttpStatusCode.BadRequest;
        //            _response.Message = "La solicitud de pago no se ha generado";
        //            _response.Errors = new List<string>() { response.ReasonPhrase.ToString() };
        //            return BadRequest(_response);

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        _response.isSuccess = false;
        //        _response.Errors = new List<string> { ex.ToString() };
        //    }

        //    return _response;
            
        //}


        [HttpPost]
        [Route("paypalCard")]
        public async Task<ActionResult<ApiResponse>> PaypalCard([FromBody] Order model)
        {
            try
            {
                //Este apartado permite verificar que se cuente con la cantidad requerida de los productos(sin cursos) caso contrario se emite que se ha producido un error 
                var shoppingCart = JsonConvert.DeserializeObject<List<OrderDto>>(model.Productos!);
                var onlyProductos = shoppingCart!.Where(x => x.Tipo == "producto").ToList();

                if (onlyProductos.Count > 0)
                {
                    foreach (var itemCart in onlyProductos!)
                    {
                        var productoStock = await _db.ProductoTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == Convert.ToInt32(itemCart.Id));
                        if (!(productoStock != null && (productoStock!.Cantidad >= Convert.ToInt32(itemCart.Cantidad))))
                        {
                            _response.isSuccess = false;
                            _response.StatusCode = HttpStatusCode.BadRequest;
                            _response.Message = $"El producto {itemCart.Titulo} solicitado no cuenta con la cantidad requerida. \n" +
                                $"Por favor, dirígete al item y cambia su cantidad.";
                            return BadRequest(_response);
                        }
                    }
                }

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
                    //var shoppingCart = JsonConvert.DeserializeObject<List<OrderDto>>(model.Productos!);

                    List<Item> listItems = new List<Item>();

                    foreach (var shop in shoppingCart!)
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
                            value = model.Total!.ToString(), //Aqui va el valor total de la venta
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
                    _response.Errors = new List<string>() { response.ReasonPhrase!.ToString() };
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
                    //HttpResponseMessage response = await client.PostAsync($"/v2/checkout/orders/{token}/capture", data);
                    HttpResponseMessage response = await client.PostAsync($"/v2/checkout/orders/{token}/capture", data);

                    //se averigua se se completo la transaccion, si es afirmativo se procede a informar al front-end que se ha confirmado y que el pago se ha realizado
                    if (response.IsSuccessStatusCode)
                    {
                        //Aqui va lo que se va a almacenar en la base de datos para poder asiganarle el rol de estudiante y los cursos que compro

                        //se envia la respuesta al front-end
                        _response.isSuccess = true;
                        _response.StatusCode = HttpStatusCode.OK;
                        var jsonResponse = response.Content.ReadAsStringAsync().Result;
                        PaypalTransaction objeto = JsonConvert.DeserializeObject<PaypalTransaction>(jsonResponse)!;
                        _response.Message = "La transacción se ha realizado correctamente";
                        //Se envia el id de la transaccion que se realizo
                        _response.Result = objeto!.purchase_units[0].payments.captures[0].id;
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
        [Route("createOrder")]
        public async Task<ActionResult<ApiResponse>> CreateOrder([FromBody] ConfirmOrder confirmOrder)
        {
            try
            {
                var cartList = JsonConvert.DeserializeObject<List<ShoppingCartDto>>(confirmOrder.Productos!);
                var userClient = JsonConvert.DeserializeObject<Cliente>(confirmOrder.Orden!);
                if (confirmOrder == null)
                {
                    _response.isSuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.Message = "Error!! No se pudo generar su pedido. ";
                    return BadRequest(_response);
                }

                if (confirmOrder.IdentifierName != "")
                {
                    var userSystem = await  _db.Users.FirstOrDefaultAsync(u => u.Id == confirmOrder.IdentifierName);
                    if (userSystem == null)
                    {
                        _response.isSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Message = "Error!! No se pudo generar su pedido. El usuario no existe ";
                        return BadRequest(_response);
                    }

                    ClienteDto clienteDto = new()
                    {
                        Id = userSystem.Id,
                        Name = userSystem.Name!,
                        LastName = userSystem.LastName!,
                        Email = userSystem.Email,
                        Phone = userSystem.PhoneNumber,
                        DirectionMain = userClient!.DirectionMain,
                        DirectionSec = userClient.DirectionSec
                    };

                    //Venta venta = new()
                    //{
                    //    Emision = DateTime.Now,
                    //    Total = Convert.ToDouble(confirmOrder.Total),
                    //    UserId = userSystem.Id,
                    //    Name = userSystem.Name,
                    //    LastName = userSystem.LastName,
                    //    Email = userSystem.Email,
                    //    Phone = userSystem.PhoneNumber
                    //};

                    //await _db.VentaTbl.AddAsync(venta);
                    //await _db.SaveChangesAsync();

                    //var ventaExist = _db.VentaTbl.Where(u => u.UserId == userSystem.Id).OrderByDescending(x => x.Emision).Take(1).FirstOrDefault();
                    var ventaExist = await GenerarVenta(confirmOrder.Total!, clienteDto, confirmOrder.TransaccionId!);
                    if (ventaExist == null)
                    {
                        _response.isSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Message = "Error!! No se pudo generar su pedido. No existe registro de la venta ";
                        return BadRequest(_response);
                    }

                    //var cartList = JsonConvert.DeserializeObject<List<ShoppingCartDto>>(confirmOrder.Productos);

                    //foreach (var itemProd in cartList)
                    //{
                    //    ShoppingCart cart = new()
                    //    {
                    //        ProductoName = itemProd.Titulo,
                    //        ProductoImagen = itemProd.Imagen,
                    //        ProductoCode = itemProd.Codigo,
                    //        Cantidad = Convert.ToInt32(itemProd.Cantidad),
                    //        Total = (Convert.ToInt32(itemProd.Cantidad) * Convert.ToDouble(itemProd.Precio)),
                    //        VentaId = ventaExist.Id
                    //    };

                    //    await _db.ShoppingCartTbl.AddAsync(cart);
                    //    await _db.SaveChangesAsync();
                    //}

                    await GenerarShoppingCart(cartList!, ventaExist);

                    await ActualizarStockProductos(cartList!);

                    var productosPedido = cartList!.Where(u => u.Tipo == "producto").ToList(); //permite obtener solo los items que son productos del carrito de compras
                    
                    //if (productosPedido.Count > 0 )
                    //{
                    //    //var userClient = JsonConvert.DeserializeObject<Cliente>(confirmOrder.Orden);
                    //    var productos = JsonConvert.SerializeObject(productosPedido);
                    //    Pedido pedido = new()
                    //    {
                    //        Emision = DateTime.Now,
                    //        Productos = productos,
                    //        VentaId = ventaExist.Id,
                    //        DirectionMain = userClient.DirectionMain,
                    //        DirectionSec = userClient.DirectionSec
                    //    };

                    //    await _db.PedidoTbl.AddAsync(pedido);
                    //    await _db.SaveChangesAsync();
                        
                    //}

                    await GenerarPedido(productosPedido, ventaExist, clienteDto);

                    //Aqui se va a enviar el mensaje por whatsapp al capernova para gestionar el envío
                    EnviarPedido(productosPedido, ventaExist, clienteDto);

                    var cursos = cartList!.Where(u => u.Tipo == "curso").ToList(); //permite obtener solo los cursos del carrito de compras

                    if (cursos.Count > 0)
                    {
                        //permite buscar la existencia de un estudiante de la tabla Student para no crearlo en caso de que encuentre una existencia
                        var userStudentExist = await _db.StudentTbl.FirstOrDefaultAsync(u => u.Id == userSystem.Id); 
                        if(userStudentExist == null)
                        {
                            await _userManager.RemoveFromRoleAsync(userSystem, "User");

                            if (await _roleManager.RoleExistsAsync("Student"))
                            {
                                await _userManager.AddToRoleAsync(userSystem, "Student");
                                Student student = new()
                                {
                                    Id = userSystem.Id,
                                    Name = userSystem.Name!,
                                    LastName = userSystem.LastName!,
                                    Email = userSystem.Email,
                                    Phone = userSystem.PhoneNumber
                                };

                                await _db.StudentTbl.AddAsync(student);
                                await _db.SaveChangesAsync();
                            }

                            //foreach (var itemCourse in cursos)
                            //{
                            //    var curso = await _db.CourseTbl.FirstOrDefaultAsync(u => u.Codigo == itemCourse.Codigo);
                            //    if (curso != null)
                            //    {
                            //        Matricula matricula = new()
                            //        {
                            //            CursoId = curso.Id,
                            //            EstudianteId = userSystem.Id,
                            //            IsActive = true,
                            //            Estado = "En Progeso"
                            //        };
                            //        _db.MatriculaTbl.Update(matricula);
                            //        await _db.SaveChangesAsync();

                            //    }
                            //}

                        }
                        //else{
                        //    foreach (var itemCourse in cursos)
                        //    {
                        //        var curso = await _db.CourseTbl.FirstOrDefaultAsync(u => u.Codigo == itemCourse.Codigo);
                        //        if (curso != null)
                        //        {
                        //            var matriculaExist = await _db.MatriculaTbl.FirstOrDefaultAsync(u => u.CursoId == curso.Id && u.EstudianteId == userStudentExist.Id);
                        //            if (matriculaExist != null)
                        //            {
                        //                Matricula matricula = new()
                        //                {
                        //                    CursoId = matriculaExist.CursoId,
                        //                    EstudianteId = matriculaExist.EstudianteId,
                        //                    IsActive = true,
                        //                    Estado = matriculaExist.Estado
                        //                };
                        //                _db.MatriculaTbl.Update(matricula);
                        //                await _db.SaveChangesAsync();
                        //            }
                        //            else
                        //            {
                        //                Matricula matricula = new()
                        //                {
                        //                    CursoId = curso.Id,
                        //                    EstudianteId = userSystem.Id,
                        //                    IsActive = true,
                        //                    Estado = "En progreso"
                        //                };
                        //                await _db.MatriculaTbl.AddAsync(matricula);
                        //                await _db.SaveChangesAsync();
                        //            }

                        //        }

                        //    }

                        //}

                        await GenerarMatricula(cursos, clienteDto,ventaExist);

                        NotificarMatricula(cursos, clienteDto, ventaExist);
                    }

                }
                else
                {
                    //var userClient = JsonConvert.DeserializeObject<Cliente>(confirmOrder.Orden);

                    ClienteDto clienteDto = new()
                    {
                        Id = userClient!.Id,
                        Name = userClient.Name,
                        LastName = userClient.LastName,
                        Email = userClient.Email,
                        Phone = userClient.Phone,
                        DirectionMain = userClient.DirectionMain,
                        DirectionSec = userClient.DirectionSec
                    };

                    //Venta venta = new()
                    //{
                    //    Emision = DateTime.Now,
                    //    Total = Convert.ToDouble(confirmOrder.Total),
                    //    UserId = userClient.Id,
                    //    Name = userClient.Name,
                    //    LastName = userClient.LastName,
                    //    Email = userClient.Email,
                    //    Phone = userClient.Phone
                    //};
                    //await _db.VentaTbl.AddAsync(venta);
                    //await _db.SaveChangesAsync();

                    //var ventaExist = _db.VentaTbl.Where(u => u.UserId == userClient.Id).OrderByDescending(x => x.Emision).Take(1).FirstOrDefault();
                    var ventaExist = await GenerarVenta(confirmOrder.Total!, clienteDto, confirmOrder.TransaccionId!);
                    if (ventaExist == null)
                    {
                        _response.isSuccess = false;
                        _response.StatusCode = HttpStatusCode.BadRequest;
                        _response.Message = "Error!! No se pudo generar su pedido. No existe registro de la venta ";
                        return BadRequest(_response);
                    }

                    //var cartList = JsonConvert.DeserializeObject<List<ShoppingCartDto>>(confirmOrder.Productos);

                    //foreach (var itemProd in cartList)
                    //{
                    //    ShoppingCart cart = new()
                    //    {
                    //        ProductoName = itemProd.Titulo,
                    //        ProductoImagen = itemProd.Imagen,
                    //        ProductoCode = itemProd.Codigo,
                    //        Cantidad = Convert.ToInt32(itemProd.Cantidad),
                    //        Total = (Convert.ToInt32(itemProd.Cantidad) * Convert.ToDouble(itemProd.Precio)),
                    //        VentaId = ventaExist.Id
                    //    };

                    //    await _db.ShoppingCartTbl.AddAsync(cart);
                    //    await _db.SaveChangesAsync();
                    //}

                    await GenerarShoppingCart(cartList!,ventaExist);
                    await ActualizarStockProductos(cartList!);


                    var productosPedido = cartList!.Where(u => u.Tipo == "producto").ToList(); //permite obtener solo los items que son productos del carrito de compras

                    //if (productosPedido != null)
                    //{
                    //    var productos = JsonConvert.SerializeObject(productosPedido);
                    //    Pedido pedido = new()
                    //    {
                    //        Emision = DateTime.Now,
                    //        Productos = productos,
                    //        VentaId = ventaExist.Id,
                    //        DirectionMain = userClient.DirectionMain,
                    //        DirectionSec = userClient.DirectionSec
                    //    };
                    //    await _db.PedidoTbl.AddAsync(pedido);
                    //    await _db.SaveChangesAsync();
                    //}
                    await GenerarPedido(productosPedido, ventaExist, clienteDto);

                    //Aqui se va a enviar el mensaje por whatsapp al capernova para gestionar el envío
                    EnviarPedido(productosPedido, ventaExist, clienteDto);

                }


                _response.isSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Message = "Transacción generada con éxito";
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.isSuccess = false;
                _response.Errors = new List<string> { ex.ToString() };
            }

            return _response;
        } 


        //[HttpPost]
        //[Route("paymentCard")]
        //public async Task<ActionResult<ApiResponse>> PaymentCard() //Aqui falta agregar el carrito de compras
        //{
        //    try
        //    {
        //        //se establece la url base
        //        var domain = "https://localhost:3000";
        //        //Se agrega las configuraciones del stripe
        //        var options = new SessionCreateOptions
        //        {
        //            LineItems = new List<SessionLineItemOptions>
        //            {
        //                new SessionLineItemOptions
        //                {
        //                    PriceData = new SessionLineItemPriceDataOptions
        //                    {
        //                        UnitAmount = 100,
        //                        Currency = "usd",
        //                        ProductData = new SessionLineItemPriceDataProductDataOptions
        //                        {
        //                            Name = "Cocina Capernova",
        //                        },
        //                    },
        //                    Quantity = 1,
        //                },
        //            },
        //            Mode = "payment",
        //            SuccessUrl = domain +"/confirmPay?status=true", //Aqui falata agregar el id de la compra para poder consultar despues con la plataforma
        //            CancelUrl = domain + "/cancelPay?status=false",

        //        };

        //        var service = new SessionService();
        //        Session session = await service.CreateAsync(options);

        //        //return Json(new { clientSecret = session.RawJObject["client_secret"] });
        //        _response.isSuccess = true;
        //        _response.Message = "Se ha generado su pedido";
        //        _response.StatusCode = HttpStatusCode.RedirectMethod;
        //        //_response.Result = session.Id;
        //        //Response.Headers.Add("Location", session.Url);
        //        _response.Result = session.Url;
        //        return Ok(_response);
        //    }
        //    catch (Exception e)
        //    {
        //        _response.isSuccess = false;
        //        _response.StatusCode = HttpStatusCode.BadRequest;
        //        _response.Message = e.Message.ToString();
        //        return BadRequest(_response);
                
        //    }
            
        //}

        ///// <summary>
        ///// En esta funcion se debe agregar el id para poder enviar a la pagina web el ID de la compra para su verificacion
        ///// </summary>
        ///// <param name="status"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("orderConfirm")]
        //public async Task<ActionResult<ApiResponse>> OrderConfirm(string status)
        //{
        //    if(status == "true")
        //    {
        //        _response.isSuccess = true;
        //        _response.StatusCode = HttpStatusCode.OK;
        //        _response.Message = "La solicitud de pago se ha generado";
        //        _response.Result = "qwertyuiop1234567890";
        //        return Ok(_response);
        //    }
        //    _response.isSuccess = false;
        //    _response.StatusCode = HttpStatusCode.BadRequest;
        //    _response.Message = "La solicitud de pago no se ha generado";
        //    return BadRequest(_response);
        //}

        #region Funciones
        private async Task<Venta>  GenerarVenta(string total, ClienteDto cliente,string transaccionId)
        {
            Venta venta = new()
            {
                Emision = DateTime.Now,
                TransaccionId = transaccionId,
                Total = double.Parse(total, CultureInfo.InvariantCulture),
                UserId = cliente.Id,
                Name = cliente.Name,
                LastName = cliente.LastName,
                Email = cliente.Email,
                Phone = cliente.Phone,
                Estado = "Pagado"
            };

            await _db.VentaTbl.AddAsync(venta);
            await _db.SaveChangesAsync();

            var ventaExist = await _db.VentaTbl.Where(u => u.UserId == cliente.Id).AsNoTracking().OrderByDescending(x => x.Emision).Take(1).FirstOrDefaultAsync();

            return  ventaExist!;
        }

        private async Task GenerarShoppingCart(List<ShoppingCartDto> cartList,Venta venta)
        {
            foreach (var itemProd in cartList)
            {
                ShoppingCart cart = new()
                {
                    ProductoName = itemProd.Titulo,
                    ProductoImagen = itemProd.Imagen,
                    ProductoCode = itemProd.Codigo,
                    Cantidad = Convert.ToInt32(itemProd.Cantidad),
                    Total = (Convert.ToInt32(itemProd.Cantidad) * Convert.ToDouble(itemProd.Precio)),
                    VentaId = venta.Id
                };

                await _db.ShoppingCartTbl.AddAsync(cart);
                await _db.SaveChangesAsync();
            }

        }

        private async Task GenerarPedido(List<ShoppingCartDto> productosPedido, Venta venta,ClienteDto cliente)
        {
            if (productosPedido.Count > 0)
            {
                var productos = JsonConvert.SerializeObject(productosPedido);
                Pedido pedido = new()
                {
                    Emision = DateTime.Now,
                    Productos = productos,
                    VentaId = venta.Id,
                    DirectionMain = cliente.DirectionMain,
                    DirectionSec = cliente.DirectionSec,
                    Estado = "POR ENTREGAR"
                };

                await _db.PedidoTbl.AddAsync(pedido);
                await _db.SaveChangesAsync();
            }
            

        }


        private void EnviarPedido(List<ShoppingCartDto> productosPedido, Venta venta, ClienteDto cliente)
        {
            string textMessage = "";
            if (productosPedido.Count > 0)
            {
                //textMessage += $""; 
                textMessage += $"<div style='display:flex; justify-content:center;'>";
                textMessage += $"<img src=\"https://drive.google.com/thumbnail?id=1Io3SAYU468d_ekK2k7_Ic7u6UXoXj9eV\" alt=\"Aqui va una imagen\" />";
                textMessage += $"</div>";
                textMessage += $"<h1 style='text-align:center'><b>Se ha realizado un pedido</b></h1>";
                textMessage += $"<h4><b>Nombre:</b> <span style='font-weight: normal;'>{venta.Name}</span></h4>";
                textMessage += $"<h4><b>Apellido:</b> <span style='font-weight: normal;'>{venta.LastName}</span></h4>";
                textMessage += $"<h4><b>Teléfono:</b> <span style='font-weight: normal;'>{venta.Phone}</span></h4>";
                textMessage += $"<h4><b>Dirección Principal:</b> <span style='font-weight: normal;'>{cliente.DirectionMain}</span></h4>";
                textMessage += $"<h4><b>Dirección Secundaria:</b> <span style='font-weight: normal;'>{cliente.DirectionSec}</span></h4>";
                textMessage += $"<h4><b>Pedido:</b></h4>";
                textMessage += $"<br />";
                textMessage += $"<table style='width:100%;border:1px solid #000;  border-collapse: collapse; text-align:center'>";
                textMessage += $"<thead>";
                textMessage += $"<tr>";
                textMessage += $"<th style='border: 1px solid #000;border-spacing: 0;' >Producto</th>";
                textMessage += $"<th style='border: 1px solid #000;border-spacing: 0;'>Cantidad</th>";
                textMessage += $"</tr>";
                textMessage += $"</thead>";
                textMessage += $"<tbody>";
                foreach (var itemPedido in productosPedido)
                {
                    textMessage += $"<tr>";
                    textMessage += $"<td style='border: 1px solid #000;border-spacing: 0;'>{itemPedido.Titulo}</td>";
                    textMessage += $"<td style='border: 1px solid #000;border-spacing: 0;'>{itemPedido.Cantidad}</td>";
                    textMessage += $"</tr>";
                }
                textMessage += $"</tbody>";
                textMessage += $"</table>";
                textMessage += $"<br />";
                textMessage += $"<p>La entrega de los productos se realizará a domicilio, por lo que uno de nuestros agentes se pondrá en contacto contigo al número de teléfono que nos proporcionaste.</p>";
                textMessage += $"<p>Para mayor información no dudes en comunicarte al 0987203469, o nuestro whatsapp y te ayudaremos con todas tus inquietudes</p>";
                textMessage += $"<br />";
                textMessage += $"<p>Muchas Gracias por preferirnos, tenga un excelente día</p>";
                textMessage += $"<br />";
                textMessage += $"<p>Atentamente, Capernova</p>";
            }

            var message = new Message(new string[] { "capernova.edu.ec@gmail.com" , cliente.Email}, $"Entregar Pedido a {venta.Name} {venta.LastName}" , textMessage);
            _emailRepository.SendEmail(message);

        }

        private async Task GenerarMatricula(List<ShoppingCartDto> cursos, ClienteDto userStudent, Venta ventaDto)
        {
            foreach (var itemCourse in cursos)
            {
                var curso = await _db.CourseTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Codigo == itemCourse.Codigo);
                if (curso != null)
                {
                    var matriculaExist = await _db.MatriculaTbl.AsNoTracking().FirstOrDefaultAsync(u => u.CursoId == curso.Id && u.EstudianteId == userStudent.Id);
                    if (matriculaExist != null)
                    {
                        Matricula matricula = new()
                        {
                            Id = matriculaExist.Id,
                            FechaInscripcion = ventaDto.Emision,
                            CursoId = matriculaExist.CursoId,
                            EstudianteId = matriculaExist.EstudianteId,
                            IsActive = true,
                            Estado = matriculaExist.Estado,
                            NotaFinal = matriculaExist.NotaFinal,
                            CertificadoId = matriculaExist.CertificadoId
                        };
                        _db.MatriculaTbl.Update(matricula);
                        await _db.SaveChangesAsync();
                    }
                    else
                    {
                        Matricula matricula = new()
                        {
                            CursoId = curso.Id,
                            EstudianteId = userStudent.Id,
                            FechaInscripcion = ventaDto.Emision,
                            IsActive = true,
                            Estado = "En progreso"
                        };
                        await _db.MatriculaTbl.AddAsync(matricula);
                        await _db.SaveChangesAsync();
                    }

                }

            }
        }


        //NotificarMatricula
        private void NotificarMatricula(List<ShoppingCartDto> cursos, ClienteDto userStudent, Venta ventaDto)
        {
            string textMessage = "";
            if (cursos.Count > 0)
            {
                //textMessage += $""; 
                textMessage += $"<div>";
                textMessage += $"<div style='display:flex; justify-content:center;'>";
                textMessage += $"<img src=\"https://drive.google.com/thumbnail?id=1Io3SAYU468d_ekK2k7_Ic7u6UXoXj9eV\" alt=\"Aqui va una imagen\" />";
                textMessage += $"</div>";
                textMessage += $"<h1 style='text-align:center'><b>Se ha realizado la matriculación de:</b></h1>";
                textMessage += $"<h4><b>Correo:</b> <span style='font-weight: normal;'>{ventaDto.Email}</span></h4>";
                textMessage += $"<h4><b>Nombre:</b> <span style='font-weight: normal;'>{ventaDto.Name}</span></h4>";
                textMessage += $"<h4><b>Apellido:</b> <span style='font-weight: normal;'>{ventaDto.LastName}</span></h4>";
                textMessage += $"<h4><b>Teléfono:</b> <span style='font-weight: normal;'>{ventaDto.Phone}</span></h4>";
                //textMessage += $"<h4><b>Dirección Principal:</b> <span style='font-weight: normal;'>{cliente.DirectionMain}</span></h4>";
                //textMessage += $"<h4><b>Dirección Secundaria:</b> <span style='font-weight: normal;'>{cliente.DirectionSec}</span></h4>";
                textMessage += $"<h4><b>Cursos Registrados:</b></h4>";
                textMessage += $"<br />";
                textMessage += $"<table style='width:100%;border:1px solid #000;  border-collapse: collapse; text-align:center'>";
                textMessage += $"<thead>";
                textMessage += $"<tr>";
                textMessage += $"<th style='border: 1px solid #000;border-spacing: 0;' >Curso</th>";
                //textMessage += $"<th style='border: 1px solid #000;border-spacing: 0;'>Cantidad</th>";
                textMessage += $"</tr>";
                textMessage += $"</thead>";
                textMessage += $"<tbody>";
                foreach (var itemCurso in cursos)
                {
                    textMessage += $"<tr>";
                    textMessage += $"<td style='border: 1px solid #000;border-spacing: 0;'>{itemCurso.Titulo}</td>";
                    textMessage += $"</tr>";
                }
                textMessage += $"</tbody>";
                textMessage += $"</table>";
                textMessage += $"<br />";
                textMessage += $"<p>La matriculación de tus cursos serán atendidos por uno de nuestros agentes para informarte del debido proceso. No te olvídes de regresar a nuestra página oficial e" +
                    $" iniciar sesión para acceder a todos los recursos de los cursos que adquiriste.</p>";
                textMessage += $"<p>Para mayor información no dudes en comunicarte al 0987203469, o nuestro whatsapp y te ayudaremos con todas tus inquietudes.</p>";
                textMessage += $"<br />";
                textMessage += $"<p>Muchas Gracias por preferirnos, tenga un excelente día</p>";
                textMessage += $"<br />";
                textMessage += $"<p>Atentamente, Capernova</p>";
                textMessage += $"</div>";
            }

            var message = new Message(new string[] { "capernova.edu.ec@gmail.com", userStudent.Email }, $"Matriculación de {ventaDto.Name} {ventaDto.LastName}", textMessage);
            _emailRepository.SendEmail(message);
        }


        private async Task ActualizarStockProductos(List<ShoppingCartDto> cartList)
        {
            var onlyProductos = cartList.Where(x => x.Tipo == "producto").ToList();
            if (onlyProductos.Count > 0)
            {
                foreach (var item in onlyProductos)
                {
                    var productoStock = await _db.ProductoTbl.AsNoTracking().FirstOrDefaultAsync(u => u.Id == item.Id);
                    Producto updateProductoStock = new()
                    {
                        Id = productoStock!.Id,
                        Codigo = productoStock.Codigo,
                        Titulo = productoStock.Titulo,
                        Cantidad = productoStock.Cantidad - item.Cantidad,
                        ImagenUrl = productoStock.ImagenUrl,
                        Detalle = productoStock.Detalle,
                        Precio = productoStock.Precio,
                        Tipo = productoStock.Tipo,
                    };

                    _db.ProductoTbl.Update(updateProductoStock);
                    await _db.SaveChangesAsync();
                    
                }
            }
            

        }

        #endregion


    }
}
