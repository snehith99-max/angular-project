import { Component, ViewEncapsulation} from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from 'src/environments/environment.development';
import { ExcelService } from 'src/app/Service/excel.service';
import { get } from 'jquery';
import  jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';
import { Options } from 'flatpickr/dist/types/options';
import flatpickr from 'flatpickr';
interface IPaymentSummaryReport {
  branch_name: string;
  department_name: string;
  month: string;
  year: string;
  branch_gid: string;
  department_gid: string;
  salary_gid:string;
}

@Component({
  selector: 'app-hrm-trn-form25component',
  templateUrl: './hrm-trn-form25component.component.html',
  styleUrls: ['./hrm-trn-form25component.component.scss'],
})
export class HrmTrnForm25componentComponent {
  month_list: any[] = [];
  branchdepartment_list: any[] = [];
  expandedRows: any[] = [];
  toggleExpansion(index: number) {
    this.expandedRows[index] = !this.expandedRows[index];
  }
  PaymentSummaryReport!: IPaymentSummaryReport;
  responsedata: any;
  constructor(private formBuilder: FormBuilder,
    private SocketService: SocketService,
    private NgxSpinnerService: NgxSpinnerService,
    private excelService : ExcelService,
     private route: ActivatedRoute,
     private router: Router, private ToastrService: ToastrService, 
     public service: SocketService) {
    this.PaymentSummaryReport = {} as IPaymentSummaryReport;
    }
    ngOnInit(): void {
      var url = 'HrmTrnForm25/GetMusterSummary'
      this.service.get(url).subscribe((result: any) => {
  
        this.responsedata = result;
        this.month_list = this.responsedata.month_list;
       //  this.overall=0;
        debugger;
        
       
        
        setTimeout(() => {
          $('#month_list').DataTable();
        }, );
  
  
      });


     
    }

    pdf(data:any,data1:any)
    {
      var param={
        branch_gid:data1.branch_gid,
        department_gid:data1.department_gid,
        year:data.year,
        month:data.month,

      }
      this.NgxSpinnerService.show();
    var url = 'HrmTrnForm25/GetForm25Rpt';
    this.SocketService.getparams(url, param).subscribe((result: any) => {
      if(result!=null){
        this.service.filedownload1(result);
      } 
      this.NgxSpinnerService.hide()

    }
  )}

    

    excel()
    {
      
    }

    ondetail() {
     debugger;
     var url = 'HrmTrnForm25/GetDetailSummary'
    
     this.service.get(url).subscribe((result: any) => {
      this.responsedata = result;
      this.branchdepartment_list = this.responsedata.form253mployee_list;
     
       });
   }
}


