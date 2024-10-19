import { Component } from '@angular/core';
import { Location } from '@angular/common';
import { FormControl, FormGroup, Validators, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
@Component({
  selector: 'app-tsk-mst-task-add',
  templateUrl: './tsk-mst-task-add.component.html',
  styles: [`
  .position-relative {
    position: relative;
  }
  
  .btn-clear {
    position: absolute;
    right: 10px;
    top: 50%;
    transform: translateY(-50%);
    background: none;
    border: none;
    cursor: pointer;
    font-size: 16px;
  }
  
  .btn-clear i {
    color: #888; /* Adjust color as needed */
  }
  
  .selected-row {
  background-color: #e7f4ff; /* You can change this color as needed */
}
.selectable-name {
cursor: pointer;
transition: background-color 0.3s;
padding: 10px;

}
.contact-info {
  border-bottom: 1px solid #eee;
}

.suggested-option {
  border: 1px solid #ddd;
  background: #f9f9f9;
  border-radius: 4px;
  box-shadow: 0 2px 5px rgba(0, 0, 0, 0.15);
  z-index: 1000;
  width: 100%;

}
.selectable-name:hover{
background-color: #e7f4ff;

}

.bg-gray {
    background-color: rgb(234 237 240) !important;
  }
  .btn-darkedblue {
    background-color: #3c3e8b !important;
    color: #fff !important;
    font-size: 1.1rem !important;
    font-weight: 630 !important;
    padding: calc(0.775rem + 1px) calc(1.5rem + 1px) !important;
    border-radius: 3px;
    border: 0;
  }
  
  .btn-darkedblue:hover {
    background-color: #3d3f6c !important;
  }
  .btn-darkedblue:disabled {
    background-color: #cccccc !important;
    cursor: not-allowed !important;
  }
  `
  ]
})
export class TskMstTaskAddComponent {
  Team_list: any;
  duplicate:boolean=false;
  customer_list: any;
  notshow:boolean = false;
  filelist: any;
  AutoIDkey: any;
  suggestedContacts: any[] = [];
  filesWithId: { file: File; AutoIDkey: string; file_name: string }[] = [];
  formDataObject: FormData = new FormData();
  DocumentForm: FormGroup | any;
  document_list: any[] = [];
  file_name: any;
  addresult: any;
  remainingChars: number=200;
  change: any;
  f :number=0;
  menulevel: any;
  Show: boolean=false;
  hide: boolean=true;
  taskchar: number =250;
  txttask_titles: any;
  tasklist:any
  estimated_hours: any;
  module_name: any;
  task_typename: any;
  severity_name: any;
  remarks: any;
  functionality_name: any;
  duplicateErrorMsg: string ='';
  functionality_gid: any;
    constructor(public router: Router, private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService, private SocketService: SocketService,
    private Location: Location, public FormBuilder: FormBuilder
  ) {
    this.createform()
    this.doucument()
  }
  doucument() {
    this.DocumentForm = this.FormBuilder.group({
      cbocamDocument: ['', Validators.required],
      documentmom: ['', Validators.required]
    });
  }
  createform() {
    this.AddForm = this.FormBuilder.group({
      text_module: [null, Validators.required],
      txt_severity: [null, Validators.required],
      text_functionality: [{ value: null, disabled: true }, Validators.required],
      // text_functionality: [null, Validators.required],
      text_functionalitys: ['', [Validators.required, Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]],
      txttask_title: ['', [Validators.required, Validators.pattern(/^(?!\s*$).+/)]],
      txttask_type: [null, Validators.required],
      txtestimated_hrs: ['', [Validators.required, Validators.pattern(/^(?!\s*$).+/)]],
      txtremarks: [''],
    })
  }
  checklist: any;
  AddForm!: FormGroup | any;
  check_list: any[] = [];
  Task_type = [
    { Tasktype_name: 'Bug Fixing', Tasktype_gid: 'TTYG_001' },
    { Tasktype_name: 'Development', Tasktype_gid: 'TTYG_002' },
    { Tasktype_name: 'Change Request', Tasktype_gid: 'TTYG_003' },
  ];
  Severity = [
    { Severity_name: 'Show Stopper', Severity_gid: 'SEVT_001' },
    { Severity_name: 'Critical Mandatory', Severity_gid: 'SEVT_002' },
    { Severity_name: 'Critical Non-Mandatory', Severity_gid: 'SEVT_003' },
    { Severity_name: 'Nice To Have', Severity_gid: 'SEVT_004' },

  ];
  ngOnInit() {
    var url = 'TskTrnTaskManagement/modulelist';
    this.SocketService.get(url).subscribe((result: any) => {
      this.Team_list = result.moduledropdown_list;
    });
    this.NgxSpinnerService.show();
    var url = 'TskTrnTaskManagement/tasklist';
    this.SocketService.get(url).subscribe((result: any) => {
    this.tasklist=result.taskdetail_list 
      this.NgxSpinnerService.hide();
    });
  }
  secondlevelmenu(){debugger
    if (this.AddForm.value.text_module.teamname_gid == null || this.AddForm.value.text_module.teamname_gid == "") {
      this.AddForm.get('text_functionality').setValue(null);
      this.AddForm.get('text_functionality').disable();
      this.AddForm.get('text_functionalitys').setValidators( [Validators.required, Validators.pattern(/^(\S+\s*)*(?!\s).*$/)]);
      this.AddForm.get('text_functionalitys').updateValueAndValidity();
      this.AddForm.get('text_functionality').reset();
      this.AddForm.get('text_functionality').clearValidators();
      this.AddForm.get('text_functionality').updateValueAndValidity();
      this.NgxSpinnerService.show()
      this.Show=true
      this.hide=false
      setTimeout(() => {
        this.NgxSpinnerService.hide()
      }, 500);


    }
    else {
      this.AddForm.get('text_functionality').setValue(null);
      this.AddForm.get('text_functionality').enable();
      this.AddForm.get('text_functionality').setValidators( [Validators.required]);
      this.AddForm.get('text_functionality').updateValueAndValidity();
      this.AddForm.get('text_functionalitys').reset();
      this.AddForm.get('text_functionalitys').clearValidators();
      this.AddForm.get('text_functionalitys').updateValueAndValidity();
      this.hide=true
      this.Show=false

    var params={
      module_gid:this.AddForm.value.text_module.teamname_gid
    }
    this.NgxSpinnerService.show()
    var url = 'TskTrnTaskManagement/leveltwo_menu';
    this.SocketService.getparams(url,params).subscribe((result: any) => {
      this.menulevel = result.menulevel;
      this.NgxSpinnerService.hide()
    });
  }
}
  // checklistClick() {
  //   const params = {
  //     subtask_name: this.checklist,

  //   }
  //   this.check_list.push({
  //     subtask_name: this.checklist,
  //   });
  //   this.checklist = null;
  // }
  delete(index: any) {
    if (index >= 0 && index < this.check_list.length) {
      this.check_list.splice(index, 1);
    }
  }
  backbutton() {
    this.Location.back()
  }
  submit() {
    debugger;
    let functionality_name = '';
    let functionality_gid = '';
  
    if (this.Show) {
      functionality_name = this.AddForm.value.text_functionalitys;
      functionality_gid = ''; // Adjust this if there's a corresponding gid for the input field
    } else if (this.hide) {
      functionality_name = this.AddForm.value.text_functionality.module_name;
      functionality_gid = this.AddForm.value.text_functionality.module_gid;
    }
    var params = {
      module_gid: this.AddForm.value.text_module.team_gid,
      module_name: this.AddForm.value.text_module.team_name,
      module_name_gid:this.AddForm.value.text_module.teamname_gid,
      functionality_name: functionality_name,
      functionality_gid: functionality_gid,
      // functionality_name: this.AddForm.value.text_functionality.module_name,
      // functionality_gid:this.AddForm.value.text_functionality.module_gid,
      task_name: this.AddForm.value.txttask_title,
      task_typename: this.AddForm.value.txttask_type.Tasktype_name,
      task_typegid: this.AddForm.value.txttask_type.Tasktype_gid,
      severity_name: this.AddForm.value.txt_severity.Severity_name,
      severity_gid: this.AddForm.value.txt_severity.Severity_gid,
      estimated_hours: this.AddForm.value.txtestimated_hrs == undefined ? "" : this.AddForm.value.txtestimated_hrs,
      remark: this.AddForm.value.txtremarks == undefined ? "" : this.AddForm.value.txtremarks,
    };

    this.filelist = "";
    for (let i = 0; i < this.document_list.length; i++) {
      this.filelist = this.filelist + this.document_list[i].document_name + "+";
    }

    this.NgxSpinnerService.show();

    var url = 'TskTrnTaskManagement/Taskadd';
    this.SocketService.post(url, params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      if (result.status == true) {
        if (this.document_list != null && this.document_list.length > 0) {
          const jsonData = JSON.stringify(this.document_list);
          this.formDataObject.append('task_gid', result.task_gid);
          this.formDataObject.append('project_flag', "Default");
          this.formDataObject.append('documentData_list', jsonData);
          this.formDataObject.append('filelist', this.filelist);

          var api = 'TskTrnTaskManagement/addtaskdocument';
          this.SocketService.postfile(api, this.formDataObject).subscribe((docResult: any) => {
          });
        } 
        if ((this.document_list != null && this.document_list.length > 0)) {
      }
      this.NgxSpinnerService.hide()
        this.ToastrService.success("Task Added successfully");
        this.router.navigate(['/ITS/ItsMstTaskCreation']);

    }
    else {
      this.NgxSpinnerService.hide();
      this.ToastrService.warning(result.message);
      this.router.navigate(['/ITS/ItsMstTaskCreation']);

    }
  });   
  }
  
  DocumentClick1() {
    const fileInput: HTMLInputElement = document.getElementById('fileInput') as HTMLInputElement;

    if (fileInput) {
      const files: FileList | null = fileInput.files;

      if (files != null && files.length != 0) {
        for (let i = 0; i < files.length; i++) {
          const file = files[i];
          this.AutoIDkey = this.generateKey() + this.document_list.length + 1;
          this.formDataObject.append(this.AutoIDkey, file);
          this.file_name = file.name;
          this.document_list.push({
            AutoID_Key: this.AutoIDkey,
            document_name: this.DocumentForm.value.cbocamDocument,
            file_name: file.name
          });
          this.filesWithId.push({
            file,
            AutoIDkey: this.AutoIDkey,
            file_name: file.name
          });
        }
        fileInput.value = '';
        this.DocumentForm.reset();
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
    }
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
  next() {
    let functionality_name = '';
    let functionality_gid = '';
  
    if (this.Show) {
      functionality_name = this.AddForm.value.text_functionalitys;
      functionality_gid = ''; // Adjust this if there's a corresponding gid for the input field
    } else if (this.hide) {
      functionality_name = this.AddForm.value.text_functionality.module_name;
      functionality_gid = this.AddForm.value.text_functionality.module_gid;
    }
    var params = {
      module_gid: this.AddForm.value.text_module.team_gid,
      module_name: this.AddForm.value.text_module.team_name,
      module_name_gid:this.AddForm.value.text_module.teamname_gid,
      functionality_name: functionality_name,
      functionality_gid: functionality_gid,
      // functionality_name: this.AddForm.value.text_functionality.module_name,
      // functionality_gid:this.AddForm.value.text_functionality.module_gid,
      task_name: this.AddForm.value.txttask_title,
      task_typename: this.AddForm.value.txttask_type.Tasktype_name,
      task_typegid: this.AddForm.value.txttask_type.Tasktype_gid,
      severity_name: this.AddForm.value.txt_severity.Severity_name,
      severity_gid: this.AddForm.value.txt_severity.Severity_gid,
      estimated_hours: this.AddForm.value.txtestimated_hrs == undefined ? "" : this.AddForm.value.txtestimated_hrs,
      remark: this.AddForm.value.txtremarks == undefined ? "" : this.AddForm.value.txtremarks,
    };

    this.filelist = "";
    for (let i = 0; i < this.document_list.length; i++) {
      this.filelist = this.filelist + this.document_list[i].document_name + "+";
    }

    this.NgxSpinnerService.show();

    var url = 'TskTrnTaskManagement/Taskadd';
    this.SocketService.post(url, params).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      if (result.status == true) {
        if (this.document_list != null && this.document_list.length > 0) {
          const jsonData = JSON.stringify(this.document_list);
          this.formDataObject.append('task_gid', result.task_gid);
          this.formDataObject.append('project_flag', "Default");
          this.formDataObject.append('documentData_list', jsonData);
          this.formDataObject.append('filelist', this.filelist);

          var api = 'TskTrnTaskManagement/addtaskdocument';
          this.SocketService.postfile(api, this.formDataObject).subscribe((docResult: any) => {
          });
        }
        if ((this.document_list != null && this.document_list.length > 0)) {
        }
        this.NgxSpinnerService.hide()
        window.scrollTo({
                  top: 0,
                });
          this.ToastrService.success("Task Added successfully");  
      }
      else {
        this.ToastrService.warning(result.message);
        // Do not navigate to another page here
      }
      if (this.Show) {
        this.AddForm.reset({
          text_module: {
            team_gid: this.AddForm.value.text_module.team_gid,
            team_name: this.AddForm.value.text_module.team_name,
            teamname_gid: this.AddForm.value.text_module.teamname_gid,
          },
          text_functionalitys: this.AddForm.value.text_functionalitys,
        });
      } else if (this.hide) {
        this.AddForm.reset({
          text_module: {
            team_gid: this.AddForm.value.text_module.team_gid,
            team_name: this.AddForm.value.text_module.team_name,
            teamname_gid: this.AddForm.value.text_module.teamname_gid,
          },
          text_functionality: {
            module_name: this.AddForm.value.text_functionality.module_name,
            module_gid: this.AddForm.value.text_functionality.module_gid,
          },
        });
      }
      this.document_list = [];
      this.remainingChars=200
      this.taskchar=250
      this.ngOnInit()
    });
}
updateRemainingCharsadd() {
  this.remainingChars = 200 - this.AddForm.value.txtremarks.length;
}
RemainingCharsadd() {
  this.taskchar = 250 - this.AddForm.value.txttask_title.length;
}
onSearch() {
  debugger;
  const query = this.AddForm.value.txttask_title?.trim().toLowerCase();;

//  const query =this.AddForm.value.txttask_title;
  if (query) {
    this.suggestedContacts = this.tasklist.filter((task_name: { task_name: string }) =>
    task_name.task_name.toLowerCase().includes(query)
    );
  } else {
    this.suggestedContacts = [];
  }
}
checkduplicate() {debugger
  const trimmedTaskName = this.txttask_titles.trim();
  if (!trimmedTaskName) {
    this.duplicate = false;
    this.duplicateErrorMsg = '';
    return;
  }

  const param = { taskname: this.txttask_titles };
  const url = 'TskTrnTaskManagement/check';

  this.SocketService.getparams(url, param).subscribe({
    next: (result: any) => {
      if (result.status === false) {
        this.duplicate = true;
        this.duplicateErrorMsg = 'Task Title already exists.';
      } else {
        this.duplicate = false;
        this.duplicateErrorMsg = '';
      }
    },
  });
}
// checkduplicate(){
//   let param={
//     taskname:this.txttask_titles
//   }
//   var url = 'TskTrnTaskManagement/check';
//     this.f=0;
//     this.SocketService.getparams(url, param).subscribe((result: any) => {
//        if(result.status==false){
//           this.duplicate=true
//           this.duplicateErrorMsg = 'This task title already exists.'; 
//           this.f=1;
//        }
//        else{
//         this.duplicate=false
//         this.duplicateErrorMsg = ''; 
//        }
//     });


// }
binddata(task_name :any){debugger
 this.txttask_titles=task_name;
 this.suggestedContacts = [];
 this.notshow = true;
 this.checkduplicate()
 if(this.f==0){
 
 }
 this.anchorbuyerdata()
}
 
anchorbuyerdata(){
  if(this.f==0){
    debugger;
    var params = {
      taskname: this.txttask_titles
  }
  this.AddForm.get('text_functionality').enable();

  this.NgxSpinnerService.show();
  var url = 'TskTrnTaskManagement/taskbind';
  this.SocketService.getparams(url, params).subscribe((result: any) => {
    this.NgxSpinnerService.hide();
    debugger;
    this.taskchar = 250 - this.AddForm.value.txttask_title.length;
    this.estimated_hours = result.estimated_hours;
    this.module_name = result.module_name;
    this.task_typename=result.task_typename;
    this.severity_name=result.severity_name;
    this.remarks=result.remarks;
    this.functionality_gid=result.functionality_gid
    if(this.functionality_gid ==''){
this.Show=true
this.hide=false
    }
    else{
      this.Show=false
      this.hide=true
    }
    this.functionality_name=result.functionality_name;
    // this.txt_contact_person=result.contact_person_name;
    this.document_list=result.documentdata_list;
    this.remainingChars = 200 - this.remarks.length;

  });
 
  }
}
clearInput(){debugger
  this.AddForm.get('txttask_title')?.reset('');
  this.AddForm.get('txtremarks')?.reset('');
  this.AddForm.get('txttask_title')?.markAsTouched();
  this.AddForm.get('text_functionality').disable();
this.document_list=[]
  this.txttask_titles='';
  this.suggestedContacts = [];
  this.notshow = false;
this.remarks=null
this.severity_name=null
this.functionality_name=null
this.module_name=null
this.estimated_hours=''
this.task_typename=null
this.taskchar = 250
this.remainingChars = 200
this.duplicateErrorMsg=''
}
}

