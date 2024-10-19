import { Component ,Renderer2, ElementRef } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES,enc } from 'crypto-js';


@Component({
  selector: 'app-pf-rpt-esi-report',
  templateUrl: './pf-rpt-esi-report.component.html',
  styleUrls: ['./pf-rpt-esi-report.component.scss']
})
export class PfRptEsiReportComponent {
  esi_list : any[] = [];
  reactiveForm : FormGroup | any;
  month: any;
  sal_year: any;
  monthsal_year: any;
  responsedata: any;
  Pfassign_type : any[] = [];

  constructor(    
    private renderer: Renderer2,
    private el: ElementRef,
    public service: SocketService,
    private ToastrService: ToastrService,
    private route: Router,
    private router: ActivatedRoute,
    private formBuilder: FormBuilder,
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
     
  
      this.reactiveForm = this.formBuilder.group({
       
      });
      
      this.GetESISummary(month,sal_year);
    }

    GetESISummary(month: any,year: any) {
      debugger;
    var url = 'PayTrnRptPFandESIFormat/GetESISummary'
    let param = {
      month: month,
      year: year,
    };
    this.service.getparams(url, param).subscribe((result: any) => {
     
      this.responsedata = result;
      this.esi_list = this.responsedata.esi_list;
      setTimeout(() => {
        $('#esi_list').DataTable()
      }, 1); 
      
  
    });
  }

  // esitxtfrmt() :void {
  //   const txtdata = this.esi_list.map(item =>(
  //     {
  //       IP_Number : item.esi_no || '',
  //       IP_Name : item.employee_name || '',
  //       Actual_Working_Days : item.actual_month_workingdays || '',
  //       ESI_Employee_Contribution : item.employee_esi || '',
  //       ESI_Employer_Contribution : item.employer_esi || '',
  //       Total_Monthly_Wages : item.earned_gross_salary || ''
  //     }));
  //     this.service.filetxtdownload(txtdata, 'ESI Report')
  //   }


    esitxtfrmt() {
      let textContent = '';
    
      this.esi_list.forEach(e => {
          textContent += `${e.esi_no}#~#${e.employee_name}#~#${e.actual_month_workingdays}#~#${e.employee_esi}#~#${e.employer_esi}#~#${e.earned_gross_salary}\n`;
      });
    
      const blob = new Blob([textContent], { type: 'text/plain' });
      const url = URL.createObjectURL(blob);
    
      const a = document.createElement('a');
      a.href = url;
      a.download = 'ESI_Report.txt';
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
      URL.revokeObjectURL(url);
    }
    
   
  

}


