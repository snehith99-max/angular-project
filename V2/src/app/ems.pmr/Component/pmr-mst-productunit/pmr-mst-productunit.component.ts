import { Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AES } from 'crypto-js';
import { ToastrService } from 'ngx-toastr';
import { SocketService } from 'src/app/ems.utilities/services/socket.service';
import { NgxSpinnerService } from 'ngx-spinner';
interface IProductUnit {
  conversion_rate: any;
  sequence_level: any;
  productuomclass_name: string;
  productuomclass_code: any;
  productuomclassedit_name: string;
  productuomclassedit_code: any;
  productuomclassedit_name1: string;
  productuomclassedit_code1: string;
  batch_flag:string;
  productunit_name:string;
  productuom_name:string;
  productuomclass_nameedit:string;
}


@Component({
  selector: 'app-pmr-mst-productunit',
  templateUrl: './pmr-mst-productunit.component.html',
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
export class PmrMstProductunitComponent {
  reactiveFormReset!: FormGroup;
  reactiveForm!: FormGroup;
  reactiveFormadd : FormGroup | any;
  responsedata: any;
  productunit_list: any;
  productunit!: IProductUnit;
  reactiveFormEdit: FormGroup | any;
  ProductunitFormEdit: FormGroup | any;
  parameterValue1: any;
  productunitgrid_list: any[] = [];
  productunitdata:any;
  showOptionsDivId:any;
  unitclass_list: any[] = [];

  constructor(private formBuilder: FormBuilder,public NgxSpinnerService:NgxSpinnerService, private route: Router, private ToastrService: ToastrService, public service: SocketService) {
    this.productunit = {} as IProductUnit;
    
    this.reactiveFormadd = new FormGroup ({
      conversion_rate:new FormControl (''),
      sequence_level:new FormControl (''),
      productuomclassedit_name1:new FormControl (''),
      productuomclassedit_code1:new FormControl (''),
      batch_flag:new FormControl (''),
      productuomclass_gid:new FormControl(''),
      productuomclass_name:new FormControl(''),
    })

  }


  data: any;
  parameterValue: any;
  parameterValue2:any;



  onedit() { }
  toggleOptions(account_gid: any) {
    if (this.showOptionsDivId === account_gid) {
      this.showOptionsDivId = null;
    } else {
      this.showOptionsDivId = account_gid;
    }
  }
  

  ngOnInit(): void {
    this.GetProductunitSummary();

    ///form values for add popup///
    this.reactiveForm = new FormGroup({
      productuomclass_name: new FormControl(
        this.productunit.productuomclass_name,
        [Validators.required]
      ),
      productuom_name: new FormControl(
        this.productunit.productuom_name,
        [Validators.required]
      ),
      productuomclass_code: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
    });
    ///form values for edit///
    this.reactiveFormEdit = new FormGroup({
      productuomclassedit_name: new FormControl(
        this.productunit.productuomclassedit_name,
        [Validators.required]
      ),
      productuomclassedit_code: new FormControl(
        this.productunit.productuomclassedit_code,
        [Validators.required]
      ),
      productunit_name: new FormControl(
        this.productunit.productunit_name,
        [Validators.required]
      ),
      productuomclass_gid: new FormControl(''),
    });

    // this.reactiveFormadd = new FormGroup ({
    //   conversion_rate:new FormControl (''),
    //   sequence_level:new FormControl (''),
    //   productuomclassedit_name1:new FormControl (''),
    //   productuomclassedit_code1:new FormControl (''),
    //   batch_flag:new FormControl (''),
    //   productuomclass_gid:new FormControl(''),
    //   productuomclass_name:new FormControl(''),
    // })

    this.ProductunitFormEdit = new FormGroup({

      productuomedit_name:  new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
      productuomedit_code: new FormControl('', [Validators.required,Validators.pattern(/^\S.*$/)]),
      sequence_leveledit: new FormControl(''),
      conversion_rateedit: new FormControl(''),
      productuomclass_gid: new FormControl(''),
      batch_flagedit: new FormControl(''),
      productuomclass_nameedit: new FormControl(this.productunit.productuomclass_nameedit, [
        Validators.required,
      ]),
      productuom_gid:new FormControl('')
    });


    var api2 = 'PmrMstProductUnit/Getproductunitclassdropdown'
    this.service.get(api2).subscribe((result: any) => {
      this.responsedata = result;
      this.unitclass_list = result.unitclass_list;
    });
    document.addEventListener('click', (event: MouseEvent) => {
      if (event.target && !(event.target as HTMLElement).closest('button.btn') && this.showOptionsDivId) {
        this.showOptionsDivId = null;
      }
    });
  
  }




  ////////////Get Summary for product unit//////////////////////
  GetProductunitSummary() {
    this.NgxSpinnerService.show()
    var url = 'PmrMstProductUnit/GetProductunitSummary'
    this.service.get(url).subscribe((result: any) => {
      $('#productunit_list').DataTable().destroy();
      this.responsedata = result;
      this.productunit_list = this.responsedata.productunit_list;
      setTimeout(() => {
        $('#productunit_list').DataTable();
      }, 1);

      this.NgxSpinnerService.hide()
    });
  }

  get productuomclassedit_code1() {
    return this.reactiveFormadd.get('productuomclassedit_code1')!;
  }
  get productuomclassedit_name1() {
    return this.reactiveFormadd.get('productuomclassedit_name1')!;
  }
  get sequence_level() {
    return this.reactiveFormadd.get('sequence_level')!;
  }
  get conversion_rate() {
    return this.reactiveFormadd.get('conversion_rate')!;
  }

  /////////For Add PopUp/////////
  get productuomclass_name() {
    return this.reactiveForm.get('productuomclass_name')!;
  }
  get productuomclass_code() {
    return this.reactiveForm.get('productuomclass_code')!;
  }
  get productunit_name() {
    return this.reactiveFormEdit.get('productunit_name')!;
  }
  get productuom_name() {
    return this.reactiveForm.get('productuom_name')!;
  }
  get productuomedit_code() {
    return this.ProductunitFormEdit.get('productuomedit_code')!;
  }
  get productuomclass_nameedit() {
    return this.ProductunitFormEdit.get('productuomclass_nameedit')!;
  }
  onsubmit() {
    
    debugger
    const productGroupName = this.reactiveForm.value.productuomclass_code?.trim();
  
    if (productGroupName !== '') {
      for (const control of Object.keys(this.reactiveForm.controls)) {
        this.reactiveForm.controls[control].markAsTouched();
      }

    // if (
    //   this.reactiveForm.value.productuomclass_name != null) {
    //   for (const control of Object.keys(this.reactiveForm.controls)) {
    //     this.reactiveForm.controls[control].markAsTouched();
    //   }
      this.reactiveForm.value;
      var url = 'PmrMstProductUnit/PostProductUnit'
      this.NgxSpinnerService.show();
      this.service.post(url, this.reactiveForm.value).subscribe((result: any) => {

        if (result.status == false) {
          this.ToastrService.warning(result.message)
          this.GetProductunitSummary();
          this.reactiveForm.reset();
          this.NgxSpinnerService.hide();
        }
        else {
          this.reactiveForm.get("productuomclass_name")?.setValue(null);
          this.reactiveForm.get("productuomclass_code")?.setValue(null);
          this.ToastrService.success(result.message)
          this.reactiveForm.reset();

          this.GetProductunitSummary();
          this.NgxSpinnerService.hide();

        }

      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
      this.reactiveForm.reset();
    }
    
  }


  /////EDIT POPUP/////
  get productuomclassedit_code() {
    return this.reactiveFormEdit.get('productuomclassedit_code')!;
  }
  get productuomclassedit_name() {
    return this.reactiveFormEdit.get('productuomclassedit_name')!;
  }

  openModaledit(parameter: string) {
    this.parameterValue1 = parameter
    this.reactiveFormEdit.get("productuomclassedit_code")?.setValue(this.parameterValue1.productuomclass_code);
    this.reactiveFormEdit.get("productuomclassedit_name")?.setValue(this.parameterValue1.productuomclass_name);
    this.reactiveFormEdit.get("productuomclass_gid")?.setValue(this.parameterValue1.productuomclass_gid);

  };

  ////////////Update popup////////
  public onupdate(): void {
    if (
      this.reactiveFormEdit.value.productuomclassedit_name != null &&  this.reactiveFormEdit.value.productuomclassedit_code != null) {
      for (const control of Object.keys(this.reactiveFormEdit.controls)) {
        this.reactiveFormEdit.controls[control].markAsTouched();
      }
      this.reactiveFormEdit.value;

      var url = 'PmrMstProductUnit/UpdatedProductunit'
      this.NgxSpinnerService.show();
      this.service
        .post(url, this.reactiveFormEdit.value)
        .pipe()
        .subscribe((result: any) => {
          this.responsedata = result;
          if (result.status == false) {
            this.ToastrService.warning(result.message);
            this.NgxSpinnerService.hide();
            this.GetProductunitSummary();
          } else {
            this.ToastrService.success(result.message);
            this.NgxSpinnerService.hide();
            this.GetProductunitSummary();   
               }

      });

    }
    else {
      this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ')
    }
  }
  ////////////Delete popup////////
  openModaldelete(parameter: string) {
    this.parameterValue = parameter

  }
  ondelete() {
    var url = 'PmrMstProductUnit/deleteProductunitSummary'
    let param = {
      productuom_gid: this.parameterValue,
    };
    this.service.getparams(url, param).subscribe((result: any) => {
      if (result.status == false) {
        this.ToastrService.warning(result.message);
      } else {
        this.ToastrService.success(result.message);
      }
      this.GetProductunitSummary();
    });
  }

  ////Expandable Grid////
  ondetail(productuomclass_gid: any) {
    var url = 'PmrMstProductUnit/GetProductUnitSummarygrid'
    let param = {
      productuomclass_gid: productuomclass_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
      this.responsedata = result;
      this.productunitgrid_list = result.productunitgrid_list;
      setTimeout(() => {
        $('#productunitgrid_list').DataTable();
      }, 1);

    });
  }
  onclose(){
    this.reactiveForm.reset();
  }
  productunitclass(productuomclass_gid:any){
    
    var url = 'PmrMstProductUnit/GetProductunits'
    
    let param = {
      productuomclass_gid: productuomclass_gid
    }
    this.service.getparams(url, param).subscribe((result: any) => {
    this.productunitgrid_list = result.productunitgrid_list;
    this.responsedata=result;
  this.reactiveFormadd.get("productuomclass_name")?.setValue(this.productunitgrid_list[0].productuomclass_name);

});
  }

  onSubmitprod() {

   var params={
     
      productuomclass_gid:this.reactiveFormadd.value.productuomclass_gid,
      conversion_rate:this.reactiveFormadd.value.conversion_rate,
      productuomclassedit_code1:this.reactiveFormadd.value.productuomclassedit_code1,
      productuomclassedit_name1:this.reactiveFormadd.value.productuomclassedit_name1,
      sequence_level:this.reactiveFormadd.value.sequence_level,
      productuomclass_name:this.reactiveFormadd.value.productuomclass_name,
      batch_flag:this.reactiveFormadd.value.batch_flag
      
  
      
    }
    var url = 'PmrMstProductUnit/PostProductunits'
    this.NgxSpinnerService.show();
      this.service.post(url,params).subscribe((result: any) => {
        if(result.status ==false){
          this.ToastrService.warning(result.message)
          this.NgxSpinnerService.hide();
          }
          else
          {
        this.ToastrService.success(result.message)
        this.NgxSpinnerService.hide();
       
          }
          this.reactiveFormadd.reset();

      });
  
    }
   

   
    get batch_flag() {
      return this.reactiveFormadd.get('batch_flag')!;
    }
    get productuomedit_name() {
      return this.ProductunitFormEdit.get('productuomedit_name')!;
    }
    get sequence_leveledit() {
      return this.ProductunitFormEdit.get('sequence_leveledit')!;
    }
    get conversion_rateedit() {
      return this.ProductunitFormEdit.get('conversion_rateedit')!;
    } 
    get batch_flagedit() {
      return this.ProductunitFormEdit.get('batch_flagedit')!;
    }

    
    // product unit class
    Openproductunit(data: any) {
      this.productunitdata = data;
      this.ProductunitFormEdit.get('productuomedit_code')?.setValue(this.productunitdata.productuomclass_code);
      this.ProductunitFormEdit.get('productuomedit_name')?.setValue(this.productunitdata.productuom_name);
      this.ProductunitFormEdit.get('productuomclass_gid')?.setValue(this.productunitdata.productuomclass_gid);
      this.ProductunitFormEdit.get('productuomclass_nameedit')?.setValue(this.productunitdata.productuomclass_gid);
      this.ProductunitFormEdit.get('productuom_gid')?.setValue(this.productunitdata.productuom_gid);
  
    }
    openeditdelete(productuom_gid:string){
      this.parameterValue2 = productuom_gid;
    }


    ondelete2(){
      var url = 'SmrMstProductUnit/Deletproductunitsalessummary';
      let param = {
        productuom_gid: this.parameterValue2
      };
     
      this.service.getparams(url, param).subscribe((result: any) => {
        if (result.status == false) {
          this.ToastrService.warning(result.message);
        } else {
          this.ToastrService.success(result.message);
        }
       
      });
    }


    onsubmitproductunitedit() {
      if (
        this.ProductunitFormEdit.value.productuomedit_name != null) {
  
        this.ProductunitFormEdit.value;
        var url = 'SmrMstProductUnit/EditsalesProductunits';
        this.NgxSpinnerService.show();
        this.service.post(url, this.ProductunitFormEdit.value).subscribe((result: any) => {
          if (result.status == false) {
            this.ToastrService.warning(result.message);
            this.NgxSpinnerService.hide();
            this.ProductunitFormEdit.reset();
          } else {
            
            this.ToastrService.success(result.message);
            this.NgxSpinnerService.hide();
            this.ProductunitFormEdit.reset();
          }
          this.GetProductunitSummary();
        });
      } else {
        this.ToastrService.warning('Kindly Fill All Mandatory Fields !! ');
      }
    }
    onclose2() {
      this.ProductunitFormEdit.reset();
    }
}
