import { Component, ElementRef, Renderer2 } from '@angular/core';
import { FormControl, FormGroup, PatternValidator, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { NgSelectModule } from '@ng-select/ng-select';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { AES, enc } from 'crypto-js';
import { saveAs } from 'file-saver';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from 'src/environments/environment';
import { dE } from '@fullcalendar/core/internal-common';


interface Idocument {
  case_gid: string;
}

@Component({
  selector: 'app-lgl-mst-casemanagement-upload',
  templateUrl: './lgl-mst-casemanagement-upload.component.html',
  styleUrls: ['./lgl-mst-casemanagement-upload.component.scss']
})
export class LglMstCasemanagementUploadComponent {
  file!: File;
  data: any;
  document!: Idocument;
  case_gid: any;
  reactiveForm!: FormGroup;
  document_list: any[] = [];
  getcasestage_list: any[] = [];
  docprovider_list: any[] = [];
  files: Array<{ name: string, size: number,doc_gid: string,file: File, blobUrl: string }> = [];
  filesWithId: { file: File; doc_gid: string; doc_attpath: string }[] = [];
  responsedata: any;
  GetDocumentType: any;
  postproposal_list :any;
  sample: any;
  template :any;
  GetViewDocument_list : any;
  formDataObject: FormData = new FormData();
  allattchement: any[] = [];
  parameterValue: any;
  institutegid: any;
  parameterValueReset: any;
  casestagename:any;
  provider:any;
  casegid:any;
  constructor(private renderer: Renderer2, public NgxSpinnerService:NgxSpinnerService,private el: ElementRef, public service: SocketService, private ToastrService: ToastrService, private route: Router, private router: ActivatedRoute) {
    this.document = {} as Idocument;
  }
  ngOnInit(): void {
    this.reactiveForm = new FormGroup({
      case_document: new FormControl(null),
      doc_name: new FormControl(null, [Validators.required,]),
      casestagename: new FormControl(null, [Validators.required,]),
      provider: new FormControl(null, [Validators.required,]),
      uploaded_by: new FormControl(null, [Validators.required,]),
      case_remarks:new FormControl('')
      
    });
    debugger;
    this.NgxSpinnerService.show()
    const case_gid = this.router.snapshot.paramMap.get('case_gid1');
    this.case_gid=case_gid;
    const secretKey = 'storyboarderp';
    const deencryptedParam = AES.decrypt(this.case_gid, secretKey).toString(enc.Utf8);
    console.log(deencryptedParam)
    this.casegid=deencryptedParam
    this.GetDocumentSummary(this.casegid)
    var api = 'CaseManagement/Getcasestage';
    this.service.get(api).subscribe((result: any) => {
      this.getcasestage_list = result.getcasestage_list;
    });

    var api1 = 'CaseManagement/Getdocprovider';
    this.service.get(api1).subscribe((result: any) => {
      this.docprovider_list = result.docprovider_list;
    });

    this.NgxSpinnerService.hide()
   
    
  }
  GetDocumentSummary(case_gid:string) {
    debugger
    var params = {
      case_gid : case_gid 
    }
    var url = 'CaseManagement/GetDocumentupload'
    this.service.getparams(url,params).subscribe((result: any) => {
      this.responsedata = result;
      this.GetViewDocument_list = this.responsedata.GetViewDocument_list;
    });
 }
   
  get case_document() {
    return this.reactiveForm.get('case_document')!;
  }
  get doc_name() {
    return this.reactiveForm.get('doc_name')!;
  }
  get casestage_name() {
    return this.reactiveForm.get('casestage_name')!;
  }

  onSubmit() {
   debugger
   console.log(this.reactiveForm)
   let formData = new FormData();
   if (this.file != null && this.file != undefined) {
   formData.append("file", this.file, this.file.name);
   formData.append("case_document",this.reactiveForm.value.case_document);
   formData.append("doc_name",this.reactiveForm.value.doc_name);
   formData.append("casestage_name",this.reactiveForm.value.casestagename.casestage_name);
   formData.append("doc_provider",this.reactiveForm.value.provider.doc_provider);
   formData.append("uploaded_by",this.reactiveForm.value.uploaded_by);
   formData.append("remarks",this.reactiveForm.value.case_remarks);
   formData.append("case_gid",this.casegid);
    }
    var api = 'CaseManagement/PostCaseDoc';
    this.NgxSpinnerService.show()
    this.service.postfile(api, formData).subscribe((result: any) => {
      debugger
      if (result.status == true) {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide()
      }
      else {
        this.ToastrService.warning(result.message) 
        this.NgxSpinnerService.hide()
      }
      this.reactiveForm.get("case_document")?.setValue(null)
      this.reactiveForm.get("doc_name")?.setValue(null)
      this.reactiveForm.get("casestage_name")?.setValue(null)
      this.reactiveForm.get("doc_provider")?.setValue(null)
      this.reactiveForm.get("uploaded_by")?.setValue(null)
      this.reactiveForm.get("remarks")?.setValue(null)
      window.location.reload();
    });
  
  }
  onChange2(event:any) {
    this.file =event.target.files[0];

}
// viewFile(doc_path: string, doc_name: string, doc_extension: string) {
//   debugger
//   const link = document.createElement('a');
//   link.href = environment.URL_FILEPATH + encodeURIComponent(doc_path) + encodeURIComponent(doc_extension);
//   link.download = doc_name + doc_extension; // Set the download attribute
//   link.target = '_blank'; // Open in a new tab
//   document.body.appendChild(link); 
//   link.click(); 
//   document.body.removeChild(link); 
// }


// viewFile(doc_path: string, doc_name: string, doc_extension: string) {
//   if (doc_path != null && doc_path != "") {
//       saveAs(doc_path);  
//   }
//   else {
//     window.scrollTo({
//       top: 0, // Code is used for scroll top after event done
//     });
//     this.ToastrService.warning('No Image Found')

//   }

// }


viewFile(data: any) {
  if (data.doc_path != null && data.doc_path != "") {
    const newTab = window.open(data.doc_path, '_blank');
    if (newTab === null) {
      window.scrollTo({
        top: 0,
      });
      this.ToastrService.error('Unable to open file. Please check your browser settings or allow popups.');
    }
  } else {
    window.scrollTo({
      top: 0,
    });
    this.ToastrService.warning('File path is not available.');
  }
}




// viewFile(data: any) {
//   debugger
//   const file = data.doc_path;
//   const contentType = "application/pdf";
//   if (contentType) {
//     const blob = new Blob([file], { type: contentType });
//     const fileUrl = URL.createObjectURL(blob);
//     const newTab = window.open(fileUrl, '_blank');

//     if (newTab) {
//       newTab.focus();
//     }
//   }

// }

openModaldelete(parameter:string)
    {
      this.parameterValue = parameter
    }
    ondelete()
    {
      debugger
      var url = 'CaseManagement/Deletedocument'
      this.NgxSpinnerService.show()
      let param = {
        doc_gid: this.parameterValue
      }
      this.service.getparams(url, param).subscribe((result: any) => {
        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.GetDocumentSummary(this.casegid);
          this.NgxSpinnerService.hide()
        }
        else {
  
          this.ToastrService.success(result.message)
          this.GetDocumentSummary(this.casegid);
          this.NgxSpinnerService.hide()

        }
       
      });
    }
    // getFileContentType(file: File): string | null {
    //   const lowerCaseFileName = file.name.toLowerCase();
    
    //   if (lowerCaseFileName.endsWith('.pdf')) {
    //     return 'application/pdf';
    //   } else if (lowerCaseFileName.endsWith('.jpg') || lowerCaseFileName.endsWith('.jpeg')) {
    //     return 'image/jpeg';
    //   } else if (lowerCaseFileName.endsWith('.png')) {
    //     return 'image/png';
    //   } else if (lowerCaseFileName.endsWith('.txt')) {
    //     return 'text/plain';
    //   }
    
    //   return null;
    // }
}