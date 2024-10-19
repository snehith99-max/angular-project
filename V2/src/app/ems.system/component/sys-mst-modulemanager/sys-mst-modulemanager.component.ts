import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { AES } from 'crypto-js';
import { environment } from 'src/environments/environment.development';


@Component({
  selector: 'app-sys-mst-modulemanager',
  templateUrl: './sys-mst-modulemanager.component.html',
  styleUrls: ['./sys-mst-modulemanager.component.scss'],
  styles: [`
table thead th, 
.table tbody td { 
 position: relative; 
z-index: 0;
} 
.table thead th:last-child, 

.table tbody td:last-child { 
 position: sticky; 

right: 0; 
 z-index: 0; 

} 
.table td:last-child, 

.table th:last-child { 

padding-right: 50px; 

} 
.table.table-striped tbody tr:nth-child(odd) td:last-child { 

 background-color: #ffffff; 
  
  } 
  .table.table-striped tbody tr:nth-child(even) td:last-child { 
   background-color: #f2fafd; 

} 
`]
})
export class SysMstModulemanagerComponent {
  showOptionsDivId: any;
  Module_list: any;
  assignmanagerForm : FormGroup | any;
  employeelist : any;
  module_code: any;
  module_name: any; 
  module_gid: any;
  mdlcbomodulemanager:any;
  constructor(public router:Router, private ToastrService: ToastrService,private NgxSpinnerService: NgxSpinnerService, private SocketService: SocketService, public FormBuilder: FormBuilder){
    this.assignmanagerForm = new FormGroup ({
      cbomodulemanager : new FormControl('',[Validators.required]),
    })
  }

  ngOnInit() {

    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    var url = 'SysMstModuleManage/GetModuleListSummary';
    this.NgxSpinnerService.show();
    this.SocketService.get(url).subscribe((result: any) => { 
      this.NgxSpinnerService.hide();
      this.Module_list= result.mdlModuleDtl;   
  }); 
} 

 assignmanagerclick(module_code: any,module_name: any) {
  this.assignmanagerForm.reset();
  this.module_code = module_code;
  this.module_name = module_name; 
  var url = 'SystemMaster/GetEmployeelist';
  this.SocketService.get(url).subscribe((result: any) => {
    this.employeelist  = result.employeelist;
  });
}
assignManagerSubmit(){ 
  var params ={
    module_gid: this.module_code,
    modulemanager_gid: this.assignmanagerForm.get('cbomodulemanager').value
  }
  this.NgxSpinnerService.show();
  var url = 'SysMstModuleManage/PostManagerAssign';
  this.NgxSpinnerService.show();
  this.SocketService.post(url,params).subscribe((result: any) => { 
    if(result.status ==true){
      this.ToastrService.success(result.message)
      this.NgxSpinnerService.hide();
      this.ngOnInit();
    }
    else{
      this.ToastrService.warning(result.message)
      this.NgxSpinnerService.hide();   
    }
  });
}

assignemployeeclick(module_gid: any){ 
  const parameter1 = `${module_gid}`; 
  const encryptedParam = AES.encrypt(parameter1,environment.secretKey).toString();
  var url = '/system/SysMstAssignemployee?hash=' + encodeURIComponent (encryptedParam);
  this.router.navigateByUrl(url)
}
approvalclick(module_gid: any){ 
  const parameter1 = `${module_gid}`; 
  const encryptedParam = AES.encrypt(parameter1,environment.secretKey).toString();
  var url = '/system/SysMstApprovalhierarchy?hash=' + encodeURIComponent (encryptedParam);
  this.router.navigateByUrl(url)
}

hierarchyclick(module_gid: any){ 
  const parameter1 = `${module_gid}`; 
  const encryptedParam = AES.encrypt(parameter1,environment.secretKey).toString();
  var url = '/system/SysMstOrganisationHierarchy?hash=' + encodeURIComponent (encryptedParam);
  this.router.navigateByUrl(url)
}

clearhierarchyclick(){
  var params ={
    module_gid: this.module_code
  }

  this.NgxSpinnerService.show();
  var url = 'SysMstModuleManage/deletehierarchy';
  this.NgxSpinnerService.show();
  this.SocketService.post(url,params).subscribe((result: any) => { 
    if(result.status ==true){
      this.ToastrService.success(result.message)
      this.NgxSpinnerService.hide();
      this.ngOnInit();
    }
    else{
      this.ToastrService.warning(result.message)
      this.NgxSpinnerService.hide();   
    }
  });

}

toggleOptions(account_gid: any) {
  if (this.showOptionsDivId === account_gid) {
    this.showOptionsDivId = null;
  } else {
    this.showOptionsDivId = account_gid;
  }
}

}
