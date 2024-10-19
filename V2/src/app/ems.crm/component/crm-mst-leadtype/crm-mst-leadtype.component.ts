import { Component } from '@angular/core';
import { SocketService} from 'src/app/ems.utilities/services/socket.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';

interface ILeadtype {
  leadtype_gid: string;
  leadtype_code: string;
  leadtype_name: string;
  leadtype_codeedit: string;
  leadtype_nameedit: string;

}

@Component({
  selector: 'app-crm-mst-leadtype',
  templateUrl: './crm-mst-leadtype.component.html',
  styleUrls: ['./crm-mst-leadtype.component.scss'],
})
export class CrmMstLeadtypeComponent {
  isReadOnly = true;

  responsedata: any;
  leadtype_list: any[] = [];
  reactiveForm!: FormGroup;
  reactiveFormEdit!: FormGroup;
  parameterValue1!: any;
  parameterValue!: any; 
  leadtype!: ILeadtype;
  showOptionsDivId: any;
  status: any;
  leadtype_gid: any;
  
  
  constructor(public service: SocketService, private ToastrService: ToastrService, private NgxSpinnerService: NgxSpinnerService) {
    this.leadtype = {} as ILeadtype;

  }

  ngOnInit(): void {
    this.GetleadtypeSummary();
    this.reactiveForm = new FormGroup({

      leadtype_name: new FormControl(this.leadtype.leadtype_name, [

        Validators.required,
        Validators.pattern(/^(\S+\s*)*(?!\s).*$/)

        // Validators.pattern('^[A-Za-z0-9 ]+$') // Allow letters, numbers, and spaces

      ]),
    });
    this.reactiveFormEdit = new FormGroup({

      leadtype_codeedit: new FormControl(this.leadtype.leadtype_codeedit, [
        Validators.required,
        Validators.pattern('[A-Za-z0-9]+')

      ]),
      leadtype_nameedit: new FormControl(this.leadtype.leadtype_nameedit, [

        Validators.required,
        Validators.pattern(/^(\S+\s*)*(?!\s).*$/)
        // Validators.pattern('^[A-Za-z0-9 ]+$') // Allow letters, numbers, and spaces

      ]),
      leadtype_gid: new FormControl(''),



    });
  }


  GetleadtypeSummary() {
    this.NgxSpinnerService.show();
    var url = 'Leadtype/GetLeadtypeSummary'
    this.service.get(url).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      $('#leadtype_list').DataTable().destroy();
      this.responsedata = result;
      this.leadtype_list = this.responsedata.leadtype_list;
      //console.log(this.entity_list)
      setTimeout(() => {
        $('#leadtype_list').DataTable();
      }, 1);


    });


  }
  get leadtype_code() {
    return this.reactiveForm.get('leadtype_code')!;
  }
  get leadtype_name() {
    return this.reactiveForm.get('leadtype_name')!;
  }
  get leadtype_codeedit() {
    return this.reactiveFormEdit.get('leadtype_codeedit')!
      ;
  }
  get leadtype_nameedit() {
    return this.reactiveFormEdit.get('leadtype_nameedit')!;
  }

  public onsubmit(): void {
    this.NgxSpinnerService.show();
    if (this.reactiveForm.value.leadtype_name != null) {

      for (const control of Object.keys(this.reactiveForm.controls)) {
        this.reactiveForm.controls[control].markAsTouched();
      }
      this.reactiveForm.value;
      var url = 'Leadtype/PostLeadtype'
      this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {
        this.NgxSpinnerService.hide();
        if (result.status == false) {
          this.reactiveForm.get("leadtype_name")?.setValue(null);
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.GetleadtypeSummary();
          this.reactiveForm.reset();

        }
        else {

          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.success(result.message)
          this.GetleadtypeSummary();
          this.reactiveForm.reset();




        }

      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }

  }
  openModaledit(parameter: string) {
    this.parameterValue1 = parameter
    this.reactiveFormEdit.get("leadtype_codeedit")?.setValue(this.parameterValue1.leadtype_code);
    this.reactiveFormEdit.get("leadtype_nameedit")?.setValue(this.parameterValue1.leadtype_name);
    this.reactiveFormEdit.get("leadtype_gid")?.setValue(this.parameterValue1.leadtype_gid);

  }
  public onupdate(): void {
    this.NgxSpinnerService.show();
    if (this.reactiveFormEdit.value.leadtype_codeedit != null && this.reactiveFormEdit.value.leadtype_nameedit != null) {
      for (const control of Object.keys(this.reactiveFormEdit.controls)) {
        this.reactiveFormEdit.controls[control].markAsTouched();

      }
      this.reactiveFormEdit.value;


      //console.log(this.reactiveFormEdit.value)
      var url = 'LeadType/UpdatedLeadtype'

      this.service.post(url, this.reactiveFormEdit.value).pipe().subscribe((result: any) => {
        this.responsedata = result;
        this.NgxSpinnerService.hide();
        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.warning(result.message)
          this.GetleadtypeSummary();
          this.reactiveFormEdit.reset();

        }
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.ToastrService.success(result.message)
          this.GetleadtypeSummary();
          this.reactiveFormEdit.reset();


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
    this.NgxSpinnerService.show();
    var url = 'Leadtype/deleteLeadtypeSummary'
    let param = {
      leadtype_gid: this.parameterValue
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.NgxSpinnerService.hide();
      if (result.status == false) {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.warning(result.message)
      }
      else {
        window.scrollTo({
          top: 0,
        });
        this.ToastrService.success(result.message)
      }
      this.GetleadtypeSummary();
    });
  }
  statusvalue(value: any , leadtype_gid: String){
    this.status = value
    this.leadtype_gid = leadtype_gid
    
  }
  onstatusupdate(leadtype_gid : String , status_flag : String){
    this.NgxSpinnerService.show();
    let params = {
     leadtype_gid: leadtype_gid,
     status_flag : status_flag
     }
     var url4 = 'Leadtype/OnStatusUpdateLeadtype'
     this.service.post(url4, params).subscribe((result: any) => {
  
        if ( result.status == false) {
         this.ToastrService.warning(result.message)
        }
        else {
         this.ToastrService.success(result.message)
          }
          this.NgxSpinnerService.hide();
          this.GetleadtypeSummary();
      });
  }
 

 
  onclose() {
    this.reactiveForm.reset();

  }
}

