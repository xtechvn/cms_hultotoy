﻿using Entities.Models;
using Entities.ViewModels;
using Entities.ViewModels.Report;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepositories
{
    public interface IDepartmentRepository
    {
        Task<long> Create(Department model);
        Task<long> Update(Department model);
        Task<Department> GetById(int id);
        Task<IEnumerable<Department>> GetAll(string name);
        Task<long> Delete(int id);
        Task<GenericViewModel<SearchReportDepartmentViewModel>> GetReportDepartment(ReportDepartmentViewModel searchModel);
        Task<string> ExportDeposit(ReportDepartmentViewModel searchModel, string FilePath);
        Task<GenericViewModel<ListReportDepartmentViewModel>> GetReportDepartmentsaler(ReportDepartmentViewModel searchModel);
        Task<GenericViewModel<SearchReportDepartmentSupplier>> GetRevenueBySupplier(ReportDepartmentViewModel searchModel);
        Task<GenericViewModel<SearchReportDepartmentClient>> GetRevenueByClient(ReportDepartmentViewModel searchModel);
        Task<GenericViewModel<DetailRevenueByDepartmentViewModel>> GetListDetailRevenueByDepartment(ReportDepartmentViewModel searchModel);
        Task<GenericViewModel<ListDetailRevenueByDepartmentViewModel>> GetListDetailRevenueBySaler(ReportDepartmentViewModel searchModel);
        Task<GenericViewModel<DetailRevenueByDepartmentViewModel>> GetListDetailRevenueBySupplier(ReportDepartmentViewModel searchModel);
        Task<GenericViewModel<DetailRevenueByDepartmentViewModel>> GetListDetailRevenueByClient(ReportDepartmentViewModel searchModel);
        Task<GenericViewModel<SearchRevenueBySupplierViewModel>> GetListTotalDebtRevenueBySupplier(RevenueBySupplierViewModel searchModel);
        Task<GenericViewModel<SearchDetailRevenueBySupplierViewModel>> GetListDetailRevenueBySupplier(RevenueBySupplierViewModel searchModel);
        Task<string> ExportDeposit(RevenueBySupplierViewModel searchModel, string FilePath);
        Task<GenericViewModel<OrderViewModel>> GetListOrder(ReportDepartmentViewModel searchModel);
        Task<string> ExportDepositSupplier(RevenueBySupplierViewModel searchModel, string FilePath, double amount);
        Task<string> ExportDepositListOrder(ReportDepartmentViewModel searchModel, string FilePath);
        Task<string> ExportDepartmentBysaler(ReportDepartmentViewModel searchModel, string FilePath);
        Task<GenericViewModel<ReportDepartmentBysaleViewModel>> GetDepartmentBysaler(ReportDepartmentViewModel searchModel);
        Task<GenericViewModel<DetailDepartmentBysaleViewModel>> DetailDepartmentBysale(ReportDepartmentViewModel searchModel);
        Task<string> ExportDetailDepartmentBysaler(ReportDepartmentViewModel searchModel, string FilePath);
    }
}
