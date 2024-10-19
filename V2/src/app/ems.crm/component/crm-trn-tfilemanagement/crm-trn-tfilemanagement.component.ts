import { Component, OnInit, ElementRef } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { AnyCatcher } from 'rxjs/internal/AnyCatcher';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';

interface IFileManagement {
  folder_name: string;
  file_gid: string;

}

@Component({
  selector: 'app-crm-trn-tfilemanagement',
  templateUrl: './crm-trn-tfilemanagement.component.html',
  styleUrls: ['./crm-trn-tfilemanagement.component.scss']
})
export class CrmTrnTfilemanagementComponent implements OnInit {
  fileInput!: ElementRef;
  formDataObject: FormData = new FormData();
  AutoIDkey: any;
  UploadForm!: FormGroup;
  file_name: any;
  fileinputvalue: string | undefined;
  AddForm!: FormGroup;
  folder_list: any[] = [];
  boxFlag: boolean = false;
  listFlag: boolean = true;
  cbofolder: any;
  ParameterValue: any;
  EditForm!: FormGroup;
  ParameterValue1: any;
  docupload_gid: any;
  docupload_name: any;
  docupload_type: any;
  parametervalue: any;
  ParameterValue2: any;
  fileinputs: any;
  folderList: any[] = [];
  currentPath: string = "Home"
  isBoxView: boolean = true;
  addFolderForm!: FormGroup;
  responsedata: any;
  breadcrumbs: any[] = [{ name: this.currentPath, id: null }];
  constructor(private NgxSpinnerService: NgxSpinnerService, private ToastrService: ToastrService, private SocketService: SocketService, public router: Router, private FormBuilder: FormBuilder) {
    this.Sample();
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
      }),

      this.UploadForm = new FormGroup({
        docupload_name: new FormControl(null,
          [
            Validators.required,
          ]),
      })
  }
  ngOnInit() {
    var url = 'FileManagement/DocumentUploadSummary';
    this.NgxSpinnerService.show();
    this.SocketService.get(url).subscribe((result: any) => {
      $('#folder_lists').DataTable().destroy();
      this.responsedata = result;
      this.folder_list = this.responsedata.DocumentUploadlist_list;
      this.NgxSpinnerService.hide();
      setTimeout(() => {
        $('#folder_lists').DataTable(
          {
            //code by snehith for customized pagination  
            "pageLength": 50, // Number of rows to display per page
            "lengthMenu": [50, 100, 200, 500], // Dropdown to change page length
          }
        );
      }, 1);
    });

  }


  addFolder() {
    this.AddForm.reset()
  }
  openFile(docupload_gid: any, docupload_name: any) {
    this.docupload_gid = docupload_gid;
    this.docupload_name = docupload_name;

    this.breadcrumbs.push({ name: docupload_name, id: docupload_gid });
    const secretKey = 'storyboarderp';
    const docupload_gid1 = AES.encrypt(this.docupload_gid, secretKey).toString();
    const docupload_name2 = AES.encrypt(this.docupload_name, secretKey).toString();



    this.router.navigate(['/crm/CrmTrnFolderview', docupload_gid1, docupload_name2]);
  }
  downloadFile() { }
  navigateToFolder(crumb: any) {
    const index = this.breadcrumbs.indexOf(crumb);
    this.breadcrumbs = this.breadcrumbs.slice(0, index);
    this.openFile(crumb.id, crumb.name);
  }
  updateBreadcrumbs() {
    if (this.docupload_gid) {
      this.breadcrumbs = [{ name: this.docupload_name, id: this.docupload_gid }];
    } else {
      this.breadcrumbs = [{ name: this.currentPath, id: this.docupload_gid }];
    }
  }
  onadd() {
    if (this.AddForm.valid) {
      const params = {
        folder_name: this.AddForm.value.folder_name,
        docupload_type: 'Folder',
        parent_gid: '$'
      };
      this.NgxSpinnerService.show();
      var url = 'FileManagement/CreateFolder';

      this.SocketService.post(url, params).subscribe((result: any) => {
        this.NgxSpinnerService.hide();

        if (result.status == true) {
          this.ToastrService.success(result.message);
          this.AddForm.reset()
          this.ngOnInit()
          this.NgxSpinnerService.hide();

        } else {
          this.ToastrService.warning(result.message);
        }

      });
    }
  }
  editinfo(parameter: string, parameter1: string, parameter2: string, parameter3: string) {
    this.ParameterValue = parameter,
      this.ParameterValue1 = parameter1,
      this.parametervalue = parameter2,
      this.ParameterValue2 = parameter3;
    if (this.ParameterValue1 != "") {
      this.EditForm.get("docupload_name")?.setValue(this.ParameterValue1)
    }
    else {
      this.EditForm.get("docupload_name")?.setValue(this.ParameterValue2)
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
          this.ngOnInit();
        }
        else {
          this.ToastrService.warning(result.message);
          this.NgxSpinnerService.hide();
        }

      })
    }
    else {
      let params = {
        docupload_gid: this.ParameterValue,
        fileupload_name: this.EditForm.value.docupload_name,

      }
      const fileExtension1 = this.ParameterValue2.split('.').pop().toLowerCase();
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
      else {
        this.ToastrService.warning(`File Format Must be ${fileExtension1}`);
      }
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



  download(path: string, name: string) {
    var params = {
      file_path: path,
      file_name: name
    }
    var url = 'FileManagement/DownloadDocumentazurefile';
    this.SocketService.post(url, params).subscribe((data: any) => {
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
  createBlobFromExtension(file_extension: any) {
    const mimeTypes: { [key: string]: string } = {
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
    if (file_extension != undefined) {
      const minetype = mimeTypes[file_extension.toLocaleLowerCase()];
      return minetype;
    }
    else {
      return
    }
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
          this.NgxSpinnerService.hide();
          this.ngOnInit();
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
          this.NgxSpinnerService.hide();
          this.ngOnInit();
        }
        else {
          this.ToastrService.warning(result.message);
          this.NgxSpinnerService.hide();

        }


      });
    }
  };

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
  document() {
    this.fileinputs = null;
  }
  addFile() {
    this.fileinputs = null;
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
          this.formDataObject.append('project_flag', "Default");
          this.formDataObject.append('docupload_gid', "");
          this.formDataObject.append('docupload_name', this.fileinputs);
          this.NgxSpinnerService.show();

        var api = 'FileManagement/DocumentUpload'

        this.SocketService.postfile(api, this.formDataObject).subscribe((result: any) => {
          if (result.status == true) {
            this.ToastrService.success(result.message);
          }
          else if (result.status == false) {
            this.ToastrService.warning(result.message)
          }
          else {
            this.ToastrService.warning(result.message)
          }
          this.NgxSpinnerService.hide();
          this.ngOnInit();

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
  }

  sortColumn(columnKey: string): void {
    return this.SocketService.sortColumn(columnKey);
  }

  getSortIconClass(columnKey: string) {
    return this.SocketService.getSortIconClass(columnKey);
  }
}