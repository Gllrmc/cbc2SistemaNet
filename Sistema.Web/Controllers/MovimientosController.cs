﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema.Datos;
using Sistema.Entidades.Maestros;
using Sistema.Entidades.Operaciones;
using Sistema.Web.Models.Operaciones;

namespace Sistema.Web.Controllers
{
    [Authorize(Roles = "Administrador,JefeAdministracion,AsistAdministracion")]
    [Route("api/[controller]")]
    [ApiController]
    public class MovimientosController : ControllerBase
    {
        private readonly DbContextSistema _context;

        public MovimientosController(DbContextSistema context)
        {
            _context = context;
        }

        // GET: api/Movimientos/Listar
        [HttpGet("[action]")]
        public async Task<IEnumerable<MovimientoViewModel>> Listar()
        {
            var movimiento = await _context.Movimientos
                .Include(a => a.empresa)
                .Include(a => a.lote)
                .ThenInclude(a => a.asocuenta)
                .Include(a => a.asiento)
                .Include(a => a.grpconcepto)
                .ToListAsync();

            return movimiento.Select(a => new MovimientoViewModel
            {
                Id = a.Id,
                empresaId= a.empresaId,
                empresa = a.empresa.nombre,
                loteId = a.loteId,
                aniomes = a.lote.anio + "/" + a.lote.mes,
                asocuenta = a.lote.asocuenta.descripcion,
                asientoId = a.asientoId,
                origen = a.origen,
                grpconceptoId = a.grpconceptoId,
                grpconcepto = a.grpconcepto.nombre,
                concepto = a.concepto,
                fecha = a.fecha,
                importe = a.importe,
                unsgimporte = Math.Abs(a.importe),
                ref0 = a.ref0,
                ref1 = a.ref1,
                ref2 = a.ref2,
                ref3 = a.ref3,
                ref4 = a.ref4,
                ref5 = a.ref5,
                ref6 = a.ref6,
                ref7 = a.ref7,
                ref8 = a.ref8,
                ref9 = a.ref9,
                etlId = a.etlId,
                iduseralta = a.iduseralta,
                fecalta = a.fecalta,
                iduserumod = a.iduserumod,
                fecumod = a.fecumod,
                activo = a.activo
            });
        }

        // GET: api/Movimientos/Listarheader
        [HttpGet("[action]")]
        public async Task<IEnumerable<HeadermovViewModel>> Listarheader()
        {
            var movimiento = await _context.Movimientos
                .Include(a => a.empresa)
                .Include(a => a.lote)
                .ThenInclude(a => a.asocuenta)
                .GroupBy(a => new { a.empresaId, a.empresa.nombre, a.loteId, a.lote.asocuenta.descripcion, a.lote.anio, a.lote.mes})
                .Select(a => new { a.Key.empresaId, a.Key.nombre, a.Key.loteId, a.Key.descripcion, a.Key.anio, a.Key.mes, Count = a.Count() })
                .ToListAsync();

            return movimiento.Select(a => new HeadermovViewModel
            {
                empresaId = a.empresaId,
                empresa = a.nombre,
                loteId = a.loteId,
                asocuenta = a.descripcion,
                aniomes = a.anio + "/" + a.mes,
                cantidad = a.Count
            });

        }


        // PUT: api/Movimientos/Actualizar
        [HttpPut("[action]")]
        public async Task<IActionResult> Actualizar([FromBody] MovimientoUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.Id <= 0)
            {
                return BadRequest();
            }

            var fechaHora = DateTime.Now;
            var movimiento = await _context.Movimientos
                .FirstOrDefaultAsync(a => a.Id == model.Id);

            if (movimiento == null)
            {
                return NotFound();
            }

            movimiento.empresaId = model.empresaId;
            movimiento.loteId = model.loteId;
            movimiento.asientoId = model.asientoId;
            movimiento.origen = model.origen;
            movimiento.grpconceptoId = model.grpconceptoId;
            movimiento.concepto = model.concepto;
            movimiento.fecha = model.fecha;
            movimiento.importe = model.importe;
            movimiento.ref0 = model.ref0;
            movimiento.ref1 = model.ref1;
            movimiento.ref2 = model.ref2;
            movimiento.ref3 = model.ref3;
            movimiento.ref4 = model.ref4;
            movimiento.ref5 = model.ref5;
            movimiento.ref6 = model.ref6;
            movimiento.ref7 = model.ref7;
            movimiento.ref8 = model.ref8;
            movimiento.ref9 = model.ref9;
            movimiento.etlId = model.etlId;
            movimiento.iduseralta = model.iduseralta;
            movimiento.fecalta = model.fecalta;
            movimiento.iduserumod = model.iduserumod;
            movimiento.fecumod = fechaHora;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Guardar Excepción
                return BadRequest();
            }

            return Ok();
        }

        // POST: api/Movimientos/Crear
        [HttpPost("[action]")]
        public async Task<IActionResult> Crear([FromBody] MovimientoCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var fechaHora = DateTime.Now;
            Movimiento movimiento = new Movimiento
            {
                empresaId = model.empresaId,
                loteId = model.loteId,
                asientoId = model.asientoId,
                origen = model.origen,
                grpconceptoId = model.grpconceptoId,
                concepto = model.concepto,
                fecha = model.fecha,
                importe = model.importe,
                ref0 = model.ref0,
                ref1 = model.ref1,
                ref2 = model.ref2,
                ref3 = model.ref3,
                ref4 = model.ref4,
                ref5 = model.ref5,
                ref6 = model.ref6,
                ref7 = model.ref7,
                ref8 = model.ref8,
                ref9 = model.ref9,
                etlId = model.etlId,
                iduseralta = model.iduseralta,
                fecalta = fechaHora,
                iduserumod = model.iduseralta,
                fecumod = fechaHora,
                activo = true
            };

            _context.Movimientos.Add(movimiento);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok();
        }

        // DELETE: api/Movimientos/Eliminar/1
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Eliminar([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movimiento = await _context.Movimientos
                .FindAsync(id);

            if (movimiento == null)
            {
                return NotFound();
            }

            _context.Movimientos.Remove(movimiento);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok(movimiento);
        }

        // PUT: api/Movimientos/Desactivar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Desactivar([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            var movimiento = await _context
                .Movimientos
                .FirstOrDefaultAsync(c => c.Id == id);

            if (movimiento == null)
            {
                return NotFound();
            }

            movimiento.activo = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Guardar Excepción
                return BadRequest();
            }

            return Ok();
        }

        // PUT: api/Movimientos/Activar/1
        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> Activar([FromRoute] int id)
        {

            if (id <= 0)
            {
                return BadRequest();
            }

            var movimiento = await _context
                .Movimientos
                .FirstOrDefaultAsync(c => c.Id == id);

            if (movimiento == null)
            {
                return NotFound();
            }

            movimiento.activo = true;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Guardar Excepción
                return BadRequest();
            }

            return Ok();
        }

        private bool MovimientoExists(int id)
        {
            return _context.Movimientos.Any(e => e.Id == id);
        }
    }
}