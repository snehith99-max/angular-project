import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { MessageService } from 'primeng/api';
import { AES } from 'crypto-js';

interface Icase {
  case_no: string;
  case_date: string;
  client_name: string;
  quotation_date: string;
}


interface UploadEvent {
  originalEvent: Event;
  files: File[];
}

@Component({
  selector: 'app-lgl-inst-casemanagement-summary',
  templateUrl: './lgl-inst-casemanagement-summary.component.html',
  styleUrls: ['./lgl-inst-casemanagement-summary.component.scss'],
  providers: [MessageService]
})
  
export class LglInstCasemanagementSummaryComponent {
  caseform!: FormGroup;
  case!: Icase;
  formDataObject: FormData = new FormData();
  GetCaseSummary_list: any[] = [];

  constructor(private formBuilder: FormBuilder,
    private route: Router,
    public NgxSpinnerService: NgxSpinnerService,
    private ToastrService: ToastrService,
    public service: SocketService, private messageService: MessageService,
    private router: ActivatedRoute) {
    this.case = {} as Icase;
  }
  ngOnInit(): void {
    debugger
    
    let user_gid = localStorage.getItem('user_gid');
    let param = {
      institute_gid: user_gid
    }
    // var api = 'CaseManagement/GetInstituteCase';
    // this.service.getparams(api,param ).subscribe((result: any) => {
    //   this.GetCaseSummary_list = result.GetCaseManagementSummart_list;
    // });
    var url= 'CaseManagement/GetInstituteCase';
    this.service.getparams(url,param).subscribe((result:any)=>{
      console.log(result.GetCaseSummary_list);
      if(result.GetCaseSummary_list != null){
        $('#GetCaseSummary_list').DataTable().destroy();
        this.GetCaseSummary_list = result.GetCaseManagementSummart_list; 
        this.NgxSpinnerService.hide();
        setTimeout(()=>{   
          $('#GetCaseSummary_list').DataTable();
        }, 1); 
      }
      else{
        this.GetCaseSummary_list = result.GetCaseManagementSummart_list; 
        setTimeout(()=>{   
          $('#GetCaseSummary_list').DataTable();
        }, 1);
        this.NgxSpinnerService.hide();
        $('#salesproductgroup_list').DataTable().destroy();
      } 
    });











  }
  
  onview(case_gid: any) {
    const key = 'storyboarderp';
    const param = (case_gid);
    const case_gid1 = AES.encrypt(param, key).toString();
    this.route.navigate(['/legal/LglIstCaseManagement-View', case_gid1])
  }
  onUpload(case_gid: any){
    const key = 'storyboarderp';
    const param = (case_gid);
    const case_gid1 = AES.encrypt(param, key).toString();
    this.route.navigate(['/legal/LglIstCaseManagement-Upload', case_gid1])
  }
  sortColumn(columnKey: string): void {
    return this.service.sortColumn(columnKey);
  }
  getSortIconClass(columnKey: string) {
    return this.service.getSortIconClass(columnKey);
  }
}
