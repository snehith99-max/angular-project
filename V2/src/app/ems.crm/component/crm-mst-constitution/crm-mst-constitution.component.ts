import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-crm-mst-constitution',
  templateUrl: './crm-mst-constitution.component.html',
  styleUrls: ['./crm-mst-constitution.component.scss'],
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
export class CrmMstConstitutionComponent {
  constitutionAddForm!:FormGroup;
  ConstitutionEditForm!:FormGroup;
  stsform!:FormGroup;
  ConstitutionList: any;
  showOptionsDivId: any;
  txtconstitution_code: any;
  txtconstitution_name: any;
  isButtonTrue:boolean=true; 
  isButtonFalse:boolean=false;
  constitution_gid:any;
  editConstitutionFormData = {
    txteditconstitutioncode: '',
    txteditconstitutionname:'',
  };
  statusFormData = {
    txtconstitution_name: '',
    rbo_status: '',
    txtremarks: '',

  
  };
  parametervalue: any;
  Constitutioninactivelog_data: any;
editloglist: any;
current_status: any;
  status: any;
  constructor(private FormBuilder: FormBuilder,private NgxSpinnerService: NgxSpinnerService,private SocketService: SocketService,private ToastrService: ToastrService)
  {
    this.AddConstitution();
    this.EditConstitution();
   this.createstsform();
  }

  ngOnInit(): void {
   this. GetconstitutionSummary();
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
 

  }
  GetconstitutionSummary(){
    this.NgxSpinnerService.show();
    var url= 'ConstitutionMaster/GetConstitutionSummary';
    this.SocketService.get(url).subscribe((result:any)=>{
      if(result.constitutionlist != null){
        $('#ConstitutionSummary').DataTable().destroy();
        this.ConstitutionList = result.constitutionlist;  
        this.NgxSpinnerService.hide();
        setTimeout(()=>{   
          $('#ConstitutionSummary').DataTable();
        }, 1);
      }
      else{
        this.ConstitutionList = result.constitutionlist; 
        setTimeout(()=>{   
          $('#ConstitutionSummary').DataTable();
        }, 1);
        this.NgxSpinnerService.hide();
        $('#ConstitutionSummary').DataTable().destroy();
      } 
    });
  } 
  AddConstitution(){
    this.constitutionAddForm = this.FormBuilder.group({
      txtconstitution_name:  ['', [Validators.required, Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]],
    });
  }
  EditConstitution() {
    this.ConstitutionEditForm = this.FormBuilder.group({
      txteditconstitutionname: ['', [Validators.required, Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]],
    });
  }
  createstsform(){
    this.stsform=this.FormBuilder.group({
      txtremarks: ['', [Validators.required,Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]],
      rbo_status: [''] 
    })
  }
  openAddConstitutionpopup(){
    this.constitutionAddForm.reset(); 
    this.ngOnInit();
  }
  
  addconstitution(){

    var params = {
      constitution_name: this.txtconstitution_name
    }
    this.NgxSpinnerService.show()
    var url = 'ConstitutionMaster/ConstitutionAdd';
    this.SocketService.post(url, params).subscribe((result:any) => {
      if(result.status == true){
        this.NgxSpinnerService.hide();
        this.ToastrService.success(result.message);
      }
      else {
        this.ToastrService.warning(result.message);
      }
      this.ngOnInit();
    })
  }
  
  toggleOptions(constitution_gid: any) {
    if (this.showOptionsDivId === constitution_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = constitution_gid;
    }
  }

  editconstitution(constitution_gid:any){

    // this.cluster_gid = cluster_gid;
        var params = {
          constitution_gid: constitution_gid
      }
      this.NgxSpinnerService.show();
      var url = 'ConstitutionMaster/GetConstitutionEdit';
      this.SocketService.getparams(url, params).subscribe((result:any) => {
        this.editConstitutionFormData.txteditconstitutioncode= result.constitution_code;
        this.editConstitutionFormData.txteditconstitutionname = result.constitution_name;
        this.constitution_gid=result.constitution_gid
        this.NgxSpinnerService.hide();    
      });
  }
  constitutionupdate(){
    var url = 'ConstitutionMaster/UpdateGetConstitution';
    var params = {
      // entity_gid: this.entity_gid,
      constitution_code: this.editConstitutionFormData.txteditconstitutioncode,
      constitution_name: this.editConstitutionFormData.txteditconstitutionname,
      constitution_gid:this.constitution_gid
    }
  
    this.NgxSpinnerService.show();
    this.SocketService.post(url, params).subscribe((result:any) => {    
    if(result.status == true){
      this.ToastrService.success(result.message);
      this.NgxSpinnerService.hide();
      this.ngOnInit();
      }
      else {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      }   
    })
}
ViewConstitution(constitution_gid: any) {
  this.constitution_gid = constitution_gid;
  this.NgxSpinnerService.show();
  this.ConstitutionEditForm.reset();
  var param = {
    constitution_gid: constitution_gid,
  }
  var url = 'ConstitutionMaster/GetConstitutionEdit';
  this.SocketService.getparams(url, param).subscribe((result: any) => {
    this.editConstitutionFormData.txteditconstitutioncode = result.constitution_code;
    this.editConstitutionFormData.txteditconstitutionname = result.constitution_name;
    this.NgxSpinnerService.hide();
  });
}
Status_update(constitution_gid: any) {
  this.constitution_gid = constitution_gid
  this.stsform.reset();
  var params = {
    constitution_gid: constitution_gid
  }
  this.NgxSpinnerService.show();
  var url = 'ConstitutionMaster/GetConstitutionEdit';
  this.SocketService.getparams(url, params).subscribe((result: any) => {
    this.constitution_gid = result.constitution_gid;
    this.statusFormData.txtconstitution_name = result.constitution_name;
    this.statusFormData.rbo_status = result.status_log;
    this.current_status = this.statusFormData.rbo_status;
    this.NgxSpinnerService.hide();
  });
  // var url = 'ConstitutionMaster/ConstitutionInactiveHistory'
  // this.SocketService.getparams(url, params).subscribe((result: any) => {
  //   this.Constitutioninactivelog_data = result.ConstitutionInactiveHistorylist;
  //   this.NgxSpinnerService.hide();
  // });
}

update_status() {
  if(this.current_status == this.statusFormData.rbo_status){
    if (this.statusFormData.rbo_status === 'N')
    this.ToastrService.warning("Constitution is already Inactive.");
    if (this.statusFormData.rbo_status === 'Y')
    this.ToastrService.warning("Constitution is already active.");
  }
  else{
    this.NgxSpinnerService.show();
    var params = {
      constitution_gid: this.constitution_gid,
      constitution_name:this.statusFormData.txtconstitution_name,
      remarks: this.statusFormData.txtremarks,
      rbo_status: this.statusFormData.rbo_status
    }
    var url = 'ConstitutionMaster/InactiveConstitution';
    this.SocketService.post(url, params).subscribe((result: any) => {
  
      this.NgxSpinnerService.hide();
      if (result.status == true) {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
      }
      else {
        this.ToastrService.info(result.message)
        this.NgxSpinnerService.hide();
      }
      this.ngOnInit()
    })
  }
  
};

delete(constitution_gid: any) {
  this.parametervalue = constitution_gid

}

statusvalue(value: any , constitution_gid: String){
  this.status = value
  this.constitution_gid = constitution_gid
  
}
ondelete() {
  this.NgxSpinnerService.show();
  var params = {
    constitution_gid: this.parametervalue
  }
  var url = 'ConstitutionMaster/ConstitutionDelete';
  this.SocketService.getparams(url, params).subscribe((result: any) => {
    if (result.status == true) {

      this.ToastrService.success(result.message);
      this.NgxSpinnerService.hide();
      this.ngOnInit();
    }
    else {
      this.ToastrService.warning(result.message);
      this.NgxSpinnerService.hide();

    }
  });
}

sortColumn(columnKey: string): void {
  return this.SocketService.sortColumn(columnKey);
}

getSortIconClass(columnKey: string) {
  return this.SocketService.getSortIconClass(columnKey);
}

onstatusupdate(constitution_gid : String , status_flag : String){
  this.NgxSpinnerService.show();
  let params = {
    constitution_gid: constitution_gid,
    status_flag: status_flag
    
   }
   var url4 = 'ConstitutionMaster/constitutionstatusupdate'
   this.SocketService.post(url4, params).subscribe((result: any) => {

      if ( result.status == false) {
       this.ToastrService.warning(result.message)
      }
      else {
       this.ToastrService.success(result.message)
        }
        this.NgxSpinnerService.hide();
        this.GetconstitutionSummary();
       
    });
}

}


