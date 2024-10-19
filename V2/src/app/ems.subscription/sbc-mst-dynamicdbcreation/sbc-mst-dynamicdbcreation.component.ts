import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, MaxLengthValidator, Validators,FormArray } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { AES } from 'crypto-js';
import { ExcelService } from 'src/app/Service/excel.service';

@Component({
  selector: 'app-sbc-mst-dynamicdbcreation',
  templateUrl: './sbc-mst-dynamicdbcreation.component.html',
  styleUrls: ['./sbc-mst-dynamicdbcreation.component.scss']
})
export class SbcMstDynamicdbcreationComponent {
  reactiveform: FormGroup | any;
  deletedb: FormGroup | any;
  parameterValue1:any;
  module_list:any;
  server_list:any;
  productlists:any;
  responsedata:any;
  cboproductmodule:any;
  mdlproduct_type:any;
  mdlmodule_name:any;
  dynamicdb_list:any[]=[];
  parameterValue:any;
  constructor(private excelService : ExcelService,private SocketService: SocketService,private fb: FormBuilder,private route: ActivatedRoute,private router: Router,private service: SocketService,private ToastrService: ToastrService,private NgxSpinnerService: NgxSpinnerService) {
    this.reactiveform = new FormGroup({

      module_name:new FormControl('', [Validators.required]),
      database_name:new FormControl('', [Validators.required]),
      server_name:new FormControl('', [Validators.required]),
      productmodule_name: new FormControl('', [Validators.required]),

    })
    this.deletedb = new FormGroup({
      database_name: new FormControl(''),

    })
  }
  ngOnInit(): void {
    this.  GetdynamicdbSummary();
    var api = 'Dynamicdb/GetModuledropdown';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.module_list = this.responsedata.Modulelists;
    });
    var api = 'Scriptmanagement/Getserverdropdown';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.server_list = this.responsedata.serverlists;
    });
    var api = 'Productmodule/GetproductmoduleSummary';
    this.service.get(api).subscribe((result: any) => {
      this.responsedata = result;
      this.productlists = this.responsedata.productlists;
    });
  }
  get database_name() {
    return this.reactiveform.get('database_name')!;
  }
  get server_name() {
    return this.reactiveform.get('server_name')!;
  }
  get module_name() {
    return this.reactiveform.get('module_name')!;
  }
  get productmodule_name() {
    return this.reactiveform.get('productmodule_name')!;
  }
 
  GetdynamicdbSummary() {
    var url = 'Dynamicdb/GetdynamicdbSummary'
    this.NgxSpinnerService.show()
    this.service.get(url).subscribe((result: any) => {
      $('#dynamicdb_list').DataTable().destroy();
      this.responsedata = result;
      this.dynamicdb_list = this.responsedata.dynamicdblists;
      this.NgxSpinnerService.hide()
      setTimeout(() => {
        $('#dynamicdb_list').DataTable();
      }, 1);
    })
  }
  onsubmit(){

    let params={
      database_name:this.reactiveform.value.database_name,
      server_name:this.reactiveform.value.server_name,
      module_name: this.reactiveform.value.module_name,
      productmodule_name: this.reactiveform.value.productmodule_name
    }
      
    var url = 'Dynamicdb/InternalDynamicDBcreationInSQLFiles';
    this.NgxSpinnerService.show();

    this.SocketService.post(url, params).subscribe((result:any) => {
      if(result.status == true){
        this.NgxSpinnerService.hide();
        this.ToastrService.success("Database created Successfully");
        this.reactiveform.reset();
      }
      else {
            
        this.ToastrService.warning("Error Occured while database!");

        this.NgxSpinnerService.hide();
        this.reactiveform.reset();         
      }
      this.ngOnInit();
    })        
  }
  onexceptionview(params:any){
    debugger
    const secretKey = 'storyboarderp';
    const param = (params);
    const encryptedParam = AES.encrypt(param, secretKey).toString();
    this.router.navigate(['/sbc/SbcMstDynamicdbexceptionerrorview', encryptedParam])
  }
  get db_name() {
    return this.deletedb.get('database_name')!;
  }
  ondelete(){
    var url = 'Dynamicdb/DeleteDatabase';
    debugger
    let params = {
      database_name:this.deletedb.value.database_name,
       server_gid : this.parameterValue,
       dynamicdbscriptmanagement_gid : this.parameterValue1,

    
    }
    this.SocketService.postparams(url, params).subscribe((result:any) => {
    if(result.status == true){
    this.ToastrService.success(result.message);
    this.GetdynamicdbSummary();
    }
    else {
    this.ToastrService.warning(result.message);
    this.GetdynamicdbSummary();
    }  
    })
    }
  openModaldelete(parameter: string,parameter1:string) {
    debugger;
    this.parameterValue = parameter,
    this.parameterValue1 = parameter1

  }
  exportExcel():void{
    const dynamicdbcreation = this.dynamicdb_list.map(item => ({
      ServerName: item.server_name || '', 
      ModuleName: item.module_name || '', 
      DatabaseName: item.company_code || '', 
      Createdby: item.created_by || '', 
      Createddate: item.created_date || '', 
    }));
    this.excelService.exportAsExcelFile(dynamicdbcreation, 'DynamicDB_creation');
  }
}
