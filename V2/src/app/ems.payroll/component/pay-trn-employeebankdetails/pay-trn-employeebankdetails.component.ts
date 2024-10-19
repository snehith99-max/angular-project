import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { environment } from 'src/environments/environment';
interface IEmployeeBankDetails {
  bank_name: string;
  ac_no:string;
  pf_no:string;
  esi_no:string;
  pan_no:string;
  uan_no:string;
}

@Component({
  selector: 'app-pay-trn-employeebankdetails',
  templateUrl: './pay-trn-employeebankdetails.component.html',
  styleUrls: ['./pay-trn-employeebankdetails.component.scss'],
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
export class PayTrnEmployeebankdetailsComponent {
  showOptionsDivId: any;
  file!: File;
  reactiveForm!: FormGroup;
  reactiveFormReset!: FormGroup;
  responsedata: any;
  employeebankdetailslist: any[] = [];
  bankdetailslist: any[] = [];
  bankdetails: any[] = [];
  Document_list: any[] = [];
  Documentdtl_list: any[] = [];
  employeegid:any;
  employeebankdetails!: IEmployeeBankDetails;
  GetEmployeeBankDetailsSummary: any;
  parameterValue: any;
  mdlBankName: any;
  
  
 
 
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService) {
    this.employeebankdetails = {} as IEmployeeBankDetails;
  }
  ngOnInit(): void {
    // this.GetEmployeeBankDetailsSummary();
     // Form values for Add popup/////
     this.reactiveForm = new FormGroup({
      
      
      employee_gid: new FormControl(''),
      ac_no: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
      pf_no: new FormControl('', [Validators.pattern(/^\S.*$/)]),
      esi_no: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
      pan_no: new FormControl('', [Validators.pattern(/^\S.*$/)]),
      uan_no: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
      bank_name : new FormControl(this.employeebankdetails.bank_name,
         [Validators.required, Validators.minLength(1), Validators.maxLength(250)]),
     

 });

         var api='PayTrnEmployeeBankDetails/GetBankDtl'
         this.service.get(api).subscribe((result:any)=>{
         this.bankdetailslist = result.GetBankDtl;
         //console.log(this.bankdetailslist)
        });

   
   //// Summary Grid//////
    var url = 'PayTrnEmployeeBankDetails/GetEmployeeBankDetailsSummary'
    this.service.get(url).subscribe((result: any) => {
  
      this.responsedata = result;
      this.employeebankdetailslist = this.responsedata.employeebankdetails_list;
      //console.log(this.employeebankdetailslist)
      setTimeout(() => {
        $('#employeebankdetailslist').DataTable();
      }, 1);
  
  
    });
    
    
    
    
    }

  ////////////Add popup validtion////////
get bank_name() {
  return this.reactiveForm.get('bank_name')!;
}
get ac_no() {
  return this.reactiveForm.get('ac_no')!;
}
get pf_no() {
  return this.reactiveForm.get('pf_no')!;
}
get esi_no() {
  return this.reactiveForm.get('esi_no')!;
}
get pan_no() {
  return this.reactiveForm.get('pan_no')!;
}
get uan_no() {
  return this.reactiveForm.get('uan_no')!;
}

////////////Add popup////////
  public onsubmit(): void {
    
      if (this.reactiveForm.value.bank_name != null && this.reactiveForm.value.bank_name != '') {
  
        for (const control of Object.keys(this.reactiveForm.controls)) {
          this.reactiveForm.controls[control].markAsTouched();
        }
        this.reactiveForm.value;
        var url='PayTrnEmployeeBankDetails/PostEmployeeBankDetails'
              this.service.post(url,this.reactiveForm.value).subscribe((result:any) => {
  
                if(result.status ==false){
                  this.ToastrService.warning(result.message)
                  // this.GetEmployeeBankDetailsSummary();
                }
                else{
                 
                
                  this.reactiveForm.get("ac_no")?.setValue(null);
                  this.reactiveForm.get("pf_no")?.setValue(null);
                  this.reactiveForm.get("esi_no")?.setValue(null);
                  this.reactiveForm.get("pan_no")?.setValue(null);
                  this.reactiveForm.get("uan_no")?.setValue(null);
                  this.ToastrService.success(result.message)
                  
                
                  // this.GetEmployeeBankDetailsSummary();
                 
                }
                
              });
                   
      }
      else {
        this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
      }
      setTimeout(function() {
        window.location.reload();
    }, 2000); // 2000 milliseconds = 2 seconds
    }

    // public onback(): void {
     
    // }

    

    openBankDetails(parameter: string) {
      this.parameterValue = parameter
      this.reactiveForm.get("employee_gid")?.setValue(this.parameterValue.employee_gid);
      console.log(this.reactiveForm)
      const employeegid = this.reactiveForm.value.employee_gid;
      this.getbankdetails(employeegid);

    }
    getbankdetails(employee_gid:any){
      var url = 'PayTrnEmployeeBankDetails/Getbankdetails'
      let param = {
         employee_gid : employee_gid 
          }
      this.service.getparams(url, param).subscribe((result: any) => {
     this.bankdetails = result.GetBank;
    
    
    });



    }



      openBank(parameter: string) {
        this.parameterValue = parameter
      this.reactiveForm.get("employee_gid")?.setValue(this.parameterValue.employee_gid);
      console.log(this.reactiveForm)
      const employeegid = this.reactiveForm.value.employee_gid;
      this.getbankdetails1(employeegid);
      }
      getbankdetails1(employee_gid:any){
        var url = 'PayTrnEmployeeBankDetails/Getbankdetails'
        let param = {
           employee_gid : employee_gid 
            }
        this.service.getparams(url, param).subscribe((result: any) => {
       this.bankdetails = result.GetBank;
       this.reactiveForm.get("bank_name")?.setValue(this.bankdetails[0].bank);
       this.reactiveForm.get("bank_code")?.setValue(this.bankdetails[0].bank_code);
       this.reactiveForm.get("ac_no")?.setValue(this.bankdetails[0].ac_no);
       this.reactiveForm.get("pf_no")?.setValue(this.bankdetails[0].pf_no);
       this.reactiveForm.get("esi_no")?.setValue(this.bankdetails[0].esi_no);
       this.reactiveForm.get("pan_no")?.setValue(this.bankdetails[0].pan_no);
       this.reactiveForm.get("uan_no")?.setValue(this.bankdetails[0].uan_no);
      
      });
  
  
  
      }

  get employee_gid() {
  return this.reactiveForm.get('employee_gid')!;
         }
  importexcel(){
    debugger;
    let formData = new FormData();
    if (this.file != null && this.file != undefined) {
      window.scrollTo({
        top: 0, // Code is used for scroll top after event done
      });
      formData.append("file", this.file, this.file.name);
      var api = 'PayTrnEmployeeBankDetails/BankDtlImport'
      this.service.postfile(api, formData).subscribe((result: any) => {
        this.responsedata = result;
        // this.router.navigate(['/crm/CrmMstProductsummary']);
       
        this.ToastrService.success("Excel Uploaded Successfully")
      });
    }
    }
  onChange1(event: any) {
          this.file = event.target.files[0];
    }
  
  // downloadfileformat() {
  //         let link = document.createElement("a");
  //         link.download = "Bank Details";
  //         link.href = "assets/media/Excels/Bank/bankdetails.xls";
  //         link.click();
  //   }

  downloadfileformat() {
    let link = document.createElement("a");
    link.download = "Bank Details";
    window.location.href = "http://"+ environment.host + "/Templates/Bank Details.xls";
    link.click();
  }
    ondetail(document_name:any){
      debugger;
      var api1='PayTrnEmployeeBankDetails/GetDocumentDtllist'
      var param={
        document_gid:document_name,
      }
      this.service.getparams(api1,param).subscribe((result:any)=>{
     
        this.responsedata=result;
        this.Documentdtl_list = this.responsedata.documentdtl_list;  
      });
    }
    getdocumentlist(){
      debugger;

      var api1='PayTrnEmployeeBankDetails/GetDocumentlist'
         this.service.get(api1).subscribe((result:any)=>{
        this.responsedata=result;
        this.Document_list = this.responsedata.document_list;
          });
          }
          onclose() {
            this.reactiveForm.reset();
        
          }


onback(){
  this.reactiveForm.reset();
 }

}

 



  






