import { Component } from '@angular/core';
import { Location } from '@angular/common';
import { FormControl, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-tsk-mst-deploy-add',
  templateUrl: './tsk-mst-deploy-add.component.html',
  styleUrls: ['./tsk-mst-deploy-add.component.scss']
})
export class TskMstDeployAddComponent {
  Team_list: any[]=[];
  filteredTeamList: any[] = [];

  version_list=[{
    version_name:'Angular'
  },{
    version_name:'Framework'
  }]
  file_list=[{
    file_name:'UI',file_gid:'FILG001'
  },{
    file_name:'Api',file_gid:'FILG002'
  },{
    file_name:'Both UI and API',file_gid:'FILG003'
  }]
  duplicate:boolean=false;
  duplicateErrorMsg: string ='';
  remarks: any;
  filelist: any;
  document_list: any[] = [];
  remainingChars:number=1000
  filesWithId: { file: File; AutoIDkey: string; file_name: string }[] = [];
  formDataObject: FormData = new FormData();
  AutoIDkey: any;
  file_name: any;
  attachshow: boolean=false;
  Document:any
  approves: boolean=false;
  AddForm!: FormGroup | any;
  version_no: any;
  constructor(public router: Router, private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService, private SocketService: SocketService,
    private Location: Location, public FormBuilder: FormBuilder
  ) {
this.createform()
  }
  createform() {
    this.AddForm = this.FormBuilder.group({
      txtscript: [null, Validators.required],
      txt_routes: [null, Validators.required],
      txt_dep: [null,Validators.required],
      txt_version: [null,Validators.required],
      txtmodule_dep: [null, Validators.required],
      txtapproval: [null, Validators.required],
      txt_description: ['', [Validators.required, Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]],
      txt_file: [null, [Validators.required]],
      txt_newdll: [null, Validators.required],
      txtversion_no: ['', [Validators.required, Validators.pattern(/^(?!\s*$).+/)]],
      txt_module: [null,[Validators.required]],
    })
  }
ngOnInit(){
  this.NgxSpinnerService.show()
  this.AddForm.get('txt_module')?.valueChanges.subscribe((selectedModules: any[]) => {
    this.filterDependentModules(selectedModules);
  });
  var url = 'TskMstCustomer/TeamSummary';
  this.SocketService.get(url).subscribe((result: any) => {
    this.Team_list = result.team_list;
    this.filteredTeamList = [...this.Team_list]
    this.NgxSpinnerService.hide()

  });
}
checkduplicate(){
  const param = { 
    version_number: this.version_no 
  };
  const url = 'TskTrnTaskManagement/versioncheck';
  this.SocketService.getparams(url, param).subscribe({
    next: (result: any) => {
      if (result.status === false) {
        this.duplicate = true;
        this.duplicateErrorMsg = 'Version.No already exists.';
      } else {
        this.duplicate = false;
        this.duplicateErrorMsg = '';
      }
    },
  });
}
filterDependentModules(selectedModules: any[]) {
  if (selectedModules) {
    this.AddForm.get('txtmodule_dep')?.reset();
    this.filteredTeamList = this.Team_list.filter(
      (item) => !selectedModules.some((selected: any) => selected.team_name === item.team_name)
    );
  } else {
    this.filteredTeamList = [...this.Team_list];
  }
}
backbutton(){
  this.Location.back()
}
updateRemainingCharsadd() {
  this.remainingChars = 1000 - this.remarks.length;
}
downloadFiles(AutoIDkey: string, file_name: string): void {debugger
  const fileObject = this.filesWithId.find((fileObj) => fileObj.AutoIDkey === AutoIDkey);

  if (fileObject) {
    const file = fileObject.file;
    const fileUrl = URL.createObjectURL(file);
    const a = document.createElement('a');
    a.href = fileUrl;
    a.download = file_name;
    a.click();
    URL.revokeObjectURL(fileUrl);
  } else {
    // Handle the case where the file object is not found
    console.error('File not found for AutoIDkey:', AutoIDkey);
  }
}
viewFile(AutoIDkey: string): void {debugger

  const fileObject = this.filesWithId.find((obj) => obj.AutoIDkey === AutoIDkey);
  if (fileObject) {
    const file = fileObject.file;
    const contentType = this.getFileContentType(file);

    if (contentType) {
      const blob = new Blob([file], { type: contentType });
      const fileUrl = URL.createObjectURL(blob);
      const newTab = window.open(fileUrl, '_blank');

      if (newTab) {
        newTab.focus();
      }

      setTimeout(() => {
        URL.revokeObjectURL(fileUrl);
      }, 60000);
    } else {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.warning('File format is not Supported..!');
    }
  } else {
    console.error('File not found for AutoIDkey:', AutoIDkey);
  }
}


getFileContentType(file: File): string | null {
  const lowerCaseFileName = file.name.toLowerCase();

  if (lowerCaseFileName.endsWith('.pdf')) {
    return 'application/pdf';
  } else if (lowerCaseFileName.endsWith('.jpg') || lowerCaseFileName.endsWith('.jpeg')) {
    return 'image/jpeg';
  } else if (lowerCaseFileName.endsWith('.png')) {
    return 'image/png';
  } else if (lowerCaseFileName.endsWith('.txt')) {
    return 'text/plain';
  }

  return null;
}
DocumentClick1() {
  const fileInput: HTMLInputElement = document.getElementById('fileInput') as HTMLInputElement;
  if (fileInput) {
    const files: FileList | null = fileInput.files;
    if (files && files.length > 0) {
      for (let i = 0; i < files.length; i++) {
        const file = files[i];
        if (file.name.toLowerCase().endsWith('.txt')) {
          this.AutoIDkey = this.generateKey() + this.document_list.length + 1;
          this.formDataObject.append(this.AutoIDkey, file);
          this.file_name = file.name;
          this.document_list.push({
            AutoID_Key: this.AutoIDkey,
            file_name: file.name,
          });
          this.filesWithId.push({
            file,
            AutoIDkey: this.AutoIDkey,
            file_name: file.name,
          });
        } else {
          window.scrollTo({
            top: 0,
          });
          this.ToastrService.error('Only .txt format is allowed. Please upload a valid .txt file.');
        }
      }
      fileInput.value = '';
      this.Document = null; 
    } else {
      this.ToastrService.warning("Kindly Upload the Document");
    }
  }
}

generateKey(): string {
  return `AutoIDKey${new Date().getTime()}`;
}
DeleteDocumentClick(index: any) {
  if (index >= 0 && index < this.document_list.length) {
    this.document_list.splice(index, 1);
    this.ToastrService.success("Document Deleted Succesfully")
  }
}
scriptyes(){
  this.attachshow=true
}
scriptno(){
  this.attachshow=false
  this.document_list=[]
}
approveyes(){
  this.approves=true
    this.AddForm.get('txtapproval').setValidators( [Validators.required, Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]);
    this.AddForm.get('txtapproval').updateValueAndValidity();
    this.AddForm.get('txtmodule_dep').setValidators( [Validators.required]);
    this.AddForm.get('txtmodule_dep').updateValueAndValidity();
}
approveno(){
  this.approves=false
  this.AddForm.get('txtmodule_dep').reset();
  this.AddForm.get('txtmodule_dep').clearValidators();
  this.AddForm.get('txtmodule_dep').updateValueAndValidity();
  this.AddForm.get('txtapproval').reset();
  this.AddForm.get('txtapproval').clearValidators();
  this.AddForm.get('txtapproval').updateValueAndValidity();
}
submit(){
  var params = {
    // module_gid: this.AddForm.value.txt_module.team_gid,
    // module_name: this.AddForm.value.txt_module.team_name,
    version:this.AddForm.value.txt_version.version_name,
    version_number: this.AddForm.value.txtversion_no,
    file_name: this.AddForm.value.txt_file.file_name,
    file_gid: this.AddForm.value.txt_file.file_gid,
    dll_status: this.AddForm.value.txt_newdll,
    dependency_status: this.AddForm.value.txt_dep,
    routes_status: this.AddForm.value.txt_routes,
    description: this.AddForm.value.txt_description,
    script_status: this.AddForm.value.txtscript,
    depmodule_list:this.AddForm.value.txtmodule_dep,
    module_list3:this.AddForm.value.txt_module,
    // dependency_module: this.AddForm.value.txtmodule_dep == undefined ? "" : this.AddForm.value.txtmodule_dep.team_name,
    // dependency_module_gid: this.AddForm.value.txtmodule_dep == undefined ? "" : this.AddForm.value.txtmodule_dep.team_gid,
    approval_name: this.AddForm.value.txtapproval == undefined ? "" : this.AddForm.value.txtapproval,
  };
  this.filelist = "";
  for (let i = 0; i < this.document_list.length; i++) {
    this.filelist = this.filelist + this.document_list[i].document_name + "+";
  }
  this.NgxSpinnerService.show();
  var url = 'TskTrnTaskManagement/Deployeadd';
  this.SocketService.post(url, params).subscribe((result: any) => {
    this.NgxSpinnerService.hide();
    if (result.status == true) {
      if (this.document_list != null && this.document_list.length > 0) {
        const jsonData = JSON.stringify(this.document_list);
        this.formDataObject.append('deployment_trackergid', result.deployment_trackergid);
        this.formDataObject.append('project_flag', "Default");
        this.formDataObject.append('documentData_list', jsonData);
        this.formDataObject.append('filelist', this.filelist);
        var api = 'TskTrnTaskManagement/addscript';
        this.SocketService.postfile(api, this.formDataObject).subscribe((docResult: any) => {
        });
      } 
      if ((this.document_list != null && this.document_list.length > 0)) {
    }
    this.NgxSpinnerService.hide()
      this.ToastrService.success("Deployment Tracker Submitted Successfully");
      this.router.navigate(['/ITS/ItsMstDeploymentTracker']);
  }
  else {
    this.NgxSpinnerService.hide();
    this.ToastrService.warning(result.message);
    this.router.navigate(['/ITS/ItsMstDeploymentTracker']);
  }
});   
}
}
