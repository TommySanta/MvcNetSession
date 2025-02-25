using Microsoft.AspNetCore.Mvc;
using MvcNetSession.Extensions;
using MvcNetSession.Models;
using MvcNetSession.Repositories;

namespace MvcNetSession.Controllers
{
    public class EmpleadosController : Controller
    {
        private RepositoryEmpleados repo;
        public EmpleadosController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> SessionEmpleadosV5(int? idEmpleado)
        {
            List<int> idsEmpleados = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");

            if (idEmpleado != null)
            {
                if (idsEmpleados == null)
                {
                    idsEmpleados = new List<int>();
                }
                if (!idsEmpleados.Contains(idEmpleado.Value))
                {
                    idsEmpleados.Add(idEmpleado.Value);
                    HttpContext.Session.SetObject("IDSEMPLEADOS", idsEmpleados);
                }
                ViewData["MENSAJE"] = "Empleados almacenados: " + idsEmpleados.Count;
            }

            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }
        public async Task<IActionResult> EmpleadosAlmacenadosV5()
        {
            List<int> idsEmpleaods = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
            if (idsEmpleaods == null)
            {
                ViewData["MENSAJE"] = "No existen empleados almacenados" + "en Session.";
                return View();
            }
            else
            {
                List<Empleado> empleados =
                    await this.repo.GetEmpleadosSessionAsync(idsEmpleaods);
                return View(empleados);
            }
        }
        public IActionResult EliminarEmpleadoSession(int idEmpleado)
        {
            List<int> idsEmpleados = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");

            if (idsEmpleados != null && idsEmpleados.Contains(idEmpleado))
            {
                idsEmpleados.Remove(idEmpleado);
                HttpContext.Session.SetObject("IDSEMPLEADOS", idsEmpleados);
            }

            return RedirectToAction("EmpleadosAlmacenadosV5");
        }
        public async Task<IActionResult> SessionEmpleadosV4(int? idEmpleado)
        {
            // Recuperamos los IDs de empleados almacenados en sesión
            List<int> idsEmpleados = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS") ?? new List<int>();

            if (idEmpleado != null)
            {
                if (!idsEmpleados.Contains(idEmpleado.Value))
                {
                    idsEmpleados.Add(idEmpleado.Value);
                    HttpContext.Session.SetObject("IDSEMPLEADOS", idsEmpleados);
                }
                ViewData["MENSAJE"] = "Empleados almacenados: " + idsEmpleados.Count;
            }

            // Obtenemos todos los empleados de la base de datos
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();

            // Filtramos los empleados que NO están en sesión
            List<Empleado> empleadosNoAlmacenados = empleados.Where(e => !idsEmpleados.Contains(e.IdEmpleado)).ToList();

            return View(empleadosNoAlmacenados);
        }
        public async Task<IActionResult> EmpleadosAlmacenadosV4()
        {
            List<int> idsEmpleaods = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
            if (idsEmpleaods == null)
            {
                ViewData["MENSAJE"] = "No existen empleados almacenados" + "en Session.";
                return View();
            }
            else
            {
                List<Empleado> empleados =
                    await this.repo.GetEmpleadosSessionAsync(idsEmpleaods);
                return View(empleados);
            }
        }

        public async Task<IActionResult> EmpleadosAlmacenadosOK()
        {
            List<int> idsEmpleaods = HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
            if (idsEmpleaods == null)
            {
                ViewData["MENSAJE"] = "No existen empleados almacenados" + "en Session.";
                return View();
            }else
            {
                List<Empleado> empleados =
                    await this.repo.GetEmpleadosSessionAsync(idsEmpleaods);
                return View(empleados);
            }
        }
        public async Task<IActionResult> SessionEmpleadosOK(int? idEmpleado)
        {
            if (idEmpleado != null)
            {
                List<int> idsEmpleados;
                if(HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS") == null)
                {
                    idsEmpleados = new List<int>();
                }
                else
                {
                    idsEmpleados =
                        HttpContext.Session.GetObject<List<int>>("IDSEMPLEADOS");
                }
                idsEmpleados.Add(idEmpleado.Value);
                HttpContext.Session.SetObject("IDSEMPLEADOS", idsEmpleados);
                ViewData["MENSAJE"] = "Empleados almacenados: " + idsEmpleados.Count;
            }
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }
        public async Task<IActionResult> SessionEmpleados(int? idEmpleado)
        {
            if (idEmpleado != null)
            {
                Empleado empleado=
                    await this.repo.FindEmpleadoAsync(idEmpleado.Value);
                List<Empleado> empleadosList;
                if(HttpContext.Session.GetObject<List<Empleado>>("EMPLEADOS") != null)
                {
                    empleadosList = HttpContext.Session.GetObject<List<Empleado>>("EMPLEADOS");
                }
                else
                {
                    empleadosList = new List<Empleado>();
                }
                empleadosList.Add(empleado);
                HttpContext.Session.SetObject("EMPLEADOS", empleadosList);
                ViewData["MENSAJE"] = "EMPLEADO " + empleado.Apellido + " almacenado correctamente.";
            }
            List<Empleado> empleados = await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }
        public IActionResult EmpleadosAlmacenados()
        {
            return View(); 
        }
        public async Task<IActionResult> SessionSalarios(int? salario)
        {
            if (salario != null)
            {
                //NECESITAMOS ALMACENAR EL SALARIO DEL EMPLEADO  
                //Y LA SUMA TOTAL DE SALARIOS QUE TENGAMOS 
                int sumaSalarial = 0;
                //PREGUNTAMOS SI YA TENEMOS LA SUMA ALMACENADA EN SESSION 
                if (HttpContext.Session.GetString("SUMASALARIAL") != null)
                {
                    //SI YA EXISTE LA SUMA SALARIAL, RECUPERAMOS  
                    //SU VALOR 
                    sumaSalarial =
    HttpContext.Session.GetObject<int>("SUMASALARIAL");
                }
                //REALIZAMOS LA SUMA 
                sumaSalarial += salario.Value;
                //ALMACENAMOS EL NUEVO VALOR DE LA SUMA SALARIAL 
                //DENTRO DE SESSION 
                HttpContext.Session.SetObject("SUMASALARIAL", sumaSalarial);
                ViewData["MENSAJE"] = "Salario almacenado: " + salario.Value;
            }
            List<Empleado> empleados =
            await this.repo.GetEmpleadosAsync();
            return View(empleados);
        }

        public IActionResult SumaSalarial()
        {
            return View();
        }

    }
}
