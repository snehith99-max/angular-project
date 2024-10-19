import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
interface Icase {
  case_no: string;
  case_date: string;
  client_name: string;
  quotation_date: string;
}
@Component({
  selector: 'app-lgl-mst-casemanagement',
  templateUrl: './lgl-mst-casemanagement.component.html',
  styleUrls: ['./lgl-mst-casemanagement.component.scss'],
})

export class LglMstCasemanagementComponent  implements OnInit {

  caseform!: FormGroup;
  case!: Icase;
  formDataObject: FormData = new FormData();
  GetCaseSummary_list: any[] = [];

  constructor(private NgxSpinnerService: NgxSpinnerService, private route: Router, private SocketService: SocketService, private ToastrService: ToastrService) {
    this.case = {} as Icase;

  }

  ngOnInit(): void {
   this.GetcaseSummary();
  } 
  GetcaseSummary() {
    debugger
    this.NgxSpinnerService.show();
    var url= 'CaseManagement/GetCaseManagementSummary';
    this.SocketService.get(url).subscribe((result:any)=>{
      console.log(result.GetCaseSummary_list);
      if(result.GetCaseSummary_list != null){
        $('#salesproductgroup_list').DataTable().destroy();
        this.GetCaseSummary_list = result.GetCaseManagementSummart_list; 
        this.NgxSpinnerService.hide();
        setTimeout(()=>{   
          $('#salesproductgroup_list').DataTable();
        }, 1); 
      }
      else{
        this.GetCaseSummary_list = result.GetCaseManagementSummart_list; 
        setTimeout(()=>{   
          $('#salesproductgroup_list').DataTable();
        }, 1);
        this.NgxSpinnerService.hide();
        $('#salesproductgroup_list').DataTable().destroy();
      } 
    });
   }


  
  sortColumn(columnKey: string): void {
    return this.SocketService.sortColumn(columnKey);
  }

  getSortIconClass(columnKey: string) {
    return this.SocketService.getSortIconClass(columnKey);
  }
  onview(case_gid: any) {
    const key = 'storyboarderp';
    const param = (case_gid);
    const case_gid1 = AES.encrypt(param, key).toString();
    this.route.navigate(['/legal/CaseManagementView', case_gid1])
  }
  onUpload(case_gid: any){
    const key = 'storyboarderp';
    const param = (case_gid);
    const case_gid1 = AES.encrypt(param, key).toString();
    this.route.navigate(['/legal/CaseManagementUpload', case_gid1])
  }
}
 
 


