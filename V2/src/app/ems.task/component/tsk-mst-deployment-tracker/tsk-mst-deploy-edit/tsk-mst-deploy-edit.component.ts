import { Component } from '@angular/core';
import { Location } from '@angular/common';
import { FormControl, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from 'src/environments/environment';
import { AES, enc } from 'crypto-js';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-tsk-mst-deploy-edit',
  templateUrl: './tsk-mst-deploy-edit.component.html',
  styleUrls: ['./tsk-mst-deploy-edit.component.scss']
})
export class TskMstDeployEditComponent {
  Team_list: any[]=[];
  filteredTeamList:any[]=[]
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
  EditForm!: FormGroup | any;
  deployment_trackergid: any;
  Doc_summary: any;
  duplicate:boolean=false;
  duplicateErrorMsg: string ='';
  version_no:any
  script_gid: any;
  constructor(private ActivatedRoute:ActivatedRoute,public router: Router, private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService, private SocketService: SocketService,
    private Location: Location, public FormBuilder: FormBuilder
  ) {
this.createform()
  }
  createform() {
    this.EditForm = this.FormBuilder.group({
      txtedit_script: [null, Validators.required],
      txt_editroutes: [null, Validators.required],
      txt_editdep: [null,Validators.required],
      txt_editversion: [null,Validators.required],
      txtmodule_editdep: [null, Validators.required],
      txtedit_approval: [null, Validators.required],
      txt_editdescription: ['', [Validators.required, Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]],
      txt_editfile: [null, [Validators.required]],
      txt_editnewdll: [null, Validators.required],
      txtedit_version_no: ['', [Validators.required, Validators.pattern(/^(?!\s*$).+/)]],
      txt_editmodule: [null,[Validators.required]],
    })
  }
  checkduplicate(){
    const param = { 
      version_number: this.version_no ,
      deployment_trackergid:this.deployment_trackergid
    };
    const url = 'TskTrnTaskManagement/versioneditcheck';
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
ngOnInit(){
  this.ActivatedRoute.queryParams.subscribe(params => {
    const urlparams = params['hash'];  
    if (urlparams) {
      const decryptedParam = AES.decrypt(urlparams, environment.secretKey).toString(enc.Utf8);
      const paramvalues = decryptedParam.split('&');
      this.deployment_trackergid = paramvalues[0];
    }
  });
  this.NgxSpinnerService.show();
  var params = {
    deployment_trackergid: this.deployment_trackergid,
  }; 
  var url = 'TskTrnTaskManagement/deployview';
  this.SocketService.getparams(url,params).subscribe((result: any) => { 
    this.EditForm.get('txt_editfile').setValue({file_name : result.file_name, file_gid : result.file_gid});
    this.EditForm.get('txt_editmodule').setValue(result.deploy_module);
    this.EditForm.get('txt_editversion').setValue({version_name:result.version});
    this.EditForm.get('txtedit_version_no').setValue(result.version_number);
    this.EditForm.get('txt_editnewdll').setValue(result.dll_status);
    this.EditForm.get('txt_editdep').setValue(result.dependency_status);
    this.EditForm.get('txt_editroutes').setValue(result.routes_status);
    this.EditForm.get('txtedit_script').setValue(result.script_status);
    this.EditForm.get('txtedit_approval').setValue(result.approval_name);
    this.remarks=result.description
    // this.EditForm.get('txt_editdescription').setValue(result.description);
    this.EditForm.get('txtmodule_editdep').setValue(result.deploydependcy_module);
    this.Doc_summary = result.scriptattach_file || [];
    if(result.dependency_status == 'Yes'){
       this.approves=true
    }
    else{
      this.approves=false
      this.EditForm.get('txtmodule_editdep').reset();
      this.EditForm.get('txtmodule_editdep').clearValidators();
      this.EditForm.get('txtmodule_editdep').updateValueAndValidity();
      this.EditForm.get('txtedit_approval').reset();
      this.EditForm.get('txtedit_approval').clearValidators();
      this.EditForm.get('txtedit_approval').updateValueAndValidity();
    }
    this.attachshow = result.script_status === 'Yes'; // Change condition as per your requirement
    // this.approves = result.dependency_status === 'Yes';
    this.NgxSpinnerService.hide();
  });
  this.EditForm.get('txt_editmodule')?.valueChanges.subscribe((selectedModules: any[]) => {
    this.filterDependentModules(selectedModules);
  });
  var url = 'TskMstCustomer/TeamSummary';
  this.SocketService.get(url).subscribe((result: any) => {
    this.Team_list = result.team_list;
    this.NgxSpinnerService.hide();
  });

}
filterDependentModules(selectedModules: any[]) {debugger
  if (selectedModules) {
    this.EditForm.get('txtmodule_editdep')?.reset();
    this.filteredTeamList = this.Team_list.filter(
      (item) => !selectedModules.some((selected: any) => selected.team_name === item.team_name)
    );
  } else {
    this.filteredTeamList = [...this.Team_list];
  }
}
viewFiles(path:string, name:string){
  debugger;
  const lowerCaseFileName = name.toLowerCase();
  if (!(lowerCaseFileName.endsWith('.pdf') ||
    lowerCaseFileName.endsWith('.jpg') ||
    lowerCaseFileName.endsWith('.jpeg') ||
    lowerCaseFileName.endsWith('.png') ||
    lowerCaseFileName.endsWith('.txt')||
    lowerCaseFileName.endsWith('.bmp'))) {
    window.scrollTo({
      top: 0,
    });
    this.ToastrService.warning('File Format Not Supported');
  }
  else {
      var params = {
        file_path: path,
        file_name: name
      }
      var url = 'TskTrnTaskManagement/DownloadDocument';
      this.SocketService.post(url, params).subscribe((result: any) => {        
          if (result != null) {
          this.SocketService.fileviewer(result);
        }
      });
  }
}
downloadFile(path:string, file_name:string) {debugger
  var params = {
    file_path : path,
    file_name : file_name
  }
  var url = 'TskTrnTaskManagement/DownloadDocument';
  this.SocketService.post(url, params).subscribe((result: any) => {
  // this.SocketService.downloadFile(params).subscribe((data:any) => {
    if(result != null){
      this.SocketService.filedownload1(result);
    }
  });
}
backbutton(){
  this.Location.back()
}
updateRemainingCharsadd() {debugger
  this.remainingChars = 1000 - this.EditForm.value.txt_editdescription.length;
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
            docupload_type: file.name,
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
scriptno(){debugger
  this.attachshow=false
  if (this.document_list && this.document_list.length == 0) {
    this.document_list = [];
  }
}
approveyes(){
  this.approves=true
    this.EditForm.get('txtedit_approval').setValidators( [Validators.required, Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]);
    this.EditForm.get('txtedit_approval').updateValueAndValidity();
    this.EditForm.get('txtmodule_editdep').setValidators( [Validators.required]);
    this.EditForm.get('txtmodule_editdep').updateValueAndValidity();
}
approveno(){debugger
  this.approves=false
  if (!this.EditForm.get('txtmodule_editdep').value) {
    this.EditForm.get('txtmodule_editdep').reset();
  }

  this.EditForm.get('txtmodule_editdep').clearValidators();
  this.EditForm.get('txtmodule_editdep').updateValueAndValidity();

  // Only reset if no values exist or if explicitly required
  if (!this.EditForm.get('txtedit_approval').value) {
    this.EditForm.get('txtedit_approval').reset();
  }

  // this.EditForm.get('txtmodule_editdep').reset();
  // this.EditForm.get('txtmodule_editdep').clearValidators();
  // this.EditForm.get('txtmodule_editdep').updateValueAndValidity();
  // this.EditForm.get('txtedit_approval').reset();
  this.EditForm.get('txtedit_approval').clearValidators();
  this.EditForm.get('txtedit_approval').updateValueAndValidity();
}
update(){debugger
  let modulelist: string | null = '';
  let aprroval='';
  if (this.EditForm.value.txt_editdep === 'Yes') {
    modulelist = this.EditForm.value.txtmodule_editdep;
    aprroval=this.EditForm.value.txtedit_approval
  } else {
    modulelist = null;
    aprroval=""
  }
  var params = {
    deployment_trackergid:this.deployment_trackergid,
    version:this.EditForm.value.txt_editversion.version_name,
    version_number: this.EditForm.value.txtedit_version_no,
    file_name: this.EditForm.value.txt_editfile.file_name,
    file_gid: this.EditForm.value.txt_editfile.file_gid,
    dll_status: this.EditForm.value.txt_editnewdll,
    dependency_status: this.EditForm.value.txt_editdep,
    routes_status: this.EditForm.value.txt_editroutes,
    description: this.EditForm.value.txt_editdescription,
    script_status: this.EditForm.value.txtedit_script,
    depmodule_list:modulelist,
    module_list3:this.EditForm.value.txt_editmodule,
    // dependency_module: this.EditForm.value.txtmodule_dep == undefined ? "" : this.EditForm.value.txtmodule_dep.team_name,
    // dependency_module_gid: this.EditForm.value.txtmodule_dep == undefined ? "" : this.EditForm.value.txtmodule_dep.team_gid,
    approval_name: aprroval,
  };
  this.filelist = "";
  for (let i = 0; i < this.document_list.length; i++) {
    this.filelist = this.filelist + this.document_list[i].document_name + "+";
  }
  this.NgxSpinnerService.show();
  var url = 'TskTrnTaskManagement/Deployeupdate';
  this.SocketService.post(url, params).subscribe((result: any) => {
    this.NgxSpinnerService.hide();
    if (result.status == true) {
      if (this.document_list != null && this.document_list.length > 0) {
        const jsonData = JSON.stringify(this.document_list);
        this.formDataObject.append('deployment_trackergid', this.deployment_trackergid);
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
      this.ToastrService.success("Deployment Tracker Updated Successfully");
      this.router.navigate(['/ITS/ItsMstDeploymentTracker']);
  }
  else {
    this.NgxSpinnerService.hide();
    this.ToastrService.warning(result.message);
    this.router.navigate(['/ITS/ItsMstDeploymentTracker']);
  }
});   
}
delete(script_gid: any) {debugger
  this.script_gid = script_gid

}
ondelete() {
  this.NgxSpinnerService.show();
  var params = {
    script_gid: this.script_gid
  }
  var url = 'TskTrnTaskManagement/scriptdelete';
  this.SocketService.getparams(url, params).subscribe((result: any) => {
    if (result.status == true) {

      this.ToastrService.success(result.message);
      this.NgxSpinnerService.hide();
      this.ngOnInit();
      window.scrollTo({
        top: 0,
      });
    }
    else {
      this.ToastrService.warning(result.message);
      this.NgxSpinnerService.hide();

    }

  });
}
}
