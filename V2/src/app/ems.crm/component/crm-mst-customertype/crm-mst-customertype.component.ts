import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';


interface Icustomertype {
  customer_type: string;
  customertype_description: string;
  customer_typeedit: string;
  customertype_descriptionedit: string;
  customertype_gid: string;
  
}
@Component({
  selector: 'app-crm-mst-customertype',
  templateUrl: './crm-mst-customertype.component.html',
  styleUrls: ['./crm-mst-customertype.component.scss']
})
export class CrmMstCustomertypeComponent {
  responsedata: any;
  isReadOnly = true;
  customertypesummary_list:any[] = [];
  reactiveForm!: FormGroup;
  reactiveFormEdit!: FormGroup;

  customertype!: Icustomertype;
  parameterValue: any;
  parameterValue1: any;
  constructor(public service: SocketService,  private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService){
    this.customertype = {} as Icustomertype;
  }
  ngOnInit(): void {
    this.GetCustomerTypeSummary();
    this.reactiveForm = new FormGroup({
      customer_type :new FormControl(this.customertype.customer_type, [
        Validators.required,
        Validators.pattern("^(?!\\s*$)[a-zA-Z\\s]*$") // Allow letters, numbers, and spaces
      ]),
      customertype_description: new FormControl(this.customertype.customertype_description,[
        // Validators.maxLength(300),
           
         ])
    });
    this.reactiveFormEdit = new FormGroup({
      customer_typeedit:new FormControl(this.customertype.customer_typeedit, [
        Validators.required,
        Validators.pattern("^(?!\\s*$)[a-zA-Z\\s]*$") // Allow letters, numbers, and spaces
      ]),
      customertype_descriptionedit:  new FormControl(this.customertype.customertype_descriptionedit,[
        //Validators.maxLength(300),
        ]),     
        customertype_gid: new FormControl(''),
        customertype_gidedit: new FormControl(''),
    });
  }
  GetCustomerTypeSummary(){
    this.NgxSpinnerService.show();
    var api = 'CustomerTypeSummary/GetCustomerTypeSummary'
    this.service.get(api).subscribe((result: any) => {
      $('#customertype').DataTable().destroy();
    this.responsedata = result;
    this.customertypesummary_list = this.responsedata.customertypesummary_lists;
    this.NgxSpinnerService.hide();
    setTimeout(() => {
      $('#customertype').DataTable();
        }, 1);
    });
    }
    // ////////////Add popup validtion////////
    get customer_type() {
      return this.reactiveForm.get('customer_type')!;
    }
    get customertype_description() {
      return this.reactiveForm.get('customertype_description')!;
    }
    // ////////////Edit popup validtion////////
    get customer_typeedit() {
      return this.reactiveFormEdit.get('customer_typeedit')!;
    }
    get customertype_descriptionedit() {
      return this.reactiveFormEdit.get('customertype_descriptionedit')!;
    }
    get customertype_gidedit() {
      return this.reactiveFormEdit.get('customertype_gidedit')!;
    }
    
    openModaldelete(parameter: string) {
      this.parameterValue = parameter
    }
      //   ////////////Edit popup////////
      openModaledit(parameter: string) {
        this.parameterValue1 = parameter
        this.reactiveFormEdit.get("customer_typeedit")?.setValue(this.parameterValue1.display_name);
        this.reactiveFormEdit.get("customertype_descriptionedit")?.setValue(this.parameterValue1.customertype_desc); 
        this.reactiveFormEdit.get("customertype_gidedit")?.setValue(this.parameterValue1.customertype_gid);
       
      }
    public onsubmit(): void{
      this.NgxSpinnerService.show();
    if(this.reactiveForm.value.customer_type!=null){
      this.reactiveForm.value;
      var url1='CustomerTypeSummary/PostCustomerType'
         this.service.post(url1,this.reactiveForm.value).subscribe((result:any) => {
         if(result.status==false){
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.reactiveForm.reset();
         }
         else{
          this.ToastrService.success(result.message)
          this.reactiveForm.reset();
         }
         this.NgxSpinnerService.hide();
         this.GetCustomerTypeSummary();
         });
    }
    else{
      window.scrollTo({

        top: 0, // Code is used for scroll top after event done

      });
      this.NgxSpinnerService.hide();
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
    }
    onupdate(){
      this.NgxSpinnerService.show();
      if(this.reactiveFormEdit.value.customer_typeedit!=null){
        this.reactiveFormEdit.value;
        var url3='CustomerTypeSummary/GetUpdateCustomerType'
           this.service.post(url3,this.reactiveFormEdit.value).subscribe((result:any) => {
           if(result.status==false){
            window.scrollTo({
  
              top: 0, // Code is used for scroll top after event done
  
            });
            this.ToastrService.warning(result.message)
            this.reactiveForm.reset();
           }
           else{
            this.ToastrService.success(result.message)
            this.reactiveForm.reset();
           }
           this.NgxSpinnerService.hide();
           this.GetCustomerTypeSummary();
           });
      }
      else{
        window.scrollTo({
  
          top: 0, // Code is used for scroll top after event done
  
        });
        this.NgxSpinnerService.hide();
        this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
      }
    }
    ondelete(){
      this.NgxSpinnerService.show();
      var url2='CustomerTypeSummary/GetDeleteCustomerType'
      let param = {
        customertype_gid : this.parameterValue 
      }
      this.service.getparams(url2,param).subscribe((result:any)=>{
        if(result.status==false){
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.reactiveForm.reset();
         }
         else{
          this.ToastrService.success(result.message)
          this.reactiveForm.reset();
         }
         this.NgxSpinnerService.hide();
         this.GetCustomerTypeSummary();
      });

    }
    openModalactive(parameter: string){
      this.parameterValue = parameter
    }
    openModalinactive(parameter: string){
      this.parameterValue = parameter
    }
    onactive(){
      this.NgxSpinnerService.show();
      let param = {
        customertype_gid : this.parameterValue 
      }
      var url4 = 'CustomerTypeSummary/ActivateCustomerType'
      this.service.getparams(url4, param).subscribe((result: any) => {
  
        if ( result.status == false) {
         this.ToastrService.warning('Error While Activate The Customer Type')
        }
        else {
         this.ToastrService.success('Customer Type Activated Successfully')
          }
          this.NgxSpinnerService.hide();
          this.GetCustomerTypeSummary();
      });
    }
    oninactive(){
      this.NgxSpinnerService.show();
      let param = {
        customertype_gid : this.parameterValue 
      }
      var url5 = 'CustomerTypeSummary/InactivateCustomerType'
      this.service.getparams(url5, param).subscribe((result: any) => {
  
        if ( result.status == false) {
         this.ToastrService.warning('Error While Deactivate The Customer Type')
        }
        else {
         this.ToastrService.success('Customer Type Deactivated Successfully')
          }
          this.NgxSpinnerService.hide();
          this.GetCustomerTypeSummary();
      });
    }
    onclose(){
      this.reactiveForm.reset();
    }
}
