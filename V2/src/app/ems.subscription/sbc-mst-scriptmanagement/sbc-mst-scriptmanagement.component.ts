import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, MaxLengthValidator, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES } from 'crypto-js';
import { ExcelService } from 'src/app/Service/excel.service';
interface Iscriptmanagement {
  
}

@Component({
  selector: 'app-sbc-mst-scriptmanagement',
  templateUrl: './sbc-mst-scriptmanagement.component.html',
  styleUrls: ['./sbc-mst-scriptmanagement.component.scss']
})
export class SbcMstScriptmanagementComponent {
  db_list:any;
  reactiveform: FormGroup | any;
  file!: File;
  responsedata:any;
  fileNames: string[] = [];
  cboselectedcompany:any;
  server_list:any;
  hosting_list:any[]=[];
  formDataObject: FormData = new FormData();
  mdlserver_name:any;
  serverdb_list:any;
  scriptmanagement!:Iscriptmanagement;

  constructor(private excelService : ExcelService,private fb: FormBuilder,private route: ActivatedRoute,private router: Router,private service: SocketService,private ToastrService: ToastrService,private NgxSpinnerService: NgxSpinnerService) {
    this.reactiveform = new FormGroup({
      server_name:new FormControl('', [Validators.required]),
      company_code:new FormControl('', [Validators.required]),
      fName: new FormControl('', [Validators.required]),
      filename_status: new FormControl('', [Validators.required]),
    })
     this.scriptmanagement = {} as Iscriptmanagement;
  } 
 
  ngOnInit(): void {
    this.GetScriptsummary();
    var api = 'Scriptmanagement/Getdatabasenamedropdown';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.db_list = this.responsedata.serverlists;
    });
    var api = 'Scriptmanagement/Getserverdropdown';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.server_list = this.responsedata.serverlists;
    });
  }
  GetScriptsummary() {
    var url = 'Scriptmanagement/GetScriptSummary'
    this.NgxSpinnerService.show()
    this.service.get(url).subscribe((result: any) => {
      $('#hosting_list').DataTable().destroy();
      this.responsedata = result;
      this.hosting_list = this.responsedata.serverlists;
      this.NgxSpinnerService.hide()
      setTimeout(() => {
        $('#hosting_list').DataTable();
      }, 1);


    })


  }
  get fName() {
    return this.reactiveform.get('fName')!;
  }
  get server_name() {
    return this.reactiveform.get('server_name')!;
  }
  get company_code() {
    return this.reactiveform.get('company_code')!;
  }
  get filename_status() {
    return this.reactiveform.get('filename_status')!;
  }
  // onChange2(event: Event) {
  //   const input = event.target as HTMLInputElement;
  //   if (input.files) {
  //     this.fileNames = Array.from(input.files).map(file => file.name);
  //   }
  // }
  onChange2(event:any) {
    this.file =event.target.files[0];

}

onChangeserver(selectedServer: any)
  {
    let server_gid = selectedServer?.server_gid;
        let params={
      server_gid:server_gid,
    }
    var api = 'Scriptmanagement/Getserverdatabasenamedropdown';
    this.service.getparams(api,params).subscribe((result: any) => {
      this.responsedata = result;
      this.serverdb_list = this.responsedata.serverlists;
      this.reactiveform.get('company_code').reset();
    });
}
  onsubmit(){
    let formData = new FormData();
    if (this.fileNames != null && this.fileNames != undefined) {
      formData.append("file", this.file, this.file.name);
      formData.append("server_name",this.reactiveform.value.server_name);
      formData.append("company_code",this.reactiveform.value.company_code);
      formData.append("filename_status",this.reactiveform.value.filename_status);
      formData.append("project_flag","Y");

       }
      //  for (const control of Object.keys(this.reactiveform.controls)) {
      //   this.reactiveform.controls[control].markAsTouched();
      // }
      // this.reactiveform.value;

    var api = 'SubTrnSubscrition/ScriptDocumentUpload';
     this.NgxSpinnerService.show()
        this.service.post(api, formData).subscribe((result: any) => {
          if (result.status == false) {
            this.ToastrService.warning(result.message);
            this.NgxSpinnerService.hide()
          }
          else {
            this.ToastrService.success(result.message);
            this.NgxSpinnerService.hide()
            this.GetScriptsummary();
          }
          this.responsedata = result;
        });
  }
  filedownload(file_path:any,file_name:any){
    debugger
    if(file_path != null){
      let params={file_path:file_path,file_name:file_name}
      this.service.post('azurestorage/DownloadDocument',params).subscribe((result:any)=>{
        if(result.status==true){
          this.service.filedownload1(result)
        }
      })
      // this.service.downloadfile(file_path,"subscription");
    }
    else{
      this.ToastrService.warning("No file has been uploaded for this order");
    }
  }
  onview(params:any){
    debugger
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/sbc/SbcMstScriptmanagementview', encryptedParam])
  }
  onexceptionview(params:any){
    debugger
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/sbc/SbcMstScriptmanagementexceptionview', encryptedParam])
  }
  exportExcel() :void {
        

    const scriptmanagement = this.hosting_list.map(item => ({
      ServerName: item.server_name || '', 
      DatabaseName: item.company_code || '',
      UploadedFile: item.file_name || '',
      Createdby: item.created_by || '',
      Createddate: item.created_date || '',      
    }));           
          this.excelService.exportAsExcelFile(scriptmanagement, 'Script_Management');
  }
}
