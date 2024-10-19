import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
interface Icase {
  casetype_code: string;
  casetype_name:string;
}
@Component({
  selector: 'app-law-mst-casetype',
  templateUrl: './law-mst-casetype.component.html',
  styleUrls: ['./law-mst-casetype.component.scss']
})
export class LawMstCasetypeComponent {

  case!: Icase;
  caseList: any;
  reactiveForm: FormGroup|any;
  reactiveFormEdit:FormGroup | any;
  parameterValue:any
  parameterValue1:any;
  responsedata: any;
  casetypecode:any;
  casetypename:any;
  constructor(private NgxSpinnerService: NgxSpinnerService, private SocketService: SocketService, private ToastrService: ToastrService) {
    this.case = {} as Icase;

  }
  ngOnInit(): void {
    this.GetcasetypeSummary();
   this.reactiveForm = new FormGroup({
     casetypecode: new FormControl(null, [Validators.required, Validators.pattern("^(?!\s*$).+")]),
     casetypename: new FormControl(null,[Validators.required, Validators.pattern("^(?!\s*$).+")]),
   });
   this.reactiveFormEdit = new FormGroup({
     casetypecode: new FormControl({ value: null, disabled: true }),    
     casetype_name: new FormControl(null,[Validators.required, Validators.pattern("^(?!\s*$).+")]),
     casetype_gid: new FormControl(''),
    
  });
  }
  GetcasetypeSummary(){
    debugger
    this.NgxSpinnerService.show();
    var url= 'LawMstCasetype/Getcasetypesummary';
    this.SocketService.get(url).subscribe((result:any)=>{
      console.log(result.case_list);
      if(result.case_list != null){
        $('#casetypeSummary').DataTable().destroy();
        this.caseList = result.case_list;  
        this.NgxSpinnerService.hide();
        setTimeout(()=>{   
          $('#casetypeSummary').DataTable();
        }, 1);
      }
      else{
        this.caseList = result.case_list; 
        setTimeout(()=>{   
          $('#casetypeSummary').DataTable();
        }, 1);
        this.NgxSpinnerService.hide();
        $('#casetypeSummary').DataTable().destroy();
      } 
    });
  }
  get casetype_name() {
    return this.reactiveForm.get('casetype_name')!;
  }
  get casetype_code() {
    return this.reactiveForm.get('casetype_code')!;
  }
  get created_by() {
    return this.reactiveForm.get('created_by')!;
  }

  sortColumn(columnKey: string): void {
    return this.SocketService.sortColumn(columnKey);
  }

  getSortIconClass(columnKey: string) {
    return this.SocketService.getSortIconClass(columnKey);
  }
  addcase(){
    debugger;
    var params = {
      casetype_name: this.reactiveForm.value.casetypename,
      casetype_code: this.reactiveForm.value.casetypecode,
    }
          var url = 'LawMstCasetype/PostcasetypeAdd'
          this.NgxSpinnerService.show()
          this.SocketService.postparams(url, params).subscribe((result: any) => {
            if (result.status == false) {
              this.ToastrService.warning(result.message)
              this.GetcasetypeSummary();  
              this.reactiveForm.reset();
              this.NgxSpinnerService.hide()
            }
            else {
              this.ToastrService.success(result.message)
              this.GetcasetypeSummary();
              this.reactiveForm.reset();
              this.NgxSpinnerService.hide()
             
            }
          });
  }
  onclose(){

  }
  editcase(parameter:any){
    debugger
    this.parameterValue1 = parameter
    this.reactiveFormEdit.get("casetype_name")?.setValue(this.parameterValue1.casetype_name);
    this.reactiveFormEdit.get("casetypecode")?.setValue(this.parameterValue1.casetype_code);
    this.reactiveFormEdit.get("casetype_gid")?.setValue(this.parameterValue1.casetype_gid);
  }
  onupdate(){
      for (const control of Object.keys(this.reactiveFormEdit.controls)) {
        this.reactiveFormEdit.controls[control].markAsTouched();
      }
      debugger
      this.reactiveFormEdit.value;
      var url1 = 'LawMstCasetype/PostUpdatecasetype'
      this.SocketService.postparams(url1, this.reactiveFormEdit.value).subscribe((result: any) => {
        this.responsedata=result;
        if(result.status ==false){
          this.ToastrService.warning(result.message)
          this.GetcasetypeSummary();
        }
        else{
          this.ToastrService.success(result.message)
          this.GetcasetypeSummary();
        }
    }); 
  }
  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }
  ondelete() {
    debugger
    var url = 'LawMstCasetype/GetDeletecasetype'
    let param = {
      casetype_gid : this.parameterValue 
    }
    this.SocketService.getparams(url, param).subscribe((result: any)=> {
      if(result.status ==false){
        this.ToastrService.warning(result.message);
        this.GetcasetypeSummary();
      }
      else{
        
        this.ToastrService.success(result.message);
        this.GetcasetypeSummary();
        
      }
      
      this.GetcasetypeSummary();
     
  
    });
  }
}

