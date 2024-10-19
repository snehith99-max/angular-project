import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';


interface IRevenue {
  revenue_code: string;
  revenue_gid: string;
  revenue_desc:string;
  revenue_name:string;
}

@Component({
  selector: 'app-otl-mst-revenuecategorysummary',
  templateUrl: './otl-mst-revenuecategorysummary.component.html',
  styleUrls: ['./otl-mst-revenuecategorysummary.component.scss']
})
export class OtlMstRevenuecategorysummaryComponent {

  isReadOnly = true;
  private unsubscribe: Subscription [] = [];
  reactiveForm!: FormGroup;
  reactiveFormEdit!: FormGroup;
  responsedata: any;
  parameterValue: any;
  showOptionsDivId: any;
  rows: any[] = [];
  parameterValue1: any;
  revenue_list1: any[] = [];
  Revenue!: IRevenue;
  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService,private NgxSpinnerService: NgxSpinnerService) {
    this.Revenue = {} as IRevenue;
  }
  ngOnInit(): void {
    // Form values for Add popup/////
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
    this.GetRevenueSummary();
    this.reactiveForm = new FormGroup({

      revenue_desc: new FormControl(this.Revenue.revenue_desc, [ Validators.required,Validators.pattern("^(?!\\s*$)[a-zA-Z,\\.]+(?:\\s[a-zA-Z,\\.]+)*$")

    ]),
        revenue_name:new FormControl(this.Revenue.revenue_name,[
          Validators.required,

        ])

    });
  }
  GetRevenueSummary(){
    this.NgxSpinnerService.show();
    var api = 'Revenue/GetRevenueSummary'
    this.service.get(api).subscribe((result: any) => {
      $('#revenue_list').DataTable().destroy();
    this.responsedata = result;
    this.revenue_list1 = this.responsedata.revenue_list1;
    this.NgxSpinnerService.hide();
    
    //console.log(this.entity_list)
    setTimeout(() => {
    $('#revenue_list').DataTable();
      }, 1);
    });
    }
 
  
get revenue_desc() {
  return this.reactiveForm.get('revenue_desc')!;
}
get revenue_name() {
  return this.reactiveForm.get('revenue_name')!;
}
public onsubmit(): void {
  if (this.reactiveForm.value.revenue_desc != null && this.reactiveForm.value.revenue_name != null) {

    for (const control of Object.keys(this.reactiveForm.controls)) {
      this.reactiveForm.controls[control].markAsTouched();
    }
    this.reactiveForm.value;
    this.NgxSpinnerService.show();
    var url='Revenue/postrevenue'
          this.service.post(url,this.reactiveForm.value).subscribe((result:any) => {

            if(result.status ==false){
              window.scrollTo({

                top: 0, // Code is used for scroll top after event done
    
              });
              this.reactiveForm.get("region_desc")?.setValue(null);
              this.reactiveForm.get("revenue_name")?.setValue(null);
              this.ToastrService.warning(result.message)
              this.GetRevenueSummary();
              this.reactiveForm.reset();

            }
            else{
              window.scrollTo({

                top: 0, // Code is used for scroll top after event done
    
              });
              this.reactiveForm.get("revenue_desc")?.setValue(null);
              this.reactiveForm.get("revenue_name")?.setValue(null);
              this.ToastrService.success(result.message)
              this.GetRevenueSummary();
              this.reactiveForm.reset();
            }

            this.NgxSpinnerService.hide();
            this.GetRevenueSummary();
            this.reactiveForm.reset();

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
  console.log(this.parameterValue);
  this.NgxSpinnerService.show();
  var url = 'Revenue/deleterevenue'
  let param = {
    revenue_gid : this.parameterValue 
  }
  this.service.getparams(url,param).subscribe((result: any) => {
    if(result.status == false){
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
      this.GetRevenueSummary();

    }
    this.NgxSpinnerService.hide();
    this.GetRevenueSummary();

  });
}

onclose() {
  this.reactiveForm.reset();

}
toggleOptions(account_gid: any) {
  if (this.showOptionsDivId === account_gid) {
    this.showOptionsDivId = null;
  } else {
    this.showOptionsDivId = account_gid;
  }
}

}
