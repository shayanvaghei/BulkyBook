﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            CoverType coverType = new CoverType();
            if (id == null)
            {
                // this if for create
                return View(coverType);
            }
            // this is for edit
           // coverType = _unitOfWork.CoverType.Get(id.GetValueOrDefault());

            var parameter = new DynamicParameters();
            parameter.Add("@Id", id);
            coverType = _unitOfWork.SP_Call.OneRecord<CoverType>(SD.Proc_CoverType_Get, parameter); // passing the id as parameter

            if (coverType==null)
            {
                return NotFound();
            }
            return View(coverType);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            // var allObj = _unitOfWork.CoverType.GetAll(); using default way
            var allObj = _unitOfWork.SP_Call.List<CoverType>(SD.Proc_CoverType_GetAll, null); // using store procedure to getall
            return Json(new { data = allObj });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {
            //if (ModelState.IsValid)
            //{
            //    if (coverType.Id==0)
            //    {
            //        _unitOfWork.CoverType.Add(coverType);
            //    }
            //    else
            //    {
            //        _unitOfWork.CoverType.Update(coverType);
            //    }
            //    _unitOfWork.Save();
            //    return RedirectToAction(nameof(Index));
            //} 

            //return View(coverType);


            if (ModelState.IsValid)
            {
                var parameter = new DynamicParameters();
                parameter.Add("@Name", coverType.Name);
                if (coverType.Id == 0)
                {
                    // for create
                    _unitOfWork.SP_Call.Execute(SD.Proc_CoverType_Create, parameter);
                }
                else
                {
                    // for update
                    parameter.Add("@Id", coverType.Id);
                    _unitOfWork.SP_Call.Execute(SD.Proc_CoverType_Update, parameter);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(coverType);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            //var objFromDb = _unitOfWork.CoverType.Get(id);
            //if (objFromDb == null)
            //{
            //    return Json(new { sucssess = false, message = "Eroor while deleting" });
            //}
            //_unitOfWork.CoverType.Remove(objFromDb);
            //_unitOfWork.Save();
            //return Json(new { success = true, message = "Delete Successful" });

            var parameter = new DynamicParameters();
            parameter.Add("@Id", id);
            var objFromDb = _unitOfWork.SP_Call.OneRecord<CoverType>(SD.Proc_CoverType_Get, parameter); // passing the id as parameter
            if (objFromDb == null)
            {
                return Json(new { sucssess = false, message = "Eroor while deleting" });
            }
            _unitOfWork.SP_Call.Execute(SD.Proc_CoverType_Delete, parameter);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });


        }
        #endregion
    }
}
