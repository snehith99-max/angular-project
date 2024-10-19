	
import { Component, OnInit, ElementRef } from '@angular/core';
import { Router, Route } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { AES, enc } from 'crypto-js';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
 
 import { Location } from '@angular/common';
@Component({
  selector: 'app-crm-trn-tfolderview',
  templateUrl: './crm-trn-tfolderview.component.html',
  styleUrls: ['./crm-trn-tfolderview.component.scss']
})
export class CrmTrnTfolderviewComponent  implements OnInit  {
  lsdocupload_gid: any;
  lsdocupload_name: any;
  docupload_type: any;
  docupload_name: any;
  docupload_gid: any;
  breadcrumbList_list: any;
  file:any;
  crumb:any;
  AddForm!: FormGroup;
  folderprt_list: any;
  boxFlag: boolean = false;
  listFlag: boolean = true;
  cbofolder: any;
  ParameterValue: any;
  EditForm!: FormGroup;
  ParameterValue1: any;
  parametervalue1:any;
  parametervalue: any;
  file_name: any;
  fileinputvalue: string | undefined;
  fileInput!: ElementRef;
  formDataObject: FormData = new FormData();
  AutoIDkey: any;
  fileinput: any;
  breadcrumbs: any[] = [{ name: 'Home', id: null }];
  constructor(private Location:Location,private NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService, private SocketService: SocketService, public route: Router, public router: ActivatedRoute, private FormBuilder: FormBuilder) {
    this.Sample()
  }
  ngOnInit(): void {
 
    
    const secretKey = 'storyboarderp';
    const lsmonth = this.router.snapshot.paramMap.get('docupload_gid1');
    this.docupload_gid = lsmonth;
    const deencryptedParam = AES.decrypt(this.docupload_gid, secretKey).toString(enc.Utf8);
    this.docupload_gid = deencryptedParam;
 
    const lsdocupload_name = this.router.snapshot.paramMap.get('docupload_name2');
    this.docupload_name = lsdocupload_name;
    const deencryptedParam1 = AES.decrypt(this.docupload_name, secretKey).toString(enc.Utf8);
    this.docupload_name = deencryptedParam1;
    
 
    
 
    this.FolderSummary(deencryptedParam);
    this.updateBreadcrumbs();
  }
 
  Sample() {
    this.AddForm = new FormGroup({
      folder_name: new FormControl(null,
        [
          Validators.required,
          Validators.pattern(/^(?!\s*$).+/),
        ]),
    }),
 
      this.EditForm = new FormGroup({
        docupload_name: new FormControl(null,
          [
            Validators.required,
            Validators.pattern(/^(?!\s*$).+/),
          ]),
      })
  }
  FolderSummary(docupload_gid: any) {
    var param = {
      parent_directorygid: this.docupload_gid
    }
    var url = 'FileManagement/FolderDtls';
    this.NgxSpinnerService.show();
    this.SocketService.getparams(url, param).subscribe((result: any) => {
      if (result.Folder_list != null) {
        $('#folderprt_list').DataTable().destroy();
        this.folderprt_list = result.Folder_list;
        this.NgxSpinnerService.hide();
        setTimeout(() => {
          $('#folderprt_list').DataTable({
            //code by snehith for customized pagination  
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          });
        }, 1);
      }
      else {
        this.folderprt_list = result.Folder_list;
        setTimeout(() => {
          var table = $('#folderprt_list').DataTable({
            //code by snehith for customized pagination  
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          });
        }, 1);
        this.NgxSpinnerService.hide();
        $('#folderprt_list').DataTable().destroy();
        this.NgxSpinnerService.hide();
      }
 
    })
  };

  
 
  addFolder() {
 this.AddForm.reset()
  }
  download(path: string, name: string) {
    var params = {
      file_path: path,
      file_name: name
    }
    var url = 'FileManagement/DownloadDocumentazurefile';
    this.SocketService.post(url,params).subscribe((data: any) => {
      if (data != null) {
        this.filedownload(data);
      }
    });
  }
  filedownload(data: any) {
    const file_type = this.createBlobFromExtension(data.format);
    const base64Content = data.file;
 
    // Convert base64 to binary
    const binaryContent = atob(base64Content);
 
    // Create a Uint8Array from the binary content
    const uint8Array = new Uint8Array(binaryContent.length);
    for (let i = 0; i < binaryContent.length; i++) {
      uint8Array[i] = binaryContent.charCodeAt(i);
    }
    const blob = new Blob([uint8Array], { type: file_type });
    const url = window.URL.createObjectURL(blob);
      const a = document.createElement('a');
      a.href = url;
      a.download = data.name; // Specify the desired file name and extension.
      document.body.appendChild(a);
      a.click();
      window.URL.revokeObjectURL(url);
  }
  createBlobFromExtension(file_extension: any){
    const mimeTypes:{ [key: string]: string } = {
      txt: 'text/plain',
      html: 'text/html',
      css: 'text/css',
      js: 'application/javascript',
      json: 'application/json',
      xml: 'application/xml',
      pdf: 'application/pdf',
      doc: 'application/msword',
      docx: 'application/vnd.openxmlformats-officedocument.wordprocessingml.document',
      xls: 'application/vnd.ms-excel',
      xlsx: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
      ppt: 'application/vnd.ms-powerpoint',
      pptx: 'application/vnd.openxmlformats-officedocument.presentationml.presentation',
      jpeg: 'image/jpeg',
      jpg: 'image/jpeg',
      png: 'image/png',
      gif: 'image/gif',
      bmp: 'image/bmp',
      tiff: 'image/tiff',
      svg: 'image/svg+xml',
      mp3: 'audio/mpeg',
      wav: 'audio/wav',
      mp4: 'video/mp4',
      mov: 'video/quicktime',
      avi: 'video/x-msvideo',
      zip: 'application/zip',
      rar: 'application/x-rar-compressed',
      // Add more mappings as needed for other extensions
    };
    if(file_extension != undefined){
    const minetype = mimeTypes[file_extension.toLocaleLowerCase()];
    return minetype;
    }
    else {
      return
    }
  }
  
  openFile(docupload_gid: any, docupload_name: any) {
 
    if( docupload_gid !== undefined)
      {
            
        this.docupload_gid = docupload_gid;
        this.docupload_name = docupload_name;
       
   
        // Filter out entries with null name
      this.breadcrumbs = this.breadcrumbs.filter(item => item.name !== null);
   
      // Add to breadcrumbs only if docupload_name is not null
      if (docupload_name !== null) {
        this.breadcrumbs.push({ name: docupload_name, id: docupload_gid });
      }
   
     
      // this.breadcrumbs.push({ name: docupload_name, id: docupload_gid });
        var param = {
          parent_directorygid: this.docupload_gid
        }
        // console.log("Open folder:");
        // console.log(param);
   
   
        var url = 'FileManagement/FolderDtls';
        this.NgxSpinnerService.show();
        this.SocketService.getparams(url, param).subscribe((result: any) => {
          if (result.Folder_list != null) {
            $('#folderprt_list').DataTable().destroy();
            this.folderprt_list = result.Folder_list;
            this.NgxSpinnerService.hide();
            setTimeout(() => {
              $('#folderprt_list').DataTable();
            }, 1);
          }
          else {
            this.folderprt_list = result.Folder_list;
            setTimeout(() => {
              var table = $('#folderprt_list').DataTable();
            }, 1);
            this.NgxSpinnerService.hide();
            $('#folderprt_list').DataTable().destroy();
            this.NgxSpinnerService.hide();
          }
   
        })
      }
      else{
        this.route.navigate(['/crm/CrmTrnFileManagement']);
      }
 
  }
  navigateToFolder(crumb: any) {
    const index = this.breadcrumbs.indexOf(crumb);
    this.breadcrumbs = this.breadcrumbs.slice(0, index);
    this.openFile(crumb.id, crumb.name);
  }
  updateBreadcrumbs() {
    if (this.docupload_gid) {
      this.breadcrumbs = [{ name: 'Home'}, { name: this.docupload_name, id: this.docupload_gid }];
    } else {
      this.breadcrumbs = [{ href:'/crm/CrmTrnFileManagement' }];
    }
  }
  getIconClass(fileName: string): string {
    if (!fileName) {
      return 'fas fa-file text-muted'; // default icon if fileName is empty or null
    }

    const fileExtension = fileName.split('.').pop()?.toLowerCase(); // add optional chaining operator (?.)
    switch (fileExtension) {
      case 'pdf':
        return 'fas fa-file-pdf text-danger';
      case 'docx':
        return 'fas fa-file-word text-primary';
      case 'xlsx':
        return 'fas fa-file-excel text-success';
      case 'csv':
        return 'fas fa-file-csv text-info'; // added icon for CSV files
      case 'jpg':
      case 'jpeg':
      case 'png':
        return 'fas fa-file-image text-warning';
      case 'zip':
        return 'fas fa-file-archive text-secondary';
      case 'txt':
        return 'fas fa-file-text text-muted';
      case 'pptx':
        return 'fas fa-file-powerpoint text-primary';
      case 'p3':
        return 'fas fa-file-audio text-secondary';
      case 'p4':
        return 'fas fa-file-video text-secondary';
      default:
        return 'fas fa-file text-muted';
    }
  }
  back()
{
 this.Location.back()
}  downloadFile() { }
 
  onadd() {
    
    if (this.AddForm.valid) {
      const params = {
        folder_name: this.AddForm.value.folder_name,
        docupload_type: 'Folder',
        parent_gid: this.docupload_gid
      };
      this.NgxSpinnerService.show();
      var url = 'FileManagement/CreateFolder';
 
      this.SocketService.post(url, params).subscribe((result: any) => {
        this.NgxSpinnerService.hide();
 
        if (result.status == true) {
          this.ToastrService.success(result.message);
          this.AddForm.reset()
          //window.location.reload()
          this.openFile(this.docupload_gid,this.AddForm.value.folder_name);
          this.NgxSpinnerService.hide();
        } else {
          this.ToastrService.warning(result.message);
        }
 
      });
    }
  }
  editinfo(parameter: string, parameter1: string, parameter2: string, parameter3:string) {
    
    this.ParameterValue = parameter,
      this.ParameterValue1 = parameter1,
      this.parametervalue = parameter2,
      this.parametervalue1 = parameter3;
      if(this.ParameterValue1 !=""){
    this.EditForm.get("docupload_name")?.setValue(this.ParameterValue1)
  }
  else
  {
    this.EditForm.get("docupload_name")?.setValue(this.parametervalue1)
  }
 
  }
  UpdateFolder() {
    
    if (this.parametervalue == 'Folder') {
    this.NgxSpinnerService.show();
    var params = {
      docupload_gid: this.ParameterValue,
      docupload_name: this.EditForm.value.docupload_name,
    }
    var url = 'FileManagement/DocumentUpdate';
    this.SocketService.post(url, params).subscribe((result: any) => {
      if (result.status == true) {
        this.ToastrService.success(result.message);
        this.NgxSpinnerService.hide();   
        this.openFile(this.docupload_gid,this.EditForm.value.docupload_name);    
        // this.FolderSummary(this.docupload_gid);
      //  console.log(this.docupload_gid)
        //this.ngOnInit();
      }
      else {
        this.ToastrService.warning(result.message);
        this.NgxSpinnerService.hide();
      }

 
    });
  }
  else

  {
    let params = {
      docupload_gid: this.ParameterValue,
      fileupload_name: this.EditForm.value.docupload_name,
      docupload_type: this.parametervalue
    }
    const fileExtension1 = this.parametervalue1.split('.').pop().toLowerCase();
    const fileExtension2 = this.EditForm.value.docupload_name.split('.').pop().toLowerCase();
    
    if (fileExtension1 === fileExtension2) {
    var url = 'FileManagement/FileUpdate';
    this.SocketService.post(url, params).subscribe((result: any) => {
      if (result.status == true) {
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
  else
  {
    this.ToastrService.warning(`File Format Must be ${fileExtension1}`);
  }
  }
  }

  document()
  {
    this.fileinput = null;
  }
 
  delete(docupload_gid: any, docupload_type: any) {
    this.parametervalue = docupload_gid,
      this.ParameterValue = docupload_type
 
  }
  ondelete() {
    this.NgxSpinnerService.show();
    if (this.ParameterValue == 'Folder') {
      var params = {
        docupload_gid: this.parametervalue
      }
      var url = 'FileManagement/FolderDelete';
      this.SocketService.getparams(url, params).subscribe((result: any) => {
        if (result.status == true) {
 
          this.ToastrService.success(result.message);
          this.openFile(this.docupload_gid,this.AddForm.value.folder_name);
          this.NgxSpinnerService.hide();
          
        }
        else {
          this.ToastrService.warning(result.message);
          this.NgxSpinnerService.hide();
 
        }
 
      });
    }
    else {
      var params = {
        docupload_gid: this.parametervalue
      }
      var url = 'FileManagement/FileDelete';
      this.SocketService.getparams(url, params).subscribe((result: any) => {
        if (result.status == true) {
 
          this.ToastrService.success(result.message);
          this.openFile(this.docupload_gid,this.AddForm.value.folder_name);
          this.NgxSpinnerService.hide();

        }
        else {
          this.ToastrService.warning(result.message);
          this.NgxSpinnerService.hide();
 
        }
 
 
      });
    }
  };

  DownloadUploadDocAllFiles(file_list: any) {
    debugger;
    const validFiles = file_list.filter((file: any) => {
      return file.fileupload_name && file.fileupload_name !== 'undefined';
    });
  
    if (validFiles.length === 0) {
      this.ToastrService.warning('No valid files to download');
    } else {
      for (let i = 0; i < validFiles.length; i++) {
        this.download(validFiles[i].file_path, validFiles[i].fileupload_name);
      }
    }
  }

  
  addFile() {
 this.fileinput = null
  }
 
  uploadexcel() {
    
    this.AutoIDkey = this.generateKey();
    this.formDataObject = new FormData();
    const fileInput: HTMLInputElement = document.getElementById('fileInput') as HTMLInputElement;
    if (fileInput) {
      const files: FileList | null = fileInput.files;
      if (files != null && files.length != 0) {
        const allowedFormats = ['.pdf', '.docx', '.doc', '.xlsx', '.xls', '.jpg', '.jpeg', '.png', '.txt', '.pptx', '.ppt', '.bmp', '.gif', '.svg', '.tiff'];
        let invalidFiles = [];

        for (let i = 0; i < files.length; i++) {
          const fileExtension = files[i].name.substring(files[i].name.lastIndexOf('.')).toLowerCase();

          if (allowedFormats.includes(fileExtension)) {
            this.formDataObject.append('file', files[i]);
            this.file_name = files[i].name;
          } else {
            invalidFiles.push(files[i].name);
          }
        }

        if (invalidFiles.length > 0) {
          // Show error message for invalid file formats
          this.ToastrService.warning('Invalid file format(s): ' + invalidFiles.join(', ') + '. Only the following formats are allowed: ' + allowedFormats.join(', '));

        }
        else {
        // const jsonData = "" + JSON.stringify(this.documentData_list) + "";
        this.formDataObject.append('project_flag', "Default");
        this.formDataObject.append('docupload_gid', this.docupload_gid);
        this.formDataObject.append('fileinput', this.fileinput)
        this.NgxSpinnerService.show();
 
        var api = 'FileManagement/DocumentUpload'
 
        this.SocketService.postfile(api, this.formDataObject).subscribe((result: any) => {
          if (result.status == true) {
            this.ToastrService.success(result.message);
            this.openFile(this.docupload_gid,this.AddForm.value.folder_name);
          }
          else if (result.status == false) {
            this.ToastrService.warning(result.message)
          }
          else {
            this.ToastrService.warning(result.message)
          }
          this.NgxSpinnerService.hide();
       
 
        });
      }
      }
    }
    else {
      this.ToastrService.warning("Kindly Upload the Document")
    }
 
  }
  generateKey(): string {
 
    return `AutoIDKey${new Date().getTime()}`;
  };
  

  sortColumn(columnKey: string): void {
    return this.SocketService.sortColumn(columnKey);
  }

  getSortIconClass(columnKey: string) {
    return this.SocketService.getSortIconClass(columnKey);
  }
}