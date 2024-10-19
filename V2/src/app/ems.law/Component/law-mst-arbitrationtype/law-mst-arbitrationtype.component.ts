import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgxSpinnerService } from 'ngx-spinner';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { ToastrService } from 'ngx-toastr';
interface Iarbit {
  arbittype_code: string;
  arbit_type:string;
}
@Component({
  selector: 'app-law-mst-arbitrationtype',
  templateUrl: './law-mst-arbitrationtype.component.html',
  styleUrls: ['./law-mst-arbitrationtype.component.scss']
})
export class LawMstArbitrationtypeComponent {
  Arbit!: Iarbit;
  ArbitList: any;
  reactiveForm!: FormGroup;
  reactiveFormEdit:FormGroup | any;
  parameterValue:any
  parameterValue1:any;
  responsedata: any;
  constructor(private NgxSpinnerService: NgxSpinnerService, private SocketService: SocketService, private ToastrService: ToastrService) {
    this.Arbit = {} as Iarbit;

  }
  ngOnInit(): void {
    this.GetArbittypeSummary();
   this.reactiveForm = new FormGroup({
     arbit_code: new FormControl(''),
     arbit_type: new FormControl(''),
     arbit_gid: new FormControl(''),
     created_by: new FormControl(''),
    
   });
   this.reactiveFormEdit = new FormGroup({
    arbit_code: new FormControl(''),
     arbit_type: new FormControl(''),
     arbit_gid: new FormControl(''),
     created_by: new FormControl(''),
    
  });
  }
  GetArbittypeSummary(){
    debugger
    this.NgxSpinnerService.show();
    var url= 'LawMstArbittype/Getarbittypesummary';
    this.SocketService.get(url).subscribe((result:any)=>{
      console.log(result.arbit_list);
      if(result.arbit_list != null){
        $('#InstituteSummary').DataTable().destroy();
        this.ArbitList = result.arbit_list;  
        this.NgxSpinnerService.hide();
        setTimeout(()=>{   
          $('#ArbitrattiontypeSummary').DataTable();
        }, 1);
      }
      else{
        this.ArbitList = result.arbit_list; 
        setTimeout(()=>{   
          $('#ArbitrattiontypeSummary').DataTable();
        }, 1);
        this.NgxSpinnerService.hide();
        $('#ArbitrattiontypeSummary').DataTable().destroy();
      } 
    });
  }
  get arbit_type() {
    return this.reactiveForm.get('arbit_type')!;
  }
  get arbit_code() {
    return this.reactiveForm.get('arbit_code')!;
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
  addarbit(){
    debugger;
  
    if (this.reactiveForm.value.arbit_type != null && this.reactiveForm.value.arbit_code != '')
      {
          this.reactiveForm.value;
          var url = 'LawMstArbittype/PostArbittypeAdd'
          this.NgxSpinnerService.show()
          this.SocketService.postparams(url, this.reactiveForm.value).subscribe((result: any) => {
            if (result.status == false) {
              this.ToastrService.warning(result.message)
              this.GetArbittypeSummary();  
              this.reactiveForm.reset();
              this.NgxSpinnerService.hide()
            }
            else {
              this.ToastrService.success(result.message)
              this.GetArbittypeSummary();
              this.reactiveForm.reset();
              this.NgxSpinnerService.hide()
             
            }
          });
        }
        else {
          this.ToastrService.warning('result.message')
        }
  }


  onclose(){

  }
  editArbit(parameter:any){
    debugger
    this.parameterValue1 = parameter
    this.reactiveFormEdit.get("arbit_type")?.setValue(this.parameterValue1.arbit_type);
    this.reactiveFormEdit.get("arbit_code")?.setValue(this.parameterValue1.arbit_code);
    this.reactiveFormEdit.get("arbit_gid")?.setValue(this.parameterValue1.arbit_gid);
  }
  onupdate(){

    if (this.reactiveFormEdit.value.arbit_type != null && this.reactiveFormEdit.value.arbit_code != null ) {
      for (const control of Object.keys(this.reactiveFormEdit.controls)) {
        this.reactiveFormEdit.controls[control].markAsTouched();
      }
      debugger
      this.reactiveFormEdit.value;
      var url1 = 'LawMstArbittype/PostUpdateArbittype'
      this.SocketService.postparams(url1, this.reactiveFormEdit.value).subscribe((result: any) => {
        this.responsedata=result;
        if(result.status ==false){
          this.ToastrService.warning(result.message)
          this.GetArbittypeSummary();
        }
        else{
          this.ToastrService.success(result.message)
          this.GetArbittypeSummary();
        }
    }); 
    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }

  }
  openModaldelete(parameter: string) {
    this.parameterValue = parameter
  }
  ondelete() {
    
    var url = 'LawMstArbittype/GetDeletearbittype'
    let param = {
      arbit_gid : this.parameterValue 
    }
    this.SocketService.getparams(url,param).subscribe((result: any) => {
      if(result.status ==false){
        this.ToastrService.warning(result.message);
        this.GetArbittypeSummary();
      }
      else{
        
        this.ToastrService.success(result.message);
        this.GetArbittypeSummary();
        
      }
      
      this.GetArbittypeSummary();
     
  
    });
  }
}
