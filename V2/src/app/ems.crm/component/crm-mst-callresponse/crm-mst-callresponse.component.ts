import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
interface ICallResponse {
  
  call_response: string;
  moving_stage: string;
  callresponse_gid: string;
  callresponseedit_name:string;
  movingstage_edit:string;
}

@Component({
  selector: 'app-crm-mst-callresponse',
  templateUrl: './crm-mst-callresponse.component.html',
  styleUrls: ['./crm-mst-callresponse.component.scss'],
  styles: [`
  table thead th, 
  .table tbody td { 
   position: relative; 
  z-index: 0;
  } 
  .table thead th:last-child, 
  
  .table tbody td:last-child { 
   position: sticky; 
  
  right: 0; 
   z-index: 0; 
  
  } 
  .table td:last-child, 
  
  .table th:last-child { 
  
  padding-right: 50px; 
  
  } 
  .table.table-striped tbody tr:nth-child(odd) td:last-child { 
  
   background-color: #ffffff; 
    
    } 
    .table.table-striped tbody tr:nth-child(even) td:last-child { 
     background-color: #f2fafd; 
  
  } 
  `]
})
export class CrmMstCallresponseComponent {
  isReadOnly = true;
  reactiveForm!: FormGroup;
    reactiveFormEdit!: FormGroup;
    responsedata: any;
    parameterValue: any;
    showOptionsDivId: any;
    parameterValue1: any;
    call_list: any[] = [];
    CallResponse!: ICallResponse;
    private confirmedStations: Array<any> = [];
  selectedStage: any;
  leadstagedropdown_list:any[]=[];
    constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService,private NgxSpinnerService: NgxSpinnerService) {
      this.CallResponse = {} as ICallResponse;
    }

    ngOnInit(): void {
      document.addEventListener('click', (event: MouseEvent) => {
        if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
          this.showOptionsDivId = null;
        }
      });
     this.GetCallResponseSummary();
     this.Getleadstagedropdown();
      // Form values for Add popup/////
      this.reactiveForm = new FormGroup({
  
        
        call_response: new FormControl(this.CallResponse.call_response, [
          Validators.required,
          Validators.pattern(/^(\S+\s*)*(?!\s).*$/) // Allow letters, numbers, and spaces
        ]),
        moving_stage:  new FormControl(this.CallResponse.moving_stage,[
          Validators.maxLength(300),
            
          ]),
          
          
        
        
       
      });
        // Form values for Edit popup/////
      this.reactiveFormEdit = new FormGroup({
        
  
        callresponseedit_name:new FormControl(this.CallResponse.callresponseedit_name, [
          Validators.required,
          Validators.pattern(/^(\S+\s*)*(?!\s).*$/) // Allow letters, numbers, and spaces
        ]),
       
        movingstage_edit:  new FormControl(this.CallResponse.movingstage_edit,[
          Validators.maxLength(300),
           
          ]),
          
        
        
        
          callresponse_gid: new FormControl(''),
        
   
      });
    }
   
    toggleOptions(callresponse_gid: any) {
      if (this.showOptionsDivId === callresponse_gid) {
        this.showOptionsDivId = null;
      } else {
        this.showOptionsDivId = callresponse_gid;
      }
    }
 
  
    //// Summary Grid//////
    GetCallResponseSummary(){
      this.NgxSpinnerService.show();
    var url = 'CallResponse/GetCallResponseSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#call_list').DataTable().destroy();
      this.responsedata = result;
      this.call_list = this.responsedata.call_lists;
      this.NgxSpinnerService.hide();
      console.log(this.call_list)
      setTimeout(() => {
        $('#call_list').DataTable();
      }, 1);
  
  
    });
  }
  // ////////////Add popup validtion////////
 
  get call_response() {
    return this.reactiveForm.get('call_response')!;

  }
  
  get moving_stage() {
    return this.reactiveForm.get('moving_stage')!;
  }
  // ////////////Edit popup validtion////////
 
  get callresponseedit_name() {
    return this.reactiveFormEdit.get('callresponseedit_name')!;
  }
  
  get movingstage_edit() {
    return this.reactiveFormEdit.get('movingstage_edit')!;
  }
  // ////////////Add popup////////

  public onsubmit(): void {
    this.NgxSpinnerService.show();
    if (this.reactiveForm.value.call_response != null && this.reactiveForm.value.moving_stage != null) {

        for (const control of Object.keys(this.reactiveForm.controls)) {
          this.reactiveForm.controls[control].markAsTouched();
        }
        this.reactiveForm.value;
        var url='CallResponse/PostCallResponse'
              this.service.post(url,this.reactiveForm.value).subscribe((result:any) => {
  
                if(result.status ==false){
                  window.scrollTo({

                    top: 0, // Code is used for scroll top after event done
        
                  });
                  this.reactiveForm.get("call_response")?.setValue(null);
                 this.reactiveForm.get("moving_stage")?.setValue(null);
                  this.ToastrService.warning(result.message)
                  this.GetCallResponseSummary();
                  this.reactiveForm.reset();
                  

                }
                else{
                  window.scrollTo({

                    top: 0, // Code is used for scroll top after event done
        
                  });
                  this.reactiveForm.get("call_response")?.setValue(null);
                 this.reactiveForm.get("moving_stage")?.setValue(null);
                  this.ToastrService.success(result.message)
                  this.NgxSpinnerService.hide();
                
                  this.GetCallResponseSummary();
                  this.reactiveForm.reset();

                 
                }
                this.GetCallResponseSummary();
                this.reactiveForm.reset();
              });
              
      }
      else {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
      }
      
    }
  //   ////////////Edit popup////////
    openModaledit(parameter: string) {
      this.parameterValue1 = parameter;
      console.log('meoiwce',this.parameterValue1 );
      this.reactiveFormEdit.get("callresponseedit_name")?.setValue(this.parameterValue1.call_response);
      this.reactiveFormEdit.get("movingstage_edit")?.setValue(this.parameterValue1.moving_stage);
      this.reactiveFormEdit.get("callresponse_gid")?.setValue(this.parameterValue1.callresponse_gid);
     
    }
  //   ////////////Update popup////////
    public onupdate(): void {
      this.NgxSpinnerService.show();
      if (this.reactiveFormEdit.value.callresponseedit_name != null && this.reactiveFormEdit.value.movingstage_edit != null) {
      
        this.reactiveFormEdit.value;
        
        //console.log(this.reactiveFormEdit.value)
        var url = 'CallResponse/GetupdateCallResponsedetails'
  
        this.service.post(url,this.reactiveFormEdit.value).pipe().subscribe((result:any)=>{
          this.responsedata=result;
          if(result.status ==false){
            window.scrollTo({

              top: 0, // Code is used for scroll top after event done
  
            });
            this.ToastrService.warning(result.message)
            this.GetCallResponseSummary();
            this.reactiveFormEdit.reset();

          }
          else{
            window.scrollTo({

              top: 0, // Code is used for scroll top after event done
  
            });
            this.ToastrService.success(result.message)
            this.GetCallResponseSummary();
            this.reactiveFormEdit.reset();

          }
          this.NgxSpinnerService.hide();
          this.GetCallResponseSummary();
        this.reactiveFormEdit.reset();

         
      }); 
  
      }
      else {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });
        this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
      }
    }
  //   ////////////Delete popup////////
    openModaldelete(parameter: string) {
      this.parameterValue = parameter
    
    }
    ondelete() {
      this.NgxSpinnerService.show();
      var url = 'CallResponse/GetdeleteCallResponsedetails'
      let param = {
        callresponse_gid : this.parameterValue 
      }
      this.service.getparams(url,param).subscribe((result: any) => {
        if(result.status ==false){
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
        }
        else{
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.success(result.message)
        }
        this.NgxSpinnerService.hide();
        this.GetCallResponseSummary();
      
    
    
      });
    }
    onclose() {
      this.reactiveForm.reset();
      this.reactiveFormEdit.reset();

  
    }
    openModalinactive(parameter: string){
      this.parameterValue = parameter
    }
    oninactive(){
      console.log(this.parameterValue);
        var url3 = 'CallResponse/GetresponseInactive'
        this.service.getid(url3, this.parameterValue).subscribe((result: any) => {
    
          if ( result.status == false) {
           this.ToastrService.warning(result.message)
          }
          else {
           this.ToastrService.success(result.message)
            }
            this.GetCallResponseSummary();
        });
    }
    openModalactive(parameter: string){
      this.parameterValue = parameter
    }
    onactive(){
        var url3 = 'CallResponse/GetresponseActive'
        this.service.getid(url3, this.parameterValue).subscribe((result: any) => {
    
          if ( result.status == false) {
           this.ToastrService.warning(result.message)
          }
          else {
           this.ToastrService.success(result.message)
            }
            this.GetCallResponseSummary();
        });
    }
    
    Getleadstagedropdown(){
      var url = 'CallResponse/Getleadstagedropdown'
      this.service.get(url).subscribe((result: any) => {
        this.responsedata = result;
        this.leadstagedropdown_list = this.responsedata.leadstagedropdown_list;    
      });
    }
    
}
