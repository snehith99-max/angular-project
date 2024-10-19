import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ExcelService } from 'src/app/Service/excel.service';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-otl-rpt-outletdaytrackerreport',
  templateUrl: './otl-rpt-outletdaytrackerreport.component.html',
  styleUrls: ['./otl-rpt-outletdaytrackerreport.component.scss']
})
export class OtlRptOutletdaytrackerreportComponent {

  outlet_overall:any[]=[];
  responsedata:any;
  constructor(private formBuilder: FormBuilder, private excelService:ExcelService,
    private ToastrService: ToastrService, private router: ActivatedRoute, 
    private route: Router, public service: SocketService,
    public NgxSpinnerService:NgxSpinnerService,) {
    
  }
  ngOnInit(): void{
    
    this.OverallReportSummary('Pending');
  }
  OverallReportSummary(value: string) {
    var url = 'otl_rpt_overallreport/Getoutletreportsummary';
    var params = { edit_status: value };
    this.NgxSpinnerService.show();
    this.service.getparams(url, params).subscribe((result: any) => {
        $('#outlet_overall').DataTable().destroy();
        this.responsedata = result;
        this.outlet_overall = this.responsedata.outlet_overall;
        setTimeout(() => {
            $('#outlet_overall').DataTable()
        }, 1);
        this.NgxSpinnerService.hide();
    });
}

  reportexportExcel(){
    const OverallExcel = this.outlet_overall.map(item => ({
      Date: item.created_date || '',
      Revenue_Amount : item.revenue_amount || '',
      Expense_Amount : item.expense_amount || '',
      Outlet : item.campaign_name || '',
      CreatedBy : item.created_by || '',
    }));
          this.excelService.exportAsExcelFile(OverallExcel, 'OverallReport_Excel');
  }
 

}
