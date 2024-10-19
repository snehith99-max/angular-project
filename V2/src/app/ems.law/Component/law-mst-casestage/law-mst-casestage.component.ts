import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
interface Icase {
  casestage_code: string;
  casestage_name:string;
}
@Component({
  selector: 'app-law-mst-casestage',
  templateUrl: './law-mst-casestage.component.html',
  styleUrls: ['./law-mst-casestage.component.scss']
})
export class LawMstCasestageComponent {

  case!: Icase;
  caseList: any;
  reactiveForm: FormGroup|any;
  reactiveFormEdit:FormGroup | any;
  parameterValue:any
  parameterValue1:any;
  responsedata: any;
  casestagecode:any;
  casestagename:any;
  constructor(private NgxSpinnerService: NgxSpinnerService, private SocketService: SocketService, private ToastrService: ToastrService) {
    this.case = {} as Icase;

  }
  ngOnInit(): void {
    this.GetcasestageSummary();
   this.reactiveForm = new FormGroup({
     casestagecode: new FormControl(null, [Validators.required, Validators.pattern("^(?!\s*$).+")]),
     casestagename: new FormControl(null,[Validators.required, Validators.pattern("^(?!\s*$).+")]),
   });
   this.reactiveFormEdit = new FormGroup({
     casestagecode: new FormControl({ value: null, disabled: true }),    
     casestage_name: new FormControl(null,[Validators.required, Validators.pattern("^(?!\s*$).+")]),
     casestage_gid: new FormControl(''),
    
  });
  }
  GetcasestageSummary(){
    debugger
    this.NgxSpinnerService.show();
    var url= 'LawMstCasestage/Getcasestagesummary';
    this.SocketService.get(url).subscribe((result:any)=>{
      console.log(result.casestage_list);
      if(result.casestage_list != null){
        $('#casestageSummary').DataTable().destroy();
        this.caseList = result.casestage_list;  
        this.NgxSpinnerService.hide();
        setTimeout(()=>{   
          $('#casestageSummary').DataTable();
        }, 1);
      }
      else{
        this.caseList = result.case_list; 
        setTimeout(()=>{   
          $('#casestageSummary').DataTable();
        }, 1);
        this.NgxSpinnerService.hide();
        $('#casestageSummary').DataTable().destroy();
      } 
    });
  }
  get casestage_name() {
    return this.reactiveForm.get('casestage_name')!;
  }
  get casestage_code() {
    return this.reactiveForm.get('casestage_code')!;
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
      casestage_name: this.reactiveForm.value.casestagename,
      casestage_code: this.reactiveForm.value.casestagecode,
    }
          var url = 'LawMstCasestage/PostcasestageAdd'
          this.NgxSpinnerService.show()
          this.SocketService.postparams(url, params).subscribe((result: any) => {
            if (result.status == false) {
              this.ToastrService.warning(result.message)
              this.GetcasestageSummary();  
              this.reactiveForm.reset();
              this.NgxSpinnerService.hide()
            }
            else {
              this.ToastrService.success(result.message)
              this.GetcasestageSummary();
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
    this.reactiveFormEdit.get("casestage_name")?.setValue(this.parameterValue1.casestage_name);
    this.reactiveFormEdit.get("casestagecode")?.setValue(this.parameterValue1.casestage_code);
    this.reactiveFormEdit.get("casestage_gid")?.setValue(this.parameterValue1.casestage_gid);
  }
  onupdate(){
      for (const control of Object.keys(this.reactiveFormEdit.controls)) {
        this.reactiveFormEdit.controls[control].markAsTouched();
      }
      debugger
      this.reactiveFormEdit.value;
      var url1 = 'LawMstCasestage/PostUpdatecasestage'
      this.SocketService.postparams(url1, this.reactiveFormEdit.value).subscribe((result: any) => {
        this.responsedata=result;
        if(result.status ==false){
          this.ToastrService.warning(result.message)
          this.GetcasestageSummary();
        }
        else{
          this.ToastrService.success(result.message)
          this.GetcasestageSummary();
        }
    }); 
  }
  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }
  ondelete() {
    debugger
    var url = 'LawMstCasestage/GetDeletecasestage'
    let param = {
      casestage_gid : this.parameterValue 
    }
    this.SocketService.getparams(url, param).subscribe((result: any)=> {
      if(result.status ==false){
        this.ToastrService.warning(result.message);
        this.GetcasestageSummary();
      }
      else{
        
        this.ToastrService.success(result.message);
        this.GetcasestageSummary();
        
      }
      
      this.GetcasestageSummary();
     
  
    });
  }
}

