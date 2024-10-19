import { Component } from '@angular/core';

import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

import { ToastrService } from 'ngx-toastr';

import { SocketService } from 'src/app/ems.utilities/services/socket.service';



interface IIndustry {

 industry_gid: string;

  industry_code: string;

  industry_name: string;
  industry_description: string;
  industryedit_code:any;

  industryedit_name: string;

  industryedit_description: string;

}



@Component({

  selector: 'app-crm-mst-categoryindustry-summary',

  templateUrl: './crm-mst-categoryindustry-summary.component.html',

  styleUrls: ['./crm-mst-categoryindustry-summary.component.scss']

})

export class CrmMstCategoryindustrySummaryComponent {

  reactiveForm!: FormGroup;

  reactiveFormEdit!: FormGroup;

  responsedata: any;

  parameterValue: any;



  parameterValue1: any;

  industry_list: any[] = [];

  industry!: IIndustry;
isReadOnly = true;

  constructor(private formBuilder: FormBuilder, private ToastrService: ToastrService, public service: SocketService) {

    this.industry = {} as IIndustry;

  }



  ngOnInit(): void {

    this.GetIndustrySummary();

    // Form values for Add popup/////

    this.reactiveForm = new FormGroup({

    

      

      industry_name: new FormControl(this.industry.industry_name, [
        Validators.maxLength(100),
        Validators.required,


        
       

      ]),
      industry_description: new FormControl(this.industry.industry_description,[
        Validators.maxLength(300),
        
      ]),
    });

    // Form values for Edit popup/////

    this.reactiveFormEdit = new FormGroup({
      industryedit_code: new FormControl(this.industry.industryedit_code, [

        Validators.required,
      ]),




      industryedit_name: new FormControl(this.industry.industryedit_name, [

        Validators.maxLength(100),
        Validators.required,

        // Validators.pattern('^[A-Za-z0-9 ]+$') // Allow letters, numbers, and spaces



      ]),

      industryedit_description: new FormControl(this.industry.industryedit_description,[
        Validators.maxLength(300),
       
      ]),
      
       

    
      industry_gid: new FormControl(''),







    });

  }



  //// Summary Grid//////

  GetIndustrySummary() {

    var url = 'Industry/GetIndustrySummary'

    this.service.get(url).subscribe((result: any) => {
      $('#industry_list').DataTable().destroy();
      this.responsedata = result;

      this.industry_list = this.responsedata.industry_list;

      //console.log(this.source_list)

      setTimeout(() => {

        $('#industry_list').DataTable();

      }, 1);





    });

  }

  // ////////////Add popup validtion////////
  get industry_code() {

    return this.reactiveForm.get('industry_code')!;

  }
  get industry_name() {

    return this.reactiveForm.get('industry_name')!;

  }
  get industry_description() {
    return this.reactiveForm.get('industry_description')!;
  }

  // ////////////Edit popup validtion////////
  get industryedit_code() {
    return this.reactiveFormEdit.get('industryedit_name')!;
  }
  get industryedit_name() {
    return this.reactiveFormEdit.get('industryedit_name')!;
  }
  get industryedit_description() {
    return this.reactiveFormEdit.get('industryedit_description')!;
  }
  // ////////////Add popup////////

  public onsubmit(): void {

    if (this.reactiveForm.value.industry_name != null ) {



      for (const control of Object.keys(this.reactiveForm.controls)) {

        this.reactiveForm.controls[control].markAsTouched();

      }

      this.reactiveForm.value;

      var url = 'Industry/PostIndustry'

      this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {



        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });
          this.reactiveForm.get("industry_name")?.setValue(null);

        
          this.ToastrService.warning(result.message)
          this.reactiveForm.reset();


          this.GetIndustrySummary();
          this.reactiveForm.reset();


        }

        else if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });


          this.ToastrService.success(result.message)
          this.reactiveForm.reset();






          this.GetIndustrySummary();
          this.reactiveForm.reset();

        } 

          
        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });


          this.ToastrService.success("Industry name added successfully")





          this.GetIndustrySummary();
          this.reactiveForm.reset();




        }



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

    this.parameterValue1 = parameter

    this.reactiveFormEdit.get("industryedit_name")?.setValue(this.parameterValue1.industry_name);

    this.reactiveFormEdit.get("industryedit_description")?.setValue(this.parameterValue1.industry_description);

    this.reactiveFormEdit.get("industry_gid")?.setValue(this.parameterValue1.industry_gid);
    this.reactiveFormEdit.get("industryedit_code")?.setValue(this.parameterValue1.industry_code)

    



  }

  //   ////////////Update popup////////

  public onupdate(): void {

    if (this.reactiveFormEdit.value.industryedit_name != null && this.reactiveFormEdit.value.industryedit_description != null) {

      for (const control of Object.keys(this.reactiveFormEdit.controls)) {

        this.reactiveFormEdit.controls[control].markAsTouched();

      }

      this.reactiveFormEdit.value;



      //console.log(this.reactiveFormEdit.value)

      var url = 'Industry/Getupdateindustrydetails'



      this.service.post(url, this.reactiveFormEdit.value).pipe().subscribe((result: any) => {

        this.responsedata = result;

        if (result.status == false) {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });

          this.ToastrService.warning(result.message)

          this.GetIndustrySummary();
          this.reactiveFormEdit.reset();


        }

        else {
          window.scrollTo({

            top: 0, // Code is used for scroll top after event done

          });

          this.ToastrService.success(result.message)
          this.reactiveFormEdit.reset();


          this.GetIndustrySummary();
          this.reactiveFormEdit.reset();


        }



      });



    }

    else {
      window.scrollTo({

        top: 0, // Code is used for scroll top after event done

      });
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')

    }

  }

    ////////////Delete popup////////

  openModaldelete(parameter: string) {

    this.parameterValue = parameter



  }

  ondelete() {

    console.log(this.parameterValue);

    var url = 'Industry/Getdeleteindustrydetails'

    let param = {

      industry_gid: this.parameterValue

    }

    this.service.getparams(url, param).subscribe((result: any) => {

      if (result.status == false) {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });

        this.ToastrService.warning(result.message)

      }

      else {
        window.scrollTo({

          top: 0, // Code is used for scroll top after event done

        });

        this.ToastrService.success(result.message)

      }

      this.GetIndustrySummary();

    });

  }

  onclose() {

    this.reactiveForm.reset();
    this.reactiveFormEdit.reset();




  }





}









