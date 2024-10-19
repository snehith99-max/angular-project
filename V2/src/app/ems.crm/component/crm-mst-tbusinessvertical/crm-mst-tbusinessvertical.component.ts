import { Component } from '@angular/core';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';


interface Ibusinessvertical_summary {
  business_vertical: string;
  businessvertical_desc: string;
  businessvertiacal_edit: string;
  businessvertical_descedit: string;
  businessvertical_gidedit: string;
}
@Component({
  selector: 'app-crm-mst-tbusinessvertical',
  templateUrl: './crm-mst-tbusinessvertical.component.html',
  styleUrls: ['./crm-mst-tbusinessvertical.component.scss'],
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
export class CrmMstTbusinessverticalComponent {
  responsedata: any;
  isReadOnly = true;
  businessvertical_summary:any[] = [];
  reactiveForm!: FormGroup;
  reactiveFormEdit!: FormGroup;
  businessvertical!: Ibusinessvertical_summary;
  parameterValue: any;
  parameterValue1: any;
  showOptionsDivId: any;
  remainingChars: any | number = 1000
  constructor(public service: SocketService,  private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService){
    this.businessvertical = {} as Ibusinessvertical_summary;
  }
  ngOnInit(): void {
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetBusinessVertical();
    this.reactiveForm = new FormGroup({
      business_vertical :new FormControl(this.businessvertical.business_vertical, [
        Validators.required,
        Validators.pattern(/^(\S+\s*)*(?!\s).*$/) // Allow letters, numbers, and spaces
      ]),
      businessvertical_desc: new FormControl(this.businessvertical.businessvertical_desc,[
        // Validators.maxLength(300),
           
         ])
    });
    this.reactiveFormEdit = new FormGroup({
      businessvertiacal_edit:new FormControl(this.businessvertical.businessvertiacal_edit, [
        Validators.required,
        Validators.pattern(/^(\S+\s*)*(?!\s).*$/)// Allow letters, numbers, and spaces
      ]),
      businessvertical_descedit:  new FormControl(this.businessvertical.businessvertical_descedit,[
        //Validators.maxLength(300),
        ]),     
        businessvertical_gid: new FormControl(''),
        businessvertical_gidedit: new FormControl(''),
    });
  }
  GetBusinessVertical(){
    this.NgxSpinnerService.show();
    var api = 'BusinessVertical/GetBusinessSummary'
    this.service.get(api).subscribe((result: any) => {
      $('#businessvertical').DataTable().destroy();
    this.responsedata = result;
    this.businessvertical_summary = this.responsedata.businessvertical_summary;
    this.NgxSpinnerService.hide();
    setTimeout(() => {
      $('#businessvertical').DataTable();
        }, 1);
    });
    }
    // ////////////Add popup validtion////////
    get business_vertical() {
      return this.reactiveForm.get('business_vertical')!;
    }
    get businessvertical_desc() {
      return this.reactiveForm.get('businessvertical_desc')!;
    }
    // ////////////Edit popup validtion////////
    get businessvertiacal_edit() {
      return this.reactiveFormEdit.get('businessvertiacal_edit')!;
    }
    get businessvertical_descedit() {
      return this.reactiveFormEdit.get('businessvertical_descedit')!;
    }
    get businessvertical_gidedit() {
      return this.reactiveFormEdit.get('businessvertical_gidedit')!;
    }
    toggleOptions(businessvertical_gid: any) {
      if (this.showOptionsDivId === businessvertical_gid) {
        this.showOptionsDivId = null;
      } else {
        this.showOptionsDivId = businessvertical_gid;
      }
    }
    openModaldelete(parameter: string) {
      this.parameterValue = parameter
    }
      //   ////////////Edit popup////////
      openModaledit(parameter: string) {
        this.parameterValue1 = parameter
        this.reactiveFormEdit.get("businessvertiacal_edit")?.setValue(this.parameterValue1.business_vertical);
        this.reactiveFormEdit.get("businessvertical_descedit")?.setValue(this.parameterValue1.businessvertical_desc); 
        this.reactiveFormEdit.get("businessvertical_gidedit")?.setValue(this.parameterValue1.businessvertical_gid);
       this.updateRemainingCharsedit();
      }
    public onsubmit(): void{
      this.NgxSpinnerService.show();
    if(this.reactiveForm.value.business_vertical!=null){
      this.reactiveForm.value;
      var url1='BusinessVertical/Postbusinessvertical'
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
         this.GetBusinessVertical();
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
      if(this.reactiveFormEdit.value.businessvertiacal_edit!=null){
        this.reactiveFormEdit.value;
        var url3='BusinessVertical/GetUpdatebusinessvertical'
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
           this.GetBusinessVertical();
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
      var url2='BusinessVertical/GetDeletebusinessvertical'
      let param = {
        businessvertical_gid : this.parameterValue 
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
         this.GetBusinessVertical();
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
        businessvertical_gid : this.parameterValue 
      }
      var url4 = 'BusinessVertical/Activatebusinessvertical'
      this.service.getparams(url4, param).subscribe((result: any) => {
  
        if ( result.status == false) {
         this.ToastrService.warning(result.message)
        }
        else {
         this.ToastrService.success(result.message)
          }
          this.NgxSpinnerService.hide();
          this.GetBusinessVertical();
      });
    }
    oninactive(){
      this.NgxSpinnerService.show();
      let param = {
        businessvertical_gid : this.parameterValue 
      }
      var url5 = 'BusinessVertical/Inactivatebusinessvertical'
      this.service.getparams(url5, param).subscribe((result: any) => {
  
        if ( result.status == false) {
         this.ToastrService.warning(result.message)
        }
        else {
         this.ToastrService.success(result.message)
          }
          this.NgxSpinnerService.hide();
          this.GetBusinessVertical();
      });
    }
    onclose(){
      this.reactiveForm.reset();
    }
    updateRemainingCharsadd() {
      this.remainingChars = 1000 - this.reactiveForm.value.businessvertical_desc.length;
    }
    updateRemainingCharsedit() {
      this.remainingChars = 1000 - this.reactiveFormEdit.value.businessvertical_descedit.length;
    }
}
