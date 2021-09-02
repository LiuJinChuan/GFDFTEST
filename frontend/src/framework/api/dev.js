import { apiRequest, BaseApi } from '@/framework/api/apiRequest'

export const ForminsApi = new BaseApi('/sys/formins');
export const MetaApi = new BaseApi('/sys/meta');
export const SyCsvApi = new BaseApi('/data/sy_csv');
//报表
export const ReportApi = new BaseApi('/sys/report');
export const ReportDtlApi = new BaseApi('/sys/reportdtl');
//代码段
export const CodeSegApi = new BaseApi('/sys/code');
export const CodeSegDtlApi = new BaseApi('/sys/codedtl');
CodeSegDtlApi.List = (segid) => apiRequest(`/sys/codedtl/list/${segid}`, "get");
//数据库、表
export const DBApi = new BaseApi('/sys/db');
export const DBTableApi = new BaseApi('/sys/dbtable');
export const DBTableFieldApi = new BaseApi('/sys/dbtablefield');
//BI仪表盘
export const ChartApi = new BaseApi('/base/chart');
ChartApi.QueryByDB = (dbid) => apiRequest(`/base/chart/db/${dbid}`, "get");
export const DashboardApi = new BaseApi('/base/dashboard');
DashboardApi.UpCharts = (obj) => apiRequest(`/base/dashboard/upchart`, "post", obj)
DashboardApi.AddCharts = (obj) => apiRequest(`/base/dashboard/addchart`, "post", obj)
DashboardApi.QueryByChart = (chartid) => apiRequest(`/base/dashboard/chart/${chartid}`, "get")
export const OtherApi = {
    execsql: (obj) => apiRequest(`/other/execsql`, "get", obj)
}