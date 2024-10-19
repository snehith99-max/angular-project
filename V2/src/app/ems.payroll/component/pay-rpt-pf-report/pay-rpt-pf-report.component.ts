import { Component ,Renderer2, ElementRef } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES,enc } from 'crypto-js';
import { DomSanitizer } from '@angular/platform-browser';
import { ItemsList } from '@ng-select/ng-select/lib/items-list';
import  jsPDF from 'jspdf';
import autoTable from 'jspdf-autotable';

@Component({
  selector: 'app-pay-rpt-pf-report',
  templateUrl: './pay-rpt-pf-report.component.html',
  styleUrls: ['./pay-rpt-pf-report.component.scss']
})

export class PayRptPfReportComponent {
  pf_list : any[] = [];
  reactiveForm : FormGroup | any;
  month: any;
  sal_year: any;
  monthsal_year: any;
  responsedata: any;
  Pfassign_type : any[] = [];
  fileUrl: any;

  constructor(    
    private renderer: Renderer2,
    private el: ElementRef,
    public service: SocketService,
    private ToastrService: ToastrService,
    private route: Router,
    private router: ActivatedRoute,
    private formBuilder: FormBuilder,
    private sanitizer: DomSanitizer
    ){}
  

  ngOnInit(): void { 
    debugger;

    const monthsal_year = this.router.snapshot.paramMap.get('monthsal_year');
    this.monthsal_year = monthsal_year;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.monthsal_year, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam);
    debugger;
    const [month, sal_year] = deencryptedParam.split('+');
    this.month = month;
    this.sal_year = sal_year;
   

    this.Pfassign(month, sal_year);
    this.reactiveForm = this.formBuilder.group({
     
    });
    this.GetPFSummary(month,sal_year);

   


  }

  GetPFSummary(month: any,year: any) {
    debugger;
  var url = 'PayTrnRptPFandESIFormat/GetPFSummary'
  let param = {
    month: month,
    year: year,
  };
  this.service.getparams(url, param).subscribe((result: any) => {
   
    this.responsedata = result;
    this.pf_list = this.responsedata.pf_listdata;
    setTimeout(() => {
      $('#pf_list').DataTable()
    }, 1); 

  });
}

  Pfassign(month: any,sal_year: any) {
    debugger;
    var url = 'PayTrnRptPFandESIFormat/Pfassign';
    let param = {
      month: month,
      sal_year: sal_year,
    };
    this.service.getparams(url, param).subscribe((result: any) => {
      this.Pfassign_type = result.Pfassign_type;
    });
  }

  // pftxtfrmt(): void {
  //   const txt_data = this.pf_list.map(item =>({
  //   UAN : item.uan_no || '',
  //   ECR_UAN_Repository: item.employee_name || '',
  //   Gross: item.Gross || '',
  //   EPF : item.EPF || '',
  //   EPS: item.EPS || '',
  //   EDLI: item.EDLI || '',
  //   EE : item.EE || '',
  //   EPS1: item.EPS1 || '',
  //   ER: item.ER || '',
  //   NCP_Days: item.lop_days || '',
  //   Refunds: item.Refunds || '',
  //   Pension_Share: item.Pension_Share || '',
  //   ER_PF_Share: item.ER_PF_Share || '',
  //   EE_Share : item.EE_Share || '',
  //   Posting_location_of_the_member: item.Posting_location_of_the_member || ''
  //   }));
  //   this.service.filetxtdownload(txt_data, 'PF Report')
  // }


pftxtfrmt() {
  let textContent = '';

  this.pf_list.forEach(e => {
      textContent += `${e.uan_no}#~#${e.employee_name}#~#${e.Gross}#~#${e.EPF}#~#${e.EPS}#~#${e.EDLI}#~#${e.EE}#~#${e.EPS1}#~#${e.ER}#~#${e.lop_days}#~#${e.Refunds}\n`;
  });

  const blob = new Blob([textContent], { type: 'text/plain' });
  const url = URL.createObjectURL(blob);

  const a = document.createElement('a');
  a.href = url;
  a.download = 'PF_Report.txt';
  document.body.appendChild(a);
  a.click();
  document.body.removeChild(a);
  URL.revokeObjectURL(url);
}


}


