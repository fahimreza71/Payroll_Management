using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PayrollWebApp.Models;
using System.Net;
using static iTextSharp.text.pdf.PdfDocument;

namespace PayrollWebApp.Controllers
{
    public class PayrollController : Controller
    {
        private readonly ApplicationDBContext context;

        public PayrollController(ApplicationDBContext context)
        {
            this.context = context;
        }
        // GET: PayrollController
        public ActionResult Index()
        {
            var list = context.Payrolls.ToList();
            foreach (var obj in list)
            {
                obj.Employees = context.Employees.FirstOrDefault
                    (u => u.EmployeeId == obj.EmployeeId);

                obj.Designations = context.Designations.FirstOrDefault
                    (x => x.DesignationId == obj.DesignationId);
            }
            return View(list);
        }

        // GET: PayrollController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PayrollController/Create
        public ActionResult Create()
        {
            IEnumerable<SelectListItem> DesignationTypes =
                    context.Designations.Select(x => new SelectListItem
                    {
                        Text = x.DesignationType,
                        Value = x.DesignationId.ToString()
                    });
            ViewBag.Designations = DesignationTypes;

            IEnumerable<SelectListItem> EmployeeList =
                context.Employees.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.EmployeeId.ToString()
                });
            ViewBag.EmployeeList = EmployeeList;

            List<SelectListItem> Month = new()
            {
                new SelectListItem{Value="January",Text="January"},
                new SelectListItem{Value="February",Text="February"},
                new SelectListItem{Value="March",Text="March"},
                new SelectListItem{Value="April",Text="April"},
                new SelectListItem{Value="May",Text="May"},
                new SelectListItem{Value="June",Text="June"},
                new SelectListItem{Value="July",Text="July"},
                new SelectListItem{Value="August",Text="August"},
                new SelectListItem{Value="September",Text="September"},
                new SelectListItem{Value="Octobar",Text="Octobar"},
                new SelectListItem{Value="November",Text="November"},
                new SelectListItem{Value="December",Text="November"}
            };
            ViewBag.Month = Month;

            return View();
        }

        // POST: PayrollController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PayrollEntity collection)
        {
            try
            {
                if(collection == null)
                {
                    return NotFound();
                }
                else
                {
                    context.Payrolls.Add(collection);
                    context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: PayrollController/Edit/5
        public ActionResult Edit(int? id)
        {
            var data = context.Payrolls.Find(id);
            if (data == null) { return NotFound(); }


            var EmpName = context.Employees.Find(data.EmployeeId);
            var Desig = context.Designations.Find(data.DesignationId);

            ViewBag.EmployeeDetails = EmpName;
            ViewBag.EmployeeName = EmpName.Name;
            //ViewBag.Designation = Desig;
            //ViewBag.DesignationType = Desig.DesignationType;
            ViewBag.BaseSalary = Desig.BaseSalary;

            IEnumerable<SelectListItem> DesignationTypes =
                    context.Designations.Select(x => new SelectListItem
                    {
                        Text = x.DesignationType,
                        Value = x.DesignationId.ToString()
                    });
            ViewBag.Designations = DesignationTypes;

            List <SelectListItem> Month = new()
            {
                new SelectListItem{Value="January",Text="January"},
                new SelectListItem{Value="February",Text="February"},
                new SelectListItem{Value="March",Text="March"},
                new SelectListItem{Value="April",Text="April"},
                new SelectListItem{Value="May",Text="May"},
                new SelectListItem{Value="June",Text="June"},
                new SelectListItem{Value="July",Text="July"},
                new SelectListItem{Value="August",Text="August"},
                new SelectListItem{Value="September",Text="September"},
                new SelectListItem{Value="Octobar",Text="Octobar"},
                new SelectListItem{Value="November",Text="November"},
                new SelectListItem{Value="December",Text="November"}
            };
            ViewBag.Month = Month;

            return View(data);
        }

        // POST: PayrollController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, PayrollEntity collection)
        {
            if (id != null)
            {
                context.Payrolls.Update(collection);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: PayrollController/Delete/5
        public ActionResult Delete(int? id)
        {
            var data = context.Payrolls.Find(id);
            data.Employees = context.Employees.Find(data.EmployeeId);
            data.Designations = context.Designations.Find(data.DesignationId);
            return View(data);
        }

        // POST: PayrollController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, PayrollEntity collection)
        {
            try
            {
                context.Payrolls.Remove(collection);
                context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult UserPayroll(int? id)
        {
            var data = context.Payrolls.FirstOrDefault(x => x.EmployeeId == id);
            if(data == null)
            {
                return NotFound("Contact With Admin & Ask Him To Update Your Details (Designation + Pay-ROll)");
            }
            data.Designations = context.Designations.Find(data.DesignationId);
            data.Employees = context.Employees.Find(id);
            return View(data);
        }

        [HttpGet]
        public ActionResult DownloadPDF(int? id)
        {
            byte[] data = GeneratePDF(id);
            return File(data,"application/pdf","PaySlip.pdf");
        }

        public byte[] GeneratePDF(int? id)
        {
            var data = context.Payrolls.Find(id);
            data.Designations = context.Designations.Find(data.DesignationId);
            data.Employees = context.Employees.Find(data.EmployeeId);

            iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4,20,20,20,20);
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter pdfWriter = PdfWriter.GetInstance(doc, memoryStream);
            doc.Open();

            // Add Title
            Paragraph title = new Paragraph("PAYSLIP");
            title.Alignment = Element.ALIGN_CENTER;
            doc.Add(title);

            // Add Date
            Paragraph date = new Paragraph($"Date: {DateTime.Now}\n\n");
            date.Alignment = Element.ALIGN_CENTER;
            doc.Add(date);

            PdfPTable infoTBL = new PdfPTable(4);
            PdfPCell cell1 = new PdfPCell();
            infoTBL.DefaultCell.Border = Rectangle.NO_BORDER;

            infoTBL.AddCell("Name : "); infoTBL.AddCell(data.Employees.Name.ToString());
            infoTBL.AddCell("Gender : "); infoTBL.AddCell(data.Employees.Gender.ToString());
            infoTBL.AddCell("Phone : "); infoTBL.AddCell(data.Employees.Phone.ToString());
            infoTBL.AddCell("Email : "); infoTBL.AddCell(data.Employees.Email.ToString());
            infoTBL.AddCell("Address : "); infoTBL.AddCell(data.Employees.Address.ToString());
            infoTBL.AddCell("Designation : "); infoTBL.AddCell(data.Designations.DesignationType.ToString());
            infoTBL.AddCell("Date Join : "); infoTBL.AddCell(data.Employees.DOJ.ToString());
            infoTBL.AddCell("Salary Month : "); infoTBL.AddCell(data.Month.ToString());

            doc.Add(infoTBL);

            Paragraph employeeInfo = new Paragraph($"\n\n");
            doc.Add(employeeInfo);

            // Create a table
            PdfPTable table = new PdfPTable(2); // 2 columns
            PdfPCell cell = new PdfPCell();
            //cell.Colspan = 3;
            //table.AddCell(cell);
            table.AddCell("Earnings");//Col 1 Row 1
            table.AddCell("Amount");//Col 2 Row 1
            table.AddCell("Base Salary");//Col 1 Row 2
            table.AddCell(data.Designations.BaseSalary.ToString());//Col 2 Row 2
            table.AddCell("Bonus");//Col 1 Row 3
            table.AddCell(data.Bonus.ToString());//Col 2 Row 3
            table.AddCell("Total");
            table.AddCell((data.Bonus + data.Designations.BaseSalary).ToString());

            // Add the table to the document
            doc.Add(table);

            Paragraph info = new Paragraph($"\nThis is system generated payslip");
            info.Alignment = Element.ALIGN_CENTER;
            doc.Add(info);

            // Close the document
            doc.Close();

            return memoryStream.ToArray();
        }
    }
}
