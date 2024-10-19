import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-hrm-mst-hrdocument',
  templateUrl: './hrm-mst-hrdocument.component.html',
  styleUrls: ['./hrm-mst-hrdocument.component.scss']
})

export class HrmMstHrdocumentComponent {
  hrdocument_data : any;
  parametervalue:any;
  hrdocumentinactivelog_data : any; 
  AddForm!: FormGroup; 
  EditForm!: FormGroup; 
  StatusForm!: FormGroup;
  hrdocument_gid = "";
  addhrDocumentFormData  = { 
    txthrdocument_name: '',
  };
  edithrDocumentFormData = {
    txtedithrdocument_name: '',
  };
  statushrDocumentFormData = {
    txthrdocument_name: '',
    rbo_status: '',
    txtremarks: ''  
  };

  constructor(public router:Router, private ToastrService: ToastrService,private NgxSpinnerService: NgxSpinnerService, private service: SocketService, public FormBuilder: FormBuilder){
    this.createForm(); 
    this.createEditForm();
    this.createStatusForm();
  }

  createForm() {
    this.AddForm = this.FormBuilder.group({txthrdocument_name: ['', [Validators.required,Validators.pattern("^(?!\s*$).+")]],});
  }
  createEditForm() {
    this.EditForm = this.FormBuilder.group({txtedithrdocument_name: ['', [Validators.required,Validators.pattern("^(?!\s*$).+")]],});
  }
  createStatusForm(){
    this.StatusForm = this.FormBuilder.group({
      txtremarks: new FormControl('',[Validators.required, Validators.pattern(/^(?!\s*$).+/)]),
      rbo_status: [''] 
    });
  }

  clearForm() {
    this.AddForm.reset(); 
  }
  
  ngOnInit() {
    var url = 'HRDocument/GetSysHRDocument';
    this.NgxSpinnerService.show();
    this.service.get(url).subscribe((result: any) => { 
    if(result.hrdocument_list != null){
      $('#HRDocumenttable').DataTable().destroy();
      this.hrdocument_data= result.hrdocument_list;  
      this.NgxSpinnerService.hide();
      setTimeout(()=>{   
        $('#HRDocumenttable').DataTable();
      }, 1);
    }
    else{
      this.hrdocument_data= result.hrdocument_list; 
      setTimeout(()=>{   
        $('#HRDocumenttable').DataTable();
      }, 1);
      this.NgxSpinnerService.hide();
      $('#HRDocumenttable').DataTable().destroy();
    } 
  }); 
}

// Add Popup Event
 openhrdocumentpopup(){
   this.clearForm();
 }
  addhrDocument(){ 
    if (this.AddForm.valid) {
      var params = {
        hrdocument_name: this.addhrDocumentFormData.txthrdocument_name,

    }  
    this.NgxSpinnerService.show();
    var url = 'HRDocument/CreateSysHRDocument';
    this.service.post(url, params).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
        this.ngOnInit();
      }
      else {
        this.ToastrService.warning(result.message)
        this.NgxSpinnerService.hide();   
      }
    }) 
    } 
  } 

  // Edit Popup Event

  edithrdocument(hrdocument_gid: any){
    this.hrdocument_gid = hrdocument_gid;
    this.EditForm.reset();
    var params = {
      hrdocument_gid: hrdocument_gid
    } 
  this.NgxSpinnerService.show();
  var url = 'HRDocument/EditSysHRDocument';
  this.service.getparams(url, params).subscribe((result: any) => {
    this.edithrDocumentFormData.txtedithrdocument_name = result.hrdocument_name;
    this.NgxSpinnerService.hide(); 
  }) 
  }

  UpdatehrDocument(){ 
    var params = {
      hrdocument_name: this.edithrDocumentFormData.txtedithrdocument_name,
      hrdocument_gid: this.hrdocument_gid
  }  
  this.NgxSpinnerService.show();
  var url = 'HRDocument/UpdateSysHRDocument';
  this.service.post(url, params).subscribe((result: any) => {
    if (result.status == true) {
      this.ToastrService.success(result.message)
      this.NgxSpinnerService.hide();
      this.ngOnInit();
    }
    else {
      this.ToastrService.warning(result.message)
      this.NgxSpinnerService.hide();   
    }
  }) 
  }

// Status Update 

Status_Click(hrdocument_gid: any){
  this.NgxSpinnerService.show();
  this.hrdocument_gid = hrdocument_gid
  this.StatusForm.reset();
  var params = {
    hrdocument_gid: this.hrdocument_gid
  }  
  var url = 'HRDocument/EditSysHRDocument';
  this.service.getparams(url, params).subscribe((result: any) => {
    this.statushrDocumentFormData.txthrdocument_name = result.hrdocument_name;
    this.statushrDocumentFormData.rbo_status = result.Status; 
    this.NgxSpinnerService.hide(); 
  }) 
  this.NgxSpinnerService.show();
  var url = 'HRDocument/InactiveSysHRDocumentHistory';
  this.service.getparams(url, params).subscribe((result: any) => {
    this.hrdocumentinactivelog_data = result.hrdocumentinactivehistory_list; 
    this.NgxSpinnerService.hide(); 
  }) 

}

update_status(){
  var params = {
    hrdocument_gid: this.hrdocument_gid,
    hrdocument_name: this.statushrDocumentFormData.txthrdocument_name,
    remarks: this.statushrDocumentFormData.txtremarks,
    rbo_status: this.statushrDocumentFormData.rbo_status 
  } 
this.NgxSpinnerService.show();
var url = 'HRDocument/InactiveSysHRDocument';
this.service.post(url, params).subscribe((result: any) => {
  if (result.status == true) {
    this.ToastrService.success(result.message)
    this.NgxSpinnerService.hide();
    this.ngOnInit();
  }
  else {
    this.ToastrService.warning(result.message)
    this.NgxSpinnerService.hide();   
  }
}) 
}

// Delete 
delete(parameter: any){
  this.parametervalue = parameter
}
ondelete(){
  this.NgxSpinnerService.show();
  var params = {
    hrdocument_gid:this. parametervalue
  }   
  var url = 'HRDocument/DeleteSysHRDocument';
  this.service.getparams(url, params).subscribe((result: any) => {
    if (result.status == true) {
      this.ToastrService.success(result.message)
      this.NgxSpinnerService.hide();
      this.ngOnInit();
    }
    else {
      this.ToastrService.warning(result.message)
      this.NgxSpinnerService.hide();   
    }
  }) 
}

close(){
  window.location.reload();
}

 
}



